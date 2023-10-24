using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using Serilog;
using  ScheduleJob.AADSync.Service.Model;
using  ScheduleJob.WebApi.Dependencies;
using  ScheduleJob.WebApi.Extension;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration["KeyVault:Endpoint"]);
var tenantId = Convert.ToString(builder.Configuration["KeyVault:TenantId"]);
var clientId = Convert.ToString(builder.Configuration["KeyVault:ClientId"]);
var clientSecret = Convert.ToString(builder.Configuration["KeyVault:ClientSecret"]);
var blobConnectionSecretName = Convert.ToString(builder.Configuration["KeyVault:BlobConnectionSecretName"]);
var ADSMongoDBConnectionSecretName = Convert.ToString(builder.Configuration["KeyVault:ADSMongoDBConnectionSecretName"]);
var azureADAppClientId= Convert.ToString(builder.Configuration["KeyVault:AzureADAppClientIdSecretName"]);
var azureADAppClientSecret= Convert.ToString(builder.Configuration["KeyVault:AzureADAppClientSecretName"]);
var emailExchangePassword = Convert.ToString(builder.Configuration["KeyVault:EmailExchangePasswordSecretName"]);
var emailExchangeUsername = Convert.ToString(builder.Configuration["KeyVault:EmailExchangeUsernameSecretName"]);

var client = new SecretClient(keyVaultEndpoint, new ClientSecretCredential(
    tenantId: tenantId,
    clientId: clientId,
    clientSecret: clientSecret));

builder.Configuration["AzureAdImportApp:ClientId"] =
  Convert.ToString(client.GetSecretAsync(azureADAppClientId).Result.Value.Value);

builder.Configuration["AzureAdImportApp:ClientSecret"] =
  Convert.ToString(client.GetSecretAsync(azureADAppClientSecret).Result.Value.Value);

builder.Configuration["ConnectionStrings:ADSMongoDBConnection"] =
  Convert.ToString(client.GetSecretAsync(ADSMongoDBConnectionSecretName).Result.Value.Value);

builder.Configuration["EmailExchangeSetting:Password"] =
  Convert.ToString(client.GetSecretAsync(emailExchangePassword).Result.Value.Value);
builder.Configuration["EmailExchangeSetting:UserName"] =
  Convert.ToString(client.GetSecretAsync(emailExchangeUsername).Result.Value.Value);

var allowedOrigion = builder.Configuration["AllowedOrigions"];

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.AzureBlobStorage(connectionString: Convert.ToString(client.GetSecretAsync(blobConnectionSecretName).Result.Value.Value),
                                storageFileName: "ScheduleJobs/{yyyy}/{MM}/{dd}/log.json"));

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services
   .AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("camelCase", conventionPack, t => true);
string? connectionString = builder.Configuration["ConnectionStrings:ADSMongoDBConnection"];
string? databaseName = builder.Configuration["MongoDbName"];

var fileShareConnectionSecretName = Convert.ToString(builder.Configuration["KeyVault:AzureFileShareConnectionSecretName"]);
var fileShareName = builder.Configuration["AzureFileShare"]; 

builder.Configuration["ConnectionStrings:AzureFileShareConnection"] =
Convert.ToString(client.GetSecretAsync(fileShareConnectionSecretName).Result.Value.Value);

var fileShareConnectionString = builder.Configuration["ConnectionStrings:AzureFileShareConnection"];

builder.Services.RegisterDependencies(connectionString, databaseName, builder.Configuration, fileShareConnectionString,fileShareName);
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins(allowedOrigion.Split(","))
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
builder.Services.Configure<AzureAdImportApp>(builder.Configuration.GetSection("AzureAdImportApp"));
builder.Services.Configure<EmailExchangeSetting>(builder.Configuration.GetSection("EmailExchangeSetting"));
var IsSwaggerEnabled = builder.Configuration.GetValue<bool>("IsSwaggerEnabled");
if (IsSwaggerEnabled)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SupportNonNullableReferenceTypes();
        c.SwaggerDoc("v1", new OpenApiInfo { Title = " Healthcare.APIs", Version = "v1" });
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


    });

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Authorization header using the Bearer scheme."
        });
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
            {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
            }
            });
    });
}
var app = builder.Build();


app.ConfigureExceptionHandler();


if (IsSwaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
