using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Models
{
    public class ProcessResult
    {
        public bool Success { get; set; }
        public string Result { get; set; }
    }
    public class StatusResult

    {
        public string name { get; set; }
        public string instanceId { get; set; }
        public string runtimeStatus { get; set; }
        //public string output { get; set; }
        public string Info { get; set; }
        public string History { get; set; }
        public string createdTime { get; set; }
        public string lastUpdatedTime { get; set; }
    }
}
