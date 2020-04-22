using CSharpDemo.utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpDemo.Job
{
    class DayJob : IJob
    {
        public  void Execute(IJobExecutionContext context)
        {
            try
            {
                Dictionary<string, string> dic = Constant.ipAndEtcIp;

                foreach (var item in dic)
                {
                    string address = item.Value;
                    ETCUtil.day(address);
                    break;
                }

                object lockObject = new object();
                lock (lockObject)
                {
                    Constant.d_toll = 0;

                    Constant.n_cnt = 0;

                    FileUtils.deleteLog(DateUtils.getBeforeDate(7));
                }
            }
            catch(Exception e)
            {
                string log = "ERROR:" + e.StackTrace + e.Message;
                //MessageBox.Show(log);
                CommonUtils.saveLog(log);
            }
        }
    }
}
