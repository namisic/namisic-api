using AppContants = Condominiums.Api.Constants;
using Condominiums.Api.Options;
using Condominiums.Api.Seeds.Base;
using MongoDB.Driver;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connectionString = builder.Configuration.GetConnectionString("MongoDbUri");
string? mongoDbname = builder.Configuration.GetValue<string>("MongoDbName");
string[]? allowedCorsOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();

if (!(allowedCorsOrigins?.Any() ?? false))
{
    Console.WriteLine("Please set the 'AllowedCorsOrigins' setting.");
    Environment.Exit(1);
}

builder.Services.Configure<GeneralSettingsOptions>(builder.Configuration.GetSection(AppContants.ConfigurationSection.GeneralSettings));

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbname));

// CORS policy.
builder.Services.AddCors(options => options.AddDefaultPolicy(
    config => config.AllowAnyHeader().AllowAnyMethod().WithOrigins(allowedCorsOrigins!)
));

builder.Services.AddApi()
    .AddAuth(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
    IFarmer farmer = scope.ServiceProvider.GetRequiredService<IFarmer>();
    await farmer.PlantAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
