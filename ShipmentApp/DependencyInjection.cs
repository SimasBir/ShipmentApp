using Microsoft.Extensions.DependencyInjection;
using ShipmentApp.Interfaces;
using ShipmentApp.Services;

namespace ShipmentApp
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<MainService>();
            services.AddTransient<ModelService>();
            services.AddTransient<PriceService>();

            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IOutputService, OutputService>();
            services.AddTransient<IOutputService, ConsoleService>();
        }
    }
}
