using System;
using System.Net;
using System.Text;

namespace MyLib.Web.Helpers
{
    public static class UriHelper
    {
        private const Int32 MaxLength = 100;
        private const String Splitter = "-";

        /// <summary>
        /// Build string that could be used as Url fragment (like slugs)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToFriendly(String text)
        {
            // Test empty string
            if (String.IsNullOrWhiteSpace(text))
                return String.Empty;

            // Normalize HTML string
            String normalized = WebUtility.HtmlDecode(text.ToLower())
                .Normalize(NormalizationForm.FormKD);

            StringBuilder sb = new StringBuilder();
            Int32 length = normalized.Length;
            Boolean splitted = false;            
            Char c;

            for (int i = 0; i < length; i++)
            {
                // get current char
                c = normalized[i];
                
                // Test chars
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    if (splitted)
                    {
                        sb.Append('-');
                        splitted = false;
                    }
                    sb.Append(c);
                }
                // Test splitters
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!splitted && sb.Length > 0)
                    {
                        splitted = true;
                    }
                }
                // End of line test
                if (sb.Length == MaxLength)
                    break;
            }
            // Return the resulat as string
            return sb.ToString();
        }
    }
}
