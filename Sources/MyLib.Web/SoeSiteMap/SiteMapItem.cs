using System;

namespace MyLib.Web.SoeSiteMap
{
    /// <summary>
    /// Site map element
    /// </summary>
    public sealed class SoeSiteMapItem
    {
        #region Declarations

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri"></param>
        public SoeSiteMapItem(String uri) => Uri = uri;

        #endregion

        #region Properties

        internal String Uri { get; }

        public Nullable<DateTime> Lastmodified { get; set; }
        public Nullable<Frequence> ChangeFrequence { get; set; }
        public Nullable<Int32> Priority { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public enum Frequence
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never,
    }
}
