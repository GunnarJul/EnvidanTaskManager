using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Models
{
    public class ResponseInformation
    {
        public string id { get; set; }
        public string statusQueryGetUri { get; set; }
        public string sendEventPostUri { get; set; }
        public string terminatePostUri { get; set; }
        public string purgeHistoryDeleteUri { get; set; }
    }
}
