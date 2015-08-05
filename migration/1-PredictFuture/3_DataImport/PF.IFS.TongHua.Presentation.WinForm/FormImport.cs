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

namespace PF.IFS.TongHua.Presentation.WinForm
{
    public partial class FormImport : Form
    {
        private readonly Importer _importer;

        public FormImport()
        {
            _importer = new Importer();
            _importer.ProgressChanged += _importer_ProgressChanged;
            InitializeComponent(); 
            textBoxShase.Text =
                @"D:\Work\code\1-PredictFuture\3_DataImport\PF.IFS.TongHuaDataReader.Test\TestData\ShangHaiDay";
            textBoxSznse.Text =
                @"D:\Work\code\1-PredictFuture\3_DataImport\PF.IFS.TongHuaDataReader.Test\TestData\ShenzhenDay";
            textBoxDividend.Text =
                @"D:\Work\code\1-PredictFuture\3_DataImport\PF.IFS.TongHuaDataReader.Test\TestData\权息资料.财经";
        }

        private void _importer_ProgressChanged(object sender, ProgressArgs e)
        {
            Invoke(new Action(() =>
            {
                textBoxLog.Text = e.Message;
                progressBar.Value = e.Progress;

                if (e.Progress < 100)
                {
                    return;
                }

                MessageBox.Show(this, e.Message, @"信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                buttonImport.Enabled = true;
                panelShase.Enabled = true;
                panelSznse.Enabled = true;
                panelDividend.Enabled = true;
            }));
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            _importer.ImportShase = checkBoxShase.Checked;
            if (checkBoxShase.Checked)
            {
                if (Directory.Exists(textBoxShase.Text) == false)
                {
                    MessageBox.Show(this, @"上证日线文件夹无效", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _importer.ShaseDayLineFolder = textBoxShase.Text;
            }

            _importer.ImportSznse = checkBoxSznse.Checked;
            if (checkBoxSznse.Checked)
            {
                if (Directory.Exists(textBoxSznse.Text) == false)
                {
                    MessageBox.Show(this, @"深证日线文件夹无效", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _importer.SznseDayLineFolder = textBoxSznse.Text;
            }

            _importer.ImportDivedend = checkBoxDividend.Checked;
            if (checkBoxDividend.Checked)
            {
                if (File.Exists(textBoxDividend.Text) == false)
                {
                    MessageBox.Show(this, @"除权数据文件无效", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _importer.DivedendFile = textBoxDividend.Text;
            }

            _importer.StartImport();

            buttonImport.Enabled = false;
            panelShase.Enabled = false;
            panelSznse.Enabled = false;
            panelDividend.Enabled = false;
        }

        private void buttonShase_Click(object sender, EventArgs e)
        {
            using (var folderdialog = new FolderBrowserDialog())
            {
                if (folderdialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxShase.Text = folderdialog.SelectedPath;
                }
            }
        }

        private void buttonSznse_Click(object sender, EventArgs e)
        {
            using (var folderdialog = new FolderBrowserDialog())
            {
                if (folderdialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxSznse.Text = folderdialog.SelectedPath;
                }
            }
        }

        private void buttonDividend_Click(object sender, EventArgs e)
        {
            using (var filedialog = new OpenFileDialog())
            {
                if (filedialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxDividend.Text = filedialog.FileName;
                }
            }
        }

        private void checkBoxShase_CheckedChanged(object sender, EventArgs e)
        {
            labelShase.Enabled = checkBoxShase.Checked;
            textBoxShase.Enabled = checkBoxShase.Checked;
            panelShaseFolder.Enabled = checkBoxShase.Checked;
        }

        private void checkBoxSznse_CheckedChanged(object sender, EventArgs e)
        {
            labelSznse.Enabled = checkBoxSznse.Checked;
            textBoxSznse.Enabled = checkBoxSznse.Checked;
            panelSznseFolder.Enabled = checkBoxSznse.Checked;
        }

        private void checkBoxDividend_CheckedChanged(object sender, EventArgs e)
        {
            labelDividend.Enabled = checkBoxDividend.Checked;
            textBoxDividend.Enabled = checkBoxDividend.Checked;
            panelDividendFolder.Enabled = checkBoxDividend.Checked;
        }
    }
}
