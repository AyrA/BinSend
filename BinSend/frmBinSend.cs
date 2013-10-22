using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CookComputing.XmlRpc;

namespace BinSend
{
    public partial class frmBinSend : Form
    {
        ThreadedSender TS;
        BitAPI BA;
        int TimeToSend = 0;

        public frmBinSend(Template init)
        {
            InitializeComponent();
            if (!QuickSettings.Has("API-ADDR") || !QuickSettings.Has("API-NAME") || !QuickSettings.Has("API-PASS"))
            {
                MessageBox.Show("Your API settings are not set. Please do so now", "API settings missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (new frmAPI().ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You cannot use this application before making the API settings.\r\nWill exit now", "API settings missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                    return;
                }
                else
                {
                    MessageBox.Show("Your API settings have been saved.\r\nIf you want to change them later, delete "+QuickSettings.FILE, "API settings OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            BA = (BitAPI)XmlRpcProxyGen.Create(typeof(BitAPI));
            BA.Url = string.Format("http://{0}/", QuickSettings.Get("API-ADDR"));
            BA.Headers.Add("Authorization", "Basic " + JsonConverter.B64enc(string.Format("{0}:{1}", QuickSettings.Get("API-NAME"), QuickSettings.Get("API-PASS"))));
            
            TS = null;

            cbFormat.DataSource = Enum.GetValues(typeof(EncodingFormat));
            cbFormat.SelectedItem = EncodingFormat.Base64;

            Identity[] myIDs=JsonConverter.getAddresses(BA.listAddresses2());
            addrbookEntry[] Entries=JsonConverter.getAddrBook(BA.listAddressBookEntries());
            string s = null;

            for (int i = 0; i < myIDs.Length; i++)
            {
                if (myIDs[i].enabled)
                {
                    if (myIDs[i].label.Trim().Length > 0)
                    {
                        s = string.Format("{0}     {1}     {2}", myIDs[i].chan ? "[DML]" : "[REG]", myIDs[i].label.Trim(), myIDs[i].address);
                    }
                    else
                    {
                        s = string.Format("{0}     {1}", myIDs[i].chan ? "[DML]" : "[REG]", myIDs[i].address);
                    }
                    cbFrom.Items.Add(s);
                }
            }
            if (cbFrom.Items.Count > 0)
            {
                cbFrom.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("You do not have any addresses. Please create at least one address in bitmessage before using this application.", "no addresses found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(1);
                return;
            }
            foreach (addrbookEntry E in Entries)
            {
                cbTo.AutoCompleteCustomSource.Add(string.Format("{0}    {1}",E.label,E.address));
                cbTo.Items.Add(string.Format("{0}    {1}", E.label, E.address));
            }
            FT_TemplateSelected(init);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (TS == null)
            {
                if (cbTo.Text.Length > 0)
                {
                    if (OFD.ShowDialog() == DialogResult.OK)
                    {
                        TimeToSend = 0;
                        lblStatus.Text = "Sending first part and calculate time...";
                        TS = new ThreadedSender(
                            OFD.SafeFileName,
                            (EncodingFormat)Enum.Parse(typeof(EncodingFormat), cbFormat.SelectedItem.ToString()),
                            cbFrom.SelectedItem.ToString().Split(' ')[cbFrom.SelectedItem.ToString().Split(' ').Length - 1],
                            cbTo.Text,
                            tbSubject.Text,
                            tbText.Text,
                            File.ReadAllBytes(OFD.FileName),
                            nudKB.Value == 0 ? 180 * 1000 * 1000 : (int)nudKB.Value * 1024);
                        TS.chunkSent += new chunkSentHandler(TS_chunkSent);
                        TS.taskFinished += new taskFinishedHandler(TS_taskFinished);
                        TS.send();
                        btnFile.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter an address into the 'To' Field", "No receiver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cbTo.Select();
                }
            }
            else
            {
                MessageBox.Show("Please wait. I am still sending");
            }
        }

        private void TS_taskFinished()
        {
            this.Invoke((MethodInvoker)delegate
            {
                pbStatus.Value = 0;
                pbStatus.Maximum = 0;
                lblStatus.Text = "Finished";
                btnFile.Enabled = true;
                this.Activate();
                this.Focus();
                this.BringToFront();
                this.Show();
                MessageBox.Show("File sent");
            });
            TS = null;
        }

        private void TS_chunkSent(int part, int maxParts, TimeSpan duration)
        {
            TimeToSend += (int)duration.TotalSeconds;
            this.Invoke((MethodInvoker)delegate
            {
                if (maxParts != pbStatus.Maximum)
                {
                    pbStatus.Value = 0;
                    pbStatus.Maximum = maxParts;
                }
                pbStatus.Value = part;
                TimeSpan TS = new TimeSpan(0, 0, TimeToSend / part * maxParts);
                TimeSpan Left = new TimeSpan(0, 0, TimeToSend / part * (maxParts - part));
                lblStatus.Text = string.Format("Sending {0}/{1}...   Estimated Total: {2:00}:{3:00}:{4:00}   Estimated Time left: {5:00} {6:00} {7:00}",
                    part,maxParts,
                    TS.Hours,TS.Minutes,TS.Seconds,
                    Left.Hours,Left.Minutes,Left.Seconds);
            });
        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmPartCollector().ShowDialog();
            this.Show();
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            frmTemplates FT = new frmTemplates();
            FT.TemplateSelected += new frmTemplates.TemplateSelectedHandler(FT_TemplateSelected);
            FT.ShowDialog();
            FT.Dispose();
            FT = null;
        }

        private void FT_TemplateSelected(Template T)
        {
            if (!string.IsNullOrEmpty(T.To))
            {
                cbTo.Text = T.To;
            }
            cbFormat.SelectedItem = T.ProposedFormat;
            tbSubject.Text = T.Subject;
            tbText.Text = T.Text;
            nudKB.Value = T.ChunkSize;
            selectAddr(T.From);
        }

        private void selectAddr(string addr)
        {
            for (int i = 0; i < cbFrom.Items.Count; i++)
            {
                if (cbFrom.Items[i].ToString().Split(' ')[cbFrom.Items[i].ToString().Split(' ').Length - 1] == addr)
                {
                    cbFrom.SelectedIndex = i;
                }
            }
        }

        private void btnEncoding_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Encoding algorithms:
Base64 - very common, efficiency: 75%
Ideal for general purpose content or displaying inline content in bitmessage
Can be used for html5 audo/video transmission

Ascii85 - uncommon, efficiency: 80%
more efficiency than Base64, but not widely supported.

Hex - common, very simple, efficiency: 50%
writes each byte as hex (00-FF) and doubles its size.
Very simple, could be typed manually in an Hex editor

Raw - UTF-8, as is, efficiency: 100%
Sends content 'as is'. If the content is not UTF-8 conform, information is lost.
Ideal to transmit precoded content.

Unknown - Do not use.
Used for incoming content with an encoding, not known to us.","Encoding",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void cbTo_Leave(object sender, EventArgs e)
        {
            cbTo.Text = cbTo.Text.Substring(cbTo.Text.LastIndexOf(" ")+1);
            if (cbTo.Text.Contains("BM-"))
            {
                cbTo.Text = cbTo.Text.Substring(cbTo.Text.IndexOf("BM-"));
            }
        }
    }
}
