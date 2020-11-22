using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace EnviTaskManagerFunctions
{
    public static class ProcessTaskStarter
    {
        [FunctionName("StartOrchestrator")]
        public static async Task<List<string>> StartOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var outputs = new List<string>();

            if (!context.IsReplaying)
                log.LogInformation("Starting Tokyo");
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Tokyo"));
            if (!context.IsReplaying)
                log.LogInformation("Starting Seattle");
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "Seattle"));
            if (!context.IsReplaying)
                log.LogInformation("Starting London");
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("Function1_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("TaskStarter")]
        public static async Task<HttpResponseMessage> TaskStarter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // starting point: Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("StartOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            var result = starter.CreateCheckStatusResponse(req, instanceId);
            return result;
        }
    }
}