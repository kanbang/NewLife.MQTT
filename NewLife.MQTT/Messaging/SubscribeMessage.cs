﻿using System;
using System.Collections.Generic;
using System.IO;

namespace NewLife.MQTT.Messaging
{
    /// <summary>订阅请求</summary>
    public sealed class SubscribeMessage : MqttIdMessage
    {
        #region 属性
        /// <summary>请求集合</summary>
        public IList<Subscription> Requests { get; set; }
        #endregion

        #region 构造
        /// <summary>实例化</summary>
        public SubscribeMessage()
        {
            Type = MqttType.Subscribe;
            QoS = QualityOfService.AtLeastOnce;
        }

        /// <summary>已重载</summary>
        public override String ToString() => Requests == null ? Type + "" : $"{Type}[Topic={Requests[0].TopicFilter}, Qos={(Int32)Requests[0].QualityOfService}]";
        #endregion

        #region 读写方法
        /// <summary>从数据流中读取消息</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文</param>
        /// <returns>是否成功</returns>
        protected override Boolean OnRead(Stream stream, Object context)
        {
            if (!base.OnRead(stream, context)) return false;

            var list = new List<Subscription>();
            while (stream.Position < stream.Length)
            {
                var ss = new Subscription(ReadString(stream), (QualityOfService)stream.ReadByte());
                list.Add(ss);
            }
            Requests = list;

            return true;
        }

        /// <summary>把消息写入到数据流中</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文</param>
        protected override Boolean OnWrite(Stream stream, Object context)
        {
            if (!base.OnWrite(stream, context)) return false;

            foreach (var item in Requests)
            {
                WriteString(stream, item.TopicFilter);
                stream.Write((Byte)item.QualityOfService);
            }

            return true;
        }
        #endregion
    }
}