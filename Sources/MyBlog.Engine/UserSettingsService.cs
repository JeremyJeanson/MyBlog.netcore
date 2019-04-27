using Microsoft.AspNetCore.Http;
using MyBlog.Engine.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace MyBlog.Engine
{
    public sealed class UserSettingsService
    {
        #region Declarations

        private const String KeyName = "UserSettings";
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserSettingsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Get current user's settings
        /// </summary>
        /// <returns></returns>
        public UserSettings Get()
        {
            // get settings from cookie
            UserSettings settings = GetFromCookie();

            // Return default settings if the cookie is empty or not used
            return settings ?? new UserSettings
                {
                    CookiesConcentClosed = false,
                    UseDyslexicFont = false
                };
        }

        /// <summary>
        /// Set current user's settings
        /// </summary>
        /// <param name="settings"></param>
        public void Set(UserSettings settings)
        {
            // Update or delate the cookie
            if (settings == null)
            {
                // Delete cookie
                DeleteCookie();
            }
            else
            {

                // Save cookie
                SaveCookie(settings);
            }
        }

        /// <summary>
        /// Get user's settings from the cookie
        /// </summary>
        /// <returns>Default value if cookie isn't used</returns>
        private UserSettings GetFromCookie()
        {
            String cookie;
            // Try to get the cookie value
            if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(KeyName, out cookie)
                || String.IsNullOrWhiteSpace(cookie))
            {
                return null;
            }

            if (String.IsNullOrWhiteSpace(cookie)) return null;

            try
            {
                // Return settings
                return JsonConvert.DeserializeObject<UserSettings>(cookie);
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Save settings into the cookie
        /// </summary>
        /// <param name="value"></param>
        private void SaveCookie(UserSettings settings)
        {
            String value = JsonConvert.SerializeObject(settings);

            // update or add the cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                KeyName,
                value,
                new CookieOptions
                {
                    // Expire in one year
                    Expires = DateTime.Now.AddYears(1)
                });
        }

        /// <summary>
        /// Delete the current cookie
        /// </summary>
        private void DeleteCookie()
        {
            // Delete the cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(KeyName);
        }

        #endregion
    }
}
