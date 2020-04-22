using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpDemo
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.content.Text.Length.ToString()); 
            //this.content.Text += new Random().Next(0,100).ToString() + "\r\n";
            this.tb.Text += new Random().Next(0, 100).ToString() + "\r\n";
        }
    }
}
