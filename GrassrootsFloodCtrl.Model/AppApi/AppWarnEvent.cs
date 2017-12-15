// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppWarnEvent.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   Defines the AppWarnEvent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Model.AppApi
{
    using System;

    using Dy.Common;

    using ServiceStack.DataAnnotations;

    /// <summary>
    /// The app warn event.
    /// </summary>
    public class AppWarnEvent
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [field("用户电话","string",null,null)]
        [StringLength(50)]
        public string UserPhone { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [field("用户名称", "string", null, null)]
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        [field("事件名称", "string", null, null)]
        [StringLength(50)]
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is start warning.
        /// </summary>
        [field("是否启动预警", "bool", null, null)]
        [StringLength(50)]
        public bool IsStartWarning { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [field("启动时间", "datetime", null, null)]
        [StringLength(50)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        [field("结束时间", "datetime", null, null)]
        [StringLength(50)]
        public DateTime? EndTime { get; set; }
        [field("adcd", "datetime", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }
    }
}