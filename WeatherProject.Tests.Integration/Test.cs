using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WeatherProject.Tests.Integration;

[Collection(SharedTestCollection.Name)]
public class Test
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _httpClient;

    public Test(ApiFactory factory)
    {
        _factory = factory;
        _httpClient = factory.HttpClient;
    }

    [Fact]
    public async Task Should_Return404_WhenItemDoesNotExist()
    {
        var response = await _httpClient.GetAsync($"/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenOk()
    {
        // Arrange
        await using var scope = _factory.Services.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();

        var item = new WeatherForecast
        {
            Id = Guid.NewGuid().ToString(),
            TemperatureC = 26,
            Summary = "Weather summary"
        };
        var itemAsJson = JsonSerializer.Serialize(item);
        var itemAsDoc = Document.FromJson(itemAsJson);
        var itemAsAttrib = itemAsDoc.ToAttributeMap();

        var request = new PutItemRequest
        {
            TableName = "MyTable",
            Item = itemAsAttrib
        };

        await client.PutItemAsync(request);

        // Act
        var response = await _httpClient.GetAsync($"/{item.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var okResult = await response.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.Equivalent(item, okResult);
    }
}