using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extra_Task_Google_Drive.Models
{
    public class GoogleDriveFile
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public long? Size { get; set; }

        public long? Version { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}