using Microsoft.Extensions.Caching.Memory;
using NLog;
using Server;
using Server.Mapper;
using Server.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
string corsStr = builder.Configuration["ConnectionStrings:CorsString"];

// Add services to the container.

builder.Services.AddControllers();

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureCors(corsStr);
builder.Services.ConfigureVersioning();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository>(serviceProvider =>
{
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var apiBaseUrl = builder.Configuration["ConnectionStrings:BaseApiUrl"];
    var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
    return new PostRepository(httpClient, apiBaseUrl, memoryCache);
});

builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(corsStr);
app.MapControllers();
app.Run();

