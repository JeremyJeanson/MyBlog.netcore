using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using MyLib.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Models
{
    public sealed class LayoutViewModel
    {
        #region Declarations

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        /// <param name="user"></param>
        /// <param name="userSettings"></param>
        public LayoutViewModel(DataService data, UserService user, UserSettingsService userSettings)
        {
            Version = ApplicationHelper.GetVersion() + " " + Resources.VersionSuffix;
            Categories = data.GetGateoriesCounters();
            Archives = data.GetArchives();
            User = user.Get();
            UserSettings = userSettings.Get();
        }

        #endregion

        #region Properties

        /// <summary>
        ///  counters
        /// </summary>
        public Counter[] Categories { get; }

        /// <summary>
        /// Archives
        /// </summary>
        public ArchiveLink[] Archives { get; }

        /// <summary>
        /// Version
        /// </summary>
        public String Version { get; }

        /// <summary>
        /// User loged in
        /// </summary>
        public UserProfile User { get; }

        /// <summary>
        /// User's settings
        /// </summary>
        public UserSettings UserSettings { get; }

        #endregion
    }
}
