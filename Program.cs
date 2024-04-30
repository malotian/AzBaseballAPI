using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddControllers().AddJsonOptions(options => {
            options.JsonSerializerOptions.Converters.Add(new DBNullJsonConverter());
        });

    })
    .ConfigureOpenApi()
    .Build();

host.Run();
