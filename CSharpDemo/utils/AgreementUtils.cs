using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDemo.utils
{
    //协议工具类
    class AgreementUtils
    {
        //得到文本命令(自定义)
        public static byte[] getTxtCommand(byte lineNo, string content)
        {
            byte[] result = new byte[1024];
            result[0] = 0xA5;
            result[1] = 0xA5;
            //总类型  1显示  2语音
            result[2] = 0x01;
            byte[] contentBytes = CommonUtils.stringToByteGB2312(content);
            int innerLength = contentBytes.Length;
            //数据总长度
            result[3] = (byte)(innerLength + 3);
            //内部命令(第几行)
            byte dataCommand = getDataCommandByLine(lineNo);
            result[4] = dataCommand;
            //内部长度
            result[5] = (byte)innerLength;
            //内部数据
            for (int i = 0; i < innerLength; i++)
            {
                result[5 + i + 1] = contentBytes[i];
            }
            //checksum
            result[5 + innerLength + 1] = getCheckSum(dataCommand, innerLength, contentBytes);
            result[5 + innerLength + 2] = 0xBE;
            result[5 + innerLength + 3] = 0xEF;
            return result;
        }       

        //得到颜色命令
        public static byte[] getColorCommand(byte lineNo,string color)
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
            //行号
            result[6] = lineNo;
            //颜色
            result[7] = getCommandByColor(color);
            //checksum
            result[8] = getCheckSum(result[4],result[5],new byte[2] { result[6] , result[7] });
            result[9] = 0xBE;
            result[10] = 0xEF;
            //第1行红色：35 02 01 00 C7
            //第2行绿色：35 02 02 01 C5
            //第3行黄色：35 02 03 02 C3
            //第4行绿色：35 02 04 01 C3

            return result;
        }

        //得到颜色对应的命令
        public static byte getCommandByColor(string color)
        {
            byte result = 0;
            switch (color)
            {
                case "red":
                    {
                        result = 0;
                        break;
                    }
                case "green":
                    {
                        result = 1;
                        break;
                    }
                case "yellow":
                    {
                        result = 2;
                        break;
                    }
                default:
                    {
                        result = 0;
                        break;
                    }
            }
            return result;    
        }

        //根据行号得到数据命令(自定义信息)
        public static byte getDataCommandByLine(byte lineNo)
        {
            byte result = 0;
            switch (lineNo)
            {
                case 1:
                    result = 5;
                    break;
                case 2:
                    result = 6;
                    break;
                case 3:
                    result = 0x17;
                    break;
                case 4:
                    result = 0x18;
                    break;
                default:
                    result = 5;
                    break;
            }
            return result;
        }

        //计算checksum (数据命令，长度，内容求和取反)
        public static byte getCheckSum(byte command, int length, byte[] content)
        {
            int sum = command + length;
            for (int i = 0; i < content.Length; i++)
            {
                sum += content[i];
            }
            //转成16进制
            string hex = sum.ToString("x8");
            //取反
            string reverse = CommonUtils.getReverseHex(hex);
            //16进制字符串转成字节
            reverse = reverse.Replace(" ", "");
            if ((reverse.Length % 2) != 0)
                reverse += " ";
            byte[] returnBytes = new byte[reverse.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(reverse.Substring(i * 2, 2), 16);
            if (returnBytes.Length > 0)
            {
                return returnBytes[0];
            }
            return 0;
        }

        //得到语音命令
        public static byte[] getSoundCommand(string type, string content)
        {
            byte[] result = new byte[1024];
            //头
            result[0] = 0xA5;
            result[1] = 0xA5;
            //总类型  1显示  2语音
            result[2] = 0x02;
            byte[] inner = getInnerSoundCommand(type, content);
            int innerLength = inner.Length;
            //总长度
            result[3] = (byte)innerLength;
            for(int i = 0; i < innerLength; i++)
            {
                result[4 + i] = inner[i];
            }
            //尾
            result[3 + innerLength + 1] = 0xBE;
            result[3 + innerLength + 2] = 0xEF;
            return result;
        }

        //得到内部的语音命令
        public static byte[] getInnerSoundCommand(string type, string content)
        {
            byte[] result = new byte[4];
            switch (type)
            {
                //收费语音
                case "03":
                    {
                        byte[] r = getChargeData(type, content);
                        return r;
                    }
                    //播报车牌
                case "04":
                    {
                        byte[] plate = CommonUtils.stringToByteGB2312(content);
                        int innerLength = plate.Length;
                        byte[] r = new byte[3 + innerLength];
                        r[0] = 0x04;
                        r[1] = (byte)innerLength;
                        for(int i = 0; i < innerLength; i++)
                        {
                            r[2 + i] = plate[i];
                        }
                        r[2 + innerLength] = getCheckSum(r[0], innerLength, plate);
                        return r;
                    }
                //播报“欢迎光临”
                case "07":
                    {
                        //07 01 01 F6
                        result[0] = 0x07;result[1] = 01;result[2] = 01;result[3] = 0xF6;
                        return result;
                    }
                    //有效期至
                case "0A":
                    {
                        byte[] r = new byte[9];
                        r[0] = 0x0A;
                        r[1] = 6;
                        char[] year = "年".ToCharArray();
                        char[] month = "月".ToCharArray();
                        char[] day = "日".ToCharArray();
                        string[] a = content.Split(year);
                        string y = a[0].Substring(2, 2);
                        string[] b = a[1].Split(month);
                        string m = b[0].PadLeft(2, '0');
                        string[] c = b[1].Split(day);
                        string d = c[0];
                        string date = y + m + d;
                        byte[] innerContent = CommonUtils.stringToByteGB2312(date);
                        int innerLength = innerContent.Length;
                        for(int i = 0; i < innerLength; i++)
                        {
                            r[2 + i] = innerContent[i];
                        }
                        r[8] = getCheckSum(r[0], innerLength, innerContent);
                        return r;
                    }
                //未授权  0B 01 01 F2
                case "0B":
                    {
                        result[0] = 0x0B;result[1] = 01;result[2] = 01;result[3] = 0xF2;
                        return result;
                    }
                //播报语音“月租车剩余XXX天”
                case "0E":
                    {
                        byte[] r = new byte[6];
                        r[0] = 0x0E;
                        r[1] = 3;
                        content = content.PadLeft(3,' ');
                        byte[] innerContent = CommonUtils.stringToByteGB2312(content);
                        int innerLength = innerContent.Length;
                        for(int i = 0; i < innerLength; i++)
                        {
                            r[2 + i] = innerContent[i];
                        }
                        r[5] = getCheckSum(r[0], innerLength, innerContent);
                        return r;
                    }
                //播报语音“请稍候” 0F 01 01 EE
                case "0F":
                    {
                        result[0] = 0x0F; result[1] = 01; result[2] = 01; result[3] = 0xEE;
                        return result;
                    }
                //播报语音 “已过期” 10 01 01 ED
                case "10":
                    {
                        result[0] = 0x10; result[1] = 01; result[2] = 01; result[3] = 0xED;
                        return result;
                    }
                //播报语音 “月租车” 0x13 01 01 EA
                case "13":
                    {
                        result[0] = 0x13; result[1] = 01; result[2] = 01; result[3] = 0xEA;
                        return result;
                    }
                //播报语音 “临时车”0x14 01 01 E9
                case "14":
                    {
                        result[0] = 0x14; result[1] = 01; result[2] = 01; result[3] = 0xE9;
                        return result;
                    }
                   //播报语音 “停车xxxx小时xx分”
                case "15":
                    {
                        int time = int.Parse(content);
                        int hourSecond = 3600;
                        int hour = time / hourSecond;
                        int y = time % hourSecond;
                        int minuteSecond = 60;
                        int minute = y / minuteSecond;
                        string hourStr = hour.ToString().PadLeft(4,' ');
                        string minuteStr = minute.ToString().PadLeft(2, ' ');
                        byte[] r = new byte[9];
                        r[0] = 0x15;
                        r[1] = 6;
                        byte[] innerContent = CommonUtils.stringToByteGB2312(hourStr + minuteStr);
                        int innerLength = innerContent.Length;
                        for(int i = 0; i < innerLength; i++)
                        {
                            r[2 + i] = innerContent[i];
                        }
                        r[8] = getCheckSum(r[0], innerLength, innerContent);
                        return r;
                    }
                //播报语音 “贵宾车” 19 01 01 E4
                case "19":
                    {
                        result[0] = 0x19; result[1] = 01; result[2] = 01; result[3] = 0xE4;
                        return result;
                    }
                // 播报“一路顺风” 08 01 01 F5
                case "08":
                    {
                        result[0] = 0x08; result[1] = 01; result[2] = 01; result[3] = 0xF5;
                        return result;
                    }
             }
            return result;
        }

        //收费
        public static byte[] getChargeData(string type,string content)
        {
            char full = ' ';
            double money = double.Parse(content);
            string text = Math.Round(money / 100, 2).ToString();//保留两位小数
            char[] c = ".".ToCharArray();
            string[] number = text.Split(c);
            string font = number[0].PadLeft(4, full);//不足4位的前面补空格
            byte[] fontArray = CommonUtils.stringToByteGB2312(font);
            byte[] innerContent = new byte[6];
            byte[] behindArray = new byte[2];
            for (int i = 0; i < 4; i++)
            {
                innerContent[i] = fontArray[i];
            }
            //是整数
            if (number.Length == 1)
            {
                behindArray[0] = 0x20;
                behindArray[1] = 0x20;
            }
            //是小数
            else
            {
                string behind = number[1].PadRight(2, full);
                behindArray = CommonUtils.stringToByteGB2312(behind);
            }
            innerContent[4] = behindArray[0];
            innerContent[5] = behindArray[1];
            byte[] r = new byte[9];
            r[0] = byte.Parse(type);
            r[1] = 0x06;
            for (int i = 0; i < 6; i++)
            {
                r[2 + i] = innerContent[i];
            }
            r[8] = getCheckSum(r[0], 6, innerContent);
            return r;
        }

    }
}
