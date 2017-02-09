using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BlissRecruitmentApp
{
    public partial class Form2 : Form
    {
        String questions;
        public Form2(int pageNumber)
        {
            InitializeComponent();
            if (pageNumber == 1)
            {
                button2.Hide();
            }
            listQuestions(pageNumber);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void listQuestions(int pageNumber)
        {

            string offset = (10 * pageNumber).ToString();
            label1.Text = "Page " + pageNumber.ToString();
            string url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/questions?10&" + offset;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                questions = sr.ReadToEnd();

            }

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<List<Dictionary<string, dynamic>>>(questions);
            listBox1.Items.Clear();

            foreach (Dictionary<string, dynamic> item in dict)
            {
                listBox1.Items.Add(item["question"]);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageNumber = getPageNumber();
            int questionId = (listBox1.SelectedIndex + 1) * pageNumber;

            this.Hide();
            Form3 detailScreen = new Form3(questionId, pageNumber);
            detailScreen.Show();
        }

        private int getPageNumber()
        {
            string[] pageLabel = label1.Text.Split(' ');
            int pageNumber = Int32.Parse(pageLabel[pageLabel.Length - 1]);
            return pageNumber;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            int pageNumber = getPageNumber();
            label1.Text = "Page " + (pageNumber + 1).ToString();
            listQuestions(pageNumber + 1);
            if(pageNumber == 1)
            {
                button2.Show();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int pageNumber = getPageNumber();
            label1.Text = "Page " + (pageNumber - 1).ToString();
            listQuestions(pageNumber - 1);

            if (pageNumber == 2)
            {
                button2.Hide();
            }

        }

    }
}
