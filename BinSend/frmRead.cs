using BinSend.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BinSend
{
    public partial class frmRead : Form
    {
        private ApiConfig API;
        private BitmessageMsg[] Messages;
        private FragmentHandler[] Fragments;

        public frmRead(ApiConfig C)
        {
            API = C;
            InitializeComponent();
            ScanMessages();
        }

        private void ScanMessages()
        {
            lvFragments.Items.Clear();
            lbFiles.Items.Clear();
            EnableAll(false);
            Thread T = new Thread(MessageReader);
            T.IsBackground = true;
            T.Start();
        }

        private void EnableAll(bool Status)
        {
            foreach (var C in Controls)
            {
                if (!(C is Label))
                {
                    ((Control)C).Enabled = Status;
                }
            }
        }

        private void MessageReader()
        {
            var Handlers = new List<FragmentHandler>();

            //Get messages from RPC or cache
            var RPC = Tools.GetRPC(API);
            var IdList = RPC.getAllInboxMessageIds().FromJson<BitmessageIdList>();
            if (IdList.inboxMessageIds != null)
            {
                Messages = new BitmessageMsg[IdList.inboxMessageIds.Length];
                for (var i = 0; i < Messages.Length; i++)
                {
                    var msg = IdList.inboxMessageIds[i];
                    SetStatus($"Reading Message {i + 1} of {Messages.Length}");
                    if (Program.MessageCache.Any(m => m.msgid == msg.msgid))
                    {
                        Messages[i] = Program.MessageCache.First(m => m.msgid == msg.msgid);
                    }
                    else
                    {
                        var container = RPC.getInboxMessageById(msg.msgid, true)
                            .FromJson<BitmessageInboxMsg>();
                        if (container.inboxMessage != null && container.inboxMessage.Length > 0)
                        {
                            Messages[i] = container.inboxMessage[0].Decode();
                        }
                    }
                }
            }

            //Add messages to cache
            int index = 0;
            foreach (var Msg in Messages)
            {
                SetStatus($"Caching message {++index}...");
                if (!Program.MessageCache.Any(m => m.msgid == Msg.msgid))
                {
                    Program.MessageCache.Add(Msg);
                }
                var F = Tools.GetFragment(Msg.message);
                if (F != null)
                {
                    var Handler = Handlers.FirstOrDefault(m => m.FromAddr == Msg.fromAddress);
                    if (Handler == null)
                    {
                        Handler = new FragmentHandler();
                        Handler.FromAddr = Msg.fromAddress;
                        Handlers.Add(Handler);
                    }
                    Handler.Add(F, Msg.msgid);
                }
            }

            Fragments = Handlers.ToArray();

            SetStatus("Populating Lists");
            Invoke((MethodInvoker)delegate
            {
                EnableAll(true);
                foreach (var F in Fragments.SelectMany(m => m.GetPrimary()))
                {
                    if (!lbFiles.Items.Contains(F))
                    {
                        lbFiles.Items.Add(F);
                    }
                }
                if (lbFiles.Items.Count > 0)
                {
                    lbFiles.SelectedIndex = 0;
                }
            });
            SetStatus("Done");
        }

        private void SetStatus(string Status)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { SetStatus(Status); });
            }
            else
            {
                lblStatus.Text = Status;
            }
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                var F = (FragmentInfo)lbFiles.SelectedItem;
                var Origin = Fragments.FirstOrDefault(m => m.GetPrimary().Contains(F));
                if (Origin != null)
                {
                    SetFragmentList(GetFragments(F, Origin));
                }
                else
                {
                    MessageBox.Show("The specified fragment doesn't matches any container available. Ensure no other application is interfacing with the bitmessage client at the moment", "Fragment container missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private FragmentInfo[] GetFragments(FragmentInfo F, FragmentHandler Origin)
        {
            if (F.Fragment.SameOrigin)
            {
                return Origin.GetOrdered(F.Fragment);
            }
            else
            {
                return Origin.GetOrderedNoOrigin(F.Fragment, Fragments);
            }
        }

        private void SetFragmentList(IEnumerable<FragmentInfo> Fragments)
        {
            lvFragments.Items.Clear();
            foreach (var F in Fragments)
            {
                if (F != null)
                {
                    lvFragments.Items.Add($"Part {F.Fragment.Part}").SubItems.Add(F.Fragment.Content.UTF().SHA1());
                }
                else
                {
                    lvFragments.Items.Add("MISSING").SubItems.Add("MISSING");
                }
            }
            lvFragments.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var F = (FragmentInfo)lbFiles.SelectedItem;
            var Origin = Fragments.FirstOrDefault(m => m.GetPrimary().Contains(F));
            if (Origin != null)
            {
                FragmentInfo[] AllFragments;
                if (F.Fragment.SameOrigin)
                {
                    AllFragments = Origin.GetOrdered(F.Fragment);
                }
                else
                {
                    AllFragments = Origin.GetOrderedNoOrigin(F.Fragment, Fragments);
                }
                if (MessageBox.Show($"Are you sure you want to delete '{F.Fragment.Name}' from bitmessage? (total {AllFragments.Length} fragments)", "Deleting Messages", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    EnableAll(false);
                    var T = new Thread(delegate ()
                    {
                        var RPC = Tools.GetRPC(API);
                        int count = 0;
                        foreach (var Frag in AllFragments)
                        {
                            if (Frag != null)
                            {
                                RPC.trashMessage(Frag.MessageId);
                                SetStatus($"Deleted message {++count}");
                            }
                            else
                            {
                                SetStatus($"Skipping empty fragment {++count}");
                            }
                        }
                        Invoke((MethodInvoker)delegate
                        {
                            lvFragments.Items.Clear();
                            lbFiles.Items.Remove(F);
                            EnableAll(true);
                            MessageBox.Show($"Fragments deleted.", "Deleting Fragments", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });
                    });
                    T.IsBackground = true;
                    T.Start();
                }
            }
            else
            {
                MessageBox.Show("The specified fragment doesn't matches any container available. Ensure no other application is interfacing with the bitmessage client at the moment", "Fragment container missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAssemble_Click(object sender, EventArgs e)
        {
            var F = (FragmentInfo)lbFiles.SelectedItem;
            var Origin = Fragments.FirstOrDefault(m => m.GetPrimary().Contains(F));
            if (Origin != null)
            {
                var All = GetFragments(F, Origin);
                if (All.Any(m => m == null))
                {
                    if (MessageBox.Show("This file has missing parts and will be partially or completely unusable, depending on the file type. Do you want to assemble this file?", "Missing Parts", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                SFD.FileName = F.Fragment.Name;
                SFD.Filter = $"{F.Fragment.Name}|{F.Fragment.Name}|Same Type|*{Path.GetExtension(F.Fragment.Name)}";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(SFD.FileName, Origin.Join(All));
                        MessageBox.Show($"Fragments sucessfully assembled.", "Assembling File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Tools.Log($"Unable to assemble fragments. Reason: {ex.Message}");
                        MessageBox.Show($"Unable to assemble fragments. Reason: {ex.Message}", "Error assembling file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("The specified fragment doesn't matches any container available. Ensure no other application is interfacing with the bitmessage client at the moment", "Fragment container missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ScanMessages();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Tools.ShowHelp(Resources.HELP_Read);
        }

        private void frmRead_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tools.CloseHelp();
        }
    }
}
