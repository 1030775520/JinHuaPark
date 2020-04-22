using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSharpDemo;

namespace CSharpDemo
{
    public partial class ModifyIP : Form
    {
        public ModifyIP()
        {
            InitializeComponent();
        }

        private static MyClass.T_NetSetup tNetSetup;

        private string _strIp;
        internal string strIp
        {
            get { return _strIp; }
            set { _strIp = value; }
        }

        private int _nCamIdIn;
        internal int nCamIdIn
        {
            get { return _nCamIdIn; }
            set { _nCamIdIn = value; }
        }

        public static uint IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        public static string IntToIp(uint ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

        private void ModifyIP_Load(object sender, EventArgs e)
        {
            int iRet = MyClass.Net_QueryNETSetup(nCamIdIn, ref tNetSetup);
            if (iRet != 0)
            {
                MessageBox.Show("查询相机网络信息失败!", "提示");
                this.Close();
                return;
            }

            labelOldIp.Text = IntToIp(tNetSetup.uiIPAddress);
            textBoxNewIp.Text = labelOldIp.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            do 
            {
                if (textBoxNewIp.Text == labelOldIp.Text)
                {
                    MessageBox.Show("你没有修改IP!", "提示");
                    break;
                }

                _strIp = textBoxNewIp.Text;

                uint uiIPAddress = tNetSetup.uiIPAddress;
                tNetSetup.uiIPAddress = IpToInt(_strIp);

                int iRet = MyClass.Net_NETSetup(_nCamIdIn, ref tNetSetup);
                if (iRet != 0)
                {
                    tNetSetup.uiIPAddress = uiIPAddress;
                    MessageBox.Show("设置相机IP失败!", "提示");
                    break;
                }

                DialogResult = DialogResult.OK;

            } while (false);

        }
    }
}