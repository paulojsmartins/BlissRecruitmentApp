using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlissRecruitmentApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ThreadPool.QueueUserWorkItem(ThreadF);//Run Thread to look to connection status
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }

        static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        static void ThreadF(Object stateInfo)
        {
            while (1 == 1) {
                
                while (!CheckForInternetConnection())
                {//Show Dialog while connection is lost
                    DialogResult dr = MessageBox.Show("Reconnect?",
                    "Connection Lost",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
                Thread.Sleep(1500);
            }
        }
    }
}
