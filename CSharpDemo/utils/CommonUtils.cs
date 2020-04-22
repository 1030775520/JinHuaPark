using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Web;

namespace CSharpDemo
{
    //常用的工具类
    class CommonUtils
    {
        //字符串转字节数组gb2312编码
        public static byte[] stringToByteGB2312(string InString)
        {
            
            return System.Text.Encoding.GetEncoding("gb2312").GetBytes(InString);
        }

        //字符串转字节数组Utf8编码
        public static byte[] stringToByteUtf8(string InString)
        {

            return System.Text.Encoding.UTF8.GetBytes(InString);
        }


        //字符串转字节数组Utf8编码
        public static byte[] stringToByte(string InString)
        {

            return System.Text.Encoding.Default.GetBytes(InString);
        }

        //字节数组转字符串
        public static string byteToString(byte[] byteArray)
        {
            return System.Text.Encoding.Default.GetString(byteArray);
        }


        //utf-8转成gb2312
        public static string utf8ToGb2312(string text)
        {
            byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(text);
            bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.GetEncoding("GB2312"), bs);
            return Encoding.GetEncoding("GB2312").GetString(bs);

        }

        //gb2312转成utf-8
        public static string gb2312ToUtf8(string gb2312)
        {
            byte[] bs = Encoding.GetEncoding("gb2312").GetBytes(gb2312);
            bs = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.GetEncoding("UTF-8"), bs);
            return Encoding.GetEncoding("UTF-8").GetString(bs);
        }

        //截取数组的前几位
        public static byte[] cutOutArray(byte[] bytes,int targetLength)
        {
            if(targetLength >= bytes.Length)
            {
                return bytes;
            }
            byte[] result = new byte[targetLength];
            for(int i=0; i < targetLength; i++)
            {
                result[i] = bytes[i];
            }
            return result;
        }

        //map转json字符串
        public static string mapToJson(Dictionary<string,string> map)
        {           
            string result = "{";
            string m = "\"";
            string colon = ":";
            string comma = ",";
            List<string> keys = new List<string>(map.Keys);
            for(int i = 0; i < keys.Count; i++)
            {
                //前面每行一个,
                if(i < keys.Count - 1)
                {
                    result += m + keys[i] + m + colon + m + map[keys[i]] + m + comma;
                }
                //最后一行用 }
                else
                {
                    result += m + keys[i] + m + colon + m + map[keys[i]] + m + "}";
                }
            }
            return result;
        }

        //封装数据
        public static string getRequestData(Dictionary<string, string> map)
        {

            StringBuilder param = new StringBuilder();
            string split = "&";
            param.Append(split);
            List<string> keys = new List<string>(map.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                //前面每行一个&
                if (i < keys.Count - 1)
                {
                    param.AppendFormat("{0}={1}", keys[i], map[keys[i]]);
                    param.Append(split);
                }
                //最后一行不用&
                else
                {
                    param.AppendFormat("{0}={1}", keys[i], map[keys[i]]);
                }
            }
            return param.ToString();
        }

        //图片转成base64
        public static string toBase64(IntPtr image, uint length)
        {
            byte[] bytes = new byte[length];
            Marshal.Copy(image, bytes, 0, (int)length);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        //得到base64
        public static string getBase64(string file)
        {
            string result = "";
            FileStream fs = new FileStream(file, FileMode.Open);
            Image img = Image.FromStream(fs);
            MemoryStream ms1 = new MemoryStream();
            img.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr1 = new byte[ms1.Length];
            ms1.Position = 0;
            ms1.Read(arr1, 0, (int)ms1.Length);
            ms1.Close();
            fs.Close();
            string base64 = Convert.ToBase64String(arr1);
            result = Convert.ToBase64String(arr1);
            return result;
        }

        //base64转成图片保存到当前文件夹并返回名字
        public static byte[] DecodeBase64(string code, string imageName)
        {
            byte[] bytes = Convert.FromBase64String(code);

            MemoryStream m = new MemoryStream(bytes);
            FileStream fileStream = null;
            //地址
            //string pathName = @"E:\issues\" + name;//ConfigurationManager.AppSettings["wx_url"] + "uploadfiles_small";
            string pathName = imageName;

            fileStream = new FileStream(pathName, FileMode.Create);

            m.WriteTo(fileStream);
            fileStream.Flush();//清出缓冲区
            fileStream.Close();//关闭流
            m.Close();
            return System.Text.Encoding.Default.GetBytes(imageName);
        }

        //通过value得到key
        public static string getKeyByValue(Dictionary<string,int> dic,int value)
        {
            if(null != dic && dic.Values.Count > 0)
            {
                foreach(string key in dic.Keys)
                {
                    if(value == dic[key])
                    {
                        return key;
                    }
                }
            }
            return "";
        }

        //通过value得到key
        public static string getKeyByValue(Dictionary<string, string> dic, string value)
        {
            if (null != dic && dic.Values.Count > 0)
            {
                foreach (string key in dic.Keys)
                {
                    if (value == dic[key])
                    {
                        return key;
                    }
                }
            }
            return "";
        }

        //得到md5
        public static string getMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        //得到uuid
        public static string getUUID()
        {
            string id = System.Guid.NewGuid().ToString("N");
            return id;
        }

        //字符串转16进制 字节数组
        public static byte[] getHex(string str)
        {
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str);
            byte[] result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = "0x" + bytes[i].ToString("X");
                result[i] =(byte)bytes[i];

            }
            return result;
        }

        //16进制取反运算
        public static string getReverseHex(string hex)
        {
            int d = Convert.ToInt32(hex, 16);//16进制的23
            d = ~d;
            string result = d.ToString("X"); // 输出十六进制
            return result.Remove(0,6);  // 输出十六进制
        }

        

        //删除文件
        public static void deleteFile(List<string> files)
        {
            if(null != files && files.Count > 0)
            {
                foreach(var file in files)
                {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                }
            }
        }

        //播放自定义语音
        public static void playVedio(string content)
        {
            SpeechSynthesizer voice = new SpeechSynthesizer();   //创建语音实例
            voice.Rate = 1; //设置语速,[-10,10]
            voice.Volume = 100; //设置音量,[0,100]
                                //voice.SpeakAsync("Hellow Word");  //播放指定的字符串,这是异步朗读

            //下面的代码为一些SpeechSynthesizer的属性，看实际情况是否需要使用

            //voice.SpeakAsyncCancelAll();  //取消朗读
            voice.Speak(content);  //同步朗读
            //voice.Pause();  //暂停朗读
            //voice.Resume(); //继续朗读
            voice.Dispose();  //释放所有语音资源
        }

        //static void Main()
        //{
        //    string s = CommonUtils.getMD5("浙B99999");
        //}
        //得到有效长度
        public static ushort getLength(byte[] data)
        {
            for(int i = 0; i < data.Length; i++)
            {
                if(data[i] == 0 && data[i+1] == 0)
                {
                    return (ushort)i;
                }
            }
            return 100;
        }

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        //存日志 
        public static void saveLog(string content)
        {
            LogWriteLock.EnterWriteLock();
            try
            {
                DateTime dateTime = DateTime.Now;
                string year = dateTime.ToString("yyyy");
                string month = dateTime.ToString("MM");
                string day = dateTime.ToString("dd");
                string name = year + "-" + month + "-" + day + ".txt";
                string filePath = Constant.logPath + "\\" + name;
                if (File.Exists(filePath) == false)
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                // 写入文件内容
                File.AppendAllText(filePath, "【" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + content + "\r\n", Encoding.Default);
            }
            catch(Exception e)
            {

            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }
        }

        //得到车辆类型 0其他车  1小型车 2大型车 
        public static string getVehType(byte type)
        {
            if(1 == type)
            {
                return "2";
            }
            else if(3 == type)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        //得到socket消息
        public static byte[] getSocketMessage(Object obj)
        {
            return CommonUtils.stringToByteUtf8(JsonConvert.SerializeObject(obj) + "\r\n");
        }

        //10进制转16进制
        public static byte[] tenToHex(int ten)
        {
            byte[] result = new byte[10];
            return result;
        }
    }
}
