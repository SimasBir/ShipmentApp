using Microsoft.Extensions.DependencyInjection;
using ShipmentApp.Services;
using System.Threading.Tasks;

namespace ShipmentApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var mainService = serviceProvider.GetService<MainService>();

            await mainService.ExecuteAsync();

        }
    }
}
