// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSendMessage.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   The app send message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Model.AppApi
{
    using Dy.Common;

    using ServiceStack.DataAnnotations;
    using System;

    /// <summary>
    /// The app send message.
    /// </summary>
    public class AppSendMessage
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the send message by user id.
        /// </summary>
        [field("信息发送人Phone", "string", null, null)]
        [StringLength(50)]
        public string SendMessagePhone { get; set; }

        /// <summary>
        /// Gets or sets the send message by user name.
        /// </summary>
        [field("信息发送人", "string", null, null)]
        [StringLength(50)]
        public string SendMessageByUserName { get; set; }

        /// <summary>
        /// Gets or sets the app warn event id.
        /// </summary>
        [field("发生的事件的Id", "int", null, null)]
        [StringLength(50)]
        public int AppWarnEventId { get; set; }

        /// <summary>
        /// Gets or sets the warninglevel.
        /// </summary>
        [field("预警等级", "int", null, null)]
        [StringLength(50)]
        public int? Warninglevel { get; set; }

        /// <summary>
        /// Gets or sets the send message.
        /// </summary>
        [field("信息内容", "string", null, null)]
        [StringLength(50)]
        public string SendMessage { get; set; }

        /// <summary>
        /// Gets or sets the receive user id.
        /// </summary>
        [field("接受信息人手机", "string", null, null)]
        public string ReceiveUserPhone { get; set; }

        /// <summary>
        /// Gets or sets the receive user name.
        /// </summary>
        [field("接收信息人", "string", null, null)]
        [StringLength(50)]
        public string ReceiveUserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is readed.
        /// </summary>
        [field("是否已经阅读", "bool", null, null)]
        public bool IsReaded { get; set; }

        /// <summary>
        /// Gets or sets the user read time.
        /// </summary>
        [field("读取信息时间", "dateTime", null, null)]
        public DateTime? UserReadTime { get; set; }
        [field("事件ID", "int", null, null)]
        public int AppWarnInfoID { get; set; }

        [field("接收时间", "dateTime", null, null)]
        public DateTime? ReceiveDateTime { get; set; }
        [field("是否已经关闭", "bool", null, null)]
        public bool IsClosed { get; set; }

        [field("Adcd", "string", null, null)]
        [StringLength(50)]
        public string ReciveAdcd { get; set; }
        [field("Adcd", "string", null, null)]
        [StringLength(50)]
        public string SendAdcd { get; set; }
        [field("Postion", "string", null, null)]
        [StringLength(500)]
        public string Position { get; set; }
        [field("Remark", "string", null, null)]
        [StringLength(500)]
        public string Remark { get; set; }
        [field("IsForward", "", null, null)]
        public bool IsForward { get; set; }

        [field("VillageMessage", "string", null, null)]
        [StringLength(500)]
        public string VillageMessage { get; set; }
        [field("IsSendMessage", "", null, null)]
        public bool IsSendMessage { get; set; }

    }
}