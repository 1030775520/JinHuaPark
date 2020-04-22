using CSharpDemo.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDemo
{
    public class DataEntity
    {
        //屏幕指令
        public List<ScreenEntity> screen;

        //语音指令
        public List<VoiceEntity> voice;

        //是否开闸 1开 0不开
        public int open_flag;

        //是否etc支付
        public int etc_flag;

        //金额
        public PkFeeEntity pk_fee;

        //停车时长
        public PkTimeEntity pk_time;
    }
}
