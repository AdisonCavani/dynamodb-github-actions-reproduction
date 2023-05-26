using Xunit;

namespace WeatherProject.Tests.Integration;

[CollectionDefinition(Name)]
public class SharedTestCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = "Test collection";
}