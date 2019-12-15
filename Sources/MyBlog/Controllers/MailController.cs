using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Engine;
using MyBlog.Engine.Models;
using MyBlog.Engine.Services;
using MyBlog.Models;
using MyBlog.Strings;
using System;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class MailController : Controller
    {
        #region Declarations

        private const String SendedView = "Sended";
        private readonly UserService _userService;
        private readonly IOptions<Settings> _options;
        private readonly MailService _mailService;

        #endregion

        #region Constructors

        public MailController(UserService userService, IOptions<Settings> options, MailService mailService)
        {
            _userService = userService;
            _options = options;
            _mailService = mailService;
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(String subject)
        {
            // Create model
            Mail model;

            // Try to get user logged
            var user = _userService.Get();

            // If user is available initialize mail
            model = user == null
                ? new Mail
                {
                    Subject = subject
                }
                : new Mail
                {
                    SenderMail = user.Email,
                    SenderName = user.Name,
                    Subject = subject
                };

            return View(model);
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Mail model)
        {
            if (ModelState.IsValid)
            {
                // Try to send mail
                if (await _mailService.Send(
                    model.SenderMail,
                    model.SenderName,
                    _options.Value.AuthorMail,
                    _options.Value.AuthorName,
                    model.Subject,
                    String.Format("<p>{0} - {1}</p><p>{2}</p>",
                        model.SenderMail,
                        model.SenderName,
                        model.Content.Replace(Environment.NewLine, "<br/>"))
                        ))
                {
                    // Mail sended
                    return RedirectToAction(SendedView);
                }
                else
                {
                    // error when trying to send mail
                    ModelState.AddModelError(String.Empty, Resources.MailSendError);
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Mail sended
        /// </summary>
        /// <returns></returns>
        public ActionResult Sended()
        {
            return View();
        }

        #endregion
    }
}