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
    public partial class add_rule : Form
    {
        public add_rule()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label12_Click(object sender, EventArgs e)
        {

        }

        private void RadioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        //改变规则类型
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBox2.Text;
            if ("按时收费".Equals(text))
            {
                time.Visible = true;
                count.Visible = false;
                split_time.Visible = false;
                day_night.Visible = false;            
            }
            else if ("按次收费".Equals(text))
            {                
                time.Visible = false;
                split_time.Visible = false;
                day_night.Visible = false;
                count.Visible = true;
            }
            else if ("分时收费".Equals(text))
            {                
                time.Visible = false;
                count.Visible = false;
                day_night.Visible = false;
                split_time.Visible = true;
            }
            else
            {               
                time.Visible = false;
                count.Visible = false;
                split_time.Visible = false;
                day_night.Visible = true;
            }
        }

        //取消
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
