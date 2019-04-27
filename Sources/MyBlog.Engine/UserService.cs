using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using MyBlog.Engine.Data.Models;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace MyBlog.Engine
{
    public class UserService
    {
        #region Declarations

        private const String UserProfileSessionKey = "UserProfile";
        private DataService _dataService;
        private IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataService"></param>
        public UserService(DataService dataService, IHttpContextAccessor httpContextAccessor)
        {
            _dataService = dataService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        
        /// <summary>
        /// Get user from claims
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public UserProfile GetFromClaims(ClaimsPrincipal principal )
        {
            if (principal == null)
            {
                principal = _httpContextAccessor.HttpContext.User;
            }            
            if (principal == null) return null;

            // Get Identifier
            Claim nameIdentifierClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            // Get name
            Claim nameClaim = principal.FindFirst(ClaimTypes.Name);
            // Add mail
            Claim mailClaim = principal.FindFirst(ClaimTypes.Email);

            // Test nameIdentifier
            if (nameIdentifierClaim == null) return null;

            // Get values
            String issuer = nameIdentifierClaim.Issuer;
            String nameIdentifier = nameIdentifierClaim.Value;

            // Try to get User from database

            // Try to get user from database
            UserProfile user = _dataService.GetUser(issuer, nameIdentifier);

            // Test user
            if (user == null)
            {
                user = new UserProfile
                {
                    Issuer = issuer,
                    NameIdentifier = nameIdentifier,
                    Name = nameClaim?.Value,
                    Email = mailClaim?.Value,
                    EmailValidate = false
                };
            }

            // Return the user
            return user;
        }

        /// <summary>
        /// Get user from session 
        /// </summary>
        /// <returns></returns>
        public UserProfile Get()
        {
            // Try to get user from session
            UserProfile user = GetFromSession();

            // Try to get user from claims
            if (user == null && _httpContextAccessor.HttpContext.User != null)
            {
                user = GetFromClaims(null);
                Set(user);
            }

            // return user
            return user;
        }

        /// <summary>
        /// Get fresh useer updated from dataBase for edition
        /// </summary>
        /// <returns></returns>
        public UserProfile GetFreshUseerUpdatedFromDataBase()
        {
            // Try know user id from session
            UserProfile user = GetFromSession();

            // Try to get user from claims
            if (user == null && _httpContextAccessor.HttpContext.User != null)
            {
                user = GetFromClaims(null);
            }
            if (user == null) return null;

            // Get fresh data from data base if user is not new
            if (user.Id != 0)
            {
                user = _dataService.GetUser(user.Id);
            }

            // return user
            return user;
        }

        /// <summary>
        /// Get usedr from session
        /// </summary>
        /// <returns></returns>
        private UserProfile GetFromSession()
        {
            // Try know user id from session
            String value = _httpContextAccessor.HttpContext.Session.GetString(UserProfileSessionKey);
            if (String.IsNullOrWhiteSpace(value)) return null;

            return JsonConvert.DeserializeObject<UserProfile>(value);
        }

        /// <summary>
        /// Set user in session
        /// </summary>
        /// <param name="user"></param>
        public void Set(UserProfile user)
        {
            String value = JsonConvert.SerializeObject(user);
            _httpContextAccessor.HttpContext.Session.SetString(UserProfileSessionKey, value);
        }

        /// <summary>
        /// Set user in session
        /// </summary>
        /// <param name="user"></param>
        public void Clear()
        {
            _httpContextAccessor.HttpContext.Session.SetString(UserProfileSessionKey, String.Empty);
        }

        #endregion
    }
}
