using EnviTaskManagerClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Services
{
    public interface ITaskService
    {
      
        StatusResult GetTaskStatus(  TaskItem item);
        StatusResult TaskStart(TaskItem item);
        StatusResult TaskTerminate(TaskItem item);
        StatusResult TaskScheduler(TaskItem item);
    }
}
