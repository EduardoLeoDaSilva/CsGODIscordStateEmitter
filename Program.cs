using CsGOStateEmitter;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<DiscordEmitter>();
builder.Services.AddSingleton<StateManagement>();
builder.Services.AddSingleton<IServiceProvider, ServiceProvider>();
GlobalConfiguration.Configuration.UseInMemoryStorage();
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings());

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<HostedDiscordService>();
builder.Services.AddScoped<HostzoneService>();

var connectionString = builder.Configuration.GetConnectionString("db");

builder.Services.AddDbContext<ApplicationContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o =>
{
    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
}));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
