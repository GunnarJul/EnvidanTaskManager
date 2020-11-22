using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;


namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {
        [FunctionName("TerminateInstance")]
        public async Task<IActionResult> TerminateInstance(
                                         [DurableClient]
                                         IDurableOrchestrationClient client,
                                         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
                                         HttpRequest req)
        {
            string reason = req.Query["reason"];

            var taskItem = await base.GetTaskItem(req);
            var taskId= _service.TaskId(taskItem);

            if (string.IsNullOrWhiteSpace(taskId))
                return new OkObjectResult("orchestratorId missing");

            DurableOrchestrationStatus status = await client.GetStatusAsync(taskId);

            if (status.RuntimeStatus == OrchestrationRuntimeStatus.Completed)
                return new OkObjectResult("Already completed");

            if (status.RuntimeStatus == OrchestrationRuntimeStatus.Terminated)
                return new OkObjectResult("Already terminated");

            await client.TerminateAsync(taskId, reason);
            return new OkObjectResult("Teminated");
        }
    }
}
