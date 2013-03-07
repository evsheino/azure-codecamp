using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace AzureCodeCamp.Utils
{
    public static class MediaServices
    {

        private static readonly string _accountKey = ConfigurationManager.AppSettings["accountKey"];
        private static readonly string _accountName = ConfigurationManager.AppSettings["accountName"];

        // Field for service context.
        private static CloudMediaContext _context = new CloudMediaContext(_accountName, _accountKey);

        //TODO: Enkoodattoman videon poisto ainakin
        //Esimerkki käyttöä
        /*static void Main(string[] args)
        {
            // Get the service context.
            _context = new CloudMediaContext(_accountName, _accountKey);

            var uploadFilePath = "C:\\Users\\Public\\Videos\\Sample Videos\\Wildlife.wmv";
            var uploadAsset = createAsset(uploadFilePath);

            //Encodataan, vai tehdäänkö suoraan create kutsun jälkeen kutsu encodelle?
            var encodeAssetId = uploadAsset.Id; // "YOUR ASSET ID";
            var streamingAsset = encodeAsset(encodeAssetId);

            var t = GetAsset("nb:cid:UUID:62c93fd4-93e3-44f1-ac42-b9b3fdac1279");
            GetStreamingURL(t);

        }*/

        public static IAsset createAsset(string uploadPath)
        {
            var assetName = Path.GetFileNameWithoutExtension(uploadPath) + "_" + DateTime.UtcNow.ToString();
            var uploadAsset = _context.Assets.Create(assetName, AssetCreationOptions.None);
            var assetFile = uploadAsset.AssetFiles.Create(Path.GetFileName(uploadPath));
            assetFile.Upload(uploadPath);

            return uploadAsset;
        }

        public static IAsset encodeAsset(string assetIdToEncode)
        {
            var encodeAssetId = assetIdToEncode;
            // Preset reference documentation: http://msdn.microsoft.com/en-us/library/windowsazure/jj129582.aspx    
            //var encodingPreset = "H264 Broadband 720p";
            var encodingPreset = "H264 Broadband SD 16x9";

            var assetToEncode = _context.Assets.Where(a => a.Id == encodeAssetId).FirstOrDefault();
            if (assetToEncode == null)
            {
                throw new ArgumentException("Could not find assetId: " + encodeAssetId);
            }

            IJob job = _context.Jobs.Create("Encoding " + assetToEncode.Name + " to " + encodingPreset);

            IMediaProcessor latestWameMediaProcessor = (from p in _context.MediaProcessors where p.Name == "Windows Azure Media Encoder" select p).ToList().OrderBy(wame => new Version(wame.Version)).LastOrDefault();
            ITask encodeTask = job.Tasks.AddNew("Encoding", latestWameMediaProcessor, encodingPreset, TaskOptions.None);
            encodeTask.InputAssets.Add(assetToEncode);
            encodeTask.OutputAssets.AddNew(assetToEncode.Name + " as " + encodingPreset, AssetCreationOptions.None);

            job.StateChanged += new EventHandler<JobStateChangedEventArgs>((sender, jsc) => Console.WriteLine(string.Format("{0}\n  State: {1}\n  Time: {2}\n\n", ((IJob)sender).Name, jsc.CurrentState, DateTime.UtcNow.ToString(@"yyyy_M_d_hhmmss"))));
            job.Submit();
            job.GetExecutionProgressTask(CancellationToken.None).Wait();

            var preparedAsset = job.OutputMediaAssets.FirstOrDefault();

            return preparedAsset;
        }

        public static IAsset GetAsset(string assetId)
        {
            // Use a LINQ Select query to get an asset.
            var assetInstance =
                from a in _context.Assets
                where a.Id == assetId
                select a;
            // Reference the asset as an IAsset.
            IAsset asset = assetInstance.FirstOrDefault();

            return asset;
        }

        public static string GetStreamingURL(IAsset preparedAsset, int daysForWhichStreamingUrlIsActive = 365)
        {
            var streamingAssetId = preparedAsset.Id; // "YOUR ASSET ID";

            var streamingAsset = _context.Assets.Where(a => a.Id == streamingAssetId).FirstOrDefault();

            var accessPolicy = _context.AccessPolicies.Create(streamingAsset.Name, TimeSpan.FromDays(daysForWhichStreamingUrlIsActive),
                                                     AccessPermissions.Read | AccessPermissions.List);
            string streamingUrl = string.Empty;
            var assetFiles = streamingAsset.AssetFiles.ToList();
            var streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith("m3u8-aapl.ism")).FirstOrDefault();
            if (streamingAssetFile != null)
            {
                var locator = _context.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy);
                Uri hlsUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest(format=m3u8-aapl)");
                streamingUrl = hlsUri.ToString();
                Console.WriteLine("M3u8");
                Console.WriteLine("Streaming Url: " + streamingUrl);
            }

            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".ism")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = _context.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy);
                Uri smoothUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest");
                streamingUrl = smoothUri.ToString();
                Console.WriteLine("ism");
                Console.WriteLine("Streaming Url: " + streamingUrl);
            }

            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".mp4")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = _context.Locators.CreateLocator(LocatorType.Sas, streamingAsset, accessPolicy);
                var mp4Uri = new UriBuilder(locator.Path);
                mp4Uri.Path += "/" + streamingAssetFile.Name;
                streamingUrl = mp4Uri.ToString();
                Console.WriteLine("Streaming Url: " + streamingUrl);
            }
            return streamingUrl;
        }

        public static void DeleteAsset(IAsset asset)
        {
            // delete the asset
            asset.Delete();

            // Verify asset deletion
            if (GetAsset(asset.Id) == null)
                Console.WriteLine("Deleted the Asset");

        }
    }
}