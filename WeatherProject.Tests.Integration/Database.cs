using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherProject.Tests.Integration;

public static class Database
{
    public static async Task ResetAsync(ApiFactory factory, string tableName)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();

        var description = await db.DescribeTableAsync(tableName);

        await db.DeleteTableAsync(tableName);

        var createTableRequest = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = description.Table.AttributeDefinitions,
            KeySchema = description.Table.KeySchema,
            ProvisionedThroughput = new ProvisionedThroughput(1, 1)
        };

        await db.CreateTableAsync(createTableRequest);
    }
}