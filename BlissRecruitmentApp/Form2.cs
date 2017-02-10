using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BlissRecruitmentApp
{
    public partial class Form2 : Form
    {
        
        public Form2(int pageNumber, string filter)
        {
            InitializeComponent();
            if (pageNumber == 1)
            {
                button2.Hide();
            }
            label2.Hide();
            listQuestions(pageNumber, filter);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void listQuestions(int pageNumber, string filter)
        {
            String questions;
            string offset = (10 * pageNumber).ToString();
            label1.Text = "Page " + pageNumber.ToString();
            string url;
            if(filter.Equals("")) {
                url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/questions?10&" + offset;
            } else
            {
                if (!label2.Visible) { 
                    textBox1.Text = filter;
                    label2.Text += filter + "'";
                    label2.Show();
                }
                url = @"https://private-anon-d7d42d7c8b-blissrecruitmentapi.apiary-mock.com/questions?10&" + offset + '&' + filter;
            }

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
        {//QUESTION CLICK
            int pageNumber = getPageNumber();
            int questionId = (listBox1.SelectedIndex + 1) * pageNumber;
            string filter = textBox1.Text;
            this.Hide();
            Form3 detailScreen = new Form3(questionId, pageNumber, filter);
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
        { //NEXT PAGE
            string filterText = textBox1.Text;

            int pageNumber = getPageNumber();
            label1.Text = "Page " + (pageNumber + 1).ToString();
            listQuestions(pageNumber + 1, filterText);
            if(pageNumber == 1)
            {
                button2.Show();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {//PREVIOUS PAGE
            string filterText = textBox1.Text;

            int pageNumber = getPageNumber();
            label1.Text = "Page " + (pageNumber - 1).ToString();
            listQuestions(pageNumber - 1, filterText);

            if (pageNumber == 2)
            {
                button2.Hide();
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        { //SEARCH
            string filterText = textBox1.Text; 
            this.listQuestions(1, filterText);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        { //REFRESH
            textBox1.Text = "";
            label2.Hide();
            label2.Text = "Search Results for '";
            this.listQuestions(1, "");
        }
    }
}
