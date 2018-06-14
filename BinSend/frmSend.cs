using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmSend : Form
    {
        private Stream Source;
        private Template Template;
        private ApiConfig API;
        private string From;
        private string To;
        private string Subject;
        private string BodyTemplate;
        private string FileName;
        private int ChunkSize;
        private int TTL;
        private List<Fragment> Fragments;
        private Thread Sender;
        private volatile bool SendMessages;
        private volatile bool TrashSent;


        public frmSend(Stream Input, string FileName, ApiConfig A, Template T, string From, string To, string Subject, string Body, int ChunkSize, int TTL)
        {
            Template = T;
            API = A;
            Source = Input;
            this.From = "INV";
            this.To = Tools.GetBmAddr(To);
            this.Subject = Subject;
            BodyTemplate = Body;
            this.ChunkSize = ChunkSize * Tools.SIZEFACTOR;
            this.TTL = TTL * Tools.TIMEFACTOR;
            this.FileName = FileName;

            if (Tools.IsMRND(From))
            {
                this.From = string.Empty;
            }
            else if (Tools.IsSRND(From))
            {
                this.From = Tools.GetRPC(A).createRandomAddress(("Random for " + Path.GetFileName(FileName)).UTF().B64());
            }
            else
            {
                this.From = Tools.GetBmAddr(From);
            }

            InitializeComponent();

            //Prevent height from changing but allows resizing
            MaximumSize = new Size(int.MaxValue, MinimumSize.Height);

            TrashSent = cbClearSent.Checked;

            lblStatus.Text = "Processing Fragments...";

            Thread Th = new Thread(ProcessFragments);
            Th.IsBackground = true;
            Th.Start();
        }

        private void ProcessFragments()
        {
            Fragments = new List<Fragment>();
            int count = 0;
            int iterator = 0;
            byte[] Data = new byte[ChunkSize];
            do
            {
                try
                {
                    count = Source.Read(Data, 0, Data.Length);
                }
                catch (Exception ex)
                {
                    Tools.Log("File fragment read aborted. Reason: " + ex.Message);
                    return;
                }
                if (count > 0)
                {
                    var F = new Fragment()
                    {
                        Encoding = Template.Encoding,
                        Name = Fragments.Count == 0 ? Path.GetFileName(FileName) : null,
                        SameOrigin = From != string.Empty,
                        Part = ++iterator
                    };
                    F.Encode(Data, 0, count);
                    Fragments.Add(F);
                }
            } while (count > 0);
            if (Fragments.Count > 0)
            {
                Fragments[0].List = Fragments
                    .Select(m => m.Content.SHA1())
                    .ToArray();
            }

            Invoke((MethodInvoker)FragmentsReady);
        }

        private static string GetTotal(TimeSpan Elapsed, int CurrentElement, int TotalElements)
        {
            var TotalTime = Elapsed.TotalSeconds / CurrentElement + TotalElements;
            return TimeSpan.FromSeconds(Math.Floor(TotalTime)).ToString("c");
        }

        private void SendFragments()
        {
            var StartTime = DateTime.UtcNow;
            var RPC = Tools.GetRPC(API);
            var FirstPart = Fragments.FirstOrDefault();
            int Total = Fragments.Count;
            var AckDataList = new List<string>();
            while (Fragments.Count > 0 && SendMessages)
            {
                var Fragment = Fragments[0];
                //Cur off milliseconds
                var Elapsed = TimeSpan.FromSeconds(Math.Floor(DateTime.UtcNow.Subtract(StartTime).TotalSeconds));
                SetProgress($"Sending Part {Fragment.Part}. Elapsed: {Elapsed.ToString("c")}. Total: {GetTotal(Elapsed, Fragment.Part, Total)}", Total - Fragments.Count, Total);
                var Addr = string.IsNullOrEmpty(From) ? RPC.createRandomAddress($"Fragment [{FirstPart.Name}:{Fragment.Part}]".UTF().B64()) : From;
                Fragments.RemoveAt(0);
                var Body = Tools.ProcessFragmentBody(BodyTemplate, Fragment, FirstPart.Name, Fragment.Part, Total, FirstPart.List).UTF().B64();
                var AckData = string.Empty;
                if (!string.IsNullOrEmpty(To))
                {
                    AckData = RPC.sendMessage(To, Addr, Subject.UTF().B64(), Body, BitmessageDefaults.DEFAULT_ENC_TYPE, TTL);
                }
                else
                {
                    AckData = RPC.sendBroadcast(Addr, Subject.UTF().B64(), Body, BitmessageDefaults.DEFAULT_ENC_TYPE, TTL);
                }
                if (!string.IsNullOrEmpty(AckData))
                {
                    AckDataList.Add(AckData);
                    var Status = BitmessageMessageStatus.notfound;

                    bool cont = true;
                    while (cont)
                    {
                        if (Enum.TryParse(RPC.getStatus(AckData), out Status))
                        {
                            switch (Status)
                            {
                                //Status on which we continue waiting
                                //We don't wait on POW itself to speed up the process and eliminate gaps
                                case BitmessageMessageStatus.broadcastqueued:
                                case BitmessageMessageStatus.awaitingpubkey:
                                case BitmessageMessageStatus.doingpubkeypow:
                                case BitmessageMessageStatus.msgqueued:
                                    Thread.Sleep(500);
                                    break;
                                default:
                                    cont = false;
                                    break;
                            }
                        }
                        else
                        {
                            Tools.Log("Unknown getStatus API response: " + RPC.getStatus(AckData));
                            cont = false;
                        }
                    }
                }
            }
            //Don't access the Checkbox.Checked property in another thread
            if (TrashSent)
            {
                int count = 0;
                foreach (var A in AckDataList)
                {
                    ++count;
                    SetProgress($"Deleting {count} of {AckDataList.Count}", count, AckDataList.Count);
                    RPC.trashSentMessageByAckData(A);
                }
            }

            SetProgress("Operation " + (SendMessages ? "completed" : "cancelled"), 100, 100);
            Invoke((MethodInvoker)delegate
            {
                cbClearSent.Enabled = false;
                btnCancel.Text = "&Close";
            });
            Sender = null;
        }

        private void FragmentsReady()
        {
            lblStatus.Text = $"{Fragments.Count} fragments ready for send";
        }

        private void SetProgress(string Text, int Current, int Max)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    SetProgress(Text, Current, Max);
                });
            }
            else
            {
                if (pbStatus.Value > Math.Max(Max, Current))
                {
                    pbStatus.Value = pbStatus.Minimum;
                }
                pbStatus.Maximum = Math.Max(Max, Current);
                pbStatus.Value = Math.Min(Current, Max);
                lblStatus.Text = Text;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            SendMessages = true;
            Sender = new Thread(SendFragments);
            Sender.IsBackground = true;
            Sender.Start();
        }

        private void frmSend_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Sender != null && e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("The application is still sending messages. Do you want to abort this?", "Aborting send", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    SendMessages = false;
                }
            }
        }

        private void cbClearSent_CheckedChanged(object sender, EventArgs e)
        {
            TrashSent = cbClearSent.Checked;
        }
    }
}
