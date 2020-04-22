using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CSharpDemo
{
    //常量类
    class Constant
    {
        public static string connectStr = "server=127.0.0.1;port=3306;database=car;user=root;password=password"; // 用户名和密码在MySQL定义的
        public static string strPlatFormName = "";              //平台软件名称
        public static string strServerIP = "127.0.0.1";         //服务器IP
        public static string strDatabaseName = "car";  //数据库名称
        public static string strDatabaseUserName = "root";      //数据库用户名
        public static string strDatabasePassWord = "password";      //数据库密码

        //检测模式
        public static byte SNAP_MODE_MANUAL = 1;//手动
        public static byte SNAP_MODE_VIDEO = 2;//视频
        public static byte SNAP_MODE_LOOP = 3;//线圈

        //云平台域名
        public static String domainName = "http://api.iparking.jsydu.com";

        //云平台请求入场地址
        public static string REQUEST_ENTER_ADDRESS = domainName + "/unmanned/vehIn";

        //云平台请求出场地址
        public static string REQUEST_OUT_ADDRESS = domainName + "https://v1.api.iparking.tech/index.php/barrier/veh_out";

        //云平台过车推送(出入口)地址                     
        public static string PASS_SEND_ADDRESS = domainName + "https://v1.api.iparking.tech/index.php/barrier/veh_in_pass";

        //云平台etc请求入场地址                     
         public static string etc_request_address = "";

        //mac地址
        public static string MAC_ADDRESS = "52:15:76:BB:86:9E";

        //appid  正式 bd84568fa28c0865  测试 e154d6ede2627e74
        public static string appid = "e154d6ede2627e74";

        //appkey 正式 9042f6ee6c1c86655a4a25b3538c05a4  测试 22bc941693cf5eccd728f9d52daa2542
        public static string appkey = "22bc941693cf5eccd728f9d52daa2542";

        //出口相机ip
        public static List<string> outIps = new List<string>();

        //入口相机ip
        public static List<string> inIps = new List<string>();

        //成功code
        public static int success = 0;

        //开闸
        public static int open = 1;

        //是否要重新连接云平台socket
        public static bool isNeedReconnect = false;

        //socket ip
        public static string socketIp = "";

        //socket port
        public static int socketPort = 1;

        //停车场code
        public static string pkl_code = "204001";

        //设备终端号
        public static string dev_id = "";

        public static string imageCode = "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";
            //ip和通道号的对应关系
        public static Dictionary<string, string> ipAndPass = new Dictionary<string, string>();

        //ip和etc盒子的对应关系
        public static Dictionary<string, string> ipAndEtcIp = new Dictionary<string, string>();

        //etc盒子秘钥
        public static string etc_key = "";

        //流水总数
        public static int n_cnt;

        //总金额 (一天的)
        public static double d_toll;

        //软件版本
        public static string pklt_version = "1.0.0_C1E0";

        //图片
        public static string image_path = System.Windows.Forms.Application.StartupPath + "\\image";

        //日志路径
        public static string logPath = System.Windows.Forms.Application.StartupPath + "\\log";

        //图片分割符
        public static string imageSplit = "&";

        //读取配置文件
        public  static void loadConfig() 
        {
            char[] split = "=".ToCharArray();
            string line = string.Empty;
            Dictionary<string, string> map = new Dictionary<string, string>();
            using(StreamReader reader = new StreamReader("config.ini",Encoding.GetEncoding("gb2312")))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] a = line.Split(split);
                    if(a.Length == 2)
                    {
                        map.Add(a[0], a[1]);
                    }
                    else if(a.Length == 4)
                    {
                        if (a[0].Contains("入口"))
                        {
                            inIps.Add(a[1]);
                        }
                        else if (a[0].Contains("出口"))
                        {
                            outIps.Add(a[1]);
                        }
                        if (!ipAndPass.ContainsKey(a[1]))
                        {
                            ipAndPass.Add(a[1], a[2]);
                        }
                        if (!ipAndEtcIp.ContainsKey(a[1]))
                        {
                            ipAndEtcIp.Add(a[1], a[3]);
                        }
                    }
                }
                reader.Close();
            }
            domainName = map["domain_name"];
            appid = map["app_id"];
            appkey = map["app_key"];
            pkl_code = map["pkl_code"];
            dev_id = map["dev_id"];
            pklt_version = map["pklt_version"];
            REQUEST_ENTER_ADDRESS = domainName + map["request_enter"];
            REQUEST_OUT_ADDRESS = domainName + map["request_out"];
            PASS_SEND_ADDRESS = domainName + map["pass_pull"];
            etc_request_address = domainName + map["etc_request_address"];
            socketIp = map["socket_ip"];
            socketPort = int.Parse(map["socket_port"]);
        }
}
}
