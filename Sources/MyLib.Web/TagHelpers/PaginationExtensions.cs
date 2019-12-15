using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using MyLib.Web.Strings;
using System;
using System.Linq;
using System.Text;

namespace MyLib.Web.TagHelpers
{
    public static class PaginationExtensions
    {
        #region Declarations

        private const Int32 PageMax = 10;

        #endregion

        #region Methodes

        /// <summary>
        /// Html helper
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="action"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static String GetPagination(this IHtmlHelper htmlHelper, String action, Int32 count, Int32 page, Int32 pageSize, Object routeValues)
        {
            // Get UrlHelper
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext);

            // Get inner html
            String innerHtml = GetInnerHtml(urlHelper, page, pageSize, count, action, routeValues);
            if (String.IsNullOrWhiteSpace(innerHtml))
            {
                // Generate nothing
                return String.Empty;
            }

            // Return the full html with nav
            return $"<nav aria-label=\"{Resources.Pagination}\">{innerHtml}</nav>";
        }

        /// <summary>
        /// Return inner html
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        internal static String GetInnerHtml(IUrlHelper urlHelper, Int32 page, Int32 pageSize, Int32 count, String action, Object routeValues)
        {
            #region Calculs

            // Add li for each page
            Int32[] pages = Enumerable.Range(page - pageSize, pageSize * 2)
                .Where(c => c >= 0 && c * pageSize < count)
                .Take(PageMax)
                .ToArray();

            // Setting fo li list
            Int32 length = pages.Length;

            // Nothing to write if we have only one page or less
            if (length <= 1)
            {
                // Generate nothing
                return null;
            }


            Int32 min = pages[0];
            Int32 max = pages[length - 1];
            Boolean haveMore = (max + 1) * pageSize < count;

            #endregion

            StringBuilder sb = new StringBuilder();

            // Add ul
            sb.Append("<ul class=\"pagination\">");

            // Add links
            for (Int32 i = 0; i < length; i++)
            {
                // Test active page
                if (page == pages[i])
                {
                    sb.Append("<li class=\"page-item active\">");
                    sb.Append($"<span class=\"page-link\">{pages[i] + 1}</span>");
                    sb.Append($"<span class=\"sr-only\">{Resources.PageCurrent}</span>");
                }
                else
                {
                    sb.Append("<li class=\"page-item\">");
                    sb.Append($"<a class=\"page-link\" href=\"{GetUrl(pages[i], urlHelper, action, routeValues)}\" aria-label=\"");
                    if (pages[i] == min && pages[i] != 0)
                    {
                        sb.Append(Resources.PaginationPrevious);
                        sb.Append("\"><span aria-hidden=\"true\">&laquo;</span>");
                    }
                    else if (haveMore && pages[i] == max)
                    {
                        sb.Append(Resources.PaginationNext);
                        sb.Append("\"><span aria-hidden=\"true\">&raquo;</span>");
                    }
                    else
                    {
                        Int32 indexDesplayed = pages[i] + 1;
                        sb.Append(Resources.PaginationIndex + " " + indexDesplayed);
                        sb.Append($"\">{indexDesplayed}");
                    }
                    sb.Append("</a>");
                }
                sb.Append("</li>");
            }

            sb.Append("</ul>");
            return sb.ToString();
        }

        /// <summary>
        /// Get an url for the page n°x
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static String GetUrl(Int32 page, IUrlHelper urlHelper, String action, Object routeValues)
        {
            var values = new RouteValueDictionary(routeValues);
            values.Add("page", page);
            return urlHelper.Action(action, values);
        }

        #endregion
    }
}
