using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {
        [FunctionName(nameof(StartTimeTriggeredSROTasks))]
        public async Task StartTimeTriggeredSROTasks(
                          [TimerTrigger("%SCHEDULER_INTERVAL%")]
                          TimerInfo timer,
                          [DurableClient]
                          IDurableOrchestrationClient client,
                          ILogger log)
        {
            var orchestratorInstanceId = ((int)TaskTypeEnum.SroData).ToString(); ; // her skal input komme fra database!
            
            DurableOrchestrationStatus status = await client.GetStatusAsync(orchestratorInstanceId);

            // avoid runtime error, from staring the same orchestration more than once. 
            if (status != null && status.RuntimeStatus != OrchestrationRuntimeStatus.Completed)
            {
                log.LogInformation($"Timer trigger already started : {DateTime.Now}");
                return;
            }

            var curstomerIds = _service.GetTimeScheduleredCustomers(TaskTypeEnum.DanDas);

            var orchestratorFunction = (curstomerIds.Count <= 1) ? nameof(StartOrchestrator) : nameof(StartOrchestratorFanInOut);

            //string instanceId = await client.StartNewAsync<string>(nameof(StartOrchestrator), orchestratorInstanceId, "-1");

            //log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}
