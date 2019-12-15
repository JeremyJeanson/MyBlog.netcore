using MyBlog.Engine.Data.Models;
using System;

namespace MyBlog.Engine.Models
{
    public sealed class LayoutViewModel
    {
        #region Declarations

        #endregion

        #region Constructor

        #endregion

        #region Properties

        /// <summary>
        ///  counters
        /// </summary>
        public Counter[] Categories { get; internal set; }

        /// <summary>
        /// Archives
        /// </summary>
        public ArchiveLink[] Archives { get; internal set; }

        /// <summary>
        /// Version
        /// </summary>
        public String Version { get; internal set; }

        /// <summary>
        /// User loged in
        /// </summary>
        public UserProfile User { get; internal set; }

        /// <summary>
        /// User's settings
        /// </summary>
        public UserSettings UserSettings { get; internal set; }

        #endregion
    }
}
