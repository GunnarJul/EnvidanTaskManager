using EnviTaskManagerFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace EnviTaskManagerFunctions
{
    public partial class EnvidanTaskManger
    {

        [FunctionName(nameof(DandasFullProcess))]
        public ProcessResult DandasFullProcess([ActivityTrigger] int customerId, ILogger log)
        {

            log.LogInformation($"Start customer {customerId} prepare dandas.");

            var result1 = _service.DandasPrePareData(customerId);
            log.LogInformation(result1.Information);
            if (!result1.Sucess)
                return result1;

            var result2 = _service.DandasProcessData (customerId);
            log.LogInformation(result2.Information);
            
            if (!result2.Sucess)
                return result2;

            var result3 = _service.DandasCompleteData (customerId);
            log.LogInformation(result3.Information);

            result3.Information = $"{result1.Information}\n {result2.Information}\n{result3.Information}";

            return result3;
        }

        [FunctionName(nameof(DandasPrepareData) )]
        public ProcessResult DandasPrepareData([ActivityTrigger] int customerId , ILogger log)
        {

            log.LogInformation($"Start customer {customerId} prepare dandas.");

            var result = _service.DandasPrePareData(customerId);
            log.LogInformation(result.Information);
            return result;
        }

        [FunctionName(nameof(DandasProcessData))]
        public ProcessResult DandasProcessData([ActivityTrigger] int customerId, ILogger log)
        {

            log.LogInformation($"Process customer {customerId}  Process dandas.");

            var result = _service.DandasProcessData(customerId);
            log.LogInformation(result.Information);
            return result;
        }

  

        [FunctionName(nameof(DandasCompleteData))]
        public ProcessResult DandasCompleteData([ActivityTrigger] int customerId, ILogger log)
        {

            log.LogInformation($"Complete customer {customerId} dandas.");

            var result = _service.DandasCompleteData(customerId);
            log.LogInformation(result.Information);
            return result;
        }

    }
}
