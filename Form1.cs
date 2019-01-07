using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ideaFileReplacer
{
    public partial class Form1 : Form
    {

        private string target_folder = "";
        private string target_file = "";
        private string file_name = "";

        private int total_files_replaced = 0;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(Replace);
            Thread th = new Thread(ts);
            th.Start();
        }

        private void Replace()
        {
            try
            {
                target_folder = "";
                target_file = "";
                file_name = "";
                total_files_replaced = 0;
                txtLog.Clear();

                addLog("process started");

                if (!CheckTextBoxes())
                    return;

                if (!CheckFolderExists())
                    return;

                if (!CheckFileExists())
                    return;


                target_folder = txtTargetFolder.Text;
                target_file = txtTargetFile.Text;
                file_name = GetFileNameFromFullPath(txtTargetFile.Text);

                addLog("get files...");

                List<string> files = Directory.GetFiles(target_folder, file_name, SearchOption.AllDirectories).ToList();

                addLog("total file count to replace : " + files.Count);

                addLog("listing paths...");
                foreach (string path in files)
                {
                    addLog(path);

                }

                if (MessageBox.Show("Devam etmek istediğinize emin misiniz?", "UYARI",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (MessageBox.Show("Bak doğru söyle emin misin ? ", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        addLog("process approved...");
                    }
                    else
                    {
                        addLog("process cancelled...");
                        return;
                    }
                }
                else
                {
                    addLog("process cancelled...");
                    return;
                }



                foreach (string path in files)
                {
                    if (!ReplaceFile(path))
                    {
                        addLog("error replacing file : " + path);
                    }
                }
                addLog("total files replaced : " + total_files_replaced);
                addLog("operation completed...");

            }
            catch (Exception ex)
            {
                addLog("EXCEPTION = General error : " + ex.Message);
            }
            


        }

        private bool ReplaceFile(string path)
        {
            try
            {
                File.Copy(target_file, path,true);
                total_files_replaced++;
                addLog("file replaced : " + path);
                return true;
            }
            catch (Exception ex)
            {
                addLog("EXCEPTION = replaceFile exception : " + ex.Message);
                return false;
            }
        }

        private void addLog(string str)
        {
            txtLog.AppendText(str + "\n");
        }

        private bool CheckFolderExists()
        {
            addLog("checking folder exists...");

            if (!Directory.Exists(txtTargetFolder.Text))
            {
                MessageBox.Show(txtTargetFolder.Text + " bulunamadı.");
                return false;
            }

            return true;

        }
        
        private bool CheckFileExists()
        {
            addLog("checking file exists...");

            if (!File.Exists(txtTargetFile.Text))
            {
                MessageBox.Show(txtTargetFile.Text + " bulunamadı.");
                return false;
            }

            return true;
        }

        private string GetFileNameFromFullPath(string path)
        {
            addLog("acquiring filename from full path...");
            return Path.GetFileName(path);
        }

        private bool CheckTextBoxes()
        {
            addLog("checking textboxes...");
            if (String.IsNullOrEmpty(txtTargetFolder.Text) || String.IsNullOrEmpty(txtTargetFile.Text))
            {
                MessageBox.Show("Hedef klasör ve hedef dosya alanlarını boş bırakamazsınız.");
                return false;
            }
            return true;
        }
    }
}