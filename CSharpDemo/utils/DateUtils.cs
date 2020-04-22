using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDemo.utils
{
    class DateUtils
    {
        //得到前几天的日期
        public static string getBeforeDate(int count)
        {
            DateTime result = new DateTime();
            result = result.AddDays(count*-1);
            return result.ToString("yyyy") + "-" + result.ToString("MM") + "-" + result.ToString("dd");
        }

        //获取当前时间戳
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
    }
}
