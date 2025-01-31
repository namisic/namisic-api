using AppContants = Condominiums.Api.Constants;
using Condominiums.Api.Options;
using Condominiums.Api.Seeds.Base;
using MongoDB.Driver;
using Microsoft.IdentityModel.Logging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connectionString = builder.Configuration["MongoDB:ConnectionString"];
string? mongoDbName = builder.Configuration["MongoDB:DbName"];
string[]? allowedCorsOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();

if (string.IsNullOrEmpty(connectionString)) {
    Console.WriteLine("Por favor configure la cadena de conexión para la base de datos de MongoDB.");
    Environment.Exit(1);
}

if (string.IsNullOrEmpty(mongoDbName)) {
    Console.WriteLine("Por favor configure el nombre la base de datos de MongoDB a utilizar.");
    Environment.Exit(1);
}

if (allowedCorsOrigins?.Length == 0)
{
    Console.WriteLine("Por favor configure los orígenes de CORS permitidos.");
    Environment.Exit(1);
}

builder.Services.Configure<GeneralSettingsOptions>(builder.Configuration.GetSection(AppContants.ConfigurationSection.GeneralSettings));

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbName));

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
