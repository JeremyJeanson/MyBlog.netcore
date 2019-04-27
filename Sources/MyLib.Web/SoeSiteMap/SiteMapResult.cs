using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MyLib.Web.SoeSiteMap
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
        public override void ExecuteResult(ActionContext context)
        {
            // Get the response
            HttpResponse response = context.HttpContext.Response;

            // Add header en encoding
            response.ContentType = "text/xml";
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };
            using (XmlWriter writer = XmlWriter.Create(response.Body, settings))
            {
                XDocument doc = _sitemap.GetXDocument();
                doc.WriteTo(writer);
            }
        }
    }
}
