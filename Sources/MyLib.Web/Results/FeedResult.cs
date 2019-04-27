using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace MyLib.Web.Results
{
    public sealed class FeedResult : ActionResult
    {
        #region Declarations

        private const String AtomContentType = "application/atom+xml";
        private const String RssContentType= "application/rss+xml";

        public enum Type
        {            
            Atom,
            Rss
        }

        private readonly SyndicationFeed _feed;
        private readonly Type _type;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="feed"></param>
        public FeedResult(SyndicationFeed feed, Type type)
        {
            _feed = feed;
            _type = type;
        }

        #endregion

        #region Methodes

        public override void ExecuteResult(ActionContext context)
        {
            // Get the response
            HttpResponse response = context.HttpContext.Response;

            // Add settings
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };

            // Write
            using (XmlWriter writer = XmlWriter.Create(response.Body, settings))
            {
                if (_type == Type.Atom)
                {
                    response.ContentType = AtomContentType;
                    Atom10FeedFormatter atomformatter = new Atom10FeedFormatter(_feed);
                    atomformatter.WriteTo(writer);
                }
                else
                {
                    response.ContentType = RssContentType;
                    Rss20FeedFormatter rssformatter = new Rss20FeedFormatter(_feed);
                    rssformatter.WriteTo(writer);
                }
            }
        }

        #endregion
    }
}
