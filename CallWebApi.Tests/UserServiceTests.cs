using System.Net;
using System.Text;
using System.Text.Json;
using CallWebApi.Models;
using CallWebApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CallWebApi.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserAsync_ShouldReturnUsers_WhenApiIsSuccessful()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new()
                {
                    Id = 1, 
                    Name = "Leanne Graham",
                    Username = "Bret",
                    Email = "Sincere@april.biz"
                }
            };
            var jsonResponse = JsonSerializer.Serialize(expectedUsers);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")    
                });
            
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")    
            };

            var loggerMock = new Mock<ILogger<UserService>>();
            var service = new UserService(httpClient, loggerMock.Object);

            // Act
            var result = await service.GetUsersAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Leanne Graham");
        }
    }
}