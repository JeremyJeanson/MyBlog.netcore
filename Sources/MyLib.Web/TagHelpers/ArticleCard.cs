using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Net;

namespace MyLib.Web.TagHelpers
{
    public class ArticleCard : TagHelper
    {
        #region Properties

        public String Title { get; set; }
        public String Uri { get; set; }

        #endregion

        #region Methodes

        /// <summary>
        /// Process this tag
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tag
            output.TagName = "div";

            // Format title 
            String title = WebUtility.HtmlDecode(Title);

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

            // Article
            output.PreContent.AppendHtml("<article class=\"card-body\"><h2 class=\"card-title\">");
            
            // Test if an uri is available
            if (String.IsNullOrWhiteSpace(Uri))
            {
                output.PreContent.Append(title);
            }
            else
            {
                output.PreContent.AppendHtml($"<a href=\"{Uri}\">");
                output.PreContent.Append(title);
                output.PreContent.AppendHtml("</a>");
            }

            // End of h2
            output.PreContent.AppendHtml("</h2>");
            // end of article
            output.PostContent.AppendHtml("</article>");
        }

        #endregion
    }
}
