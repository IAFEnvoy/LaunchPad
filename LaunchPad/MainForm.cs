using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LaunchPad
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        Hotkeys hotKsys = new Hotkeys();

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadList();
            }
            catch
            {
                retryer.Enabled = true;
            }
            hotKsys.Regist(this.Handle, (int)Hotkeys.Modifiers.Alt, Keys.Z, ShowMainForm);//给打开窗体事件注册Alt+Z
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
        void ShowMainForm()
        {
            Form f=new ShowForm();
            f.Show();
            f.Activate();
            while (f.Visible == true) Application.DoEvents();
            f.Dispose();
        }
        protected override void WndProc(ref Message m)
        {
            hotKsys.ProcessHotKey(m);
            base.WndProc(ref m);
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hotKsys.UnRegist(this.Handle, ShowMainForm);//取消注册Alt+Z
            Program.WriteList();
            System.Environment.Exit(0);
        }

        private void 排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f1 = new SortForm();
            f1.Show();
            while (f1.Visible == true) Application.DoEvents();
            f1.Dispose();
            LoadList();
        }
        void LoadList()
        {
            listView1.Items.Clear();
            for (int i = 0; i < Global.l.Count(); i++)
            {
                ListViewItem lvi;
                if (Global.l[i].type=="文件夹") lvi = new ListViewItem(Global.l[i].path,0);
                else lvi = new ListViewItem(Global.l[i].path, Getfileicon(Global.l[i].path));
                lvi.Tag = Global.l[i].type;
                listView1.Items.Add(lvi);
            }
        }
        private int Getfileicon(string filename)
        {
            string[] s = filename.Split('.');
            if (s.Length == 1) return 1;
            string houzhuiming = string.Empty;
            foreach (string s1 in s) houzhuiming = s1;
            if (houzhuiming == "exe" || houzhuiming == "EXE")
            {
                for (int i = 0; i < Icons.Images.Count; i++)
                {
                    if (Icons.Images.Keys[i] == filename) return i;
                }
                Icons.Images.Add(filename, EXEIcon.GetIcon(filename));
                return Icons.Images.Count - 1;
            }
            for (int i = 0; i < Icons.Images.Count; i++)
            {
                if (Icons.Images.Keys[i] == houzhuiming) return i;
            }
            string fileName = "tmp." + houzhuiming;
            File.Create(fileName).Close();
            Image img = System.Drawing.Icon.ExtractAssociatedIcon(fileName).ToBitmap();
            File.Delete(fileName);
            Icons.Images.Add(houzhuiming, img);
            return Icons.Images.Count - 1;
        }

        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hotKsys.UnRegist(this.Handle, ShowMainForm);//取消注册Alt+Z
            Program.WriteList();
            System.Environment.Exit(0);
        }

        private void 保存当前排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存排序到。。。";
            sfd.Filter = "*.px|*.px";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                sw.WriteLine(Global.l.Count().ToString());
                for (int i = 0; i < Global.l.Count(); i++)
                {
                    sw.WriteLine(Global.l[i].path);
                    sw.WriteLine(Global.l[i].type);
                }
                sw.Close();
            }
        }

        private void 加载排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult re = MessageBox.Show("是否保存当前排序？", "", MessageBoxButtons.YesNoCancel);
            if (re == DialogResult.Cancel) return;
            if (re == DialogResult.Yes) 保存当前排序ToolStripMenuItem_Click(sender, e);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "加载排序";
            ofd.Filter = "*.px|*.px";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Global.l.Clear();
                StreamReader sr = new StreamReader(ofd.FileName);
                int cnt = int.Parse(sr.ReadLine());
                for (int i = 0; i < cnt; i++)
                {
                    Types.ItemP ip = new Types.ItemP();
                    ip.path = sr.ReadLine();
                    ip.type = sr.ReadLine();
                    Global.l.Add(ip);
                }
            }
        }

        private void 映射文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult re = MessageBox.Show("是否保存当前排序？", "", MessageBoxButtons.YesNoCancel);
            if (re == DialogResult.Cancel) return;
            if (re == DialogResult.Yes) 保存当前排序ToolStripMenuItem_Click(sender, e);

        }

        private void Retryer_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadList();
            }
            catch
            {
                retryer.Enabled = true;
                return;
            }
            retryer.Enabled = false;
        }
    }
}
