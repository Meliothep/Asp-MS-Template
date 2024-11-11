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

/**
var kafkaUrl = builder.Configuration["Kafka:BootstrapServers"];
var producerConfig = new ProducerConfig { BootstrapServers = kafkaUrl };
using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
**/

builder.Services.AddHealthChecks()
    //.AddKafka(producerConfig)
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
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
