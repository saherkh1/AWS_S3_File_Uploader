using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace AWSFileUploader.Models
{
    public class AwsService
    {
        private readonly string _region;
        private readonly string _accessKey;
        private readonly string _secretAccessKey;
        private readonly Dispatcher _dispatcher;

        public AwsService(string region, string accessKey, string secretAccessKey)
        {
            _region = region;
            _accessKey = accessKey;
            _secretAccessKey = secretAccessKey;
            _dispatcher = Application.Current.Dispatcher;
        }

        public bool CheckConnection()
        {
            try
            {
                using (var client = new AmazonS3Client(_accessKey, _secretAccessKey, RegionEndpoint.GetBySystemName(_region)))
                {
                    // Check connection by listing buckets
                    var response = client.ListBuckets();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Connection Error: {ex.Message}");
                return false;
            }
        }

        public bool UploadFile(string bucketName, string filePath, EventHandler<Amazon.S3.Transfer.UploadProgressArgs> OnUploadProgressEvent)
        {
            try
            {
                using (var client = new AmazonS3Client(_accessKey, _secretAccessKey, RegionEndpoint.GetBySystemName(_region)))
                {
                    var fileTransferUtility = new TransferUtility(client);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        FilePath = filePath,
                    };

                    uploadRequest.UploadProgressEvent += OnUploadProgressEvent;

                    fileTransferUtility.Upload(uploadRequest);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Upload Error: {ex.Message}");
                return false;
            }
        }
    }
}
