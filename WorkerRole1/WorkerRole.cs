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
using System.Data.Entity;

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
                    string title = sourceBlob.Metadata["title"];
                    int category = 1; //int.Parse(sourceBlob.Metadata["category"]);
                    int userId = int.Parse(sourceBlob.Metadata["userid"]);
                    IAsset assetToBeProcessed = BlobToMedia.UseAzureStorageSdkToUpload(sourceBlob.Name);
                    IAsset encodedAsset = MediaServices.encodeAsset(assetToBeProcessed.Id);
                    MediaServices.DeleteAsset(assetToBeProcessed);
                    string streamingUrl = MediaServices.GetStreamingURL(encodedAsset);
                    sourceBlob.Delete();

                    JoukkoVideoDBContext db = new JoukkoVideoDBContext();
                    JoukkoVideo joukkovideo = new JoukkoVideo();

                    joukkovideo.path = streamingUrl;
                    var user = db.UserProfiles.Single(u => u.UserId == userId);
                    joukkovideo.user = user;
                    joukkovideo.title = title;
                    
                    db.JoukkoVideos.Add(joukkovideo);
                    db.SaveChanges();

                    
                }

                Thread.Sleep(10000);
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
