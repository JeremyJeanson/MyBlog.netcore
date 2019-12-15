using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyBlog.Engine.Services;
using System;
using System.IO;
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
        private static async Task Run(HttpContext context)
        {
            try
            {
                // Get a MetaWeblogService
                MetaWeblogService servrice = (MetaWeblogService)context.RequestServices.GetService(typeof(MetaWeblogService));

                var sourceload = new System.Threading.CancellationTokenSource();

                // Read the boy of the request to use
                Stream body = await ReadAsync(context.Request.Body);

                // Read the request
                XDocument input = await XDocument.LoadAsync(body, LoadOptions.None, sourceload.Token);
                XDocument output = servrice.Process(input);

                // Add header en encoding
                context.Response.ContentType = "text/xml";
                // Write
                await context.Response.WriteAsync(output.ToString());
            }
            catch(Exception ex)
            {
                await context.Response.WriteAsync(
                    MetaWeblogService.ProcessError(ex.Message)
                    .ToString());
            }
        }

        /// <summary>
        /// Read a stream with ReadAsync to a stream 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static async Task<Stream> ReadAsync(Stream stream)
        {
            var output = new MemoryStream();
            // Copy the stream
            await stream.CopyToAsync(output);
            // Reset the position of te output stream
            output.Position = 0;
            return output;
        }
    }
}
