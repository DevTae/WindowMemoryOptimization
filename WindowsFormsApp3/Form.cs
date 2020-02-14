using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        bool coolTimeValid = true;

        // 첫번째 필터링 대상 : 윈도우 기본 프로세스 
        String[] avoid_process_name = { "csrss", "lsass", "mstask",
                                        "smss", "spoolsv", "svchost", "System",
                                        "System Idle Process", "winlogon",
                                        "winmgmt", "msdtc", "ctfmon", "dfssvc" };

        Process[] thisProcess = null;

        private void button_Click(object sender, EventArgs e)
        {
            if(coolTimeValid)
            {
                // 자기 프로그램 현황 조사
                string path = Application.ExecutablePath;
                string[] token = path.Split(new char[] { '\\' });
                string exename = token[token.Length - 1];
                thisProcess = Process.GetProcessesByName(exename.Substring(0, exename.Length - 4));

                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    try
                    {
                        bool isChecked = true;

                        // 윈도우 기본 프로세스 필터링
                        foreach (String processname in avoid_process_name)
                            if (processname == process.ProcessName) isChecked = false;

                        // 자기 프로그램인지 확인
                        foreach (Process proc in thisProcess)
                            if (process.ProcessName == proc.ProcessName) isChecked = false;

                        if (isChecked) process.Kill();
                    }
                    catch (Exception exception)
                    {
                        continue;
                    }
                }
                Process.Start("explorer.exe");

                // CoolTime Evaluation
                button.Enabled = false;
                button.Text = "Cool..";
                timer.Enabled = true;
                coolTimeValid = false;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            button.Enabled = true;
            button.Text = "Reboot";
            coolTimeValid = true;
            timer.Enabled = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://ahdelron.tistory.com/");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://vandp.tistory.com/");
        }
    }
}
