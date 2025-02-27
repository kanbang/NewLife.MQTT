﻿using System;
using System.Collections.Generic;
using System.IO;

namespace NewLife.MQTT.Messaging
{
    /// <summary>取消订阅</summary>
    public sealed class UnsubscribeMessage : MqttIdMessage
    {
        #region 属性
        /// <summary>主题过滤器</summary>
        public IList<String> TopicFilters { get; set; }
        #endregion

        #region 构造
        /// <summary>实例化</summary>
        public UnsubscribeMessage()
        {
            Type = MqttType.UnSubscribe;
            QoS = QualityOfService.AtLeastOnce;
        }

        /// <summary>已重载</summary>
        public override String ToString() => TopicFilters == null ? Type + "" : $"{Type}[Topic={TopicFilters[0]}]";
        #endregion

        #region 读写方法
        /// <summary>从数据流中读取消息</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文</param>
        /// <returns>是否成功</returns>
        protected override Boolean OnRead(Stream stream, Object context)
        {
            if (!base.OnRead(stream, context)) return false;

            var list = new List<String>();
            while (stream.Position < stream.Length)
            {
                list.Add(ReadString(stream));
            }
            TopicFilters = list;

            return true;
        }

        /// <summary>把消息写入到数据流中</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文</param>
        protected override Boolean OnWrite(Stream stream, Object context)
        {
            if (!base.OnWrite(stream, context)) return false;

            foreach (var item in TopicFilters)
            {
                WriteString(stream, item);
            }

            return true;
        }
        #endregion
    }
}