using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using CSharpDemo;
//using MySql.Data.MySqlClient;
using System.Globalization;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using CSharpDemo.utils;
using CSharpDemo.entity;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using CSharpDemo.entity.ETC;
using CSharpDemo.entity.socket;

namespace CSharpDemo
{
    public partial class Demo : Form
    {
        private static Load load = null;
        public Demo()
        {
            //load = new Load();
            //load.Show();
            /*if (connectDataBase() < 0)
            {
                MessageBox.Show("数据库初始化失败，请确认数据库成功后，进行操作!", "提示");
            }*/
            InitializeComponent();
            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
        }

        bool bDataSuc = false;
        //MySqlConnection conn;
        //MySqlCommand cmd;
       // MySqlDataReader reader;

        //连接数据库
        private int connectDataBase()
        {
            // 创建连接
            //conn = new MySqlConnection(Constant.connectStr);

            try
            {
                // 打开连接
                //conn.Open();
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                // 关闭连接
                //conn.Close();
            }

            return 1;
        }

        static string[] strMemberType = new string[]{
            "临时车",
            "月卡",
            "贵宾卡",
            "储值卡",
            "领导车",
            "特殊车",
            "内部车",
            "白名单",
            "黑名单"
        };

        static string [] szPlateDefaultWord = new string[]{
	        "京",
	        "津",
	        "沪",
	        "渝",
	        "冀",
	        "晋",
	        "辽",
	        "吉",
	        "黑",
	        "苏",
	        "浙",
	        "皖",
	        "闽",
	        "赣",
	        "鲁",
	        "豫",
	        "鄂",
	        "湘",
	        "粤",
	        "琼",
	        "川",
	        "贵",
	        "云",
	        "陕",
	        "甘",
	        "宁",
	        "青",
	        "藏",
	        "桂",
	        "蒙",
	        "新",
	        "全国"
        };

        //去掉I，0两个英文字母
        static byte [] g_ucLocalCity = new byte[26 - 2 + 1];
        static uint [] g_uiPlateDefaultWord  = new uint [32];

        private static string strImageDir = "";

        private static string logPath = "";
        //入口相机ID1
        private static int nCamIdIn1 = -1;
        //入口相机ID
        private static int nCamIdIn = -1;
        //出口相机ID
        private static int nCamIdOut = -1;

        //相机ip和id的键值对
        private Dictionary<string, int> ipAndId = new Dictionary<string, int>();

        //private static MyClass.FGetImageCB fGetImageCB;
        private static MyClass.FGetImageCB2 fGetImageCB2;

        private static MyClass.FGetOffLinePayRecordCB fGetOffLinePayRecordCB;
        private static MyClass.FGetOffLineImageCBEx fGetOffLineImageCBEx;
        private static MyClass.FNetFindDeviceCallback fNetFindDeviceCallback;
        private static MyClass.FGetReportCBEx fGetReportCBEx;

        private static MyClass.T_VideoDetectParamSetup tVideoDetectParamSetup;
        private static MyClass.T_RS485Data tRS485Data;
        private static MyClass.T_DCTimeSetup tDCTimeSetup;
        private static MyClass.T_VehicleVAFunSetup tVehicleVAFunSetup;

        private static MyClass.T_RS485Data t_RS485;

        static int GetLocalCityIndex(byte ucLocalCity)
        {
            int nIndex = 0;
            for (int i = 0; i < g_ucLocalCity.Length; i++)
            {
                if (ucLocalCity == g_ucLocalCity[i])
                {
                    nIndex = i;
                    break;
                }
            }

            return nIndex;
        }

        public static uint Reverse_uint(uint uiNum)
        {
            return ((uiNum & 0x000000FF) << 24) |
                   ((uiNum & 0x0000FF00) << 8) |
                   ((uiNum & 0x00FF0000) >> 8) |
                   ((uiNum & 0xFF000000) >> 24);
        }


        //出入口车辆识别回调函数
        private int FGetImageCB2(int tHandle, uint uiImageId, ref MyClass.T_ImageUserInfo2 tImageInfo, ref MyClass.T_PicInfo tPicInfo)
        {
            MyClass.T_ImageUserInfo2 image = tImageInfo;
            MyClass.T_PicInfo pic = tPicInfo;
            Thread thread = new Thread(() => mainThread(tHandle, image, pic));
            thread.IsBackground = true;
            thread.Start();
            return 1;
        }

        private void mainThread(int tHandle,MyClass.T_ImageUserInfo2 tImageInfo,MyClass.T_PicInfo tPicInfo)
        {
            try
            {
                //车牌
                BitConverter.ToInt64(tImageInfo.szLprResult, 0);
                String plateNo = CommonUtils.byteToString(CommonUtils.cutOutArray(tImageInfo.szLprResult, 8));
                //抓拍时间(入场时间,格式YYYYMMDDHHMMSSmmm)
                String time = CommonUtils.byteToString(CommonUtils.cutOutArray(tImageInfo.acSnapTime, 14));
                DateTime dt = DateTime.ParseExact(time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                time = dt.ToString().Replace("/", "-");
                //相机ip
                string ip = CommonUtils.getKeyByValue(ipAndId, tHandle);
                Dictionary<string, string> map = new Dictionary<string, string>();
                //终端版本
                map.Add("version", Constant.pklt_version);
                //停车场编号
                map.Add("pkl_code", Constant.pkl_code);
                //停车场通道编号
                map.Add("ch_code", Constant.ipAndPass[ip]);
                //车牌号
                map.Add("veh_plate", plateNo);
                //车辆类型
                map.Add("veh_type", CommonUtils.getVehType(tImageInfo.ucVehicleSize));
                //车牌类型
                map.Add("lprType", tImageInfo.ucLprType.ToString());
                //车牌颜色
                map.Add("veh_plate_color", tImageInfo.ucPlateColor.ToString());
                //车辆颜色
                map.Add("veh_color", tImageInfo.ucVehicleColor.ToString());
                //相机ip
                map.Add("pklh_ip ", ip);
                //时间
                map.Add("veh_passtime", time);
                byte checkType = tImageInfo.ucSnapType;
                //检测模式是线圈,过车推送
                if (Constant.SNAP_MODE_LOOP == checkType)
                {
                    List<string> fileNames = FileUtils.getName(Constant.image_path, ip);
                    //出场
                    if (Constant.outIps.Contains(CommonUtils.getKeyByValue(ipAndId, tHandle)))
                    {
                        //方向 1 入场过车  2出场过车
                        map.Add("direction", "2");
                    }
                    //入场
                    else
                    {
                        //方向 1 入场过车  2出场过车
                        map.Add("direction", "1");
                    }
                    string veh = ""; string veh_plate = "";
                    foreach (string file in fileNames)
                    {
                        if (file.Contains("plate"))
                        {
                            veh_plate = file;
                        }
                        else
                        {
                            veh = file;
                        }
                    }
                    char[] c = Constant.imageSplit.ToCharArray();
                    string[] s = veh.Split(c);
                    map.Remove("veh_plate");
                    map.Add("veh_plate", s[1]);
                    map.Remove("veh_type");
                    map.Add("veh_type", s[2]);
                    //车身图片
                    string veh_photo = CommonUtils.getBase64(veh);
                    map.Add("veh_photo", veh_photo);
                    //车牌图片
                    string veh_plate_photo = CommonUtils.getBase64(veh_plate);
                    map.Add("veh_plate_photo", veh_plate_photo);
                    string requestParam = CommonUtils.getRequestData(map);
                    //发送过车推送请求
                    string httpResult = HttpUtil.Post(Constant.PASS_SEND_ADDRESS, requestParam);
                    ResponseEntity responseEntity = JsonConvert.DeserializeObject<ResponseEntity>(httpResult);
                    analysisMessage(tHandle, responseEntity);
                    string log = "";
                    if (responseEntity.error_code == Constant.success)
                    {
                        log = "发送过车推送失败,返回结果:" + responseEntity.error_code;
                        showLog(log);
                        CommonUtils.saveLog(log);

                        string directName = "Fail" + Constant.imageSplit + map["direction"] + Constant.imageSplit + Constant.ipAndPass[ip] + Constant.imageSplit
                            + map["veh_plate"] + Constant.imageSplit + map["veh_type"] + Constant.imageSplit + map["veh_passtime"];
                        directName = directName.Replace(":", "#");//由于文件夹名字不能带有:
                        if (!Directory.Exists(Constant.image_path + "\\" + directName))//如果不存在就创建 dir 文件夹  
                            Directory.CreateDirectory(Constant.image_path + "\\" + directName);
                        string name = "car";
                        //暂存图片(车辆)
                        CommonUtils.DecodeBase64(veh_photo, Constant.image_path + "\\" + directName + "\\" + "car.jpg");
                        CommonUtils.DecodeBase64(veh_plate_photo, Constant.image_path + "\\" + directName + "\\" + "car_plate.jpg");

                        analysisMessage(tHandle, responseEntity);
                    }
                    else
                    {
                        log = "发送过车推送成功!";
                        showLog(log);
                        CommonUtils.saveLog(log);
                    }
                }
                //检测模式是视频,请求出入场
                else
                {    //出场
                    if (Constant.outIps.Contains(CommonUtils.getKeyByValue(ipAndId, tHandle)))
                    {
                        string requestParam = CommonUtils.getRequestData(map);
                        //发送出场请求
                        string httpResult = HttpUtil.Post(Constant.REQUEST_OUT_ADDRESS, requestParam);
                        ResponseEntity responseEntity = JsonConvert.DeserializeObject<ResponseEntity>(httpResult);
                        analysisMessage(tHandle, responseEntity);
                        DataEntity dataEntity = responseEntity.data;
                        if (1 == dataEntity.etc_flag)
                        {
                            //etc扣款
                            RequestParam request = new RequestParam();
                            PkFeeEntity pkFeeEntity = dataEntity.pk_fee;
                            PkTimeEntity pkTimeEntity = dataEntity.pk_time;
                            if (null != pkFeeEntity && null != pkTimeEntity)
                            {
                                request.money = pkFeeEntity.yuan;
                                request.license = plateNo;
                                request.address = Constant.ipAndEtcIp[ip];
                                request.time = pkTimeEntity.minute;
                                bool payResult = EtcPay(request);
                                if (payResult)
                                {
                                    //开闸
                                    MyClass.T_ControlGate tControlGate = new MyClass.T_ControlGate();
                                    tControlGate.ucState = 1;
                                    tControlGate.ucReserved = new byte[3] { 0, 0, 0 };
                                    int iRet = MyClass.Net_GateSetup(tHandle, ref tControlGate);

                                    object lockThis = new object();
                                    lock (lockThis)
                                    {
                                        Constant.n_cnt++;
                                        Constant.d_toll += pkFeeEntity.yuan;
                                    }
                                }
                            }
                        }

                        if (Constant.success == responseEntity.error_code)
                        {
                            showLog("发送出场请求成功!");
                            CommonUtils.saveLog("发送出场请求成功!");
                        }
                        else
                        {
                            string log = "发送出场请求失败,返回结果:" + responseEntity.error_code;
                            showLog(log);
                            CommonUtils.saveLog(log);
                            byte[] a = AgreementUtils.getTxtCommand(2, "请缴费");
                            byte[] b = AgreementUtils.getTxtCommand(3, dataEntity.pk_fee.yuan + "元");
                            executeCommand(tHandle, a);
                            executeCommand(tHandle, b);
                        }
                    }
                    //入场
                    else
                    {
                        //开线程请求ETC 0元扣款
                        RequestParam r = new RequestParam();
                        r.money = 0;
                        r.license = plateNo;
                        r.address = Constant.ipAndEtcIp[ip];
                        Thread thread = new Thread(() => Etc(r));
                        thread.IsBackground = true;
                        thread.Start();
                        //    (
                        //delegate () { Etc(0, plateNo, ip); }
                        //);
                        string requestParam = CommonUtils.getRequestData(map);
                        //发送入场请求
                        string httpResult = HttpUtil.Post(Constant.REQUEST_ENTER_ADDRESS, requestParam);
                        ResponseEntity responseEntity = JsonConvert.DeserializeObject<ResponseEntity>(httpResult);
                        string log = "";
                        if (Constant.success == responseEntity.error_code)
                        {
                            log = "发送入场请求成功!";
                            showLog(log);
                            CommonUtils.saveLog(log);
                        }
                        else
                        {
                            log = "发送入场请求失败,返回结果:" + responseEntity.error_code;
                            showLog(log);
                            CommonUtils.saveLog(log);
                        }
                        analysisMessage(tHandle, responseEntity);
                    }
                    //保存图片 ip _ 车牌 _ 类型
                    string imageName = ip + Constant.imageSplit + plateNo + Constant.imageSplit + map["veh_type"] + Constant.imageSplit;
                    FileUtils.delete(strImageDir, ip);

                    //车辆图像
                    if (tImageInfo.ucViolateCode == 0)
                    {
                        string szLprResult = System.Text.Encoding.Default.GetString(tImageInfo.szLprResult).Replace("\0", "");
                        //车型
                        string strVehicleBrand = System.Text.Encoding.Default.GetString(tImageInfo.strVehicleBrand).Replace("\0", "");

                        if (tPicInfo.ptPanoramaPicBuff != IntPtr.Zero && tPicInfo.uiPanoramaPicLen != 0)
                        {
                            byte[] BytePanoramaPicBuff = new byte[tPicInfo.uiPanoramaPicLen];
                            Marshal.Copy(tPicInfo.ptPanoramaPicBuff, BytePanoramaPicBuff, 0, (int)tPicInfo.uiPanoramaPicLen);
                            string strImageFile = String.Format("{0}\\{1}.jpg", strImageDir, imageName);
                            FileStream fs = new FileStream(strImageFile, FileMode.Create, FileAccess.Write | FileAccess.Read, FileShare.None);
                            fs.Write(BytePanoramaPicBuff, 0, (int)tPicInfo.uiPanoramaPicLen);
                            //pictureBoxPlate.Image = Image.FromStream(fs);
                            fs.Close();
                            fs.Dispose();
                        }

                        if (tPicInfo.ptVehiclePicBuff != IntPtr.Zero && tPicInfo.uiVehiclePicLen != 0)
                        {
                            byte[] ByteVehiclePicBuff = new byte[tPicInfo.uiVehiclePicLen];
                            Marshal.Copy(tPicInfo.ptVehiclePicBuff, ByteVehiclePicBuff, 0, (int)tPicInfo.uiVehiclePicLen);
                            string strImageFile = String.Format("{0}\\{1}_plate.jpg", strImageDir, imageName);
                            FileStream fs = new FileStream(strImageFile, FileMode.Create, FileAccess.Write | FileAccess.Read, FileShare.None);
                            fs.Write(ByteVehiclePicBuff, 0, (int)tPicInfo.uiVehiclePicLen);
                            //pictureBoxPlateImage.Image = Image.FromStream(fs);
                            fs.Close();
                            fs.Dispose();
                        }
                        //}
                    }
                }
            }
            catch(Exception e)
            {
                string log = "Thread Error:" + e.Message + e.StackTrace;
                showLog(log);
                CommonUtils.saveLog(log);
            }
        }

        //显示日志
        private void showLog(string message)
        {
            object lockThis = new object();
            lock (lockThis)
            {
                int length = this.logContent.Text.Length;
                if(length > 1000)
                {
                    this.logContent.Text = "";
                }
                DateTime dateTime = DateTime.Now;
                string year = dateTime.ToString("yyyy");
                string month = dateTime.ToString("MM");
                string day = dateTime.ToString("dd");
                this.logContent.Text += "【" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "】" + message + "\r\n";
            }
        }

        //发送ETC 0元扣款请求
        //double money,string license,string cameraIp
        private void Etc(RequestParam request)
        {
            try
            {
                //RequestParam request = (RequestParam)obj;
                string result = ETCUtil.money(request);
                EtcResponse etcResponse = JsonConvert.DeserializeObject<EtcResponse>(result);
                int code = etcResponse.code;
                string log = "";
                if (Constant.success == code)
                {
                    log = "发送ETC 0元扣款请求成功!";
                    showLog(log);
                    CommonUtils.saveLog(log);
                }
                else
                {
                    log = "发送ETC 0元扣款请求失败,错误代码:" + code;
                    showLog(log);
                    CommonUtils.saveLog(log);
                }
                Dictionary<string, string> paramMap = new Dictionary<string, string>();
                paramMap.Add("pkl_code", Constant.pkl_code);
                paramMap.Add("veh_plate", request.license);
                paramMap.Add("etc_info", result);
                string requestParam = CommonUtils.getRequestData(paramMap);
                string httpResult = HttpUtil.Post(Constant.etc_request_address, requestParam);
            }
            catch(Exception e)
            {
                string log = "THREAD_ERROR:"+e.StackTrace+e.Message;
                showLog(log);
                CommonUtils.saveLog(log);
            }
        }
        

        //ETC收费
        private bool EtcPay(RequestParam request)
        {
            string log = "";
            string result = ETCUtil.money(request);
            EtcResponse etcResponse = JsonConvert.DeserializeObject<EtcResponse>(result);
            int code = etcResponse.code;
            if (etcResponse.code == 0)
            {
                log = "ETC扣款成功,金额:" + request.money;
                showLog(log);
                CommonUtils.saveLog(log);
                return true;
            }
            else
            {
                log = "ETC扣款失败,错误代码:" + code;
                showLog(log);
                CommonUtils.saveLog(log);
                return false;
            }
        }

        //补发过车推送线程方法
        private void repairPass()
        {
            try
            {
                while (true)
                {
                    string[] dires = Directory.GetDirectories(strImageDir);
                    if (null != dires && dires.Length > 0)
                    {
                        foreach (string dire in dires)
                        {
                            Dictionary<string, string> map = new Dictionary<string, string>();
                            char[] split = Constant.imageSplit.ToCharArray();
                            string[] s = dire.Split(split);
                            //"Fail_" + map["direction"] + "_" + Constant.ipAndPass[ip] + "_" + map["veh_plate"] +"_" + map["veh_type"]+"_"+ map["veh_passtime"];
                            map.Add("pkl_code", Constant.pkl_code);
                            map.Add("ch_code", s[2]);
                            map.Add("veh_plate", s[3]);
                            string[] files = Directory.GetFiles(dire);
                            string veh_plate_photo = "";
                            string veh_photo = "";
                            if (null != files && files.Length > 0)
                            {
                                foreach (string f in files)
                                {
                                    if (f.Contains("plate"))
                                    {
                                        veh_plate_photo = CommonUtils.getBase64(f);
                                    }
                                    else
                                    {
                                        veh_photo = CommonUtils.getBase64(f);
                                    }
                                }
                            }

                            map.Add("veh_plate_photo", veh_plate_photo);
                            map.Add("veh_type", s[4]);
                            map.Add("veh_photo", veh_photo);
                            map.Add("direction", s[1]);
                            map.Add("veh_passtime", s[5].Replace("#",":"));
                            string requestParam = CommonUtils.getRequestData(map);
                            //发送过车推送请求
                            string httpResult = HttpUtil.Post(Constant.PASS_SEND_ADDRESS, requestParam);
                            ResponseEntity responseEntity = JsonConvert.DeserializeObject<ResponseEntity>(httpResult);
                            int code = responseEntity.error_code;
                            string log = "";
                            if (code == Constant.success)
                            {
                                //删除文件夹
                                DirectoryInfo subdir = new DirectoryInfo(dire);
                                subdir.Delete(true);
                                log = "补发过车推送成功!";
                                showLog(log);
                                CommonUtils.saveLog(log);
                            }
                            else
                            {
                                log = "补发过车推送失败,代码:" + code;
                                showLog(log);
                                CommonUtils.saveLog(log);
                            }
                        }
                    }
                    Thread.Sleep(5 * 60 * 1000);
                }
            }
            catch(Exception e)
            {
                string log = "THREAD_ERROR:" + e.StackTrace + e.Message;
                showLog(log);
                CommonUtils.saveLog(log);
            }
        }

        //解析消息
        public void analysisMessage(int tHandle, ResponseEntity responseEntity)
        {
            string log = "";
            try
            {
                DataEntity dataEntity = responseEntity.data;
                if (null == dataEntity)
                {
                    return;
                }
                //是否开闸
                if (Constant.open == dataEntity.open_flag)
                {
                    //开闸
                    MyClass.T_ControlGate tControlGate = new MyClass.T_ControlGate();
                    tControlGate.ucState = 1;
                    tControlGate.ucReserved = new byte[3] { 0, 0, 0 };
                    int iRet = MyClass.Net_GateSetup(tHandle, ref tControlGate);
                }
                //显示命令
                List<ScreenEntity> screen = dataEntity.screen;
                if (null != screen && screen.Count > 0)
                {
                    foreach (var s in screen)
                    {
                        byte lineNo = s.line_no;
                        string text = s.text;
                        if (null != text && !"".Equals(text))
                        {
                            byte[] txtCommand = AgreementUtils.getTxtCommand(lineNo, text);
                            executeCommand(tHandle, txtCommand);
                        }
                        string color = s.color;
                        if (null != color && !"".Equals(color))
                        {
                            byte[] colorCommand = AgreementUtils.getColorCommand(lineNo, color);
                            executeCommand(tHandle, colorCommand);
                        }
                    }
                }
                //语音命令
                List<VoiceEntity> voice = dataEntity.voice;
                if (null != voice && voice.Count > 0)
                {
                    foreach (var v in voice)
                    {
                        string type = v.type;
                        if (null == type || "".Equals(type))
                        {
                            continue;
                        }
                        Thread.Sleep(1000);
                        string content = v.content;
                        byte[] soundCommand = AgreementUtils.getSoundCommand(type, content);
                        executeCommand(tHandle, soundCommand);
                        //车牌播报等7秒
                        if ("04".Equals(type))
                        {
                            Thread.Sleep(7000);
                        }
                    }
                }
                log = "解析消息成功!";
            }
            catch(Exception e)
            {
                log = "解析消息失败:"+e.StackTrace+e.Message;
            }
            finally
            {
                showLog(log);
                CommonUtils.saveLog(log);
            }
        }

        //执行命令
        public void executeCommand(int tHandle, byte[] command)
        {
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(command);
            t_RS485.data = command;
            int result = MyClass.Net_SendRS485Data(tHandle, ref t_RS485);
        }

        //脱机数据回调
        private int FGetOffLineImageCBEx(int tHandle, uint uiImageId, ref MyClass.T_ImageUserInfo tImageInfo, ref MyClass.T_PicInfo tPicInfo, IntPtr obj)
        {
            if (tHandle == nCamIdIn)
            {
  
            }
           
            return 0;
        }

        private int FNetFindDeviceCallback(ref MyClass.T_RcvMsg ptFindDevice, IntPtr obj)
        {
            //breakerIPOut.Items.Add(ModifyIP.IntToIp(Reverse_uint(ptFindDevice.tNetSetup.uiIPAddress)));
            return 0;
        }

        private int FGetReportCBEx(int tHandle, byte ucType, IntPtr ptMessage, IntPtr pUserData)
        {
            string strMsg = String.Format("{0}", ucType);

            //MessageBox.Show(strMsg);

            if (14 == ucType) //语音对讲通知
            {
                if (DialogResult.Yes == MessageBox.Show("是否接受相机端发起的语音对讲", "提示", MessageBoxButtons.YesNo))
                {
                    buttonTalk_Click(null, null);
                }
            }

            return 0;
        }

        private void Demo_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            MyClass.Net_Init();

            int ptLen = 255;
            StringBuilder strVersion = new StringBuilder(ptLen);
            MyClass.Net_GetSdkVersion(strVersion, ref ptLen);
            this.Text += "(" + strVersion + ")";

            fNetFindDeviceCallback = new MyClass.FNetFindDeviceCallback(FNetFindDeviceCallback);

            strImageDir = System.Windows.Forms.Application.StartupPath + "\\image";
            logPath = System.Windows.Forms.Application.StartupPath + "\\log";
            Constant.logPath = logPath;
            if (!Directory.Exists(strImageDir))
            {
                Directory.CreateDirectory(strImageDir);
            }

            for (int i = 0; i < szPlateDefaultWord.Length; i++)
            {
                //comboBoxProv.Items.Add(szPlateDefaultWord[i]);
                if (i != szPlateDefaultWord.Length - 1)
                {
                    byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(szPlateDefaultWord[i]);
                    g_uiPlateDefaultWord[i] = (uint)((utf8[2] << 16) | (utf8[1] << 8) | utf8[0]);
                }
                else
                {
                    g_uiPlateDefaultWord[i] = 0;
                }
            }

            int nLocalCityIndex = 0;
            for (int i = 'A'; i <= 'Z'; i++)
            {
                if (i == 'I' || i == 'O')
                {
                    continue;
                }
                g_ucLocalCity[nLocalCityIndex++] = (byte)i;
            }
            g_ucLocalCity[nLocalCityIndex++] = 0;
            initCamera();
            //socket连接云平台
            Thread t1 = new Thread(communication);
            t1.IsBackground = true;
            t1.Start();
            //监听socket线程
            Thread t2 = new Thread(monitorSocket);
            t2.IsBackground = true;
            t2.Start();

            //每隔5分钟补发过车推送以及清空屏幕上的日志显示
            //Thread t2 = new Thread(repairPass);
            //t2.IsBackground = true;
            //t2.Start();

            //load.Close();
        }

        //线程通信
        private void communication()
        {
            try
            {
                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //发起建立连接的请求
                //Parse：将一个字符串的ip地址转换成一个IPAddress对象
                IPAddress ipaddress = IPAddress.Parse(Constant.socketIp);
                EndPoint point = new IPEndPoint(ipaddress, Constant.socketPort);
                tcpClient.Connect(point);//通过IP和端口号来定位一个所要连接的服务器端
                Constant.isNeedReconnect = false;
                //注册消息
                ClientMessageEntity registerEntity = new ClientMessageEntity();
                registerEntity.cmd = "reg";
                registerEntity.dev_id = Constant.dev_id;
                registerEntity.time = DateUtils.GetTimeStamp();
                registerEntity.ver = Constant.pklt_version;
                registerEntity.pkl_code = Constant.pkl_code;
                tcpClient.Send(CommonUtils.getSocketMessage(registerEntity));

                //开启心跳线程
                Thread thread = new Thread(() => heart(tcpClient));
                thread.IsBackground = true;
                thread.Start();

                while (true)
                {
                    byte[] data = new byte[1024 * 1024];
                    //传递一个byte数组，用于接收数据。length表示接收了多少字节的数据
                    int length = tcpClient.Receive(data);

                    string m = Encoding.UTF8.GetString(data, 0, length);//只将接收到的数据进行转化
                    string[] mArray = m.Split("\r\n".ToCharArray());
                    if (null == mArray || mArray.Length < 1)continue;
                    for (int i = 0; i < mArray.Length; i++)
                    {
                        string oneMessage = mArray[i];
                        if (string.IsNullOrEmpty(oneMessage))continue;
                        string log = "收到socket服务端消息:" + oneMessage;
                        showLog(log);
                        CommonUtils.saveLog(log);
                        //检查参数
                        SocketMessageEntity messageEntity = JsonConvert.DeserializeObject<SocketMessageEntity>(oneMessage);

                        byte state = messageEntity.state;
                        if (0 == state) continue;//是注册或者心跳返回的信息,否则是主动开闸操作
                        List<string> needControlIp = new List<string>();
                        if ("0".Equals(messageEntity.ch_code))//全部闸机
                        {
                            needControlIp.AddRange(Constant.ipAndPass.Keys);
                        }
                        else//单个闸机
                        {
                            needControlIp.Add(CommonUtils.getKeyByValue(Constant.ipAndPass, messageEntity.ch_code));
                        }
                        if (null == needControlIp || needControlIp.Count < 1)continue;
                        foreach(var ip in needControlIp)
                        {
                        if (!ipAndId.ContainsKey(ip)) continue;
                        int cameraId = ipAndId[ip];
                        string controlLog = "";
                        //开闸
                        if (1 == state)
                        {
                            MyClass.T_ControlGate tControlGate = new MyClass.T_ControlGate();
                            tControlGate.ucState = state;
                            tControlGate.ucReserved = new byte[3] { 0, 0, 0 };
                            //开关闸
                            int iRet = MyClass.Net_GateSetup(cameraId, ref tControlGate);
                            if (iRet == 0)
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机开闸成功!";
                            }
                            else
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机开闸失败,结果:" + iRet;
                            }
                        }
                        //常开
                        else if (2 == state)
                        {
                            MyClass.T_ControlGateQueue t = new MyClass.T_ControlGateQueue();
                            t.ucState = 1;
                            t.ucIndex = 100;
                            int result = MyClass.Net_ControlGateQueue(cameraId, ref t);
                            if (result == 0)
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机常开成功!";
                            }
                            else
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机常开失败,结果:" + result;
                            }
                        }
                        //关闭常开
                        else if (3 == state)
                        {
                            MyClass.T_ControlGateQueue t = new MyClass.T_ControlGateQueue();
                            t.ucState = 2;
                            t.ucIndex = 100;
                            int result = MyClass.Net_ControlGateQueue(cameraId, ref t);
                            if (result == 0)
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机关闭常开成功!";
                            }
                            else
                            {
                                controlLog = "云平台控制ip为" + ip + "的相机关闭常开失败,结果:" + result;
                            }
                        }
                        showLog(controlLog);
                        CommonUtils.saveLog(controlLog);
                        }
                    }
                    //向服务器端发送消息
                    //string message2 = "result:"+1;
                    //将字符串转化为字节数组，然后发送到服务器端
                    //tcpClient.Send(Encoding.UTF8.GetBytes(message2));
                }
            }
            catch(Exception e)
            {
                string log = "Thread Error:" + e.Message + e.StackTrace;
                showLog(log);
                CommonUtils.saveLog(log);
                Constant.isNeedReconnect = true;
            }

        }

        //心跳线程
        private void heart(Socket socketClient)
        {
            ClientMessageEntity heartEntity = new ClientMessageEntity();
            heartEntity.cmd = "heart";
            heartEntity.dev_id = Constant.dev_id;
            heartEntity.pkl_code = Constant.pkl_code;
            try
            {
                while (true)
                {
                    int result = socketClient.Send(CommonUtils.stringToByteUtf8(JsonConvert.SerializeObject(heartEntity)));
                    //1分钟发一次
                    Thread.Sleep(60 * 1000);
                }
            }
            catch(Exception e)
            {
                string log = "Thread Error:" + e.Message + e.StackTrace;
                showLog(log);
                CommonUtils.saveLog(log);
                Constant.isNeedReconnect = true;
            }
        }

        //监听socket线程
        private void monitorSocket()
        {
            while (true)
            {
                if (Constant.isNeedReconnect)
                {
                    Thread t1 = new Thread(communication);
                    t1.IsBackground = true;
                    t1.Start();
                }
                //两分钟监听一次
                Thread.Sleep(2 * 60 * 1000);
            }
        }

        

        private void Demo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (nCamIdIn != -1)
            {
                MyClass.Net_StopVideo(nCamIdIn);
                MyClass.Net_DisConnCamera(nCamIdIn);
                MyClass.Net_DelCamera(nCamIdIn);
                nCamIdIn = -1;
            }
            MyClass.Net_UNinit();
            CommonUtils.saveLog("关闭程序");
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            MyClass.T_FrameInfo tFrameInfo = new MyClass.T_FrameInfo();

            int iRet = MyClass.Net_ImageSnap(nCamIdIn, ref tFrameInfo);
            if (iRet == 0)
            {
                //MessageBox.Show("抓拍成功", "提示");
            }
            else
            {
                MessageBox.Show("抓拍失败", "提示");
            }
        }

        private void buttonAutoGrab_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            //////////////////////////////////////////////////////////////////////////////
            //直接保存为JPG图片
            //string strTime = DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmssfff");
            //string strJpgPath = strTime + ".jpg";
            //int iRet = MyClass.Net_SaveJpgFile(nCamIdIn, strJpgPath);
            //if (iRet == 0)
            //{
            //    //MessageBox.Show("手动抓图成功", "提示");
            //    if (File.Exists(strJpgPath))
            //    {
            //         Process.Start("Explorer.exe", strJpgPath);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("手动抓图失败", "提示");
            //}

            ////////////////////////////////////////////////////////////////////////
            //获取当前帧的JPG缓存
            IntPtr ucJpgBuffer = IntPtr.Zero; 
            ulong ulSize = 0;

            int iRet = MyClass.Net_GetJpgBuffer(nCamIdIn, ref ucJpgBuffer, ref ulSize);

            if (iRet == 0)
            {
                //将当前帧的JPG缓存刷到控件上
                byte[] ByteJpgBuffer = new byte[ulSize];
                Marshal.Copy(ucJpgBuffer, ByteJpgBuffer, 0, (int)ulSize);
                MemoryStream ms = new MemoryStream(ByteJpgBuffer);
                //pictureBoxPlate.Image = Image.FromStream(ms);


                //////////////////////////////////////////////////////////////////////
                //将缓存保存为JPG图片
                //string strTime = DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmssfff");
                //string strJpgPath = strTime + ".jpg";
                //FileStream fs = new FileStream(strJpgPath, FileMode.Create, FileAccess.Write | FileAccess.Read, FileShare.None);
                //fs.Write(ByteJpgBuffer, 0, (int)ulSize);
                //fs.Close();
                //fs.Dispose();

                //if (File.Exists(strJpgPath))
                //{
                //    Process.Start("Explorer.exe", strJpgPath);
                //}
            }
            else
            {
                MessageBox.Show("手动抓图失败", "提示");
            }

            if (ucJpgBuffer != IntPtr.Zero)
            {
                MyClass.Net_FreeBuffer(ucJpgBuffer);
                ucJpgBuffer = IntPtr.Zero;
            }

        }

        private void checkBoxShowPlateRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

           /* if (checkBoxShowPlateRegion.Checked)
            {
                MyClass.Net_ShowPlateRegion(nCamIdIn, 1);
            }
            else
            {
                MyClass.Net_ShowPlateRegion(nCamIdIn, 0);
            }*/
        }

        private void buttonTimeRead_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            int iRet = MyClass.Net_QueryTimeSetup(nCamIdIn, ref tDCTimeSetup);
            if (iRet != 0)
            {
                MessageBox.Show("查询相机时间失败!", "提示");
                return;
            }

            //dateTimePickerCAM.Value = new DateTime(tDCTimeSetup.usYear, tDCTimeSetup.ucMonth, tDCTimeSetup.ucDay,
                                                   //tDCTimeSetup.ucHour, tDCTimeSetup.ucMinute, tDCTimeSetup.ucSecond);
        }


        private void buttonVehicleVAFunSetup_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            //tVehicleVAFunSetup.uiPlateDefaultWord = g_uiPlateDefaultWord[comboBoxProv.SelectedIndex];
            //tVehicleVAFunSetup.ucLocalCity = g_ucLocalCity[comboBoxCity.SelectedIndex];

            int iRet = MyClass.Net_VehicleVAFunSetup(nCamIdIn, ref tVehicleVAFunSetup);
            if (iRet != 0)
            {
                MessageBox.Show("设置相机默认字失败!", "提示");
                return;
            }
        }

        //入口相机
        private void buttonVehicleVAFunQuery_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            int iRet = MyClass.Net_QueryVehicleVAFunSetup(nCamIdIn, ref tVehicleVAFunSetup);
            if (iRet != 0)
            {
                MessageBox.Show("查询相机默认字失败!", "提示");
                return;
            }

            for (int i = 0; i < g_uiPlateDefaultWord.Length; i++)
		    {
			    if (g_uiPlateDefaultWord[i] == tVehicleVAFunSetup.uiPlateDefaultWord)
			    {
				    //comboBoxProv.Text = szPlateDefaultWord[i];
				    break;
			    }
		    }

            byte ucLocalCity = g_ucLocalCity[GetLocalCityIndex(tVehicleVAFunSetup.ucLocalCity)];
            if (ucLocalCity == 0)
            {
                //comboBoxCity.Text = "全省"; 
            }
            else
            {
                //comboBoxCity.Text = string.Format("{0}", (Char)ucLocalCity);
            }
        }

        //出口相机
        private void buttonVehicleVAFunQuery_Click2(object sender, EventArgs e)
        {
            if (nCamIdOut == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            int iRet = MyClass.Net_QueryVehicleVAFunSetup(nCamIdOut, ref tVehicleVAFunSetup);
            if (iRet != 0)
            {
                MessageBox.Show("查询相机默认字失败!", "提示");
                return;
            }

            for (int i = 0; i < g_uiPlateDefaultWord.Length; i++)
            {
                if (g_uiPlateDefaultWord[i] == tVehicleVAFunSetup.uiPlateDefaultWord)
                {
                    //comboBoxProv.Text = szPlateDefaultWord[i];
                    break;
                }
            }

            byte ucLocalCity = g_ucLocalCity[GetLocalCityIndex(tVehicleVAFunSetup.ucLocalCity)];
            if (ucLocalCity == 0)
            {
                //comboBoxCity.Text = "全省";
            }
            else
            {
                //comboBoxCity.Text = string.Format("{0}", (Char)ucLocalCity);
            }
        }


        private static bool m_bTalking = false;
        private void buttonTalk_Click(object sender, EventArgs e)
        {
            if (nCamIdIn == -1)
            {
                MessageBox.Show("相机未连接", "提示");
                return;
            }

            int iRet = 0;

            if (false == m_bTalking)
            {
                iRet = MyClass.Net_StartTalk(nCamIdIn);

                if (0 == iRet)
                {
                    m_bTalking = true;
                    //buttonTalk.Text = "结束对讲";
                    MessageBox.Show("开始对讲成功", "提示");
                }
                else
                {
                    MessageBox.Show("开始对讲失败", "提示");
                }
            }
            else
            {
                iRet = MyClass.Net_StopTalk(nCamIdIn);

                if (0 == iRet)
                {
                    m_bTalking = false;
                    //buttonTalk.Text = "开始对讲";
                    MessageBox.Show("结束对讲成功", "提示");
                }
                else
                {
                    MessageBox.Show("结束对讲失败", "提示");
                   
                }
            }
        }

        private void Label48_Click(object sender, EventArgs e)
        {

        }

        private void Label9_Click(object sender, EventArgs e)
        {

        }

        private void LabelPlateColor_Click(object sender, EventArgs e)
        {

        }

        //判断是否连接成功(是 返回true 否 返回false)
        private bool connectedOrNot(string ip)
        {
            if(ip == "")
            {
                MessageBox.Show("请输入相机IP!", "提示");
                return false;
            }

            if (ipAndId.ContainsKey(ip))
            {
                MessageBox.Show("相机已连接!", "提示");
                return false;
            }

            return true;
        }

        //初始化相机
        public void initCamera()
        {
            Cursor.Current = Cursors.WaitCursor;
            do
            {
                List<string> inIps = Constant.inIps;
                List<string> outIps = Constant.outIps;
                inIps.AddRange(outIps);
                if (null != inIps && inIps.Count > 0)
                {
                    foreach(string ip in inIps)
                    {
                        nCamIdIn1 = MyClass.Net_AddCamera(ip);

                        if (nCamIdIn1 == -1)
                        {
                            MessageBox.Show("添加相机失败,ip:"+ip, "提示");
                            break;
                        }

                        int iRet = MyClass.Net_ConnCamera(nCamIdIn1, 30000, 10);

                        if (iRet != 0)
                        {
                            MyClass.Net_DelCamera(nCamIdIn1);
                            MessageBox.Show("连接相机失败,ip:"+ip, "提示");
                            break;
                        }
                        else
                        {
                            ipAndId.Add(ip, nCamIdIn1);
                        }

                        //MyClass.Net_RegOffLineClient(nCamIdIn1);
                    }
                }
                

                //fGetImageCB = new MyClass.FGetImageCB(FGetImageCB);
                //MyClass.Net_RegImageRecv(fGetImageCB);
                fGetImageCB2 = new MyClass.FGetImageCB2(FGetImageCB2);
                MyClass.Net_RegImageRecv2(fGetImageCB2);

                /*fGetOffLinePayRecordCB = new MyClass.FGetOffLinePayRecordCB(FGetOffLinePayRecordCB);
                MyClass.Net_RegOffLinePayRecord(nCamIdIn, fGetOffLinePayRecordCB, IntPtr.Zero);*/

                fGetOffLineImageCBEx = new MyClass.FGetOffLineImageCBEx(FGetOffLineImageCBEx);
                MyClass.Net_RegOffLineImageRecvEx(nCamIdIn1, fGetOffLineImageCBEx, IntPtr.Zero);

                fGetReportCBEx = new MyClass.FGetReportCBEx(FGetReportCBEx);
                MyClass.Net_RegReportMessEx(nCamIdIn1, fGetReportCBEx, IntPtr.Zero);

                //iRet = MyClass.Net_QueryVideoDetectSetup(nCamIdIn1, ref tVideoDetectParamSetup);
                //if (iRet != 0)
                //{
                //    //MessageBox.Show("查询视频检测区域参数失败!", "提示");
                //}
                //else
                //{

                //}

                //buttonVehicleVAFunQuery_Click(sender, e);

            } while (false);
            Cursor.Current = Cursors.Default;
        }

        //控制屏幕
        private void Button42_Click(object sender, EventArgs e)
        {
           // MyClass.T_LedSetup t_LedSetup = new MyClass.T_LedSetup();
           // //使能标志 0不使能  1使能
           // t_LedSetup.ucAudioEnable = 1;
           // //出口空闲4行内容
           // MyClass.T_SubLedSetup first = new MyClass.T_SubLedSetup();
           // //使能标志 0不使能  1使能
           // first.ucEnable = 1;
           // first.ucLedLine = 1;
           // //0 时间  2自定义 4欢迎语
           // first.ucMode = 2;
           // first.ucInterVal = 1;
           // first.aucContent = 
           //     //getData();
           //     new byte[100];
           // byte[] b = CommonUtils.stringToByte("gbk");
           // b.CopyTo(first.aucContent, 0);
           // MyClass.T_SubLedSetup second = new MyClass.T_SubLedSetup();
           // second.ucEnable = 1;
           // second.ucLedLine = 2;
           // //0 时间  2自定义 4欢迎语
           // second.ucMode = 2;
           // second.ucInterVal = 1;
           // second.aucContent = //getData();
           //     new byte[100];
           // b.CopyTo(second.aucContent, 0);
           // MyClass.T_SubLedSetup third = new MyClass.T_SubLedSetup();
           // third.ucEnable = 1;
           // third.ucLedLine = 3;
           // //0 时间  2自定义 4欢迎语
           // third.ucMode = 2;
           // third.ucInterVal = 1;
           // third.aucContent = //getData();
           //     new byte[100];
           // b.CopyTo(third.aucContent, 0);
           // MyClass.T_SubLedSetup forth = new MyClass.T_SubLedSetup();
           // forth.ucEnable = 1;
           // forth.ucLedLine = 4;
           // //0 时间  2自定义 4欢迎语
           // forth.ucMode = 2;
           // forth.ucInterVal = 1;
           // forth.aucContent = //getData();
           //     new byte[100];
           // b.CopyTo(forth.aucContent, 0);
            
           //// t_LedSetup.atSubLedOutIdle = new MyClass.T_SubLedSetup[] { first,second,third,forth};
           // t_LedSetup.atSubLedInIdle = new MyClass.T_SubLedSetup[] { first, second, third, forth };
           // //t_LedSetup.atSubLedInBusy = new MyClass.T_SubLedSetup[] { first, second, third, forth };
           // //t_LedSetup.atSubLedOutBusy = new MyClass.T_SubLedSetup[] { first, second, third, forth };
           // int result = MyClass.Net_LedSetup(0 ,ref t_LedSetup);
           // if(0 == result)
           // {
           //     MessageBox.Show("设置成功!", "提示");
           // }
           // else
           // {
           //     MessageBox.Show("设置失败,状态码:" + result, "提示");
           // }
        }

        //得到数据
        private byte[] getData1()
        {
            byte[] result = new byte[1024];
            result[0] = 0xA5;
            result[1] = 0xA5;
            result[2] = 0x01;
            result[3] = 0x09;
            result[4] = 0x03;
            result[5] = 0x06;
            result[6] = 0x20;
            result[7] = 0x33;
            result[8] = 0x34;
            result[9] = 0x35;
            result[10] = 0x20;
            result[11] = 0x20;
            result[12] = 0xFA;
            result[13] = 0xBE;
            result[14] = 0xEF;
            //A5 A5 01 09 03 06 20 33 34 35 20 20 FA BE EF
            return result;
        }

        //得到数据
        private byte[] getTxt()
        {
            byte[] result = new byte[1024];
            result[0] = 0xA5;
            result[1] = 0xA5;
            //总类型  1显示  2语音
            result[2] = 0x01;
            //数据总长度
            result[3] = 0x0B;
            //内部类型
            result[4] = 0x06;
            //内部长度
            result[5] = 0x08;

            result[6] = 179;
            result[7] = 181;
            result[8] = 197;
            result[9] = 198;
            result[10] = 202;
            result[11] = 182;
            result[12] = 177;
            result[13] = 240;
            result[14] = 0xDD;
            result[15] = 0xBE;
            result[16] = 0xEF;
            //自定义易泊(第1行)
            //A5 A5 01 07 05 04 D2 D7 B2 B4 E7 BE EF
            //第2行显示 车牌识别
            //0x06 + 0x08 + 0xB3 + 0xB5 + 0xC5 + 0xC6 + 0xCA + 0xB6 + 0xB1 + 0xF0 + 0xDD
            // 0x05 + 0x04 + 0xD2 + 0xD7 + 0xB2 + 0xB4 + 0xE7
            return result;
        }

        //语音
        public byte[] sound()
        {
            byte[] result = new byte[1024];
            result[0] = 0xA5;
            result[1] = 0xA5;
            //总类型  1显示  2语音
            result[2] = 0x02;
            //数据总长度
            result[3] = 0x09;
            //内部类型
            result[4] = 0x03;
            //内部长度
            result[5] = 0x06;

            result[6] = 0x20;
            result[7] = 0x20;
            result[8] = 0x20;
            result[9] = 0x20;
            result[10] = 0x36;
            result[11] = 0x35;
            result[12] = 0x0B;
            result[13] = 0xBE;
            result[14] = 0XEF;
            //0x03 + 0x06+ 0x20+ 0x20 + 0x20 + 0x20 +0x36+0x35 +0x0B
            return result;
        }

        //颜色
        public byte[] color()
        {
            byte[] result = new byte[1024];
            result[0] = 0xA5;
            result[1] = 0xA5;
            //总类型  1显示  2语音
            result[2] = 0x01;
            //数据总长度
            result[3] = 0x05;
            //内部类型
            result[4] = 0x35;
            //内部长度
            result[5] = 0x02;

            result[6] = 0x03;
            result[7] = 0x02;
            result[8] = 0xC3;
            result[9] = 0xBE;
            result[10] = 0xEF;
            //第1行红色：35 02 01 00 C7
           //第2行绿色：35 02 02 01 C5
//第3行黄色：35 02 03 02 C3
//第4行绿色：35 02 04 01 C3

            return result;
        }

        //添加收费规则
        private void Button13_Click(object sender, EventArgs e)
        {
            add_rule add_Rule =  new add_rule();
            add_Rule.ShowDialog();
        }

        //相机抓拍
        private void Button43_Click(object sender, EventArgs e)
        {
            MyClass.T_FrameInfo tFrameInfo = new MyClass.T_FrameInfo();

            int iRet = MyClass.Net_ImageSnap(0, ref tFrameInfo);
            if (iRet == 0)
            {
                //MessageBox.Show("抓拍成功", "提示");
            }
            else
            {
                MessageBox.Show("抓拍失败", "提示");
            }
        }

        //模拟请求出场
        private void Button11_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("收费:10元", "收费确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                MessageBox.Show("成功放行!");
            }
            else
            {
                MessageBox.Show("没有放行!");
            }
        }

        //白名单设置
        private void Button43_Click_1(object sender, EventArgs e)
        {
            RequestParam r = new RequestParam();
            r.address = "123";
            Thread thread = new Thread(() => Etc(r));
            thread.IsBackground = true;
            thread.Start();
            int nCamId = 0;

            MyClass.T_LprResult tLprResult = new MyClass.T_LprResult();
            //byte[] LprResult = System.Text.Encoding.Default.GetBytes(textBoxPlate.Text);
            byte[] LprResult = System.Text.Encoding.Default.GetBytes("浙F42222");

            tLprResult.LprResult = new byte[16];
            LprResult.CopyTo(tLprResult.LprResult, 0);

            //byte[] StartTime = System.Text.Encoding.Default.GetBytes(dateTimePickerStart.Value.ToString("yyyyMMddHHmmss"));
            byte[] StartTime = System.Text.Encoding.Default.GetBytes("20190717150202");
            tLprResult.StartTime = new byte[16];
            StartTime.CopyTo(tLprResult.StartTime, 0);

            // byte[] EndTime = System.Text.Encoding.Default.GetBytes(dateTimePickerEnd.Value.ToString("yyyyMMddHHmmss"));
            byte[] EndTime = System.Text.Encoding.Default.GetBytes("20190719150802");
            tLprResult.EndTime = new byte[16];
            EndTime.CopyTo(tLprResult.EndTime, 0);

            MyClass.T_BlackWhiteListCount tBlackWhiteListCount = new MyClass.T_BlackWhiteListCount();
            tBlackWhiteListCount.uiCount = 1;
            string strAucLplPath = System.Windows.Forms.Application.StartupPath + "\\lpr.ini";

            if (File.Exists(strAucLplPath))
            {
                File.Delete(strAucLplPath);
            }
            byte[] aucLplPath = System.Text.Encoding.Default.GetBytes(strAucLplPath);

            tBlackWhiteListCount.aucLplPath = new byte[256];
            aucLplPath.CopyTo(tBlackWhiteListCount.aucLplPath, 0);

            int iRet = MyClass.Net_BlackWhiteListSetup(ref tLprResult, ref tBlackWhiteListCount);
            if (iRet == 0)
            {

            }
            else
            {
                MessageBox.Show("生成白名单失败", "提示");
            }

            MyClass.T_BlackWhiteList tBalckWhiteList = new MyClass.T_BlackWhiteList();
            //黑白名单(0黑  1白)
            tBalckWhiteList.LprMode = 1;
            //0  重新发送，即删除原有名单，重新设置
            //1  续传，即不删除原有名单，继续发送
            //2  删除，即删除已有名单中的指定车牌，指定车牌为消息传输中的车牌
            tBalckWhiteList.Lprnew = 1;
            tBalckWhiteList.aucLplPath = new byte[256];
            aucLplPath.CopyTo(tBalckWhiteList.aucLplPath, 0);

            int sendResult = MyClass.Net_BlackWhiteListSend(nCamId, ref tBalckWhiteList);
            if (sendResult == 0)
            {
                MessageBox.Show("新增白名单成功", "提示");
            }
            else
            {
                MessageBox.Show("新增白名单失败，状态码:"+ sendResult, "提示");
            }
        }

        private void ipIn1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button48_Click(object sender, EventArgs e)
        {
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = AgreementUtils.getTxtCommand(1, "asdf");
            int result1 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = AgreementUtils.getTxtCommand(2, "asdf");
            int result2 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = AgreementUtils.getTxtCommand(3, "1234");
            int result3 = MyClass.Net_SendRS485Data(0,ref t_RS485);
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = AgreementUtils.getTxtCommand(4, "1234");
            int result4 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if (0 == result3)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败,返回:" + result3);
            }
        }

        private void Button49_Click(object sender, EventArgs e)
        {
            byte[] data = //sound();
                AgreementUtils.getSoundCommand("04", "浙FS0P13");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = data;
            int result = MyClass.Net_SendRS485Data(0, ref t_RS485);
            Thread.Sleep(3000);
            byte[] data2 = //sound();
            AgreementUtils.getSoundCommand("14", "");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = data2;
            int result2 = MyClass.Net_SendRS485Data(0, ref t_RS485);

            byte[] data3 = //sound();
            AgreementUtils.getSoundCommand("07", "");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = data3;
            int result3 = MyClass.Net_SendRS485Data(0, ref t_RS485);

            if (0 == result)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败,返回:" + result);
            }
            
        }

        private void Button50_Click(object sender, EventArgs e)
        {
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = 1024;
            t_RS485.data = AgreementUtils.getTxtCommand(4, "asdf");
            int result = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if (0 == result)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败,返回:" + result);
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        //万能语音
        private void Button2_Click(object sender, EventArgs e)
        {
            string soundContent = "浙FS0P13无权限,请到保安室登记入场";
            byte[] data = UniversalSoundAgreementUtils.getSoundCommand(soundContent);
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(data);
            t_RS485.data = data;
            int result = MyClass.Net_SendRS485Data(0, ref t_RS485);


            if (0 == result)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败,返回:" + result);
            }
        }

        //万能语音文本
        private void Button3_Click(object sender, EventArgs e)
        {
            byte[] data = UniversalSoundAgreementUtils.getTxtCommand(2,"你好嘉广科技");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(data);
            t_RS485.data = data;
            int result = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if (0 == result)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败,返回:" + result);
            }
        }

        //万能广告
        private void Button4_Click(object sender, EventArgs e)
        {
            byte[] result1 = UniversalSoundAgreementUtils.getColourAdvertisement(1, 2,"嘉广科技");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result1);
            t_RS485.data = result1;
            int c = MyClass.Net_SendRS485Data(0, ref t_RS485);
            byte[] result2 = UniversalSoundAgreementUtils.getColourAdvertisement(2, 3, "智慧停车");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result2);
            t_RS485.data = result2;
            int c2 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            byte[] result3 = UniversalSoundAgreementUtils.getColourAdvertisement(3, 2, "一车一杆");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result3);
            t_RS485.data = result3;
            int c3 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            byte[] result4 = UniversalSoundAgreementUtils.getColourAdvertisement(4, 3, "请勿泊车");
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result4);
            t_RS485.data = result4;
            int c4 = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if (c == 0)
            {
                MessageBox.Show("发送成功!");
            }
        }

        //设置字体
        private void Button5_Click(object sender, EventArgs e)
        {
            byte[] result = new byte[1024];
            result[0] = 0xAA;
            result[1] = 0x55;
            //保留字
            result[2] = 0x01;
            result[3] = 0x64;
            result[4] = 0x00;
            //命令
            result[5] = 0x50;
            //长度
            result[6] = 0x02;
            //数据 0：粗体 1：宋体
            result[7] = 0x00;
            //校检
            result[8] = 0x00;
            result[9] = 0x00;
            //结束符
            result[10] = 0xAF;
            byte[] a = new byte[8];
            for(int i = 0; i < 8; i++)
            {

                a[i] = result[2 + i];
            }
            byte[] c = UniversalSoundAgreementUtils.CRC16(a);
            result[8] = c[0];
            result[9] = c[1];
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result);
            t_RS485.data = result;
            int s = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if (s == 0)
            {
                MessageBox.Show("发送成功!");
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            byte[] result = new byte[1024];
            result[0] = 0xAA;
            result[1] = 0x55;
            //保留字
            result[2] = 0x01;
            result[3] = 0x64;
            result[4] = 0x00;
            //命令
            result[5] = 0x07;
            //长度
            result[6] = 0x00;
            result[7] = 0x00;
            //数据 

            //校检
            result[8] = 0x00;
            result[9] = 0x00;
            //结束符
            result[10] = 0xAF;
            byte[] a = new byte[8];
            for (int i = 0; i < 8; i++)
            {

                a[i] = result[2 + i];
            }
            byte[] c = UniversalSoundAgreementUtils.CRC16(a);
            result[8] = c[0];
            result[9] = c[1];
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result);
            t_RS485.data = result;
            int s = MyClass.Net_SendRS485Data(0, ref t_RS485);
        }

        //时间显示
        private void Button7_Click(object sender, EventArgs e)
        {
            byte[] result = UniversalSoundAgreementUtils.setTimeShowLineNo(0);
            t_RS485.rs485Id = 0;
            t_RS485.dataLen = CommonUtils.getLength(result);
            t_RS485.data = result;
            int s = MyClass.Net_SendRS485Data(0, ref t_RS485);
            if(s == 0)
            {
                MessageBox.Show("发送成功!");
            }
            else
            {
                MessageBox.Show("发送失败!");
            }
        }
    }
}