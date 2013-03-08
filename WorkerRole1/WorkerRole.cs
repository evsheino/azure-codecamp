using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
//using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.MediaServices.Client;
using AzureCodeCamp.Utils;
using AzureCodeCamp.Models;
using System.Web;
using System.Text;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {

            var sourceContainer = BlobStorage.container;
            
            while (true)
            {
                foreach (CloudBlockBlob sourceBlob in sourceContainer.ListBlobs())
                {
                    sourceBlob.FetchAttributes();
                    int userId = int.Parse(sourceBlob.Metadata["userid"]);
                    IAsset assetToBeProcessed = BlobToMedia.UseAzureStorageSdkToUpload(sourceBlob.Name);
                    IAsset encodedAsset = MediaServices.encodeAsset(assetToBeProcessed.Id);
                    MediaServices.DeleteAsset(assetToBeProcessed);
                    string streamingUrl = MediaServices.GetStreamingURL(encodedAsset);
                    sourceBlob.Delete();
                }

                Thread.Sleep(10000);
                break;
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
