using System;

namespace MyLib.Web.Helpers
{
    public static class DateHelper
    {
        private const string W3CFormat = "{0:0000}-{1:00}-{2:00}";

        /// <summary>
        /// Get date at W3C Format yyyy-mm-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String GetW3CFormat(DateTime date)
        {
            return String.Format(
                W3CFormat,
                date.Year,
                date.Month,
                date.Day);
        }
    }
}
