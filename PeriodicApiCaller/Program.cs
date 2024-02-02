using Microsoft.Extensions.DependencyInjection;
using PeriodicApiCaller;
using PeriodicApiCaller.ApiFetcher;

IServiceCollection services = new ServiceCollection();

//Singleton ?
services.AddSingleton<IInputProcessor, InputProcessor>();
services.AddSingleton<IPeriodicApiFetcher, PeriodicApiFetcher>();
services.AddSingleton<IApiService, ApiService>();
services.AddHttpClient();

IServiceProvider serviceProvider = services.BuildServiceProvider();

var inputProcessor = serviceProvider.GetRequiredService<IInputProcessor>();


await inputProcessor.ReadInput();

