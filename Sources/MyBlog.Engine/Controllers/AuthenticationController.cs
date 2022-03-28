using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MyBlog.Engine.Services;

namespace MyBlog.Engine.Controllers
{
    /// <summary>
    /// Controller for authentication process
    /// </summary>
    [Authorize]
    public sealed class AuthenticationController : Controller
    {
        #region Declarations

        private const String InnerIndexView = "_Index";
        private readonly IOptions<Settings> _options;
        private readonly UserService _userService;

        #endregion

        #region Constructors


        public AuthenticationController(IOptions<Settings> options, UserService userService)
        {
            _options = options;
            _userService = userService;
        }

        #endregion

        #region Methodes

        #region Get login form

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public ActionResult Index(String returnUrl)
        {
            var model = new AuthenticationProviders
            {
                Providers = GetAccountProviders().ToArray()
            };

            // Test return url
            model.ReturnUrl = String.IsNullOrEmpty(returnUrl)
                ? _options.Value.Url
                : returnUrl;

            // Test the method used
            if (Request.Method == Constants.MethodPost)
            {
                return PartialView(InnerIndexView, model);
            }
            return View(model);
        }

        /// <summary>
        /// Return the list of providers
        /// </summary>
        /// <returns></returns>
        private IEnumerable<AuthenticationProvider> GetAccountProviders()
        {
            if (_options.Value.TwitterAuthentication?.Active ?? false)
            {
                yield return new AuthenticationProvider
                {
                    Style = "twitter",
                    Icon = "twitter",
                    Provider = "Twitter",
                    Name = "Twitter"
                };
            }
            if (_options.Value.FacebookAuthentication?.Active ?? false)
            {
                yield return new AuthenticationProvider
                {
                    Style = "facebook",
                    Icon = "facebook-f",
                    Provider = "Facebook",
                    Name = "Facebook"
                };
            }
            if (_options.Value.GoogleAuthentication?.Active ?? false)
            {
                yield return new AuthenticationProvider
                {
                    Style = "google",
                    Icon = "google-plus-g",
                    Provider = "Google",
                    Name = "Google"
                };
            }
            if (_options.Value.MicrosoftAccountAuthentication?.Active ?? false)
            {
                yield return new AuthenticationProvider
                {
                    Style = "microsoft",
                    Icon = "windows",
                    Provider = "Microsoft",
                    Name = "Microsoft"
                };
            }
        }

        #endregion

        #region Exernal login process

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(AuthenticationChoice model)
        {
            // Get properties for redirection url
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback", "Authentication",new { model.ReturnUrl }),
            };
            // Challenge
            return Challenge(properties, model.Provider);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(AuthenticationCallback model)
        {
            // authenticate the user with the custom scheme
            var result = await HttpContext.AuthenticateAsync(Constants.SignInScheme);
            //// Get claims
            //var claims = result.Principal.Claims;

            //// singin localy with claims
            //await HttpContext.SignInAsync(Constants.SignInScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));

            // Get user from claims
            var user = _userService.GetFromClaims(result.Principal);
            if (user == null) return View(model.RemoteError);

            // Set user in session to use later
            _userService.Set(user);

            // Test new user, to edit profile
            if (user.Id == 0)
            {
                return RedirectToAction("Edit", "Account");
            }

            // Check redirection lenght
            if (String.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                Redirect("~/");
            }

            // Check redirection url
            if (model.ReturnUrl.StartsWith(_options.Value.Url, StringComparison.InvariantCultureIgnoreCase))
            {
#pragma warning disable SCS0027 // Potential Open Redirect vulnerability was found where '{0}' in '{1}' may be tainted by user-controlled data from '{2}' in method '{3}'.
                return Redirect(model.ReturnUrl);
#pragma warning restore SCS0027 // Potential Open Redirect vulnerability was found where '{0}' in '{1}' may be tainted by user-controlled data from '{2}' in method '{3}'.
            }

            // Redirection
            return Redirect("~/");
        }

        #endregion

        #region Logout process

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            // clear session
            _userService.Clear();

            // Redirect to success page
            return RedirectToAction("LogoutSuccess");
        }

        /// <summary>
        /// Log out successed
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ViewResult LogoutSuccess()
        {
            return View();
        }

        #endregion

        #endregion
    }
}
