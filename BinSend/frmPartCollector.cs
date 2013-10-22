using System;
using System.Windows.Forms;
using System.IO;
using CookComputing.XmlRpc;
using System.Collections.Generic;
using System.Drawing;

namespace BinSend
{
    public partial class frmPartCollector : Form
    {
        private BitAPI BA;
        private List<ValidPart> Parts;
        private Color R, G;
        private List<SentFile> AllFiles;

        public frmPartCollector()
        {
            R = Color.FromArgb(0xFF, 0xDD, 0xDD);
            G = Color.FromArgb(0xDD, 0xFF, 0xDD);

            InitializeComponent();
            BA = (BitAPI)XmlRpcProxyGen.Create(typeof(BitAPI));
            BA.Url = string.Format("http://{0}/", QuickSettings.Get("API-ADDR"));
            BA.Headers.Add("Authorization", "Basic " + JsonConverter.B64enc(string.Format("{0}:{1}", QuickSettings.Get("API-NAME"), QuickSettings.Get("API-PASS"))));
            getFiles();
        }

        private ValidPart FirstPart(string FileName)
        {
            return getPart(FileName, 1);
        }

        private ValidPart getPart(string FileName, int Part)
        {
            ValidPart toReturn = null;
            foreach (ValidPart P in Parts)
            {
                if (P.FileName.ToLower() == FileName.ToLower() && P.Part == Part)
                {
                    //we set P as return value only if it is valid or the first part.
                    if (Part == 1 || isValid(P, FirstPart(FileName)))
                    {
                        toReturn = P;
                    }
                }
            }
            return toReturn;
        }

        private ValidPart getPartByHash(string Hash)
        {
            foreach (ValidPart P in Parts)
            {
                if (P.CurrentHash.ToLower() == Hash.ToLower())
                {
                    return P;
                }
            }
            return null;
        }

        private ValidPart[] getPartsByHash(string Hash)
        {
            List<ValidPart> _Parts = new List<ValidPart>();
            foreach (ValidPart P in Parts)
            {
                if (P.CurrentHash.ToLower() == Hash.ToLower())
                {
                    _Parts.Add(P);
                }
            }
            return _Parts.ToArray();
        }

        private ValidPart getPartByID(string ID)
        {
            foreach (ValidPart P in Parts)
            {
                if (P.ID.ToLower() == ID.ToLower())
                {
                    return P;
                }
            }
            return null;
        }

        private void getFiles()
        {
            msgID[] ss = JsonConverter.getIDs(BA.getAllInboxMessageIds());

            lvParts.Items.Clear();
            lbFiles.Items.Clear();
            Parts = new List<ValidPart>();
            AllFiles = new List<SentFile>();
            foreach (msgID ID in ss)
            {
                BitMsg IB = JsonConverter.getByID(BA.getInboxMessageById(ID.msgid));
                if (!string.IsNullOrEmpty(IB.msgid))
                {
                    if (IB.message.Contains(ValidPart.SEQ_BEGIN))
                    {
                        try
                        {
                            Parts.Add(new ValidPart(IB.message, IB.fromAddress, IB.toAddress, IB.msgid));
                            addFile(Parts[Parts.Count - 1]);
                        }
                        catch
                        {
                            //Invalid/corrupted message.
                        }
                    }
                }
            }
        }

        private void addFile(ValidPart validPart)
        {
            for (int i = 0; i < AllFiles.Count; i++)
            {
                //check for identical names and hash lists
                if (AllFiles[i].FileName.ToLower() == validPart.FileName.ToLower() && checkHL(AllFiles[i].AllHashes, validPart.Hashes))
                {
                    //updates the File, if it is the first part.
                    if (validPart.Part == 1)
                    {
                        AllFiles[i] = new SentFile(validPart.FileName, validPart.CurrentHash, validPart.Hashes, validPart.ID);
                    }
                    return;
                }
            }
            //add new file
            AllFiles.Add(new SentFile(validPart.FileName, validPart.CurrentHash, validPart.Hashes, validPart.ID));
            lbFiles.Items.Add(AllFiles[AllFiles.Count - 1].FileName);
        }

        private bool checkHL(string[] HL1, string[] HL2)
        {
            if (HL1.Length != HL2.Length)
            {
                return false;
            }
            foreach (string H1 in HL1)
            {
                bool ok = false;
                foreach (string H2 in HL2)
                {
                    ok = ok | H2.ToLower() == H1.ToLower();
                }
                if (!ok)
                {
                    return false;
                }
            }
            return true;
        }

        private bool joinFile(SentFile SF)
        {
            bool ok = true;
            ValidPart First=getPartByID(SF.ID);
            ValidPart[] Segments = getFileParts(SF);
            int i = 1;
            if (File.Exists(@"PARTS\" + First.FileName))
            {
                File.Delete(@"PARTS\" + First.FileName);
            }
            FileStream FS = File.Create(@"PARTS\" + First.FileName);
            foreach (ValidPart P in Segments)
            {
                if (i == 1 || isValid(P, First))
                {
                    FS.Write(P.data, 0, P.data.Length);
                }
                else
                {
                    ok = false;
                }
                i++;
            }
            FS.Close();
            return ok;
        }

        private ValidPart[] getFileParts(SentFile SF)
        {
            List<ValidPart> _Parts=new List<ValidPart>();
            ValidPart First = getPartByID(SF.ID);

            foreach (string Hash in SF.AllHashes)
            {
                foreach (ValidPart P in getPartsByHash(Hash))
                {
                    if (isValid(P, First))
                    {
                        _Parts.Add(P);
                        break;
                    }
                }
            }
            //sort in ascending Part order
            List<ValidPart> retValue = new List<ValidPart>(_Parts.Count);
            //run until all parts are processed
            for (int j = 1; j <= First.MaxParts;j++ )
            {
                bool added = false;
                for (int i = 0; i < _Parts.Count; i++)
                {
                    //if the next part is found, add to sorted list
                    //and remove from old list
                    if (_Parts[i].Part == j)
                    {
                        retValue.Add(_Parts[i]);
                        _Parts.RemoveAt(i);
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    //missing Index?
                    ValidPart P = getPartByHash(First.Hashes[j - 1]);
                    retValue.Add(P);
                }
            }
            return retValue.ToArray();
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                lvParts.Items.Clear();
                ValidPart[] FileParts= getFileParts(AllFiles[lbFiles.SelectedIndex]);
                if (FileParts.Length>0)
                {
                    foreach (ValidPart P in FileParts)
                    {
                        if (P != null)
                        {
                            var LVI = lvParts.Items.Add(P.FileName);
                            LVI.SubItems.Add(string.Format("{0:00} / {1:00}", P.Part, P.MaxParts));
                            LVI.SubItems.Add(P.CurrentHash);
                            LVI.SubItems.Add(string.Format("{0}", P.PartLength / 1024));
                            LVI.SubItems.Add(isValid(P, FileParts[0]) ? "Yes" : "No");
                            LVI.BackColor = (isValid(P, FileParts[0]) ? G : R);
                        }
                        else
                        {
                            var LVI = lvParts.Items.Add("--MISSING--");
                            LVI.SubItems.Add("? / ?");
                            LVI.SubItems.Add("Part not yet received");
                            LVI.SubItems.Add("0");
                            LVI.SubItems.Add("No");
                            LVI.BackColor = R;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("First part is missing. Cannot verify other parts without it", "Part missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private bool isValid(ValidPart toCheck, ValidPart FirstPart)
        {
            //check if it is a part at all
            if (toCheck == null)
            {
                return false;
            }
            if (FirstPart == null)
            {
                toCheck.Tag = "First part missing to verify!";
                return false;
            }
            //check, if self reference
            if (toCheck == FirstPart)
            {
                return true;
            }
            //check same origin policy
            if (FirstPart.SameOrigin)
            {
                if (!toCheck.SameOrigin || toCheck.From != FirstPart.From)
                {
                    toCheck.Tag = "Part came from a wrong address";
                    return false;
                }
            }
            //check if part range matches
            if (toCheck.Part > FirstPart.MaxParts || toCheck.MaxParts!=FirstPart.MaxParts)
            {
                toCheck.Tag = "Part does not fits into the range defined in the first message";
                return false;
            }
            //check if hash is valid
            if (!toCheck.validHash)
            {
                toCheck.Tag = "Hash is invalid. This part is corrupt";
                return false;
            }
            if (toCheck.CurrentHash != FirstPart.Hashes[toCheck.Part - 1])
            {
                toCheck.Tag = "Hash from current part is valid but does not matches hash list from first part.";
                return false;
            }
            toCheck.Tag = null;
            return true;
        }

        private void lvParts_DoubleClick(object sender, EventArgs e)
        {
            if (lvParts.SelectedItems.Count > 0)
            {
                ValidPart P = getPartByHash(lvParts.SelectedItems[0].SubItems[2].Text);
                if (P != null)
                {
                    if (P.Tag != null)
                    {
                        MessageBox.Show(P.Tag.ToString(), "Invalid part description", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        previewPart(P);
                    }
                }
                else
                {
                    MessageBox.Show("This part is missing.\r\nIf it does not arrives soon,\r\nask the sender of the first part to send it again.", "Invalid part description", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnAssemble_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                if (MessageBox.Show(string.Format("This will assemble (not execute!) all existing valid parts into the file '{0}'\r\nNever execute files from untrusted sources, especially DML addresses!\r\nContinue?", lbFiles.SelectedItem.ToString()),"Assemble file",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (joinFile(AllFiles[lbFiles.SelectedIndex]))
                    {
                        MessageBox.Show("Your parts have been assembled to a file. You can delete the parts now.", "File joined", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("At least one part was missing or invalid.\r\nThe file may be unusable after the first missing part.", "File joined", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                if (MessageBox.Show(string.Format("Delete the file '{0}'?\r\nThis will only remove leftover parts and messages from the inbox and not the assembled file.", lbFiles.SelectedItem.ToString()), "Delete parts/messages", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    delMessages(lbFiles.SelectedItem.ToString());
                    getFiles();
                }
            }
        }

        private void delMessages(string name)
        {
            ValidPart First = FirstPart(name);
            if (First == null)
            {
                MessageBox.Show("The first part is missing. Please delete the parts manually in the part list.", "First part missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                foreach (ValidPart P in Parts)
                {
                    if (P.FileName.ToLower() == name.ToLower() && isValid(P,First))
                    {
                        BA.trashMessage(P.ID);
                    }
                }
            }
            
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (lvParts.SelectedItems.Count > 0)
            {
                previewPart(getPartByHash(lvParts.SelectedItems[0].SubItems[2].Text));
            }
        }

        private void previewPart(ValidPart validPart)
        {
            if (validPart != null)
            {
                new frmPreview(HexView(validPart.data)).ShowDialog();
            }
            else
            {
                MessageBox.Show("This part is missing.", "Part missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string HexView(byte[] data)
        {
            int i, j;
            string line = null;
            string retValue = string.Empty;
            for (i = 0; i < data.Length; i += 16)
            {
                line = string.Empty;
                for (j = 0; j < 16; j++)
                {
                    if (i + j < data.Length)
                    {
                        line += data[i + j].ToString("X2") + " ";
                    }
                    else
                    {
                        //Fill up empty Space
                        line += "   ";
                    }
                }
                line = line + "\t";
                for (j = 0; j < 16; j++)
                {
                    if (i + j < data.Length)
                    {
                        //Filter some Invalid chars
                        if (data[i + j] >= 32 &&
                            data[i + j] != 127 &&
                            data[i + j] != 128 &&
                            (data[i + j] <= 0x80 || data[i + j] >= 0xA0))
                        {
                            line += (char)data[i + j];
                        }
                        else
                        {
                            line += ".";
                        }
                    }
                    else
                    {
                        //Fill up empty Space
                        line += " ";
                    }
                }
                retValue += line + "\r\n";
            }
            return retValue;
        }

        private void btnDelPart_Click(object sender, EventArgs e)
        {
            if (lvParts.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Delete all selected parts?", "Deleting Parts", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lvParts.SuspendLayout();
                    while (lvParts.SelectedItems.Count>0)
                    {
                        ValidPart P = getPartByHash(lvParts.SelectedItems[0].SubItems[2].Text);
                        if (P != null)
                        {
                            BA.trashMessage(P.ID);
                            if (File.Exists(string.Format(@"PARTS\{0}.{1}", P.FileName, P.Part)))
                            {
                                File.Delete(string.Format(@"PARTS\{0}.{1}", P.FileName, P.Part));
                            }
                            lvParts.SelectedItems[0].Remove();
                        }
                    }
                    lvParts.ResumeLayout();
                }
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            getFiles();
        }
    }
}
