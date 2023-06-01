using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sheep_injector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        [DllImport("C:\\Users\\lol\\source\\repos\\sheep-injector\\x64\\Debug\\sheep-injector-api.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static bool inject(int pid, string dllPath);

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    listDLLs.Items.Add(new ListViewItem(new string[] { ofd.FileName, "bleh :P" }));
                }
            }
        }

        private void tmrProcesses_Tick(object sender, EventArgs e)
        {
            // todo: make it update when a new process is started
            Process[] procs = Process.GetProcesses();

            foreach (ListViewItem item in listProcesses.Items)
            {
                if (procs.Where(p => p.Id == ((Process)item.Tag).Id).Count() == 0) 
                {
                    item.Remove();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs) 
            {
                ListViewItem item = new ListViewItem(new string[] { proc.ProcessName, proc.Id.ToString() });
                item.Tag = proc;
                listProcesses.Items.Add(item);
            }

            tmrProcesses.Start();
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            Process proc = (Process)(listProcesses.SelectedItems[0].Tag);
            inject(proc.Id, listDLLs.SelectedItems[0].Text);
        }
    }
}
