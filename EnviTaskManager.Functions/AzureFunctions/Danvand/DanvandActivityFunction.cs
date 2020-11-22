using EnviTaskManagerFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {

        [FunctionName(nameof(DanvandFullProcess))]
        public ProcessResult DanvandFullProcess([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Start curstomerId: {curstomerId} prepare Danvand.");

            var result1 = _service.DanvandPrePareData(curstomerId);
            log.LogInformation(result1.Information);
            if (!result1.Sucess)
                return result1;

            var result2 = _service.DanvandProcessData (curstomerId);
            log.LogInformation(result2.Information);
            
            if (!result2.Sucess)
                return result2;

            var result3 = _service.DanvandCompleteData (curstomerId);
            log.LogInformation(result3.Information);

            result3.Information = $"{result1.Information}\n {result2.Information}\n{result3.Information}";

            return result3;
        }

        [FunctionName(nameof(DanvandPrepareData) )]
        public ProcessResult DanvandPrepareData([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Start curstomerId: {curstomerId} prepare Danvand.");

            var result = _service.DanvandPrePareData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

        [FunctionName(nameof(DanvandProcessData))]
        public ProcessResult DanvandProcessData([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Process curstomerId: {curstomerId} Process Danvand.");

            var result = _service.DanvandProcessData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

  
        [FunctionName(nameof(DanvandCompleteData))]
        public ProcessResult DanvandCompleteData([ActivityTrigger] int curstomerId, ILogger log)
        {

            log.LogInformation($"Complete curstomerId: {curstomerId} Danvand.");

            var result = _service.DanvandCompleteData(curstomerId);
            log.LogInformation(result.Information);
            return result;
        }

    }
}
