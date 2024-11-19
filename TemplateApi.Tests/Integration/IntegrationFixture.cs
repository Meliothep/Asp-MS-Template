
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace TemplateApi.Tests.Integration;
public class IntegrationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("My_DB")
            .WithUsername("My_User")
            .WithPassword("My_Password")
            .WithPortBinding(5432)
            .Build();

    private readonly KafkaContainer _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithPortBinding(9092)
            .Build();

    public WebApplicationFactory<Program>? factory;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _kafkaContainer.StartAsync();
        factory = new WebApplicationFactory<Program>();
    }

    public async Task DisposeAsync()
    {
        await factory!.DisposeAsync();
        await _dbContainer.DisposeAsync();
        await _kafkaContainer.DisposeAsync();
    }
}