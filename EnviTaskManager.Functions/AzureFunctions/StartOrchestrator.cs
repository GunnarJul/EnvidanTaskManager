using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {
        [FunctionName(nameof(StartOrchestrator))]
        public async Task<List<string>> StartOrchestrator(
         [OrchestrationTrigger] IDurableOrchestrationContext context,
         ILogger log)
        {

            var outputs = new List<string>();

            var taskItem = context.GetInput<TaskItem>();
            if (taskItem == null)
            {
                outputs.Add("Error getting task information");
                return outputs;
            }
            TaskTypeEnum taskType = (TaskTypeEnum)taskItem.TaskType;
            
            string prepare = ActivityFunctionPrepare(taskType);
            var resultPrepare = await context.CallActivityAsync<ProcessResult>(prepare, taskItem.CustomerId);
            outputs.Add(resultPrepare.Information);
            if (!resultPrepare.Sucess)
                return outputs;

            string process = ActivityFunctionProcess(taskType);
            var resultProcess = await context.CallActivityAsync<ProcessResult>(process, taskItem.CustomerId);
            outputs.Add(resultProcess.Information);
            if (!resultProcess.Sucess)
                return outputs;

            string complete = ActivityFunctionComplete(taskType);
            var resultComplete = await context.CallActivityAsync<ProcessResult>(complete, taskItem.CustomerId);
            outputs.Add(resultComplete.Information);
            return outputs;

        }


        private string ActivityFunctionPrepare(TaskTypeEnum TaskType)
        {
            var table = new Dictionary<TaskTypeEnum, string>{
                    {TaskTypeEnum.DanDas, nameof (DandasPrepareData)},
                    {TaskTypeEnum.Danvand, nameof (DanvandPrepareData)},
                    {TaskTypeEnum.SroData, nameof (SROPrepare)}
                };
            if (table.ContainsKey(TaskType))
                return table[TaskType];
            return string.Empty;
        }

        private string ActivityFunctionProcess(TaskTypeEnum TaskType)
        {
            var table = new Dictionary<TaskTypeEnum, string>{
                    {TaskTypeEnum.DanDas, nameof (DandasProcessData)},
                    {TaskTypeEnum.Danvand, nameof (DanvandProcessData)},
                    {TaskTypeEnum.SroData, nameof (SROProcessData)}
                };
            if (table.ContainsKey(TaskType))
                return table[TaskType];
            return string.Empty;
        }

        private string ActivityFunctionComplete(TaskTypeEnum TaskType)
        {
            var table = new Dictionary<TaskTypeEnum, string>{
                    {TaskTypeEnum.DanDas, nameof (DandasCompleteData)},
                    {TaskTypeEnum.Danvand, nameof (DanvandCompleteData)},
                    {TaskTypeEnum.SroData, nameof (SROCompleteData)}
                };
            if (table.ContainsKey(TaskType))
                return table[TaskType];
            return string.Empty;
        }
    }
}
