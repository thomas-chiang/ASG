using System.Net;
using System.Net.Http.Json;
using ASG.Api.IntegrationTests.Common;
using ASG.Contracts.ApolloSyncGaia1001FormOperation;
using FluentAssertions;
using TestCommon.TestConstants;

namespace ASG.Api.IntegrationTests.Controllers;

[Collection(AsgApiFactoryCollection.CollectionName)]
public class CreateApolloSyncGaia1001FormOperationTests
{
    private readonly HttpClient _client;

    public CreateApolloSyncGaia1001FormOperationTests(AsgApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenValidRequest_ShouldCreateApolloSyncGaia1001FormOperation()
    {
        // Arrange
        var createApolloSyncGaia1001FormOperationRequest = new CreateApolloSyncGaia1001FormOperationRequest(
            Constants.Gaia1001Forms.DefaultFormKind,
            Constants.Gaia1001Forms.DefaultFormNo
        );

        // Act
        var response = await _client.PostAsJsonAsync("ApolloSyncGaia1001FormOperation",
            createApolloSyncGaia1001FormOperationRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createApolloSyncGaia1001FormOperationResponse =
            await response.Content.ReadFromJsonAsync<CreateApolloSyncGaia1001FormOperationResponse>();
        createApolloSyncGaia1001FormOperationResponse.Should().NotBeNull();
        createApolloSyncGaia1001FormOperationResponse!.ApolloSyncGaia1001FormOperation.SituationEnum.Should().BeEmpty();
    }
}