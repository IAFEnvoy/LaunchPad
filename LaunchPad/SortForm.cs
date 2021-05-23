using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LaunchPad
{
    public partial class SortForm : Form
    {
        public SortForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ListViewItem lvi = new ListViewItem(fbd.SelectedPath);
                lvi.Tag = "文件夹";
                listView1.Items.Add(lvi);
            }
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择文件（支持快捷方式）";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                if (fi.Extension == ".lnk" || fi.Extension == ".LNK")
                {
                    ListViewItem lvi = new ListViewItem(GetInkFile(ofd.FileName));
                    lvi.Tag = "文件";
                    listView1.Items.Add(lvi);
                }
                if (fi.Extension == ".exe")
                {
                    ListViewItem lvi = new ListViewItem(ofd.FileName);
                    lvi.Tag = "文件";
                    listView1.Items.Add(lvi);
                }
                else
                {
                    ListViewItem lvi = new ListViewItem(ofd.FileName);
                    lvi.Tag = "文件";
                    listView1.Items.Add(lvi);
                }
            }
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }
        private string GetInkFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                WshShell shell = new WshShell();
                IWshShortcut lws = (IWshShortcut)shell.CreateShortcut(filename);
                return lws.TargetPath;
            }
            else
            {
                return "";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                if (listView1.SelectedIndices[0] == 0)
                    button3.Enabled = false;
                if (listView1.SelectedIndices[0] == listView1.Items.Count - 1)
                    button4.Enabled = false;
            }
        }

        private void SortForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Global.l.Count(); i++)
            {
                ListViewItem lvi = new ListViewItem(Global.l[i].path);
                lvi.Tag = Global.l[i].type;
                listView1.Items.Add(lvi);
            }
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int a = listView1.SelectedIndices[0];
            listView1.Items.RemoveAt(a);
            if (listView1.Items.Count > 0)
            {
                listView1.SelectedIndices.Clear();
                if (listView1.Items.Count == a) listView1.SelectedIndices.Add(a - 1);
                else listView1.SelectedIndices.Add(a);
                listView1.Focus();
            }
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int a = listView1.SelectedIndices[0];
            ListViewItem lvi = listView1.Items[a];
            ListViewItem lvi2 = listView1.Items[a - 1];
            listView1.Items[a - 1] = new ListViewItem("222");
            listView1.Items[a] = new ListViewItem("111");
            listView1.Items[a - 1] = lvi;
            listView1.Items[a] = lvi2;
            listView1.SelectedIndices.Clear();
            listView1.SelectedIndices.Add(a - 1);
            listView1.Focus();
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int a = listView1.SelectedIndices[0];
            ListViewItem lvi = listView1.Items[a];
            ListViewItem lvi2 = listView1.Items[a + 1];
            listView1.Items[a + 1] = new ListViewItem("222");
            listView1.Items[a] = new ListViewItem("111");
            listView1.Items[a] = lvi2;
            listView1.Items[a + 1] = lvi;
            listView1.SelectedIndices.Clear();
            listView1.SelectedIndices.Add(a + 1);
            listView1.Focus();
            listView1_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Global.l.Clear();
            foreach (ListViewItem lvi in listView1.Items)
            {
                Types.ItemP tp = new Types.ItemP();
                tp.path = lvi.Text;
                tp.type = (string)lvi.Tag;
                Global.l.Add(tp);
            }
            Hide();
        }
    }
}
