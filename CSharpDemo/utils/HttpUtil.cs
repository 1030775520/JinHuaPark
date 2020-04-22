using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CSharpDemo
{
    //http工具类
    class HttpUtil
    {
        //发送http  get请求
        public static String GET(String share_url)
        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(share_url);//通过网页链接对网络请求类进行初始化
            request.Method = "GET";//设置请求方式为get
            request.AllowAutoRedirect = true;//允许网页重定向
            // request.Headers.Set("Content-Range", " bytes 0 - 126399 / 8065760");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0";
            //反爬虫的设置  设置浏览器标识

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//得到网页返回reponse对象
            Stream stream = response.GetResponseStream();//得到网页输出流  
            StreamReader read = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));//对返回的数据进行解码
            String nextline = "";
            String html = "";
            while ((nextline = read.ReadLine()) != null)//不断地读取输入流,读取网页源码
            {
                html += nextline;
            }
            read.Close();
            return html;//返回网页源码
        }

        //发送http post请求
        public static string Post(string strUrl, string strParam)
        {
            string strResult = "error";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(strUrl);
                Encoding encoding = Encoding.UTF8;
                //encoding.GetBytes(postData);
                StringBuilder param = new StringBuilder();
                param.Append("&");
                param.AppendFormat("{0}={1}", "app_id", Constant.appid);
                param.Append("&");
                param.AppendFormat("{0}={1}", "app_key", Constant.appkey);
                param.Append(strParam);
                byte[] bs = Encoding.UTF8.GetBytes(param.ToString());
                string responseData = System.String.Empty;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;
                
                try
                {
                    using (System.IO.Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                    using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)req.GetResponse())
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            responseData = reader.ReadToEnd().ToString();
                            strResult = responseData;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    strResult = "error：" + ex.Message;//返回异常信息
                }
            }
            catch (System.Exception ex)
            {
                strResult = "error：" + ex.Message;//返回异常信息
            }
            return strResult;
        }

        //发送http post请求 请求格式为json
        public static string PostOfJson(string strUrl, string strParam)
        {
            string responseTxt = string.Empty;
            try
            {
                var memStream = new MemoryStream();
                var cc = Encoding.UTF8.GetBytes(strParam);
                var request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "POST";
                request.ContentType = "application/json";

                memStream.Write(cc, 0, cc.Length);

                request.ContentLength = memStream.Length;

                var requestStream = request.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) //Encoding.GetEncoding("gb2312")
                {
                    responseTxt = reader.ReadToEnd();
                }
                response.Close();

                return responseTxt;
            }
            catch (Exception ex)
            {
                return responseTxt;
            }
        }
    }
}
