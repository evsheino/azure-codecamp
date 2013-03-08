using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace AzureCodeCamp.Utils
{
    public static class BlobStorage
    {
        public static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

        // Create the blob client.
        public static CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        // Retrieve a reference to a container. 
        public static CloudBlobContainer container = blobClient.GetContainerReference("video");
        
        public static CloudBlockBlob uploadBlob(HttpPostedFileBase file, int userid, string title, int category)
        {
            string fn = file.FileName;
            CloudBlockBlob blockBlob  = container.GetBlockBlobReference(fn);
            blockBlob.UploadFromStream(file.InputStream);
            
            //Asetetaan metadataan userid
            blockBlob.Metadata["userid"] = userid.ToString();
            blockBlob.Metadata["title"] = title;
            blockBlob.Metadata["category"] = category.ToString();
            blockBlob.SetMetadata();
            return blockBlob;
        }
        
            
    }
}