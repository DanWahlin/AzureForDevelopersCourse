using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureForDevelopersCourse.Model;
using AzureForDevelopersCourse.Repository;
using AzureForDevelopersCourse.Repository.BlobStorage;
using AzureForDevelopersCourse.Repository.Queues;
using AzureForDevelopersCourse.Repository.TableStorage;
using AzureForDevelopersCourse.Repository.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AzureForDevelopersCourse.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using AzureForDevelopersCourse.Repository.CosmosDB;

namespace AzureForDevelopersCourse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentsDbContext>(options =>
            {   
                var connStr = Configuration.GetConnectionString("SqlServerConnectionString");
                options.UseSqlServer(connStr);
            });

            services.AddScoped<IAzureTableStorage<Customer>>(factory =>
            {
                return new AzureTableStorage<Customer>(GetAzureSettings());
            });

            services.AddScoped<IAzureBlobStorage>(factory =>
            {
                return new AzureBlobStorage(GetAzureSettings());
            });

            services.AddScoped<IAzureQueuesStorage>(factory =>
            {
                return new AzureQueuesStorage(GetAzureSettings());
            });

            services.AddScoped<IAzureServiceBus>(factory =>
            {
                return new AzureServiceBus(GetAzureSettings());
            });

            services.AddScoped<ICosmosDBRepository<Movie>>(factory =>
            {
                return new CosmosDBRepository<Movie>(GetCosmosDBSettings());
            });

            services.AddScoped<IStudentsRepository, StudentsRepository>();
            services.AddScoped<StudentsDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
        }

        private AzureSettings GetAzureSettings() {
            return new AzureSettings(
                storageAccount: Configuration["Azure:Storage:Account"],
                storageKey: Configuration["Azure:Storage:Key"],
                serviceBusConnStr: Configuration["Azure:ServiceBus:ConnectionString"],
                serviceBusQueueName: Configuration["Azure:ServiceBus:QueueName"]);
        }

        private CosmosDBSettings GetCosmosDBSettings() {
            return new CosmosDBSettings(
                uri: Configuration["Azure:CosmosDB:URI"],
                key: Configuration["Azure:CosmosDB:Key"],
                databaseName: Configuration["Azure:CosmosDB:DatabaseName"],
                containerName: Configuration["Azure:CosmosDB:ContainerName"]);
        }
    }
}
