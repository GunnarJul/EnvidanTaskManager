using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient
{
    public class AppSettings
    {
        public string AzureFunctionURLStatus { get; set; }
        public string AzureFunctionURLStart { get; set; }
        public string AzureFunctionURLTerminate { get; set; }

        public string AzureFunctionURLSchedulerRunner { get; set; }
    }
}
