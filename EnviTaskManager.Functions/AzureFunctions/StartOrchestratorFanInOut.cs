using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public partial  class EnvidanTaskManger
    {

        /// <summary>
        /// Run activity function in parallel on multiple datasets
        /// </summary>
        /// <param name="context"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("StartOrchestratorFanInOut")]
        public async Task<List<ProcessResult>> StartOrchestratorFanInOut(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var outputs = new List<string>();
            var ids = context.GetInput<string>();
            var curstomerIds = SplitArgs(ids);
            var processTaskList = new List<Task<ProcessResult>>();
            foreach (var curstomerId in curstomerIds)
            {
                var task = context.CallActivityAsync<ProcessResult>(nameof(DandasFullProcess), curstomerId);
            }

            var taskResults = await Task.WhenAll(processTaskList);

            return taskResults.ToList();
        }
    }
}
