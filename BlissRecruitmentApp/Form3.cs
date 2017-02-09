using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Net.Mail;



namespace BlissRecruitmentApp
{
    public partial class Form3 : Form
    {
        int actualPage;
        int questionId;

        public Form3(int questionId, int pageNumber)
        {
            InitializeComponent();
            label4.Hide();
            this.getQuestionDetails(questionId);
            actualPage = pageNumber;
            this.questionId = questionId;
        }

        private void getQuestionDetails(int questionId)
        {
            string details;
            string url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/questions/" + questionId.ToString();
            Console.WriteLine(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                details = sr.ReadToEnd();
            }

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, dynamic>>(details);

            int i = 0;
            int j = 0;
            foreach (var item in dict["choices"])
            {
                string texto = item["choice"] + System.Environment.NewLine + item["votes"] + " votes";
                
                var radioB= new RadioButton();
                radioB.Text = texto;
                radioB.AutoSize = true;
                radioB.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);

                tableLayoutPanel1.Controls.Add(radioB, i, j);
                if (i < 1)
                {
                    i++;
                } else if (j < 1)
                {
                    j++;
                } else
                {
                    i--;
                }
                
            }

            label1.Text = dict["question"];
            label2.Text += dict["id"];
            string publishedDate = dict["published_at"];

            string[] words = publishedDate.Split('T');
            string date1 = words[0];
            string date2 = words[1].Split('.')[0];

            label3.Text += date1 + " " + date2;
        }

        private void shareLink(string email)
        {
            string re;
            string url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/share?" + email + '&' + "https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/questions/" + this.questionId;
            Console.WriteLine(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                re = sr.ReadToEnd();
            }

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, dynamic>>(re);
            if(dict["status"] == "OK")
            {
                MessageBox.Show("Shared successfully.");
            } else
            {
                MessageBox.Show("Shared unsuccessfully",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            }

        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton) sender;
            var newLines = new Regex(@"\r\n|\n|\r", RegexOptions.Singleline);
            var choice = newLines.Split(r.Text);
            voteOn(choice[0]);
        }

        private void voteOn(string choice)
        {
            //Existe API p/ votar?
            label4.Text = "You voted on " + choice + " successfully.";
            label4.Show();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 listScreen = new Form2(actualPage);
            listScreen.Show();
        }
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Write your e-mail to Share",
                    "Empty Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            } else {
                shareLink(textBox1.Text);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
