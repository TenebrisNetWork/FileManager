using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //метод отображения файлов и папок
        private void ShowFiles(String path)
        {
            if(Path.GetExtension(path) == "")
            {
                listBox1.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(path);

                DirectoryInfo[] dirs = dir.GetDirectories();

                foreach (DirectoryInfo crrDir in dirs)
                {
                    listBox1.Items.Add(crrDir);
                }

                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo crrFile in files)
                {
                    listBox1.Items.Add(crrFile);
                }
            } else
            {
                Process.Start(path);
            }
            
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "C:\\";
            ShowFiles(textBox1.Text);
        }
        //Событие нажатия кнопки "Перейти"
        private void Button1_Click(object sender, EventArgs e)
        { 
            ShowFiles(textBox1.Text);
        }
        //Событие двойного клика на файле
        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
            ShowFiles(textBox1.Text);
        }
        //Событие нажатия кнопки "Назад"
        private void Button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text[textBox1.Text.Length - 1] == '\\')
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);

                while(textBox1.Text[textBox1.Text.Length -1] != '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
            } else if(textBox1.Text[textBox1.Text.Length - 1] != '\\') {
                while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
            }

            ShowFiles(textBox1.Text);
        }

        private void ListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
            }
        }

        private void КопироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = listBox1.SelectedItem.ToString();
            Clipboard.SetText(textBox1.Text + "\\" + filename);
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Если директория target.FullName не существует, создать ее
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Копируем файлы из sourceDirectory в targetDirectory
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }
            //копируем поддиректории
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void ВставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(Clipboard.GetText()) == "")
            {
                Copy(Clipboard.GetText(), textBox1.Text + "\\" + Path.GetFileName(Clipboard.GetText()) + "-copy");
            }
            else
            {
                File.Copy(Clipboard.GetText(), textBox1.Text + "\\" + Path.GetFileNameWithoutExtension(Clipboard.GetText()) + "-copy" + Path.GetExtension(Clipboard.GetText()));
            }
            ShowFiles(textBox1.Text);
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Path.GetExtension(listBox1.SelectedItem.ToString()) == "")
            {
                DirectoryInfo DI = new DirectoryInfo(textBox1.Text + "\\" + Path.GetFileName(listBox1.SelectedItem.ToString()));
                DI.Delete(true);

            } else
            {
                File.Delete(textBox1.Text + "\\" + Path.GetFileName(listBox1.SelectedItem.ToString()));
            }
            ShowFiles(textBox1.Text);
        }

        private void ПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            while(Directory.Exists(textBox1.Text + "\\" + "Новая папка " + i))
            {
                i++;
            }
            Directory.CreateDirectory(textBox1.Text + "\\" + "Новая папка " + i);
            ShowFiles(textBox1.Text);
        }

        private void ФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            while (File.Exists(textBox1.Text + "\\" + "Новый файл " + i))
            {
                i++;
            }
            File.Create(textBox1.Text + "\\" + "Новый файл " + i);
            ShowFiles(textBox1.Text);
        }
    }
}
