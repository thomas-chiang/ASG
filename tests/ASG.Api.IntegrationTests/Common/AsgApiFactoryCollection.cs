namespace ASG.Api.IntegrationTests.Common;

[CollectionDefinition(CollectionName)]
public class AsgApiFactoryCollection : ICollectionFixture<AsgApiFactory>
{
    public const string CollectionName = "AsgApiFactoryCollection";
}