using System;

namespace MyBlog.Engine.Data.Models
{
    public sealed class PostWithSummary : PostDisplayedBase
    {
        public String Summary { get; set; }

        public Boolean ContentIsSplitted { get; set; }

        public String HtmlSummary
        {
            get
            {
                return Summary;
            }
        }
    }
}
