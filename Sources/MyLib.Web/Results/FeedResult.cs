using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

        private const String DeclarationToRemove = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
        private const String DeclarationToUse = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

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

        public override async void ExecuteResult(ActionContext context)
        {
            // Get the response
            HttpResponse response = context.HttpContext.Response;

            // Add settings
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(true),
                Async = true
            };

            // Write
            String content;
            using(StringWriter stringWriter= new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                if (_type == Type.Atom)
                {
                    response.ContentType = AtomContentType;
                    Atom10FeedFormatter formatter = new Atom10FeedFormatter(_feed);
                    formatter.WriteTo(xmlWriter);
                }
                else
                {
                    response.ContentType = RssContentType;
                    Rss20FeedFormatter formatter = new Rss20FeedFormatter(_feed);
                    formatter.WriteTo(xmlWriter);
                }
                await xmlWriter.FlushAsync();
                content = SanitateXml(stringWriter.ToString());                
            }
        
            await response.WriteAsync(content);
        }

        /// <summary>
        /// Sanitate XML to remove wrong format
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static String SanitateXml(String xml)
        {
            // Test input
            if (String.IsNullOrWhiteSpace(xml)) return String.Empty;

            return DeclarationToUse
                + xml.Substring(DeclarationToRemove.Length);
        }

        #endregion
    }
}
