using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharpDemo
{
    class MyClass
    {
        public const int MAX_HOST_LEN = 16;
        public const int ONE_DIRECTION_LANES = 5;
        public const int VERSION_NAME_LEN = 64;

        //最大的led屏幕数量
        public const int MAX_LED_COUNT = 4;

        //最大的led屏幕内容
        public const int MAX_LED_CONTENT = 100;

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_Init();

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Net_UNinit();

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_AddCamera(string ptIp);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_DelCamera(int tHandle);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ConnCamera(int tHandle,  ushort usPort,  ushort usTimeout);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_DisConnCamera(int tHandle);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StartVideo(int tHandle, int niStreamType, IntPtr hWnd);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StopVideo(int tHandle);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GetSdkVersion([MarshalAs(UnmanagedType.LPStr)]StringBuilder szVersion, ref int ptLen);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_ImageUserInfo
        {
            public ushort usWidth;   /*图片的宽度，单位:像素*/
            public ushort usHeight;  /*图片的高度，单位:像素*/
            public byte ucVehicleColor;/*车身颜色，E_ColorType*/
            public byte ucVehicleBrand;/*车标，E_VehicleFlag*/
            public byte ucVehicleSize;/*车型(大中小)，ITS_Tb_Vt*/
            public byte ucPlateColor;/*车牌颜色，E_ColorType*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] szLprResult;/*车牌，若为'\0'，表示无效GB2312编码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] usLpBox;  /*车牌位置，左上角(0, 1), 右下角(2,3)*/
            public byte ucLprType;/*车牌类型*/
            public ushort usSpeed;     /*单位km/h*/
            public byte ucSnapType;/*抓拍模式*/
            public byte ucReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] acSnapTime;         /*图片抓拍时间:格式YYYYMMDDHHMMSSmmm(年月日时分秒毫秒)*/
            public byte ucViolateCode;    /*违法代码E_ViolationCode*/
            public byte ucLaneNo;          /*车道号,从0开始编码*/
            public uint uiVehicleId; 		/*检测到的车辆id，若为同一辆车，则id相同*/
            public byte ucScore;    		/*车牌识别可行度*/
            public byte ucDirection;       /*行车方向E_Direction*/
            public byte ucTotalNum;        /*该车辆抓拍总张数*/
            public byte ucSnapshotIndex;   /*当前抓拍第几张，从0开始编号*/
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_ImageUserInfo2
        {
            public ushort usWidth;   /*图片的宽度，单位:像素*/
            public ushort usHeight;  /*图片的高度，单位:像素*/
            public byte ucVehicleColor;/*车身颜色，E_ColorType*/
            public byte ucVehicleBrand;/*车标，E_VehicleFlag*/
            public byte ucVehicleSize;/*车型(大中小)，ITS_Tb_Vt*/
            public byte ucPlateColor;/*车牌颜色，E_ColorType*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] szLprResult;/*车牌，若为'\0'，表示无效GB2312编码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] usLpBox;  /*车牌位置，左上角(0, 1), 右下角(2,3)*/
            public byte ucLprType;/*车牌类型*/
            public ushort usSpeed;     /*单位km/h*/
            public byte ucSnapType;/*抓拍模式*/
            public byte ucReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] acSnapTime;         /*图片抓拍时间:格式YYYYMMDDHHMMSSmmm(年月日时分秒毫秒)*/
            public byte ucViolateCode;    /*违法代码E_ViolationCode*/
            public byte ucLaneNo;          /*车道号,从0开始编码*/
            public uint uiVehicleId; 		/*检测到的车辆id，若为同一辆车，则id相同*/
            public byte ucScore;    		/*车牌识别可行度*/
            public byte ucDirection;       /*行车方向E_Direction*/
            public byte ucTotalNum;        /*该车辆抓拍总张数*/
            public byte ucSnapshotIndex;   /*当前抓拍第几张，从0开始编号*/

            public uint     uiVideoProcTime;   /*摄像头从触发拍照到上传车牌的用时，单位ms*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	        public byte[]	strVehicleBrand; /*车型品牌*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	        public byte[]	strConfidence;	  /*车型可信度*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20 * 12)]
            public byte[] strSpecialCode; /*特征码, 实际上是20个字符串的数组(char strSpecialCode[20][12], 存储的是20个浮点数)，此处用一维数组标准*/
	        public byte   ucHasCar;				/*是否有车*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public byte[]	aucReserved1;			/*保留字段*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 252)]
	        public byte[]   aucReserved2;		/*保留字段*/
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_PicInfo
        {
            public uint uiPanoramaPicLen;  /*全景图片大小*/
            public uint uiVehiclePicLen;      /*车牌图片大小*/
            public IntPtr ptPanoramaPicBuff;   /*全景图片缓冲区*/
            public IntPtr ptVehiclePicBuff;  /*车牌图片缓冲区*/
        };

        //特征码比较，fSpecialCode1，fSpecialCode2都为20个浮点数数组，返回分数，分数越高越相似
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern float Net_MatchSpecialCode2(float[] fSpecialCode1, float[] fSpecialCode2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCB(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecv(FGetImageCB fCb);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCB2(int tHandle, uint uiImageId, ref T_ImageUserInfo2 tImageInfo, ref T_PicInfo tPicInfo);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecv2(FGetImageCB2 fCb);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCBEx(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecvEx(int tHandle, FGetImageCBEx fCb, IntPtr obj);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCBEx2(int tHandle, uint uiImageId, ref T_ImageUserInfo2 tImageInfo, ref T_PicInfo tPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecvEx2(int tHandle, FGetImageCBEx2 fCb, IntPtr obj);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetOffLineImageCBEx(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLineImageRecvEx(int tHandle, FGetOffLineImageCBEx fCb, IntPtr obj);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetReportCBEx(int tHandle, byte ucType, IntPtr ptMessage, IntPtr pUserData);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegReportMessEx(int tHandle, FGetReportCBEx fCb, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLineClient(int tHandle);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_FrameInfo
        {
            public uint uiFrameId;          //帧ID
            public uint uiTimeStamp;        //RTP时间戳
            public uint uiEncSize;          //帧大小
            public uint uiFrameType;        //1:i帧 0:其它
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ImageSnap(int tHandle, ref T_FrameInfo ptImageSnap);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_ControlGate
        {
            [MarshalAs(UnmanagedType.U1)]
            public byte ucState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] ucReserved;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GateSetup(int tHandle, ref T_ControlGate ptControlGate);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_BlackWhiteList
        {
            public byte LprMode;  /* 0：黑名单；1：白名单*/
            public byte LprCode;      /* 0：车牌号码utf-8字符编码；1：车牌号码gb2312字符编码*/
            public byte Lprnew; /*0： 重新发送；1：续传；2:删除；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_BlackWhiteListSend(int tHandle, ref T_BlackWhiteList ptBalckWhiteList);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_GetBlackWhiteList
        {
            public byte LprMode;  /* 0：黑名单；1：白名单*/
            public byte LprCode;      /* 0：车牌号码utf-8字符编码；1：车牌号码gb2312字符编码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_GetBlackWhiteList(int tHandle, ref T_GetBlackWhiteList ptGetBalckWhiteList);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_GetBlackWhiteListAsCSV(int tHandle, ref T_GetBlackWhiteList ptGetBalckWhiteList);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_LprResult
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
  	        public byte [] LprResult;/*车牌号码；单条消息最多80条车牌号码；其它分多条消息发送*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte [] StartTime;//eg:20151012190303 YYYYMMDDHHMMSS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte [] EndTime;//eg:20151012190303 YYYYMMDDHHMMSS
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct T_BlackWhiteListCount
        {
            public int uiCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_BlackWhiteListSetup(ref T_LprResult ptLprResult, ref T_BlackWhiteListCount ptBlackWhiteListCount);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveImageToJpeg(int tHandle, string ucPathNmme);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GetJpgBuffer(int tHandle, ref IntPtr ucJpgBuffer, ref ulong ulSize);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_FreeBuffer(IntPtr pJpgBuffer);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveJpgFile(int tHandle, string strJpgFile);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveBmpFile(int tHandle, string strBmpFile);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StartRecord(int tHandle, string strFile);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StopRecord(int tHandle);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ShowPlateRegion(int tHandle, int niShowMode);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_UpdatePlateRegion(int tHandle);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_VehPayRsp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	        public byte [] acPlate;	 /* 车牌号码，GB2312编码 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
	        public byte [] acEntryTime; /* 入场时间*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
	        public byte [] acExitTime; /* 出场时间*/
            public uint uiRequired;  /* 应付金额，0.1元为单位*/
            public uint uiPrepaid;  	/* 已付金额，0.1元为单位*/
            public byte ucVehType;  /* 车辆类型1小车2大车 E_ParkVehSize*/
            public byte ucUserType;  /*会员类型E_MemberType*/
            public byte ucResultCode; /* 收费结果状态码0 成功 1 没有找到入场记录*/
            public byte acReserved;
        };

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetOffLinePayRecordCB(int tHandle, byte ucType, ref T_VehPayRsp ptVehPayInfo, uint uiLen, ref T_PicInfo ptPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLinePayRecord(int tHandle, FGetOffLinePayRecordCB fCb, IntPtr obj);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_NetSetup
        {
	        public byte	ucEth;				/* 以太网口编号,目前只支持0*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public byte	[] aucReserved;
	        public uint	uiIPAddress;		
	        public uint	uiMaskAddress;
	        public uint	uiGatewayAddress;
	        public uint	uiDNS1;
	        public uint	uiDNS2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_HOST_LEN)]
	        public byte	[] szHostName;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_NETSetup(int tHandle, ref T_NetSetup ptNetSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryNETSetup(int tHandle, ref T_NetSetup ptNetSetup);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FDrawFunCallBack(int tHandle, IntPtr hdc, int width, int height, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SetDrawFunCallBack(int tHandle, FDrawFunCallBack fCb, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ReadTwoEncpyption(int tHandle, [MarshalAs(UnmanagedType.LPStr)]StringBuilder pBuff, uint uiSizeBuff);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_WriteTwoEncpyption(int tHandle, string pUserData, uint uiDataLen);

        /* 点*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Point
        {
	        public short sX;
	        public short sY;
        };

        /* 线*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Line
        {
	        public T_Point tStartPoint;
	        public T_Point tEndPoint;
        };

        /* 区域*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Rect
        {
	        public T_Point tLefTop;
	        public T_Point tRightBottom;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_DivisionLine
        {
	        public byte ucDashedLine;
	        public byte ucReserved;
	        public T_Line tLine;
        };

        /*视频检测区域参数配置*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_VideoDetectParamSetup
        {
	        public byte		ucLanes;		/*车道数  */
	        public byte		ucSnapAutoBike; /*摩托车抓拍1:抓拍0:不抓拍*/
	        public ushort	usBanTime;		/*违停时长阀值，单位:秒*/
	        public byte		ucVirLoopNum;	/*虚拟线圈，抓拍摩托车使用*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public byte	[]	aucReserved;

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_Point	[]			 atPlateRegion;		/*车牌识别区域*/
	        public T_Line				 aStopLine;              /*卡口:触发线 电子警察:停止线*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_Point	[]			atSpeedRegion;		/*测速区域  */
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_Line	[]			atOccupCheckLine;	/*占有率检测线   */
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = ONE_DIRECTION_LANES + 1, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_DivisionLine	[]	atDivisionLine;/*车道分割线*/
	        public T_Line				tPrefixLine;		/*电警前置线*/
	        public T_Line				tLeftBorderLine;	/*电警左边界线*/
	        public T_Line				tRightBorderLine;	/*电警右边界线*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = ONE_DIRECTION_LANES * 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_Point	[]			atVirLoop; /*虚拟线圈*/
        	
	        /*非法停车区*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
	        public T_Point	[]	atBanRegion;   /*非法禁停区*/
        	
	        public  ushort		usCameraHeight;        /*相机安装高度*/
	        public  ushort		usRectLength;          /*路面矩形长度(厘米)*/
	        public  ushort		usRectWidth;           /*路面矩形宽度(厘米)*/
	        public  ushort		usTotalDis;            /*矩形坐上角到摄像机垂直投影的距离(厘米)*/
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_VideoDetectSetup(int tHandle, ref T_VideoDetectParamSetup ptVideoDetectParamSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryVideoDetectSetup(int tHandle, ref T_VideoDetectParamSetup ptVideoDetectParamSetup);

        [StructLayout(LayoutKind.Sequential)]
        public  struct T_RS485Data
        {
	        public  byte		rs485Id;
	        public  byte		ucReserved; 
	        public  ushort		dataLen; 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
	        public  byte []	    data;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_SendRS485Data(int tHandle, ref T_RS485Data ptRS485Data);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_DCTimeSetup
        {
	        public ushort usYear;
	        public byte	ucMonth;
	        public byte	ucDay;
	        public byte	ucHour;
	        public byte	ucMinute;
	        public byte	ucSecond;
	        public byte	ucDayFmt;
	        public byte	ucTimeFmt;
	        public byte	ucTimeZone;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public byte	[] aucReserved;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_TimeSetup(int tHandle, ref T_DCTimeSetup ptTimeSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryTimeSetup(int tHandle, ref T_DCTimeSetup ptTimeSetup);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_VehicleVAFunSetup
        {
	        public byte	ucPlateRecogEn;				/* 车牌识别使能*/
	        public byte	ucVehicleSizeRecogEn;		/* 车型识别使能*/
	        public byte	ucVehicleColorRecogEn;		/* 车身颜色识别使能*/
	        public byte	ucVehicleBrandRecogEn;		/* 车标识别使能*/
	        public byte	ucVehicleSizeClassify;		/* 同一辆车抓拍间隔时间高字节*/
	        public byte	ucLocalCity; 				/*车牌的第二个字符，'A'~'Z'的数字编码*/
	        public byte ucPlateDirection;           /*车牌方向E_PlateDirection*/
	        public byte	ucCpTimeInterval;           //同一辆车抓拍间隔时间低字节
	        public uint	uiPlateDefaultWord;			/* 默认省份，采用UTF-8编码*/
        	
	        public ushort usMinPlateW;				/*车牌识别最小宽度,单位:像素*/
            public ushort usMaxPlateW;				/*车牌识别最大宽度，单位:像素*/
	        public byte	ucDoubleYellowPlate;		/*双层黄牌识别，1：识别 0：不知别*/
	        public byte	ucDoubleArmyPlate;			/*双层军牌识别，1：识别 0：不知别*/
	        public byte	ucPolicePlate;				/*武警车牌识别，1：识别 0：不知别*/
	        public byte	ucPlateFeature;	            /*车牌特写*/ 
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_VehicleVAFunSetup(int tHandle, ref T_VehicleVAFunSetup ptVehicleVAFunSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryVehicleVAFunSetup(int tHandle, ref T_VehicleVAFunSetup ptVehicleVAFunSetup);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_MACSetup
        {
	        public byte	ucEth;				/* 以太网口编号,目前只支持0*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	        public byte	[] aucReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public byte	[] aucMACAddresss;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_RcvMsg
        {
	        public uint		uiFlag;								/*标志位，111表示Version、IP、MAC*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public byte	[]	aucDstMACAdd;					    /*目标MAC地址*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
	        public byte []  aucAdapterName;	                    /*网络适配器名称*/		
	        public uint		uiAdapterSubMask;					/*网络适配器子网掩码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []  ancDevType;						    /* 设备类型，出厂时设定；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []  ancSerialNum;					    /* 设备序列号*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
	        public byte []  ancAppVersion;	                    /* 软件版本*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
	        public byte []  ancDSPVersion;	                    /* DSP版本*/
            public T_NetSetup tNetSetup;						/* 网络信息*/
            public T_MACSetup tMacSetup;						/* MAC信息*/
        };
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FNetFindDeviceCallback(ref T_RcvMsg ptFindDevice, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_FindDevice(FNetFindDeviceCallback fCb, IntPtr obj);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_QueVersionRsp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szKernelVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szFileSystemVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szAppVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szWebVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szHardwareVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szDevType;		/* 设备类型描述，出厂时设定；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	        public byte []		szSerialNum;    	/*产品序列号*/
	        public uint	        uiDateOfExpiry;		/*产品有效期*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[]       szDSPVersion;
        };

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_StartTalk(int tHandle);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_StopTalk(int tHandle);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_SubLedSetup
        {
            public byte ucEnable;
            public byte ucInterVal;
            public byte ucLedLine;
            public byte ucMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_CONTENT)]
            public byte[] aucContent;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct T_LedSetup
        {
            public byte ucAudioEnable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] ucReserverd;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_COUNT)]
            public T_SubLedSetup[] atSubLedInIdle;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_COUNT)]
            public T_SubLedSetup[] atSubLedInBusy;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_COUNT)]
            public T_SubLedSetup[] atSubLedOutIdle;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LED_COUNT)]
            public T_SubLedSetup[] atSubLedOutBusy;
        }

        //设置led屏幕显示内容
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_LedSetup(int tHandle, ref T_LedSetup t_LedSetup);

        //查询Led屏幕显示内容
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryLedSetup(int tHandle, ref T_LedSetup t_LedSetup);

        [StructLayout(LayoutKind.Sequential)]
        public  struct T_ParkNum
        {
          public uint ParkNum;
        }

        //设置空余停车位
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_ParkNumSetup(int tHandle, ref T_ParkNum t_ParkNum);

        

        //线圈参数
        [StructLayout(LayoutKind.Sequential)]
        public struct T_LoopParamSetup
        {
        public byte ucLaneIndex;
        public byte ucLoopLaneIndex;
        public byte ucMode;
        public byte szReserved;
        public byte usLoopWidth;
        public byte usLoopInterval;
        }

        //队列结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct T_ControlGateQueue
        {
            [MarshalAs(UnmanagedType.U1)]
            public byte ucState;//1 开启队列模式 2 关闭队列模式
            [MarshalAs(UnmanagedType.U1)]
            public byte ucIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] ucReserved;
        }

        //设置队列模式
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ControlGateQueue(int tHandle, ref T_ControlGateQueue ptControlGate);

    }
}
