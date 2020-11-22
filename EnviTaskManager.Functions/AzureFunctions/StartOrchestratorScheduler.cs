using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {
        //https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp#pattern-4-monitoring
        [FunctionName(nameof(StartOrchestratorScheduler))]
        public async Task StartOrchestratorScheduler(
                          [OrchestrationTrigger]   
                          IDurableOrchestrationContext  context,
                          ILogger log)
        {
            var taskItem = context.GetInput<TaskItem>();
            if (taskItem == null)
            {
                var error = new ProcessResult { Sucess = false, Information = "Error getting task information" };
                AddLog(log, error);
                return;
            }
            TaskTypeEnum taskType = (TaskTypeEnum)taskItem.TaskType;
            
            string prepare = ActivityFunctionPrepare(taskType);
            string process = ActivityFunctionProcess(taskType);
            string complete = ActivityFunctionComplete(taskType);

            
            double secondInAFullDay = 86400;
            
         

            //// hvordan tjekker vi at task'en ikke allerde er scheduleret ?
            //// i så fald skal den terminineres.

            var date = context.CurrentUtcDateTime.Hour > taskItem.MonitorHour ? context.CurrentUtcDateTime.AddDays(1) : context.CurrentUtcDateTime;
            var nextTime = new DateTime(date.Year, date.Month, date.Day, taskItem.MonitorHour, 0, 0);
            var pollingInterval = nextTime.Subtract(context.CurrentUtcDateTime).TotalSeconds;
            DateTime expiryTime = new DateTime(3000, 1, 1);
            
            while (context.CurrentUtcDateTime < expiryTime)
            {
                // Orchestration sleeps until this time.
                var nextCheck = context.CurrentUtcDateTime.AddSeconds(pollingInterval);
                await context.CreateTimer(nextCheck, CancellationToken.None);
                
                var resultPrepare = await context.CallActivityAsync<ProcessResult>(prepare, taskItem.CustomerId);
                AddLog(log,resultPrepare);

                if (resultPrepare.Sucess)
                {
                    var resultProcess = await context.CallActivityAsync<ProcessResult>(process, taskItem.CustomerId);
                    AddLog(log, resultProcess);

                    if (resultProcess.Sucess)
                    {
                        var resultComplete = await context.CallActivityAsync<ProcessResult>(complete, taskItem.CustomerId);
                        AddLog(log, resultComplete);
                    }
                }
                // her sikres at denne uendelig løkke, ikke medfører at durable storage account fyldes op.
                context.ContinueAsNew(null); // her har man mulighed for at have informationer til næste genneløb
                pollingInterval = secondInAFullDay ;
            }

        }

        private void AddLog(ILogger log, ProcessResult result)
        {
            if (result.Sucess) log.LogInformation(result.Information);
            else log.LogError(result.Information);
        }
    }
}
