using EnviTaskManagerFunctions.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnviTaskManagerFunctions.Models
{
    public class TaskItem
    {

        public int TaskType { get; set; }
        public int CustomerId { get; set; }
        public int MonitorHour { get; set; }
  
        
    }
}
