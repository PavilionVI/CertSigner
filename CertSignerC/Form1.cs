using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CertUtilities;

namespace CertSigner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "EXEs (*.exe)|*.exe";
            openFileDialog1.Title = "Select File To Steal Signature";
            openFileDialog1.ShowDialog();
            string x = openFileDialog1.FileName;
           textBox1.Text = x   ;
           
        }
        private void GetPointer(String ThePath)
        {
            if (checkBox1.Checked == true) 
            {
                byte[] TheCert = CertUtils.GetCert(ThePath, true);
                string SavFile = "";
                saveFileDialog1.ShowDialog();
                SavFile = saveFileDialog1.FileName;

                FileStream fs2 = new FileStream(SavFile, FileMode.Create);
                fs2.Write(TheCert, 0, TheCert.Length);
                fs2.Flush();
                fs2.Close();
            }
             else
            {
                byte[] TheCert = CertUtils.GetCert(ThePath, false);
                string SavFile = "";
                saveFileDialog1.ShowDialog();
                SavFile = saveFileDialog1.FileName;

                FileStream fs2 = new FileStream(SavFile, FileMode.Create);
                fs2.Write(TheCert, 0, TheCert.Length);
                fs2.Flush();
                fs2.Close();
            }


          
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            else
                GetPointer(textBox1.Text);
            
                           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        
    }
}
