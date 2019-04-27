using Microsoft.Extensions.Options;
using MyBlog.Strings;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Engine
{
    public sealed class MailService
    {
        #region Declarations

        private readonly Settings _options;
        public DataService _dataService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dataService"></param>
        public MailService(IOptions<Settings> options, DataService dataService)
        {
            _options = options.Value;
            _dataService = dataService;
        }

        #endregion

        #region Properties

        #endregion

        #region Mail methodes

        /// <summary>
        /// Send plain text mail
        /// </summary>
        /// <param name="receiverMail"></param>
        /// <param name="fromMailMail"></param>
        /// <param name="fromName"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<Boolean> Send(String toMail, String toName, String subject, String content)
        {
            try
            {
                // Get a new client 
                var client = new SendGridClient(_options.SendGrid);
                // Initialize the mail
                var message = new SendGridMessage()
                {
                    From = new EmailAddress(_options.SendMailFrom, _options.Title),
                    Subject = subject,
                    HtmlContent = content + "<p> Blog : " + _options.Url + "</p>"
                };
                message.AddTo(new EmailAddress(toMail, toName));
                var response = await client.SendEmailAsync(message);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        #endregion

        #region Mail validation

        /// <summary>
        /// Send an email for validation and save token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Boolean> SendValidationMail(Int32 userId)
        {
            // Get mail data for validation
            UserValidationMailData data = _dataService.GetSendValidationMailToken(userId);

            // Save 
            if (data == null)
            {
                return false;
            }

            //Send mail
            return await SendValidationMail(userId, data.Name, data.Email, data.Token);
        }

        /// <summary>
        /// Send an email for validation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="mail"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<Boolean> SendValidationMail(Int32 userId, String name, String mail, Guid token)
        {
            // Save 
            String url = String.Format(
                "{0}/Account/ValidateMail/{1}?token={2}",
                _options.Url,
                userId.ToString(),
                token.ToString());

            String content = String.Format(Resources.EmailValidationContentFormat,
                    name,
                    _options.Url,
                    url);

            // Send mail
            return await Send(
                mail,
                name,
                Resources.EmailValidationSubject,
                content
                );
        }

        #endregion

        #region Mail when comment were added

        /// <summary>
        /// Send mail on comments subsribers when a comment has been added
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="authorOfComment"></param>
        /// <returns></returns>
        public async Task NotifyUsersForComment(Int32 postId, Int32 authorOfComment)
        {
            String authorMail = _options.AuthorMail;
            String authorName = _options.AuthorName;

            // Get data
            NotifyUserForCommentData data = _dataService.GetNotifyUserForCommentData(postId, authorOfComment);

            if (data == null) return;

            // Format the mail subject
            String subject = String.Format(Resources.EMailCommentAddedSubject, data.PostTitle);
            Boolean result = true;

            // Send mails to users
            if (data.Users?.Any() ?? false)
            {
                // Send mails
                foreach (var user in data.Users)
                {
                    result &= await Send(
                        user.Email,
                        user.Name,
                         subject,
                        String.Format(Resources.EMailCommentAddedContent,
                            user.Name,
                            data.PostUri,
                            data.PostTitle)
                        );
                }
            }
            // Send mail to the author
            result &= await Send(
                authorMail,
                authorName,
                subject,
                String.Format(Resources.EMailCommentAddedContent,
                    authorName,
                    data.PostUri,
                    data.PostTitle)
                );

            if (!result) Trace.TraceError("Erros when sennding comment notification on post " + postId);
        }

        #endregion
    }
}
