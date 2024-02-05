using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeriodicApiCaller;
using PeriodicApiCaller.ApiFetcher;
using PeriodicApiCaller.ApiFetcher.Interfaces;
using PeriodicApiCaller.Configuration;
using PeriodicApiCaller.Core;
using PeriodicApiCaller.Core.Interfaces;
using PeriodicApiCaller.Interfaces;
using PeriodicApiCaller.Persistence;
using PeriodicApiCaller.Persistence.Repositories;
using PeriodicApiCaller.Persistence.Repositories.Interfaces;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<ApiServiceSettings>(configuration.GetSection("ApiServiceSettings"));

        services.AddDbContext<WeatherInfoDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("WeatherInfoDb")));

        services.AddScoped<IInputProcessor, InputProcessor>();
        services.AddScoped<ICityValidatorService, CityValidatorService>();
        services.AddScoped<IApiService, ApiService>();
        services.AddScoped<IApiFetcher, ApiFetcher>();
        services.AddHttpClient();
        services.AddScoped<IWeatherInfoRepository, WeatherInfoRepository>();

        services.AddSingleton<IAuthTokenService, AuthTokenService>();
        services.AddSingleton<IJobOrchestrator, JobOrchestrator>();

    }).Build();

// Automatically apply pending migrations
DatabaseInitializer.Initialize(host.Services);

var inputProcessor = host.Services.GetRequiredService<IInputProcessor>();

await inputProcessor.ReadInput(args);

