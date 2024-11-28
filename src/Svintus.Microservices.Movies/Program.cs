using Svintus.Microservices.Movies.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<MovieGrpcService>();
app.Run();