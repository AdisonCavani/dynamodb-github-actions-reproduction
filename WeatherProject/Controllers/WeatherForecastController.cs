using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;

namespace WeatherProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IAmazonDynamoDB _client;


    public WeatherForecastController(IAmazonDynamoDB client)
    {
        _client = client;
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = "MyTable",
            Key = new()
            {
                {"pk", new() {S = $"ITEM#{id.ToString()}"}}
            }
        };

        var response = await _client.GetItemAsync(request);

        if (!response.IsItemSet)
            return NotFound();
        
        var itemAsDoc = Document.FromAttributeMap(response.Item);
        var dto = JsonSerializer.Deserialize<WeatherForecast>(itemAsDoc.ToJson());

        return Ok(dto);
    }
}