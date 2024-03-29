﻿using System;
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
                                        "winmgmt", "msdtc", "ctfmon", "dfssvc",
                                        "PresentationFontCache", "시스템 유효 시간 프로세스",
                                        "dwm", "services", "wininit",
                                        "dashost", "dllhost", "sihost", "fontdrvhost" }; // dwm 시각적 효과 부여 프로세스
                                        // fontdrvhost.exe 종료하면 한글 입력 및 출력 상태 이상해짐

        Process[] thisProcess = null;

        private void button_Click(object sender, EventArgs e)
        {
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            pictureBox2.Enabled = true;
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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (coolTimeValid)
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
                //Process.Start("explorer.exe");

                // CoolTime Evaluation
                pictureBox2.Enabled = false;
                timer.Enabled = true;
                coolTimeValid = false;
            }
        }
    }
}
