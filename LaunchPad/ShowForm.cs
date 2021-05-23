using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LaunchPad
{
    public partial class ShowForm : Form
    {
        public ShowForm()
        {
            InitializeComponent();
        }
        PictureBox[,] pic = new PictureBox[10, 20];
        Label[,] labels = new Label[10, 20];
        Color c = Color.Black;

        void ResetPictureBox()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 10; j++)
                {
                    pic[j, i] = new PictureBox();
                    pic[j, i].Location = new Point(j * 100 + 48, i * 100 + 35);
                    pic[j, i].Size = new Size(64, 64);
                    pic[j, i].SizeMode = PictureBoxSizeMode.StretchImage;
                    pic[j, i].BackColor = Color.Transparent;
                    this.Controls.Add(pic[j, i]);

                    labels[j, i] = new Label();
                    labels[j, i].Location = new Point(j * 100 + 35, i * 100 + 100);
                    labels[j, i].AutoSize = false;
                    labels[j, i].Size = new Size(100, 30);
                    labels[j, i].BackColor = Color.Transparent;
                    labels[j, i].ForeColor = c;
                    this.Controls.Add(labels[j, i]);
                }
        }
        private void ShowForm_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            Hide();
        }
        void LoadList()
        {
            int x = 0, y = 0;
            for (int i = 0; i < Global.l.Count(); i++)
            {
                if (Global.l[i].type == "文件") pic[x, y].Image = Getfileicon(Global.l[i].path);
                else pic[x, y].Image = Properties.Resources.FileFolder;
                pic[x, y].Tag = Global.l[i].path;
                pic[x, y].Click += Pic_Click;

                string[] s = Global.l[i].path.Split('\\');
                string ll = string.Empty;
                foreach (string s1 in s) ll = s1;
                labels[x, y].Text = ll + "\n(" + Global.l[i].path + ")";
                labels[x, y].Tag = Global.l[i].path;
                labels[x, y].Click += Labels_Click;

                toolTip1.SetToolTip(pic[x, y], Global.l[i].path);
                toolTip1.SetToolTip(labels[x, y], Global.l[i].path);

                x++;
                if (x >= 10) { x -= 10; y++; }
                if (y >= 4) break;
            }
        }

        private void Labels_Click(object sender, EventArgs e)
        {
            Process.Start((string)((Label)sender).Tag);
            Hide();
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            Process.Start((string)((PictureBox)sender).Tag);
            Hide();
        }

        private Image Getfileicon(string filename)
        {
            string[] s = filename.Split('.');
            string houzhuiming = string.Empty;
            foreach (string s1 in s) houzhuiming = s1;
            if (houzhuiming == "exe" || houzhuiming == "EXE")
            {
                return EXEIcon.GetIcon(filename);
            }
            File.Create(filename + "dicon").Close();
            Image img = Icon.ExtractAssociatedIcon(filename).ToBitmap();
            File.Delete(filename + "dicon");
            return img;
        }

        private void ShowForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = false;
            int value = 0;
            //行
            if (e.Modifiers.CompareTo(Keys.Control) == 0)
            {
                if (flag == true) return;
                value += 10;
                flag = true;
            }
            if (e.Modifiers.CompareTo(Keys.Shift) == 0)
            {
                if (flag == true) return;
                value += 20;
                flag = true;
            }
            if (e.Modifiers.CompareTo(Keys.Alt) == 0)
            {
                if (flag == true) return;
                value += 30;
                flag = true;
            }
            flag = false;
            //列
            if (e.KeyCode == Keys.D0)
            {
                if (flag == true) return;
                value += 0;
                flag = true;
            }
            if (e.KeyCode == Keys.D1)
            {
                if (flag == true) return;
                value += 1;
                flag = true;
            }
            if (e.KeyCode == Keys.D2)
            {
                if (flag == true) return;
                value += 2;
                flag = true;
            }
            if (e.KeyCode == Keys.D3)
            {
                if (flag == true) return;
                value += 3;
                flag = true;
            }
            if (e.KeyCode == Keys.D4)
            {
                if (flag == true) return;
                value += 4;
                flag = true;
            }
            if (e.KeyCode == Keys.D5)
            {
                if (flag == true) return;
                value += 5;
                flag = true;
            }
            if (e.KeyCode == Keys.D6)
            {
                if (flag == true) return;
                value += 6;
                flag = true;
            }
            if (e.KeyCode == Keys.D7)
            {
                if (flag == true) return;
                value += 7;
                flag = true;
            }
            if (e.KeyCode == Keys.D8)
            {
                if (flag == true) return;
                value += 8;
                flag = true;
            }
            if (e.KeyCode == Keys.D9)
            {
                if (flag == true) return;
                value += 9;
                flag = true;
            }
            if (value >= Global.l.Count()) return;
            if (flag == false) return;
            Process.Start(Global.l[value].path);
            Hide();
        }

        private void ShowForm_Load(object sender, EventArgs e)
        {
            Size = new Size(1050, 470);

            if (File.Exists(Application.StartupPath + @"\timg.jpg") == true)
            {
                BackgroundImage = Image.FromFile(Application.StartupPath + @"\timg.jpg");
                c = PictureColor.ComputeBitmapColor((Bitmap)BackgroundImage);
            }
            if (File.Exists(Application.StartupPath + @"\timg.png") == true)
            {
                BackgroundImage = Image.FromFile(Application.StartupPath + @"\timg.png");
                c = PictureColor.ComputeBitmapColor((Bitmap)BackgroundImage);
            }
            if (File.Exists(Application.StartupPath + @"\timg.gif") == true)
            {
                BackgroundImage = Image.FromFile(Application.StartupPath + @"\timg.gif");
                c = PictureColor.ComputeBitmapColor((Bitmap)BackgroundImage);
            }
            ResetPictureBox();
            LoadList();
            label1.ForeColor = c;
            label3.ForeColor = c;
            label4.ForeColor = c;
            label5.ForeColor = c;
            label6.ForeColor = c;
        }

        private void ShowForm_Shown(object sender, EventArgs e)
        {

            //Size = new Size(pic[9, 3].Left + pic[9,3].Width + 18, pic[9, 3].Top + pic[9,3].Height + 21);
        }
    }
}
