// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppWarnInfo.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   Defines the AppWarnInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Model.AppApi
{
    using Dy.Common;

    using ServiceStack.DataAnnotations;

    /// <summary>
    /// The app warn info.
    /// </summary>
    public class AppWarnInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the app warn event id.
        /// </summary>
        [field("事件ID（外键）", "int", null, null)]
        [StringLength(50)]
        public int AppWarnEventId { get; set; }

        /// <summary>
        /// Gets or sets the warn level.
        /// </summary>
        [field("预警等级", "int", null, null)]
        [StringLength(50)]
        public int? WarnLevel { get; set; }

        /// <summary>
        /// Gets or sets the warn message.
        /// </summary>
        [field("发送的消息", "string", null, null)]
        [StringLength(50)]
        public string WarnMessage { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        [field("备注", "string", null, null)]
        [StringLength(50)]
        public string Remark { get; set; }
    }
}