using CallWebApi.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpClient("UserApi", client =>
        {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        });
    })
    .Build();

var httpClientFactory = host.Services.GetRequiredService<IHttpClientFactory>();

try
{
    var client = httpClientFactory.CreateClient("UserApi");

    var users = await client.GetFromJsonAsync<IEnumerable<User>>("/users")
        ?? throw new NullReferenceException();

    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user?.Id}\nName: {user?.Name}\nUsername: {user?.Username}\nEmail: {user?.Email}\nPhone: {user?.Phone}\nWebsite: {user?.Website}" +
        $"\nAddress: {user?.Address.Street}, Suite: {user?.Address.Suite}, City: {user?.Address.City}, ZipCode: {user?.Address.ZipCode} \n" +
        $"Latitude: {user?.Address.Geo.Lat}, Longitude: {user?.Address.Geo.Lng}\n");
    }
}
catch (NullReferenceException ex)
{
    Console.WriteLine($"Erro na requisição: {ex.Message}");
}