using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using EnviTaskManagerFunctions.Models;

namespace EnviTaskManagerFunctions.Service
{
    public class TaskService : ITaskService
    {
        private SqlConnection _connection;
        public TaskService()
        {
            _connection = new SqlConnection("");
        }
        public string TaskId(TaskItem taskItem)
        {
            return $"{((int)taskItem.TaskType).ToString()}-{taskItem.CustomerId}";
        }

        public ProcessResult DandasPrePareData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $" {nameof(DandasPrePareData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}  " };
        }

        public ProcessResult DandasProcessData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $" {nameof(DandasProcessData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }


        public ProcessResult DandasCompleteData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $"{nameof(DandasCompleteData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }

       
         public List<int> GetTimeScheduleredCustomers(TaskTypeEnum taskType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.Danvand:
                    return new List<int> { 130, 45, 28, 15, 16 };
                case TaskTypeEnum.DanDas:
                    return new List<int> { 130, 45, 28, 15, 16 };
                case TaskTypeEnum.SroData:
                    return new List<int> { 131, 46, 29, 199, 996 };
                default:
                    return new List<int>();
            }

        }

        public ProcessResult DanvandPrePareData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $" {nameof(DanvandPrePareData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }

        public ProcessResult DanvandProcessData(int customerId)
        {
            if( customerId == 130)
            return new ProcessResult { Sucess = false, Information = $" {nameof(DanvandProcessData)} customer {customerId} failed {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}" };
            System.Threading.Thread.Sleep(120000);

            return new ProcessResult { Sucess = true, Information = $" {nameof(DanvandProcessData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}" };
        }

        public ProcessResult DanvandCompleteData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $"{nameof(DanvandCompleteData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }

        public ProcessResult SROPrePareData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $" {nameof(SROPrePareData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }

        public ProcessResult SROProcessData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $" {nameof(SROProcessData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }

        public ProcessResult SROCompleteData(int customerId)
        {
            System.Threading.Thread.Sleep(2000);
            return new ProcessResult { Sucess = true, Information = $"{nameof(SROCompleteData)} customer {customerId} done {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} " };
        }
    }
}
