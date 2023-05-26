using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeatherProject.Tests.Integration;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly ICompositeService _container = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName!, "docker-compose.yml"))
        .RemoveOrphans()
        .Build();

    public HttpClient HttpClient { get; private set; } = default!;

    public Task InitializeAsync()
    {
        _container.Start();
        HttpClient = CreateClient();

        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        _container.Stop();
        _container.Dispose();

        return Task.CompletedTask;
    }
}