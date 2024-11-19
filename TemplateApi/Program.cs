using Scalar.AspNetCore;

using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var kafkaUrl = builder.Configuration["Kafka:BootstrapServers"];
var producerConfig = new ProducerConfig { BootstrapServers = kafkaUrl };
using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

var npgSqlString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddHealthChecks()
    .AddKafka(producerConfig, "logs")
    .AddNpgSql(npgSqlString!)
    ;

var app = builder.Build();

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }