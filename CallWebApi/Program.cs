using System.Text.Json;
using CallWebApi.Model;

HttpClient client = new()
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
};

var response = await client.GetAsync("/users");
Console.WriteLine($"Out: {((int)response.StatusCode)}"); //Just to check the status code

try
{
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine($"In: {((int)response.StatusCode)}");

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(content);

        if (users is not null)
        {
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user?.Id}\nName: {user?.Name}\nUsername: {user?.Username}\nEmail: {user?.Email}\nPhone: {user?.Phone}\nWebsite: {user?.Website}" +
                $"\nAddress: {user?.Address.Street}, Suite: {user?.Address.Suite}, City: {user?.Address.City}, ZipCode: {user?.Address.ZipCode} \n" +
                $"Latitude: {user?.Address.Geo.Lat}, Longitude: {user?.Address.Geo.Lng}\n");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Somehow, it was not possible to complete the Task!");
    Console.WriteLine($"{ex.Source}\n{ex.StackTrace}");
}


//Console.WriteLine($"UserId: {posts?.UserId}, \n Id: {posts?.Id}, \n Title: {posts?.Title}, \n Posts: {posts?.Body}");

