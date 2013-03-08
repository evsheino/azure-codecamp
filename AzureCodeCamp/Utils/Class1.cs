
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Configuration;
using Microsoft.WindowsAzure.StorageClient;


namespace AzureCodeCamp.Utils
{
    public class Class1
    {
        private static readonly string _accountKey = ConfigurationManager.AppSettings["accountKey"];
        private static readonly string _accountName = ConfigurationManager.AppSettings["accountName"];

        // Field for service context.
        private static CloudMediaContext _context = new CloudMediaContext(_accountName, _accountKey);

        public static IAsset UseAzureStorageSdkToUpload()
        {
            //
            //Create an empty asset:
            Guid g = Guid.NewGuid();
            IAsset assetToBeProcessed = _context.Assets.Create("NewAsset_" + g.ToString(), AssetCreationOptions.None);

            //
            //Create a locator to get the SAS url:
            IAccessPolicy writePolicy = _context.AccessPolicies.Create("Policy For Copying", TimeSpan.FromMinutes(30), AccessPermissions.Write | AccessPermissions.List);
            ILocator destinationLocator = _context.Locators.CreateSasLocator(assetToBeProcessed, writePolicy, DateTime.UtcNow.AddMinutes(-5));

            //
            //Create CloudBlobClient:
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
            ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

        // Create the blob client.
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient cloudClient = storageAccount.CreateCloudBlobClient();
            
            //
            //Create the reference to the destination container:
            string destinationContainerName = (new Uri(destinationLocator.Path)).Segments[1];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer destinationContainer = cloudClient.GetContainerReference(destinationContainerName);

            //
            // We will set these differently based on how we are getting the files into the asset:
            var destinationFileBlobName = "";
            long destinationFileLength = 0;


            //Depending on your use-case, you may be moving something your already have, or uploading from disk.
            //Use the following switch to try out the two:
            bool copyFromExistingBlob = true;


            if (copyFromExistingBlob)
            {
                //
                //Specific things you'll need to set:
                var sourceContainerName = "video";
                var sourceFileBlobName = "testi2.mp4";

                destinationFileBlobName = sourceFileBlobName;

                //
                //Create the reference to the source container, in this case, a container called uploads:
                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer sourceContainer = cloudClient.GetContainerReference(sourceContainerName);

                //
                //Get and validate the source blob, in this case a file called FileToCopy.mp4:
                Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob sourceFileBlob = sourceContainer.GetBlockBlobReference(sourceFileBlobName);
                sourceFileBlob.FetchAttributes();
                long sourceFileLength = sourceFileBlob.Properties.Length;
                if (sourceFileLength == 0)
                    throw new FileNotFoundException(string.Format("The source {0} file does not exist or has a file size of zero.", sourceFileBlobName));

                //If we got here then we can assume the source is valid and accessible.

                //
                //Create destination blob for copy, in this case, we choose to rename the file:
                Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob destinationFileBlob = destinationContainer.GetBlockBlobReference(destinationFileBlobName);

                //
                //Do the blob-to-blob copy:
                destinationFileBlob.StartCopyFromBlob(sourceFileBlob);
                // Will fail here if project references are bad (the are lazy loaded).

                //
                //Check destination blob:
                destinationFileBlob.FetchAttributes();
                destinationFileLength = destinationFileBlob.Properties.Length;
                if (destinationFileLength != sourceFileLength)
                    throw new Exception(string.Format("The copied file {0} does not have the same size as the source file {1}.", sourceFileBlobName, destinationFileBlobName));

                //If we got here then the copy worked.
            }
          

            //
            //Publish the asset:
            var destinationAssetFile = assetToBeProcessed.AssetFiles.Create(destinationFileBlobName);
            destinationAssetFile.IsPrimary = true;
            destinationAssetFile.ContentFileSize = destinationFileLength; //setter only available in SDK > v2.0.0.5
            destinationAssetFile.Update();

            assetToBeProcessed = RefreshAsset(assetToBeProcessed, _context);
            return MediaServices.encodeAsset(assetToBeProcessed.Id);

            
            //
            //At this point, you can create a job using your asset.
            // ...
            //Console.WriteLine(String.Format("You are ready to use your asset with Id: {0} Name: {1} FileCount: {2}", assetToBeProcessed.Id, assetToBeProcessed.Name, assetToBeProcessed.AssetFiles.Count()));
            //
        }

        //Where:

        public static IAsset RefreshAsset(IAsset asset, CloudMediaContext context)
        {
            asset = context.Assets.Where(a => a.Id == asset.Id).FirstOrDefault();
            return asset;
        }
    }
}