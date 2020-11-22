using EnviTaskManagerClient.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Services
{
    public class TaskService : ITaskService
    {
        private enum TaskTypeEnum { Unknown = 0, Danvand = 1, DanDas = 2, SroData = 3 }
        private readonly ILogger<TaskService> _logger;
        private AppSettings AppSettings { get; set; }

        public TaskService(ILogger<TaskService> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            AppSettings = settings.Value;
        }

        public StatusResult GetTaskStatus(TaskItem taskItem)
        {

            var json = JsonConvert.SerializeObject(taskItem);
            try
            {
                var processResult = GetPostRequest(AppSettings.AzureFunctionURLStatus, json).Result;


                string output = string.Empty;
                var status = new StatusResult();
                dynamic data = JsonConvert.DeserializeObject(processResult.Result);

                status.createdTime = data?.createdTime;
                status.instanceId = InstanceIdName(data?.instanceId);
                status.lastUpdatedTime = data?.lastUpdatedTime;
                status.name = data?.name;
                status.runtimeStatus = RunTimeStatusName(data?.runtimeStatus);
                if (data?.output != null)
                {
                    status.Info = ParseOutputInfo(data?.output);
                }

                if (data?.history != null)
                {
                    status.History = ParseOutputInfo(data?.history);
                }

                return status;
            }
            catch (Exception ex)
            {
                return new StatusResult { Info = ex.Message };
            }
        }

        public StatusResult TaskStart(TaskItem item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var processResult = GetPostRequest(AppSettings.AzureFunctionURLStart, json).Result;
                if (processResult.Success)
                    return GetTaskStatus(item);
                return new StatusResult { Info = "Ukendt fejl" };
            }
            catch (Exception ex)
            {
                return new StatusResult { Info = ex.Message };
            }
        }


        public StatusResult TaskTerminate(TaskItem item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var processResult = GetPostRequest(AppSettings.AzureFunctionURLTerminate, json).Result;
                if (processResult.Success)
                    return GetTaskStatus(item);
                return new StatusResult { Info = "Ukendt fejl" };
            }
            catch (Exception ex)
            {
                return new StatusResult { Info = ex.Message };
            }
        }

        public StatusResult TaskScheduler(TaskItem item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var processResult = GetPostRequest(AppSettings.AzureFunctionURLSchedulerRunner, json).Result;
                if (processResult.Success)
                    return GetTaskStatus(item);
                return new StatusResult { Info = "Ukendt fejl" };
            }
            catch (Exception ex)
            {
                return new StatusResult { Info = ex.Message };
            }
        }


        private string RunTimeStatusName(object status)
        {
            if (status == null ||
               (!int.TryParse(status.ToString(), out var enumVal)))
                return "Unknown";
            var runtimeStatus = (OrchestrationRuntimeStatus)enumVal;
            return runtimeStatus.ToString();
        }

        private string InstanceIdName(object name)
        {
            if (name == null) return "Unknown";
            var info = name.ToString();
            var words = info.Split('-');
            if (words.Length < 2) return info;
            int.TryParse(words[0], out var enumVal);
            var instanceIdName = (TaskTypeEnum)enumVal;
            return instanceIdName.ToString();
        }

        private string ParseOutputInfo(object data)
        {
            if (data == null) return string.Empty;
            if (data.GetType().Equals(typeof(JArray)))
            {
                var lst = new List<string>();
                JArray parsedArray = (JArray)data;

                for (int idx = 0; idx < parsedArray.Count; idx++)
                {
                    JToken token = parsedArray[idx];
                    lst.Add(token.ToString());
                }
                return string.Join("<br/>", lst);
            }
            return data.ToString();
        }



        public async Task<ProcessResult> GetPostRequest(string url, string json)
        {

            var processResult = new ProcessResult { Success = true, Result = "" };

            using (var client = new HttpClient())
            {
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url),
                        Content = content
                    };

                    HttpResponseMessage result = await client.SendAsync(request);
                    if (result.IsSuccessStatusCode)
                    {
                        processResult.Result = result.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        processResult.Result = result.StatusCode.ToString();
                    }
                }
            }
            return processResult;
        }

        public async Task<ProcessResult> GetRequest(string url)
        {
            var processResult = new ProcessResult { Success = true, Result = "" };
            using (var client = new HttpClient())
            {
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(url)
                    };

                    HttpResponseMessage result = await client.SendAsync(request);
                    if (result.IsSuccessStatusCode)
                    {
                        processResult.Result = result.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        processResult.Result = result.StatusCode.ToString();
                    }
                }
                return processResult;
            }


        }


    }
}