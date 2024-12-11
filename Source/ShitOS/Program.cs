using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShitOS.Application;
using ShitOS.Core.Command;
using ShitOS.Core.LoadBalancer;
using ShitOS.Core.Pocessor;
using ShitOS.Core.Task;
using ShitOS.Core.TaskManager;
using ShitOS.Features.SystemOptions;

TimerOptions timerOptions = new()
{
    Fps = 10,
    TicsPerSecond = 2
};
        
OsCommandsFactory commandsFactory = new();
commandsFactory.ExceutableOptions = OsCommandOptions.Executable;
commandsFactory.IOOptions = OsCommandOptions.IO;
        
OsTaskFactoryOptions taskFactoryOptions = new(1000, commandsFactory);
OsTaskFactory taskFactory = new(taskFactoryOptions);
        
OsTaskManagerOptions taskManagerOptions = new(
    10_000,
    1
);

IOsLoadBalancer loadBalancer = new FIFOWithoutPriorityLoadBalancer();
QueueOsTaskManager taskManager = new(taskManagerOptions, loadBalancer);
        
OsProcessorOptions processorOptions = new(
    taskManager,
    timerOptions.TicsPerSecond
);
        
OsAccumulator accumulator = new OsAccumulator(
    processorOptions
);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<ApplicationRouting>(".app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(taskFactory);
builder.Services.AddSingleton(commandsFactory);
builder.Services.AddSingleton<IOsTaskManager>(taskManager);
builder.Services.AddSingleton(accumulator);
builder.Services.AddSingleton(timerOptions);


await builder.Build().RunAsync();
