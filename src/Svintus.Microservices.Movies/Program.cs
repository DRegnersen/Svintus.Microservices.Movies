using Svintus.Microservices.Movies.Services;
using Svintus.Movies.Application;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddApplication(configuration)
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<MovieGrpcService>();
app.Run();