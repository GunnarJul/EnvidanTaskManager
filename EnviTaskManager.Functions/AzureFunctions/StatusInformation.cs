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
        [FunctionName("StatusInformation")]
        public async Task<IActionResult> StatusInformation(
                                    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
                                    HttpRequest req,
                                    [DurableClient]
                                    IDurableOrchestrationClient client)
        {

            var taskItem = await GetTaskItem(req);

            var taskId = _service.TaskId(taskItem);
            if (string.IsNullOrWhiteSpace(taskId))
                return new OkObjectResult("orchestratorId missing");
            
            DurableOrchestrationStatus status = await client.GetStatusAsync(taskId,true,false,false);
            return new OkObjectResult(status);
        }
    }
}
