using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MyLib.Web.Soe
{
    public sealed class SoeSiteMapResult : ActionResult
    {
        private readonly SoeSiteMap _sitemap;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sitemap"></param>
        public SoeSiteMapResult(SoeSiteMap sitemap)
        {
            _sitemap = sitemap;
        }

        /// <summary>
        /// Execute Result
        /// </summary>
        /// <param name="context"></param>
        public override async void ExecuteResult(ActionContext context)
        {
            // Get the response
            HttpResponse response = context.HttpContext.Response;

            // Add header en encoding
            response.ContentType = "text/xml";
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                Async = true
            };

            // Write
            String content;
            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
            {
                XDocument doc = _sitemap.GetXDocument();
                doc.WriteTo(writer);
                await writer.FlushAsync();
                content = stringWriter.ToString();
            }

            await response.WriteAsync(content);
        }
    }
}
