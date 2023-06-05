using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSFileUploader.Models
{
    public class UploadProgressEventArgs : EventArgs
    {
        public int PercentDone { get; }

        public UploadProgressEventArgs(int percentDone)
        {
            PercentDone = percentDone;
        }
    }
}
