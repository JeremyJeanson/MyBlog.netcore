using Microsoft.AspNetCore.Razor.TagHelpers;
using MyBlog.Strings;
using System;
using System.Net;

namespace MyBlog.Engine.TagHelpers
{
    public sealed class Socials:TagHelper
    {
        #region Declarations

        public enum SocialnetWork
        {
            Twitter = 0,
            Facebook = 1,
            LinkedIn = 2,
            // GooglePlus = 3,
            Reddit = 4,
            Pinterest = 5,
            Yahoo = 6,
            Vk = 7,
            Viadeo = 8,
            Yammer = 9
        }

        #endregion

        #region Constructors
        #endregion

        #region Properties

        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Uri { get; set; }

        #endregion

        #region Methodes

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Tag
            output.TagName = "div";
            // Css
            output.Attributes.Add("class", "share");

            // Add buttons
            output.Content.AppendHtml(GetSocial("FaceBook", "facebook", "fab fa-facebook-f"));
            output.Content.AppendHtml(GetSocial("Twitter", "twitter", "fab fa-twitter"));
            output.Content.AppendHtml(GetSocial("Linked In", "linkedin", "fab fa-linkedin-in"));
            output.Content.AppendHtml(GetSocial("Yammer", "yammer", "fab fa-yammer"));
            output.Content.AppendHtml("<div class=\"dropdown\">");
            output.Content.AppendHtml($"<button class=\"btn btn-light\" type=\"button\" id=\"menu{Id}\" data-bs-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" aria-label=\"{Resources.ShowMoreShareOptions}\"><i class=\"fas fa-share-alt\" aria-hidden=\"true\"></i></button>");
            output.Content.AppendHtml($"<div class=\"dropdown-menu\" aria-labelledby=\"menu{Id}\">");
            output.Content.AppendHtml(GetSocialDrop("Viadéo", "viadeo", "fab fa-viadeo"));
            output.Content.AppendHtml(GetSocialDrop("Reddit", "reddit", "fab fa-reddit-alien"));
            output.Content.AppendHtml(GetSocialDrop("Pinterest", "pinterest", "fab fa-pinterest-p"));
            output.Content.AppendHtml(GetSocialDrop("Yahoo", "yahoo", "fab fa-yahoo"));
            output.Content.AppendHtml(GetMail());
            output.Content.AppendHtml("</div></div>");
        }

        private String GetSocial(String networkName, String networkKey, String fontAwesome)
        {
            return $"<a target=\"_blank\" class=\"btn btn-social-icon btn-{networkKey}\" href=\"/Share/?id={Id}&N={networkKey}\"><i aria-hidden=\"true\" class=\"{fontAwesome}\"></i><span class=\"visually-hidden\">{Resources.ShareWith} {networkName}</span></a>";
        }

        private String GetSocialDrop(String networkName, String networkKey, String fontAwesome)
        {
            return $"<a target=\"_blank\" class=\"dropdown-item\" href=\"/Share/?id={Id}&N={networkKey}\"><i aria-hidden=\"true\" class=\"{fontAwesome}\"></i> <span aria-hidden=\"true\">{networkName}</span><span class=\"visually-hidden\">{Resources.ShareWith} {networkName}</span></a>";
        }

        private String GetMail()
        {
            return $"<a target=\"_blank\" class=\"dropdown-item\" href=\"mailto:?subject={System.Uri.EscapeDataString(WebUtility.HtmlDecode(Title))}&body={System.Uri.EscapeDataString(Uri)}\"><i aria-hidden=\"true\" class=\"fas fa-envelope\"></i> <span aria-hidden=\"true\">Mail</span><span class=\"visually-hidden\">{Resources.ShareWith} mail</span></a>";
        }

        #endregion
    }
}
