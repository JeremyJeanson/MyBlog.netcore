using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.Web.TagHelpers
{
    /// <summary>
    /// Taghelper to create radio button with bootstrap
    /// </summary>
    public sealed class RadioButton:TagHelper
    {
        #region Properties

        /// <summary>
        /// This radio is checked?
        /// </summary>
        public Boolean Checked { get; set; }

        /// <summary>
        /// Title to display
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Group name of radios
        /// </summary>
        public String Group { get; set; }

        #endregion

        #region Methodes

        /// <summary>
        /// Procvess the tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tag
            output.TagName = "label";
            output.TagMode = TagMode.StartTagAndEndTag;

            // Add class
            output.Attributes.Add("class", 
                Checked
                ? "btn btn-primary active"
                : "btn btn-primary");

            output.Attributes.Add("id", context.UniqueId);

            // Inner HTML
            output.Content.AppendHtml($"<input type=\"radio\" name=\"{Group}\" autocomplete=\"off\" id=\"{context.UniqueId}\" ");
            if (Checked)
            {
                output.Content.AppendHtml("checked");
            }
            output.Content.AppendHtml($"> {Title}");
        }

        #endregion
    }
}
