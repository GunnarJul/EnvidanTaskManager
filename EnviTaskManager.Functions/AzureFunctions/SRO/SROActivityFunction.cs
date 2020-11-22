using EnviTaskManagerFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {

        [FunctionName(nameof(SROFullProcess))]
        public ProcessResult SROFullProcess([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Start  prepare SRO.");
 
            var result1 = _service.SROPrePareData(curstomerId);
            log.LogInformation(result1.Information);
            if (!result1.Sucess)
                return result1;

            var result2 = _service.SROProcessData (curstomerId);
            log.LogInformation(result2.Information);
            
            if (!result2.Sucess)
                return result2;

            var result3 = _service.SROCompleteData (curstomerId);
            log.LogInformation(result3.Information);

            result3.Information = $"{result1.Information}\n {result2.Information}\n{result3.Information}";

            return result3;
        }

        [FunctionName(nameof(SROPrepare) )]
        public ProcessResult SROPrepare([ActivityTrigger]int curstomerId, ILogger log)
        {

            log.LogInformation($"Start customer{curstomerId} prepare SRO.");

            var result = _service.SROPrePareData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

        [FunctionName(nameof(SROProcessData))]
        public ProcessResult SROProcessData([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Process customer{curstomerId} Process SRO.");

            var result = _service.SROProcessData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

  

        [FunctionName(nameof(SROCompleteData))]
        public ProcessResult SROCompleteData([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Complete customer{curstomerId} SRO.");

            var result = _service.SROCompleteData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

    }
}
