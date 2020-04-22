using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpDemo.utils
{
    public class FileUtils
    {
        //删除根目录下包含key的文件
        public static void delete(string root,string key)
        {
            string[] files = Directory.GetFiles(root);
            foreach(string file in files)
            {
                if (file.Contains(key))
                {
                    File.Delete(file);
                }
            }
        }

        //得到根目录下包含key的文件名
        public static List<string> getName(string root,string key)
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(root);
            foreach (string file in files)
            {
                if (file.Contains(key))
                {
                    result.Add(file);
                }
            }
            return result;
        }

        //删除日志
        public static void deleteLog(string dateStr)
        {
            DateTime date = DateTime.Parse(dateStr);
            string[] files = Directory.GetFiles(Constant.logPath);
            if(null != files && files.Length > 0)
            {
                char[] c = "\\".ToCharArray();
                char[] d = ".".ToCharArray();
                foreach(string file in files)
                {
                    string[] name = file.Split(c);
                    if(name.Length > 1)
                    {
                        string logName = name[name.Length - 1];
                        string[] dateString = logName.Split(d);
                        string fileDate = dateString[0];
                        TimeSpan timeSpan = date.Subtract(DateTime.Parse(fileDate));
                        //删除7天前的日志
                        if(timeSpan.TotalSeconds > 0)
                        {
                            delete(Constant.logPath, fileDate);
                        }
                    }
                }
            }
        }
    }
}
