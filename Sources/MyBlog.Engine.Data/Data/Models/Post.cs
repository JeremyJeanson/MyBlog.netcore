using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace MyBlog.Engine.Data.Models
{
    public class Post : IPost
    {
        #region Declarations

        private const String SummarySeparatorStart = "<summary>";
        private const String SummarySeparatorEnd = "</summary>";
        private const String ObsoleteBorderPattern = @"<([^>]*)(\sborder="".+?""(\s|))(.*?)>";
        private const String ObsoleteBorrderReplacement = "<$1 $3>";

        #endregion

        #region Properties Stored

        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(150)]
        public String Title { get; set; }

        public String BeginningOfContent { get; set; }

        public String EndOfContent { get; set; }

        public Boolean ContentIsSplitted { get; set; }

        public DateTime DateCreatedGmt { get; set; }

        public List<PostCategory> Categories { get; set; }

        public Boolean Published { get; set; }

        public List<Comment> Comments { get; set; }

        public List<UserProfilePost> Followers { get; set; }

        #endregion

        #region Properties generated

        /// <summary>
        /// Content available for desplay
        /// </summary>
        [NotMapped]
        public String HtmlFull
        {
            get
            {
                return
                    //CommonMark.CommonMarkConverter.Convert(
                    BeginningOfContent
                    + Environment.NewLine
                    + EndOfContent;
                //);
            }
        }

        [NotMapped]
        public String Url { get; set; }

        #endregion

        #region Methods        

        public void SetContentFromHtml(String value)
        {
            // Remove obsolete border attribute on images (often added by Open Live Writer)
            value = Regex.Replace(value, ObsoleteBorderPattern, ObsoleteBorrderReplacement, 
                RegexOptions.IgnoreCase
                | RegexOptions.Multiline
                | RegexOptions.CultureInvariant
                | RegexOptions.Compiled);

            var rows = value.Split(new String[] { SummarySeparatorStart, SummarySeparatorEnd }, StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length == 0) return;

            // add
            BeginningOfContent = rows[0];

            if (rows.Length>=2)
            {
                EndOfContent = rows[1];
                ContentIsSplitted = true;
            }
        }

        public String GetFullContentForOpenLiveWriter()
        {
            if (ContentIsSplitted)
            {
                return SummarySeparatorStart + BeginningOfContent + SummarySeparatorEnd
                    + EndOfContent;
            }
            return BeginningOfContent;
        }

        #endregion
    }
}
