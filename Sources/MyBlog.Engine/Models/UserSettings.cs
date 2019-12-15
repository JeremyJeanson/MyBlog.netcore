using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Engine.Models
{
    public sealed class UserSettings
    {
        #region Declarations

        private Int32 _lineHeight;
        private Int32 _zoom;
        private Int32 _theme;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal UserSettings()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// User concent to cookies
        /// </summary>
        public Boolean CookiesConcentClosed { get; set; }

        /// <summary>
        /// use the dyslexic font
        /// </summary>
        public Boolean UseDyslexicFont { get; set; }

        /// <summary>
        /// Justify or not the text
        /// </summary>
        public Boolean TextIsJutified { get; set; } = true;

        /// <summary>
        /// Set the line height
        /// </summary>
        public Int32 LineHeight
        {
            get => _lineHeight;
            set
            {
                if (value < 0 || value > 2) value = 0;
                _lineHeight = value;
            }
        }

        /// <summary>
        /// Zoom factor of the display
        /// </summary>
        public Int32 Zoom
        {
            get => _zoom;
            set
            {
                if (value < 0 || value > 2) value = 0;
                _zoom = value;
            }
        }

        /// <summary>
        /// Theme
        /// </summary>
        public Int32 Theme
        {
            get => _theme;
            set
            {
                if (value < 0 || value > 2) value = 0;
                _theme = value;
            }
        }

        #endregion
    }
}
