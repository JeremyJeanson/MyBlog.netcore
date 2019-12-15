using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Engine.Data.Models;
using MyBlog.Engine.Models;
using MyBlog.Engine.Services;
using System;
using System.Threading.Tasks;

namespace MyBlog.Engine.Controllers
{
    /// <summary>
    /// Allow to edit the user account
    /// </summary>
    [Authorize]
    public sealed class AccountController : Controller
    {
        #region Declarations

        private const String EditActionName = "Edit";
        private readonly DataService _dataService;
        private readonly UserService _userService;
        private readonly MailService _mailService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="userService"></param>
        public AccountController(DataService dataService, UserService userService,MailService mailService)
        {
            _dataService = dataService;
            _userService = userService;
            _mailService = mailService;
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Redirect to Edit
            return RedirectToAction(EditActionName);
        }

        /// <summary>
        /// Get Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            // Get current user
            UserProfile user = _userService.GetFreshUseerUpdatedFromDataBase();
            if (user == null) return NotFound();

            // Set model
            EditAccount model = new EditAccount
            {
                User = user,
                Success = null // none action done at this time
            };
            return View(model);
        }

        /// <summary>
        /// Account was edited by user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Edit(EditAccount model)
        {
            // Test model
            if (ModelState.IsValid)
            {
                // Get current user from session
                UserProfile user = _userService.Get();

                // Update editable data
                user.Name = model.User.Name;
                user.Email = model.User.Email;

                // update database
                // Try to save changes
                var result = _dataService.EditUser(user);
                // Send mail if requested
                if (result?.MailChanged ?? false)
                {
                    await _mailService.SendValidationMail(user.Id);
                }

                model.Success = result?.Result;
                // Refresh
                model.User = user;

                // Update the inforamtion for the userservice
                _userService.Set(user);
            }
            return View(model);
        }

        /// <summary>
        /// Allow user to ask for a new mail validation token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PartialViewResult> SendValidationMail()
        {
            Boolean model;
            // Get current user
            UserProfile user = _userService.Get();
            if (user == null)
            {
                model = false;
            }
            else
            {
                model = await _mailService.SendValidationMail(user.Id);
            }
            return PartialView("_SendValidationMail", model);
        }

        /// <summary>
        /// ValidateMail (url send by mail to users)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult ValidateMail(Int32 id, Guid token)
        {
            Boolean model;
            model = _dataService.ValidateMail(id, token);
            return View(model);
        }

        #endregion
    }
}
