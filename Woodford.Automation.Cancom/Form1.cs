using FluentFTP;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Woodford.Automation.Cancom {
    public partial class Form1 : Form {


        //        ftp.woodford.co.za
        //Username: Fines
        //Password: F1n3s123

        private static ISchedulerFactory schedFact = new StdSchedulerFactory();

        private static IScheduler sched;
        private int hour = 18;
        private int min = 00;

        public Form1() {
            InitializeComponent();
            hour = Convert.ToInt32(ConfigurationSettings.AppSettings["runatHour"]);
            min = Convert.ToInt32(ConfigurationSettings.AppSettings["runatMinute"]);
        }

        private void Form1_Load(object sender, EventArgs e) {
            Console.WriteLine("Closing all chrome instances...");

            Process[] chromeInstances = Process.GetProcessesByName("chrome");

            foreach (Process p in chromeInstances)
                p.Kill();

            Console.WriteLine("Chrome intances closed");

            Console.WriteLine("Pause App...");
            Thread.Sleep(2000);
            Console.WriteLine("Un-Pause App");

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            Console.WriteLine("Starting Automation...");



            Console.WriteLine("Starting Job Scheduler...");

            sched = schedFact.GetScheduler();


            IJobDetail processReport = JobBuilder.Create<ProcessReportJob>()
                 .WithIdentity("automation", "processReport")
                 .Build();

            ITrigger processReportTrigger = TriggerBuilder.Create()
                .WithIdentity("processReportTrigger", "automation")
                .StartAt(DateBuilder.TodayAt(hour, min, 0))
                .WithSimpleSchedule(x => x
                        .WithIntervalInHours(24)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(processReport, processReportTrigger);

            sched.Start();

            Console.WriteLine(string.Format("Job Scheduler Started: Job to run at {0}:{1}", hour, min));

        }

    }

}
