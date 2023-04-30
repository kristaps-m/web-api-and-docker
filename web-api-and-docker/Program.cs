using Microsoft.AspNetCore.Authentication;
using ParseTools;
using ParseTools.Interfaces;
using ParseTools.returnFormats;
using web_api_and_docker.Data;
using web_api_and_docker.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJsonFileFormatTools, JsonFileFormatTools>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddSingleton<ICredentialsStorage, CredentialsStorage>();
builder.Services.AddSingleton<IStringDecoding, StringDecoding>();
builder.Services.AddSingleton<IFormatFactory, FormatFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // BasicAuthentication
app.UseAuthorization();

app.MapControllers();

app.Run();
