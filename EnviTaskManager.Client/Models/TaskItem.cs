using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Models
{


    public class ActiviyInformation
    {
        public string Key { get;set;}
        public string Value { get; set; }
    }

    public class TaskItem
    {
        public int TaskType { get; set; }
        public int CustomerId { get; set; }
        public int MonitorHour{ get; set; }
    }
}
