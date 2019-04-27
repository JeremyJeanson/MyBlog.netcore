using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MyBlog.Engine.Midleware
{
    public sealed class MetaWeblog
    {
        /// <summary>
        /// Configure the Middleware
        /// </summary>
        /// <param name="application"></param>
        public static void Configure(IApplicationBuilder application)
        {
            application.Run(Run);
        }

        /// <summary>
        /// Porcess a request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task Run(HttpContext context)
        {
            // Get a MetaWeblogService
            MetaWeblogService servrice = (MetaWeblogService)context.RequestServices.GetService(typeof(MetaWeblogService));

            // Read the request
            XDocument input = XDocument.Load(context.Request.Body);
            XDocument output = servrice.Process(input);

            // Add header en encoding
            context.Response.ContentType = "text/xml";
            // Create setting
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };
            // Write the output
            using (XmlWriter writer = XmlWriter.Create(context.Response.Body, settings))
            {
                output.WriteTo(writer);
            }
            return Task.CompletedTask;
        }
    }
}
