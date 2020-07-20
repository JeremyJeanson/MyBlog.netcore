using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using MyBlog.Engine.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MyBlog.Engine.Services
{
    public sealed class FilesService
    {
        #region Declarations

        private const string ContainerName = "posts";
        private readonly Settings _options;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public FilesService(IOptions<Settings> options)
        {
            _options = options.Value;
        }

        #endregion

        #region Properties

        #endregion

        #region Methodes

        /// <summary>
        /// Initilize
        /// </summary>
        public async void Initilize()
        {
            try
            {
                var container = GetBlogContainer();

                // Create if not exists
                await container.CreateIfNotExistsAsync();

                // Set permissions
                await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            }
            catch (Exception ex)
            {
                Trace.TraceError("FilesService.Initilize:" + ex.Message);
            }
        }

        /// <summary>
        ///  Get blob container
        /// </summary>
        /// <returns></returns>
        private BlobContainerClient GetBlogContainer()
        {
            // Get the container
            BlobContainerClient container = new BlobContainerClient(
                    _options.AzureStorageConnectionString,
                    ContainerName);
            return container;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<Uri> Upload(String name, Byte[] content)
        {
            // Get the blog by name
            var blob = GetBlogContainer().GetBlobClient(name);
            
            // upload bytes
            await blob.UploadAsync(new MemoryStream(content));

            // Return the blog uri
            return blob.Uri;
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Uri> Delete(String name)
        {
            // Get the blog by name
            var blob = GetBlogContainer().GetBlobClient(name);
            // upload bytes
            await blob.DeleteIfExistsAsync();

            // Return the blog uri
            return blob.Uri;
        }

        #endregion
    }
}
