using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Services;
using CapitalGainsTaxes;

internal class Program
{

    private static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            services.GetRequiredService<App>().Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
            {
                services.AddSingleton<IGainRule, GainRule>();
                services.AddSingleton<IProcessor, Processor>();
                services.AddSingleton<ISender, Sender>();
                services.AddSingleton<App>();
            }).ConfigureAppConfiguration((host, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            });
        }
    }
}