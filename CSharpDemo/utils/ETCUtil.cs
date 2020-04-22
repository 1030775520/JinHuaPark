using CSharpDemo.entity;
using CSharpDemo.entity.ETC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpDemo.utils
{
    class ETCUtil
    {
        //扣款
        public static string money(RequestParam request)
        {
            DateTime date = DateTime.Now;
            string year = date.ToString("yyyy");
            string month = date.ToString("MM");
            string day = date.ToString("dd");
            string hour = date.ToString("HH");
            string minute = date.ToString("mm");
            string second = date.ToString("ss");
            int dateInt = int.Parse(year + month + day);
            int timeInt = int.Parse(hour + minute + second);
            int random = new Random().Next(0, 100);
            int serial = int.Parse(hour + minute + second+ random);
            string s = date.ToString("s");
            string uuid = CommonUtils.getUUID();
            string key = Constant.etc_key;
            ETCEntity eTCEntity = new ETCEntity();
            eTCEntity.sys_time = s;
            eTCEntity.method = "hswg.parktrade.deduct";
            eTCEntity.uuid = uuid;
            eTCEntity.version = "2000";
            eTCEntity.sign = CommonUtils.getMD5(uuid + key).ToUpper();
            Biz_Content biz_Content = new Biz_Content();
            biz_Content.sys_date = dateInt;
            biz_Content.time = timeInt;
            biz_Content.serial = serial;
            biz_Content.park_time = request.time;
            biz_Content.license = request.license;
            biz_Content.money = request.money;
            eTCEntity.biz_content = Newtonsoft.Json.JsonConvert.SerializeObject(biz_Content);
            string param = Newtonsoft.Json.JsonConvert.SerializeObject(eTCEntity);
            string result = HttpUtil.PostOfJson(request.address, param);
            return result;
        }

        //日报
        public static string day(string address)
        {
            DateTime date = DateTime.Now;
            string year = date.ToString("yyyy");
            string month = date.ToString("MM");
            string day = date.ToString("dd");
            int dateInt = int.Parse(year + month + day);
            string s = date.ToString("s");
            string uuid = CommonUtils.getUUID();
            string key = Constant.etc_key;
            DayEntity eTCEntity = new DayEntity();
            eTCEntity.sys_time = s;
            eTCEntity.method = "hswg.parktrade.dayreport";
            eTCEntity.uuid = uuid;
            eTCEntity.version = "2000";
            eTCEntity.sign = CommonUtils.getMD5(uuid + key).ToUpper();
            DayContent biz_Content = new DayContent();
            biz_Content.d_toll = Constant.d_toll;
            biz_Content.n_cnt = Constant.n_cnt;
            biz_Content.n_date = dateInt;
            eTCEntity.biz_content = Newtonsoft.Json.JsonConvert.SerializeObject(biz_Content);
            string param = Newtonsoft.Json.JsonConvert.SerializeObject(eTCEntity);
            string result = HttpUtil.PostOfJson(address, param);
            return result;
        }
    }
}
