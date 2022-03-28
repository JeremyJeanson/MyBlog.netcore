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

        public String Onclick { get; set; }

        #endregion

        #region Methodes

        /// <summary>
        /// Procvess the tag helper
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            output.Content.AppendHtml($"<input type=\"radio\" class=\"btn-check\" name=\"{Group}\" autocomplete=\"off\" id=\"{context.UniqueId}\" onclick=\"{Onclick}\"");
            if (Checked)
            {
                output.Content.AppendHtml("checked");
            }
            output.Content.AppendHtml(">");

            // Tag
            output.Content.AppendHtml($"<label class=\"btn btn-outline-primary\" for={context.UniqueId} onclick=\"{Onclick}\">{Title}</label>");
        }

        #endregion
    }
}
