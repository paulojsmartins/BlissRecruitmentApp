using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;


namespace BlissRecruitmentApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.getHealth();
        }

        private void getHealth()
        {
            string text;
            string url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/health";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, string>>(text);

            if (dict["status"] != "OK")
            {//Show Dialog to Retry connection
                DialogResult result1 = MessageBox.Show("Reconnect?",
                "Error Connecting to Bliss Recruitment App",
                MessageBoxButtons.YesNo);

                if (result1 == DialogResult.Yes)
                {
                    this.getHealth();
                }
            }
            else
            { //Health is "OK" 
                Form2 listScreen = new Form2(1, "");
                listScreen.Show();
                this.Hide();
            }
            
        }

        

    }
}
