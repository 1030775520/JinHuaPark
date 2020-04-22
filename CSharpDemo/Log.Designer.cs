using System.Drawing;
using System.Windows.Forms;

namespace CSharpDemo
{
    partial class Log
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.content = new System.Windows.Forms.Label();
            this.tb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "W";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // content
            // 
            this.content.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.content.Location = new System.Drawing.Point(222, 32);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(229, 91);
            this.content.TabIndex = 1;
            // 
            // tb
            // 
            this.tb.Location = new System.Drawing.Point(222, 168);
            this.tb.Multiline = true;
            this.tb.Name = "tb";
            this.tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb.Size = new System.Drawing.Size(208, 130);
            this.tb.TabIndex = 2;
            this.tb.WordWrap = false;
            // 
            // Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tb);
            this.Controls.Add(this.content);
            this.Controls.Add(this.button1);
            this.Name = "Log";
            this.Text = "W";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics grp = e.Graphics;
            grp.DrawString("string", this.Font, Brushes.AliceBlue, 0, 0);
        }

        #endregion

        private Button button1;
        private Label content;
        private TextBox tb;
    }
}