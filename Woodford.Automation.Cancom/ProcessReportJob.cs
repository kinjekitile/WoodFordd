using FluentFTP;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Woodford.Automation.Cancom {
    public class ProcessReportJob : IJob {
        //static string ftpUsername = "administrator"; //"Fines";
        //static string ftpPassword = "NM7esUtw"; //"F1n3s123";
        //static string ftpAddress = "41.76.213.8"; //"ftp.woodford.co.za";
        //static string ftpFolder = "woodford"; //"Fines";

        static string ftpUsername = "Fines";
        static string ftpPassword = "F1n3s123";
        static string ftpAddress = "ftp.woodford.co.za";
        static string ftpFolder = "Fines";

        string pathToWatch = @"C:\Users\Oliver\Downloads";
        private static Process chromeProcess = null;

        public ProcessReportJob() {
            ftpAddress = ConfigurationManager.AppSettings["ftpAddress"];
            ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
            ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            pathToWatch = ConfigurationManager.AppSettings["downloadPath"];
        }
        public void Execute(IJobExecutionContext context) {
            
            Console.WriteLine("Process Report Job Started");


            Console.WriteLine("Starting Chrome...");
            chromeProcess = Process.Start("chrome.exe", "https://www.tsdrms.net/");
            Console.WriteLine("Chrome Started");


            Console.WriteLine("Starting File System Watcher...");
            Console.WriteLine("Watching folder: " + pathToWatch);
            FileSystemWatcher watcher = new FileSystemWatcher();

            //Set the filter to all files.
            watcher.Filter = "*.*";

            //Subscribe to the Created event.
            //watcher.Created += new FileSystemEventHandler(watcher_FileCreated);
            watcher.Renamed += new RenamedEventHandler(watcher_FileRenamed);
            watcher.Changed += new FileSystemEventHandler(watcher_FileChanged);
            //Set the path 
            watcher.Path = pathToWatch;

            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("File System Watcher Started");
          
        }

        private static void watcher_FileChanged(object sender, FileSystemEventArgs e) {


            if (e.Name.StartsWith("RAReporting") && e.Name.EndsWith(".xslx")) {
                Console.WriteLine("File Changed: " + e.Name);
            }

        }

        private static void watcher_FileRenamed(object sender, FileSystemEventArgs e) {


            if (e.Name.StartsWith("RAReporting") && e.Name.EndsWith(".xlsx")) {
                Console.WriteLine("File Renamed to: " + e.Name);

                Console.WriteLine("Closing Chrome...");

                chromeProcess.Kill();

                Console.WriteLine("Chrome Closed");

                Console.WriteLine("Process Report Job Completed");

                SendFileToFTP(e.FullPath);
            }

        }

        private static void SendFileToFTP(string filePath) {
            Console.WriteLine("Creating FTP Client...");
            // create an FTP client
            FtpClient client = new FtpClient(ftpAddress);
            Console.WriteLine("FTP Client created");

            Console.WriteLine("Setting FTP Credentials...");
            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            Console.WriteLine("FTP Credentials set");

            Console.WriteLine("Connecting to FTP Server...");
            // begin connecting to the server
            client.Connect();
            Console.WriteLine("FTP Server Connected");


            Console.WriteLine("Uploading file to FTP...");

            string filename = Path.GetFileName(filePath);
            filename = "/cancomraw_" + DateTime.Today.ToString("ddMMyyyy") + ".xlsx";
            // upload a file
            client.UploadFile(filePath, filename);

            Console.WriteLine("File Upload Complete");

            Console.WriteLine("Confirm File Uploaded...");
            // check if a file exists
            if (client.FileExists(filename)) {
                Console.WriteLine("File Upload Confirmed");
            } else {
                Console.WriteLine("File Upload could NOT be confirmed");
            }

            Environment.Exit(0);
        }

        private static void watcher_FileCreated(object sender, FileSystemEventArgs e) {

            string sFile = e.FullPath;

            Console.WriteLine("File Created: " + sFile);


        }

    }
}
