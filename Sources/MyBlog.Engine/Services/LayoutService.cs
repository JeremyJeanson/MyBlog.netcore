using MyBlog.Engine.Models;
using MyBlog.Strings;
using MyLib.Web.Helpers;

namespace MyBlog.Engine.Services
{
    public sealed class LayoutService
    {
        #region Declarations

        #endregion

        #region Constructor

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data"></param>
        /// <param name="user"></param>
        /// <param name="userSettings"></param>
        public LayoutService(DataService data, UserService user, UserSettingsService userSettings)
        {
            Model = new LayoutViewModel
            {
                Version = ApplicationHelper.GetVersion() + " " + Resources.VersionSuffix,
                Categories = data.GetGateoriesCounters(),
                Archives = data.GetArchives(),
                User = user.Get(),
                UserSettings = userSettings.Get(),
            };
        }

        #endregion

        #region Properties

        #endregion

        #region Methodes

        public LayoutViewModel Model { get; }

        #endregion
    }
}
