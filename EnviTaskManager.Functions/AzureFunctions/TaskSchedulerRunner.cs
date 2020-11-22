using EnviTaskManagerFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions 
{
    public partial class EnvidanTaskManger
    {
        [FunctionName(nameof(TaskSchedulerRunner))]
        public async Task<IActionResult> TaskSchedulerRunner(
                                         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                         HttpRequest req,
                                         [DurableClient]
                                         IDurableOrchestrationClient client,
                                         ILogger log)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var taskItem = await GetTaskItem(req);
            if (taskItem == null)
                return new OkObjectResult($"TaskItem argument  not valid");

            var orchestratorInstanceId = _service.TaskId(taskItem);

            if (string.IsNullOrWhiteSpace(orchestratorInstanceId.ToString()))
                return new OkObjectResult("orchestratorId missing");

            DurableOrchestrationStatus status = await client.GetStatusAsync(orchestratorInstanceId);
            
            if (status != null && status.RuntimeStatus == OrchestrationRuntimeStatus.Running)
                return new OkObjectResult(status);

            if (status != null && status.RuntimeStatus == OrchestrationRuntimeStatus.Pending)
            {
                await client.TerminateAsync(orchestratorInstanceId, "New scheduler time");
            }

            string instanceId = await client.StartNewAsync<TaskItem>( nameof(StartOrchestratorScheduler), orchestratorInstanceId, taskItem);
   
             var result = client.CreateCheckStatusResponse(req, instanceId);
            return result;
        }
    }
}
