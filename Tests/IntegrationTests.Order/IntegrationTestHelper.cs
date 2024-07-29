using System.Net.Http.Headers;
using AutoFixture;
using MassTransit.Testing;

namespace IntegrationTests.Order;

public class IntegrationTestHelper : IClassFixture<IntegrationTestFactory<Program>>
{
    public Fixture Fixture { get; } = new();

    public readonly IntegrationTestFactory<Program> Factory;
    public readonly HttpClient HttpClient;

    public IntegrationTestHelper(IntegrationTestFactory<Program> factory)
    {
        Factory = factory;
        TestHarness = factory.Services.GetTestHarness();

        HttpClient = factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    }

    public ITestHarness TestHarness { get; set; }
}