using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Net;

namespace MyLib.Web.TagHelpers
{
    /// <summary>
    /// Panel taghelper for bootstrap 4
    /// </summary>
    public sealed class Panel : TagHelper
    {
        /// <summary>
        /// Title
        /// </summary>
        public String Title { get; set; }

        public String Icon { get; set; }

        public Boolean RemovePadding{ get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Add div
            output.TagName = "div";

            // Add card class
            var classes = output.Attributes.FirstOrDefault(c => c.Name == "class");
            if (classes == null)
            {
                output.Attributes.Add("class", "card mb-3");
            }
            else
            {
                output.Attributes.SetAttribute("class", "card mb-3 " + classes.Value);
            }

            // Add title
            if (!String.IsNullOrWhiteSpace(Title) || !String.IsNullOrWhiteSpace(Icon))
            {
                output.PreContent.AppendHtml("<div class=\"card-header\"><h2>");
                // Add icon
                if (!String.IsNullOrWhiteSpace(Icon))
                {
                    output.PreContent.AppendHtml($"<i class=\"fas {Icon}\" aria-hidden=\"true\"></i> ");
                }
                // Format title
                output.PreContent.Append(WebUtility.HtmlDecode(Title));
                output.PreContent.AppendHtml("</h2></div>");
            }

            // Add body
            if (!RemovePadding)
            {
                output.PreContent.AppendHtml("<div class=\"card-body\">");
                output.PostContent.AppendHtml("</div>");
            }
        }
    }
}
