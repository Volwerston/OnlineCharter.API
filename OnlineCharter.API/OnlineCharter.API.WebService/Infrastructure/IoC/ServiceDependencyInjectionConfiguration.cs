using System.Data;
using System.Data.SqlClient;
using DataSource.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using Persistence;
using Persistence.Models;
using Services.Implementations;
using Services.Interfaces;
using Template.Interfaces;

namespace OnlineCharter.API.WebService.Infrastructure.IoC
{
    using Settings = Settings.Settings;

    public static class ServiceDependencyInjectionConfiguration
    {
        public static void RegisterDependencies(
            IServiceCollection services,
            Settings settings)
        {
            RegisterInfrastructure(services, settings);

            RegisterServices(services);
        }

        private static void RegisterInfrastructure(
            IServiceCollection services,
            Settings settings)
        {
            // MongoDB
            services.AddScoped(sp => new MongoClient(settings.MongoDbConnectionString));
            services.AddScoped(sp => new DocumentDbContext(
                sp.GetService<MongoClient>(),
                settings.MongoDbName));
            
            // Azure Storage
            services.AddScoped(sp =>
            {
                var sa = CloudStorageAccount.Parse(settings.AzureStorageConnectionString);
                return sa.CreateCloudBlobClient();
            });

            // SQL
            services.AddScoped<IDbConnection>(sp => 
                new SqlConnection(settings.SqlDbConnectionString));

            // Repositories
            services.AddScoped<IDataSourceRepository>(sp => new DataSourceRepository(
                sp.GetService<DocumentDbContext>(),
                sp.GetService<CloudBlobClient>(),
                settings.AzureStorageBlobContainerPath));

            services.AddScoped<IDataSourceUploadProcessRepository, DataSourceUploadProcessRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IDataSourceSchemaGenerator, DataSourceSchemaGenerator>();
            services.AddScoped<IDataSourceOrchestrator, DataSourceOrchestrator>();

            services.AddScoped<ITemplateService, TemplateService>();
        }
    }
}
