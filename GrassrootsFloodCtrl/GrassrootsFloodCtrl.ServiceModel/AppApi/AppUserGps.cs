// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppUserGps.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   The app user gps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    using System.Collections.Generic;

    /// <summary>
    /// The app user gps.
    /// </summary>
    public class AppUserGps
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserRealName { get; set; }

        /// <summary>
        /// Gets or sets the lng.
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// Gets or sets the user adcd.
        /// </summary>
        public string UserAdcd { get; set; }

        /// <summary>
        /// Gets or sets the create or update time.
        /// </summary>
        public string CreateOrUpdateTime { get; set; }


        public int IsInCounty { get; set; }
    }
}