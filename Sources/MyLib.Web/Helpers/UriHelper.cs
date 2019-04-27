using System;
using System.Net;
using System.Text;
using System.Web;

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
            if (String.IsNullOrWhiteSpace(text))
                return String.Empty;

            String normalized = WebUtility.HtmlDecode(text)
                .Normalize(NormalizationForm.FormKD);
            //String normalized = text.Normalize(NormalizationForm.FormKD);

            Int32 length = normalized.Length;
            Boolean splitted = false;
            StringBuilder sb = new StringBuilder(length);
            Char c;

            for (int i = 0; i < length; i++)
            {
                c = normalized[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    if (splitted)
                    {
                        sb.Append('-');
                        splitted = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (splitted)
                    {
                        sb.Append('-');
                        splitted = false;
                    }
                    // Tricky way to convert to lowercase
                    sb.Append((Char)(c | 32));

                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!splitted && sb.Length > 0)
                    {
                        splitted = true;
                    }
                }

                if (sb.Length == MaxLength)
                    break;
            }
            return sb.ToString();

        }

        /// <summary>
        /// Uri absolue vers l'application web courante
        /// </summary>
        [Obsolete("Do not use with .net core")]
        public static string AbsoluteApplicationUri
        {
            get
            {
                //if (HttpContext.Current == null)
                //    return String.Empty;

                //// Récupération de la requète courante
                //HttpRequest request = HttpContext.Current.Request;

                //// Récupération de l'uri sans application
                //String result = request.Url.GetLeftPart(UriPartial.Authority);

                //// Ajout du path applicatif
                //result += request.ApplicationPath;

                //// Ajout d'un "/" si il manque
                //if (!result.EndsWith("/"))
                //    result += "/";
                //return result;
                return null;
            }
        }
    }
}
