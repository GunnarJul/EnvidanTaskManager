using EnviTaskManagerFunctions.Models;
using EnviTaskManagerFunctions.Service;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviTaskManagerFunctions
{
    public abstract class TaskAzureFunctionBase

    {
        protected readonly ITaskService _service;

        public TaskAzureFunctionBase(ITaskService service)
        {
            _service = service;
        }
        protected TaskItem TaskItemFromString(string Json)
        {
            try
            {
                return JsonConvert.DeserializeObject<TaskItem>(Json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        protected string JsonFromTaskItem(TaskItem taskItem)
        {
            return JsonConvert.SerializeObject(taskItem);
        }
        protected List<int> SplitArgs(string arg)
        {
            var result = new List<int>();
            if (string.IsNullOrEmpty(arg))
            {
                return result;
            }
            var words = arg.Split(',').ToList();

            foreach (var word in words)
            {
                if (int.TryParse(word, out var val) &&
                    !result.Contains(val))
                {
                    result.Add(val);
                }
            }
            return result;
        }
        protected async Task<TaskItem> GetTaskItem(HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return TaskItemFromString(requestBody);
        }

    }
}
