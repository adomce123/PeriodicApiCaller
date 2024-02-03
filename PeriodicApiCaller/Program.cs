using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeriodicApiCaller;
using PeriodicApiCaller.ApiFetcher;
using PeriodicApiCaller.Configuration;
using PeriodicApiCaller.Core;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        //Singleton ?
        services.AddSingleton<IInputProcessor, InputProcessor>();
        services.AddSingleton<IPeriodicApiFetcher, PeriodicApiFetcher>();
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<ICityValidatorService, CityValidatorService>();
        services.AddSingleton<IWeatherDataOrchestrator, WeatherDataOrchestrator>();
        services.AddHttpClient();

        services.Configure<ApiServiceSettings>(configuration.GetSection("ApiServiceSettings"));
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
    })
    .Build();

var inputProcessor = host.Services.GetRequiredService<IInputProcessor>();

await inputProcessor.ReadInput();

