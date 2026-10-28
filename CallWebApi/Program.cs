using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CallWebApi.Configuration;
using CallWebApi.Services;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Binding the appsettings.json
        services.Configure<ApiSettings>(context.Configuration.GetSection(ApiSettings.SectionName));

        // Registering Typed Client(IHttpClientFactory under the hood)
        services.AddHttpClient<IUserService, UserService>((ServiceProvider, client) =>
        {
            var baseUrl = context.Configuration[$"{ApiSettings.SectionName}:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = "https://jsonplaceholder.typicode.com/";
            }

            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddTransient<IUserService, UserService>();
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var userService = host.Services.GetRequiredService<IUserService>();

try
{
    var users = await userService.GetUsersAsync(CancellationToken.None);

    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user.Id}\n" +
            $"Name: {user.Name}\n" +
            $"Username: {user.Username}\n" +
            $"Email: {user.Email}\n" +
            $"Phone: {user.Phone}\n" +
            $"Website: {user.Website}\n" +
            $"Address: {user.Address.Street}, Suite: {user.Address.Suite}, " +
            $"City: {user.Address.City}, ZipCode: {user.Address.ZipCode}\n" +
            $"Latitude: {user.Address.Geo.Lat}, Longitude: {user.Address.Geo.Lng}\n");
    }
}
catch (Exception ex)
{
    logger.LogCritical(ex, "The application failed while executing.");
}