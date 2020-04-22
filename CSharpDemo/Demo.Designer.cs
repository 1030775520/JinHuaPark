namespace CSharpDemo
{
    partial class Demo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label36 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button42 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.button49 = new System.Windows.Forms.Button();
            this.button50 = new System.Windows.Forms.Button();
            this.logContent = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(548, 204);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "相机参数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "相机IP地址";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(1157, 420);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(198, 146);
            this.label36.TabIndex = 46;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // button42
            // 
            this.button42.Location = new System.Drawing.Point(1167, 24);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(75, 23);
            this.button42.TabIndex = 0;
            this.button42.Text = "屏幕控制";
            this.button42.UseVisualStyleBackColor = true;
            this.button42.Click += new System.EventHandler(this.Button42_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(1167, 153);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(83, 23);
            this.button11.TabIndex = 59;
            this.button11.Text = "模拟请求出场";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // button43
            // 
            this.button43.Location = new System.Drawing.Point(1167, 107);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(75, 23);
            this.button43.TabIndex = 60;
            this.button43.Text = "白名单设置";
            this.button43.UseVisualStyleBackColor = true;
            this.button43.Click += new System.EventHandler(this.Button43_Click_1);
            // 
            // button48
            // 
            this.button48.Location = new System.Drawing.Point(1167, 254);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(75, 23);
            this.button48.TabIndex = 61;
            this.button48.Text = "RS485";
            this.button48.UseVisualStyleBackColor = true;
            this.button48.Click += new System.EventHandler(this.Button48_Click);
            // 
            // button49
            // 
            this.button49.Location = new System.Drawing.Point(1167, 304);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(75, 23);
            this.button49.TabIndex = 62;
            this.button49.Text = "语音";
            this.button49.UseVisualStyleBackColor = true;
            this.button49.Click += new System.EventHandler(this.Button49_Click);
            // 
            // button50
            // 
            this.button50.Location = new System.Drawing.Point(1167, 196);
            this.button50.Name = "button50";
            this.button50.Size = new System.Drawing.Size(75, 23);
            this.button50.TabIndex = 63;
            this.button50.Text = "颜色";
            this.button50.UseVisualStyleBackColor = true;
            this.button50.Click += new System.EventHandler(this.Button50_Click);
            // 
            // logContent
            // 
            this.logContent.Location = new System.Drawing.Point(206, 92);
            this.logContent.Multiline = true;
            this.logContent.Name = "logContent";
            this.logContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logContent.Size = new System.Drawing.Size(705, 554);
            this.logContent.TabIndex = 64;
            this.logContent.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1167, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 65;
            this.button1.Text = "线程";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1167, 374);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 66;
            this.button2.Text = "万能语音";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1167, 440);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 67;
            this.button3.Text = "万能文字";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1167, 498);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 68;
            this.button4.Text = "万能广告";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1167, 542);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 69;
            this.button5.Text = "设置字体";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1167, 584);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 70;
            this.button6.Text = "查询版本";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1167, 622);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 71;
            this.button7.Text = "时间显示";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1355, 743);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logContent);
            this.Controls.Add(this.button50);
            this.Controls.Add(this.button49);
            this.Controls.Add(this.button48);
            this.Controls.Add(this.button43);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button42);
            this.Controls.Add(this.label36);
            this.MaximizeBox = false;
            this.Name = "Demo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "出入口管理系统";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Demo_FormClosed);
            this.Load += new System.EventHandler(this.Demo_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button42;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button43;
        private System.Windows.Forms.Button button48;
        private System.Windows.Forms.Button button49;
        private System.Windows.Forms.Button button50;
        private System.Windows.Forms.TextBox logContent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

