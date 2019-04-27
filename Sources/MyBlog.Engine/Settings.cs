using System;

namespace MyBlog.Engine
{
    /// <summary>
    /// Configuration used with IOptions<>
    /// </summary>
    public sealed class Settings
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Settings()
        {
            // Display
            PostQuantityPerPage =  5;
            PostQuantityPerSearch = 10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Sub title
        /// </summary>
        public String SubTitle { get; set; }

        /// <summary>
        /// Author name
        /// </summary>
        public String AuthorName { get; set; }

        /// <summary>
        /// Author mail
        /// </summary>
        public String AuthorMail { get; set; }

        /// <summary>
        /// Author mail
        /// </summary>
        public String SendMailFrom { get; set; }

        /// <summary>
        /// Web site url
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// Quantity of post available on each page
        /// </summary>
        public Int32 PostQuantityPerPage { get; set; }

        /// <summary>
        /// Quantity of post available on each search
        /// </summary>
        public Int32 PostQuantityPerSearch { get; set; }

        /// <summary>
        /// API key for sendgrid
        /// </summary>
        public String SendGrid { get; set; }

        /// <summary>
        /// Azure Storage ConnectionString fro blobs (File Service)
        /// </summary>
        public String AzureStorageConnectionString { get; set; }

        /// <summary>
        /// Athentication with Microsoft Account is available
        /// </summary>
        public AuthenticationSettings MicrosoftAccountAuthentication { get; set; }

        /// <summary>
        /// Athentication with Twitter is available
        /// </summary>
        public AuthenticationSettings TwitterAuthentication { get; set; }

        /// <summary>
        /// Athentication with Facebook is available
        /// </summary>
        public AuthenticationSettings FacebookAuthentication { get; set; }

        /// <summary>
        /// Athentication with Google+ is available
        /// </summary>
        public AuthenticationSettings GoogleAuthentication { get; set; }

        #endregion
    }

    public sealed class AuthenticationSettings
    {
        public Boolean Active { get; set; }
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
    }
}