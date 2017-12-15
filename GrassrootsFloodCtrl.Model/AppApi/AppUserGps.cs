// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppUserGps.cs" company="lizhenhua">
//   lizhenhua
// </copyright>
// <summary>
//   Defines the AppUserGps type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Model.AppApi
{
    using Dy.Common;

    using ServiceStack.DataAnnotations;

    /// <summary>
    /// The app user gps.
    /// </summary>
   public class AppUserGps
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [field("username", "string", null, null)]
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the lng.
        /// </summary>
        [field("Lng", "string", null, null)]
        [StringLength(50)]
        public string Lng { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        [field("Lat", "string", null, null)]
        [StringLength(50)]
        public string Lat { get; set; }

        /// <summary>
        /// Gets or sets the user adcd.
        /// </summary>
        [field("用户的ADCD", "string", null, null)]
        [StringLength(50)]
        public string UserAdcd { get; set; }

        /// <summary>
        /// Gets or sets the create or update time.
        /// </summary>
        [field("增加或修改时间", "string", null, null)]
        [StringLength(50)]
        public string CreateOrUpdateTime { get; set; }

        [field("用户真实姓名", "string", null, null)]
        [StringLength(50)]
        public string UserRealName { get; set; }
    }
}