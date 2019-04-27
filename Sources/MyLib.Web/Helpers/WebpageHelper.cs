using System;
using System.Net;
using System.Text.RegularExpressions;

namespace MyLib.Web.Helpers
{
    public static class WebpageHelper
    {
        private const Int32 MaxMetaDescriptionLength = 150;
        private const String MetaDescritionTooLongSufix = "...";
        private const String HmltPattern = "<[^>]+>";

        /// <summary>
        /// Get description for HTML meta
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String GetMetaDescrition(String html)
        {
            // Test args
            if (String.IsNullOrWhiteSpace(html)) return String.Empty;

            // Get decoded string
            String text = Regex.Replace(html, HmltPattern, String.Empty,
                RegexOptions.IgnoreCase
                | RegexOptions.Multiline
                | RegexOptions.CultureInvariant
                | RegexOptions.Compiled);

            // This text is empty?
            if (String.IsNullOrWhiteSpace(text)) return String.Empty;

            // This text is too long?
            if (text.Length <= MaxMetaDescriptionLength) return CleanMetaDescrition(text);

            // Get index of split location
            Int32 index = text.IndexOf(' ', MaxMetaDescriptionLength);
            if (index < 0)
            {
                index = MaxMetaDescriptionLength;
            }

            // Trim and add "..."
            return CleanMetaDescrition(text.Substring(0, index))
                + MetaDescritionTooLongSufix;
        }

        /// <summary>
        /// Clean meta
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static String CleanMetaDescrition(String html)
        {
            // Decode
            String result = WebUtility.HtmlDecode(html);
            // Remove Line breaks
            return result.Replace(Environment.NewLine, " ");
        }
    }
}
