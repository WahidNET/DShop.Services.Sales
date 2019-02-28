using System.Threading.Tasks;
using DShop.Common.Logging;
using DShop.Common.Vault;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DShop.Services.Sales
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .UseVault()
                .UseLogging()
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
    }
}
