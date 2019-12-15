using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MyLib.Web.Strings;
using System;

namespace MyLib.Web.TagHelpers
{
    public sealed class Pagination:TagHelper
    {
        #region Declarations

        private readonly IUrlHelper _urlHelper;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="urlHelper"></param>
        public Pagination(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        #endregion

        #region Properties

        public String Action { get; set; }
        public Int32 Count { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public Object RouteValues { get; set; }

        #endregion

        #region Methodes

        /// <summary>
        /// Process the tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Get inner html
            String innerHtml = PaginationExtensions.GetInnerHtml(_urlHelper, Page, PageSize, Count, Action, RouteValues);

            if (String.IsNullOrWhiteSpace(innerHtml))
            {
                // Generate nothing
                output.SuppressOutput();
                return;
            }
            
            // Tag
            output.TagName = "nav";

            // Add label
            output.Attributes.Add("aria-label", Resources.Pagination);

            // Add inner html
            output.Content.Clear();
            output.Content.AppendHtml(innerHtml);
        }

        #endregion
    }
}
