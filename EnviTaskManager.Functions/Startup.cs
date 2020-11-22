using EnviTaskManagerFunctions.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EnviTaskManagerFunctions.Startup))]
namespace EnviTaskManagerFunctions
{

    public class Startup : FunctionsStartup
    {
 
        public override void Configure(IFunctionsHostBuilder builder)
        {
   
            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddLogging();
        }
    }
}

