using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LaunchPad
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ReadList();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            WriteList();
        }
        public static void ReadList()
        {
            if(File.Exists(Application.StartupPath + @"\list") == false) return;
            StreamReader sr = new StreamReader(Application.StartupPath + @"\list");
            int cnt = int.Parse(sr.ReadLine());
            for (int i = 0; i < cnt; i++)
            {
                Types.ItemP ip = new Types.ItemP();
                ip.path = sr.ReadLine();
                ip.type = sr.ReadLine();
                Global.l.Add(ip);
            }
            sr.Close();
        }
        public static void WriteList()
        {
            StreamWriter sw = new StreamWriter(Application.StartupPath + @"\list");
            sw.WriteLine(Global.l.Count().ToString());
            for(int i=0;i< Global.l.Count(); i++)
            {
                sw.WriteLine(Global.l[i].path);
                sw.WriteLine(Global.l[i].type);
            }
            sw.Close();
        }
    }
}
