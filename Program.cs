using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

internal class Program {

    public static async Task Main(string[] args) {

        var serviceProvider = new ServiceCollection()
                        .AddHttpClient()
                        .AddSingleton<IpLocationService>()
                        .BuildServiceProvider(); 

        var ipService = serviceProvider.GetRequiredService<IpLocationService>(); 
        var response = await ipService.Locate("142.250.178.163");
        var formatted = JsonSerializer.Serialize(response, new JsonSerializerOptions() { WriteIndented = true });

        System.Console.WriteLine(formatted);

    }

}