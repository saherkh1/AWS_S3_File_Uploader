using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSFileUploader.Models
{
    public class Settings
    {
        public string Region { get; set; }
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }

        public Settings()
        {
            Region = string.Empty;
            BucketName = string.Empty;
            AccessKey = string.Empty;
            SecretAccessKey = string.Empty;
        }
    }
}
