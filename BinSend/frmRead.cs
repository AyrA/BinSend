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
            EnableAll(false);
            Thread T = new Thread(MessageReader);
            T.IsBackground = true;
            T.Start();
        }

        private void EnableAll(bool Status)
        {
            foreach (var C in Controls)
            {
                ((Control)C).Enabled = Status;
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
                Messages = IdList
                    .inboxMessageIds
                    .Select(m =>
                        Program.MessageCache.Any(n => n.msgid == m.msgid) ?
                        Program.MessageCache.First(n => n.msgid == m.msgid) :
                        RPC.getInboxMessageById(m.msgid, true).FromJson<BitmessageInboxMsg>().inboxMessage.FirstOrDefault().Decode())
                    .ToArray();
            }



            //Add messages to cache
            foreach (var Msg in Messages)
            {
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
                //MessageBox.Show($"Got {Messages.Length} messages and extracted {Fragments.Length} unique fragments");
                if (lbFiles.Items.Count > 0)
                {
                    lbFiles.SelectedIndex = 0;
                }
            });
        }

        private void lbFiles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                var F = (Fragment)lbFiles.SelectedItem;
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

        private Fragment[] GetFragments(Fragment F, FragmentHandler Origin)
        {
            if (F.SameOrigin)
            {
                return Origin.GetOrdered(F);
            }
            else
            {
                return Origin.GetOrderedNoOrigin(F, Fragments);
            }
        }

        private void SetFragmentList(IEnumerable<Fragment> Fragments)
        {
            lvFragments.Items.Clear();
            foreach (var F in Fragments)
            {
                if (F != null)
                {
                    lvFragments.Items.Add($"Part {F.Part}").SubItems.Add(F.Content.UTF().SHA1());
                }
                else
                {
                    lvFragments.Items.Add("MISSING").SubItems.Add("MISSING");
                }
            }
            lvFragments.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            var F = (Fragment)lbFiles.SelectedItem;
            var Origin = Fragments.FirstOrDefault(m => m.GetPrimary().Contains(F));
            if (Origin != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete '{F}' from birmessage", "Deleting Messages", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Origin.Delete(Tools.GetRPC(API));
                    lvFragments.Items.Clear();
                    lbFiles.Items.Remove(F);
                }
            }
            else
            {
                MessageBox.Show("The specified fragment doesn't matches any container available. Ensure no other application is interfacing with the bitmessage client at the moment", "Fragment container missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAssemble_Click(object sender, System.EventArgs e)
        {
            var F = (Fragment)lbFiles.SelectedItem;
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
                SFD.FileName = F.Name;
                SFD.Filter = $"{F.Name}|{F.Name}|Same Type|*{Path.GetExtension(F.Name)}";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(SFD.FileName, Origin.Join(All));
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
    }
}
