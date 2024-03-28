using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Menu
{
    public partial class Form1 : Form
    {
        private string fileName;
        public Form1()
        {
            InitializeComponent();
            SetupOpenDLG();
            SetupSaveDLG();
            fileName = "TextPad - NewFile.rtf";
            this.Text = fileName;
        }

        private void MenuUndo_Click(object sender, EventArgs e)
        {
            rtb.Undo();
        }

        private void MenuPaste_Click(object sender, EventArgs e)
        {
            rtb.Paste();
        }

        private void MenuCopy_Click(object sender, EventArgs e)
        {
            rtb.Copy();
        }

        private void MenuCut_Click(object sender, EventArgs e)
        {
            rtb.Cut();
        }

        private void MenuNew_Click(object sender, EventArgs e)
        {
            if (rtb.Modified)
            {
                DialogResult saveResult = MessageBox.Show("Do you want to save your file?\nYou might lose your progress if you don't save!", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (saveResult == DialogResult.Yes)
                {
                    MenuSave_Click(sender, e);
                }
                else if (saveResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            rtb.Clear();
            this.Text = "TextPad - NewFile.rtf";
            fileName = this.Text;
            rtb.Modified = false;
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            if (rtb.Modified)
            {
                DialogResult saveResult = MessageBox.Show("Do you want to save your file?\nYou might lose your progress if you don't save!", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (saveResult == DialogResult.Yes)
                {
                    MenuSave_Click(sender, e);
                }
                else if (saveResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            if (openFileDLG.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            fileName = openFileDLG.FileName;
            this.Text = "TextPad - " + openFileDLG.SafeFileName;
            if (openFileDLG.FilterIndex == 2)
            {
                rtb.LoadFile(openFileDLG.FileName, RichTextBoxStreamType.PlainText);
            }
            else if (openFileDLG.FilterIndex == 1)
            {
                rtb.LoadFile(openFileDLG.FileName, RichTextBoxStreamType.RichText);
            }
            else
            {
                MessageBox.Show("Doesn't support the chosen format!","Error 2",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            rtb.Modified = false;
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {

        }

        private void openFileDLG_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void MenuSaveAs_Click(object sender, EventArgs e)
        {
            if (saveFileDLG.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            fileName = saveFileDLG.FileName;
            if (saveFileDLG.FilterIndex == 1)
            {
                rtb.SaveFile(saveFileDLG.FileName, RichTextBoxStreamType.RichText);
            }
            else if (saveFileDLG.FilterIndex == 2)
            {
                rtb.SaveFile(saveFileDLG.FileName, RichTextBoxStreamType.PlainText);
            }
            else
            {
                MessageBox.Show("Doesn't work with this format");
                return;
            }
            rtb.Modified = false;
        }

        private void MenuWordWrap_Click(object sender, EventArgs e)
        {
            MenuWordWrap.Checked = !MenuWordWrap.Checked;
            rtb.WordWrap = MenuWordWrap.Checked;
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (fileName == "TextPad - NewFile.rtf")
            {
                MenuSaveAs_Click(sender, e);
            }
            else
            {
                rtb.SaveFile(fileName, RichTextBoxStreamType.RichText);
            }
            rtb.Modified = false;
        }

        private void MenuSelectAll_Click(object sender, EventArgs e)
        {
            rtb.SelectAll();
        }

        private void MenuFont_Click(object sender, EventArgs e)
        {
            fontDLG.ShowApply = true;
            fontDLG.ShowHelp = true;
            fontDLG.ShowColor = true;
            fontDLG.ShowDialog();
        }

        private void MenuRedo_Click(object sender, EventArgs e)
        {
            rtb.Undo();
        }

        private void rtb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOnExit(sender, e);
        }

        private void SetupOpenDLG()
        {
            openFileDLG.Filter = "Rich Text Format|*.rtf|Plain Text|*.txt|Word Files|*.doc;*.docx|All files|*.*";
            openFileDLG.FileName = "";
        }
        private void SetupSaveDLG()
        {
            saveFileDLG.Filter = "Rich Text Format|*.rtf|Plain Text|*.txt|Word Files|*.doc;*.docx|All files|*.*";
        }

        private void fontDLG_Apply(object sender, EventArgs e)
        {
            if (rtb.SelectionFont != null)
            {
                rtb.SelectionFont = fontDLG.Font;
                rtb.SelectionColor = fontDLG.Color;
            }
            else if (rtb.SelectionFont == null)
            {
                rtb.Font = fontDLG.Font;
                rtb.ForeColor = fontDLG.Color;
            }
        }
        private void SaveOnExit(object sender, FormClosingEventArgs e)
        {
            if (rtb.Modified)
            {
                DialogResult result = MessageBox.Show("Do you want to save your file?\nYou might lose your progress if you don't save!", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (string.IsNullOrEmpty(fileName) || fileName == "TextPad - NewFile.rtf")
                    {
                        MenuSaveAs_Click(sender, e);
                        return;
                    }

                    if (!File.Exists(fileName))
                    {
                        MenuSaveAs_Click(sender, e);
                        return;
                    }

                    rtb.SaveFile(fileName, RichTextBoxStreamType.RichText);
                    rtb.Modified = false;
                }
            }
        }
    }
}
