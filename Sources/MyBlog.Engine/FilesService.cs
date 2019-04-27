using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyBlog.Engine
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
                CloudBlobContainer container = GetBlogContainer();

                // Create if not exists
                await container.CreateIfNotExistsAsync();

                // Set permissions
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Container
                    });
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
        private CloudBlobContainer GetBlogContainer()
        {
            // Get the configuration
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_options.AzureStorageConnectionString);
            // Get a new client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get the container
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
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
            CloudBlockBlob blob = GetBlogContainer().GetBlockBlobReference(name);

            // upload bytes
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);

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
            CloudBlockBlob blob = GetBlogContainer().GetBlockBlobReference(name);
            // upload bytes
            await blob.DeleteIfExistsAsync();

            // Return the blog uri
            return blob.Uri;
        }

        #endregion
    }
}
