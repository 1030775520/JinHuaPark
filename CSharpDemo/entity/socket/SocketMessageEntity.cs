using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDemo.entity.socket
{
    class SocketMessageEntity
    {
        public string ip;
        //1 开   2 常开  3关闭常开
        public byte state;

        //错误代码  0无错  其他有错
        public byte error_code;

        //错误消息(如果有错此消息不为空)
        public string error_msg;

        //数据
        public Object data;

        //通道号
        public string ch_code;
    }
}
