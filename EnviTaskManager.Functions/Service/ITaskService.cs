using EnviTaskManagerFunctions.Models;
using System.Collections.Generic;

namespace EnviTaskManagerFunctions.Service
{
    public  enum TaskTypeEnum { Unknown=0, Danvand =1, DanDas =2, SroData =3 }
    public interface ITaskService
    {
        string TaskId(TaskItem taskItem);
        List<int> GetTimeScheduleredCustomers(TaskTypeEnum taskType);
        
        ProcessResult DandasPrePareData(int customerId);
        ProcessResult DandasProcessData(int customerId);
        ProcessResult DandasCompleteData(int customerId);

        ProcessResult DanvandPrePareData(int customerId);
        ProcessResult DanvandProcessData(int customerId);
        ProcessResult DanvandCompleteData(int customerId);

        ProcessResult SROPrePareData(int customerId);
        ProcessResult SROProcessData(int customerId);
        ProcessResult SROCompleteData(int customerId);

    }
}
