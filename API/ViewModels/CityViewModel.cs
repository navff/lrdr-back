using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    /// <summary>
    /// Город
    /// </summary>
    public class CityViewModelGet
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Город
    /// </summary>
    public class CityViewModelPost
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }

}