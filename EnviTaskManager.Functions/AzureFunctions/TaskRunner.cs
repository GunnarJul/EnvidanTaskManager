using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger : TaskAzureFunctionBase
    {
        //private readonly ITaskService _service;
        //private static string _orchestratorInstanceName = "OrchestratorId";

        public EnvidanTaskManger(ITaskService service) : base(service)
        {

        }


        [FunctionName("TaskRunner")]
        public async Task<IActionResult> Run(
                                         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] 
                                         HttpRequest req,
                                         [DurableClient]
                                         IDurableOrchestrationClient client,
                                         ILogger log)
        {
            
            var taskItem= await GetTaskItem(req); 
            if (taskItem == null)
                return new OkObjectResult($"TaskItem argument  not valid");

            var orchestratorInstanceId = _service.TaskId(taskItem);

            if (string.IsNullOrWhiteSpace(orchestratorInstanceId.ToString()))
                return new OkObjectResult("orchestratorId missing");

            DurableOrchestrationStatus status = await client.GetStatusAsync(orchestratorInstanceId );

            // avoid runtime error, from staring the same orchestration more than once. 
            if (status != null &&  status.RuntimeStatus == OrchestrationRuntimeStatus.Running )
                return new OkObjectResult(status);

            string instanceId = await client.StartNewAsync<TaskItem>(nameof(StartOrchestrator), orchestratorInstanceId,taskItem );

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var result = client.CreateCheckStatusResponse(req, instanceId);
            return result;
        }


        //[FunctionName("StartTimeTriggeredTasks")]
        //public async Task StartTimeTriggeredTasks(
        //                  [TimerTrigger("%SCHEDULER_INTERVAL%")] 
        //                  TimerInfo timer,
        //                  [DurableClient]
        //                  IDurableOrchestrationClient client,
        //                  ILogger log)
        //{
        //    var orchestratorInstanceId = "timeTriggerStartup"; // her skal input komme fra database!

        //    DurableOrchestrationStatus status = await client.GetStatusAsync(orchestratorInstanceId);

        //    // avoid runtime error, from staring the same orchestration more than once. 
        //    if (status != null && status.RuntimeStatus != OrchestrationRuntimeStatus.Completed)
        //    {
        //        log.LogInformation($"Timer trigger already started : {DateTime.Now}");
        //        return;
        //    }

        //    var curstomerIds= _service.GetTimeScheduleredCustomers(TaskTypeEnum.DanDas);
        //    var orchestratorFunction = (curstomerIds.Count <= 1) ? nameof(StartOrchestrator) : nameof(StartOrchestratorFanInOut);


        //    string instanceId = await client.StartNewAsync<string>(nameof(StartOrchestrator), orchestratorInstanceId, "130");

        //    log.LogInformation($"Started orchestration with ID = '{instanceId}'.");


        //}
    }
}





