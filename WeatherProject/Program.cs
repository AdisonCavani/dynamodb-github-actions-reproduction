using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var credentials = new BasicAWSCredentials("0", "0");
var config = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.EUCentral1,
    ServiceURL = "http://localhost:8000"
};

builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, config));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}