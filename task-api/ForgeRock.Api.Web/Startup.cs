using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForgeRock.Api.Web.Data;
using ForgeRock.Api.Web.Domain;
using ForgeRock.Api.Web.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;

namespace ForgeRock.Api.Web
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
            var redisConnectionString = Configuration.GetValue<string>("RedisConnectionString");
            services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(redisConnectionString));

            services.AddTransient(c =>
            {
                var m = c.GetRequiredService<IRedisClientsManager>();
                return m.GetClient();
            });

            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<ITaskRepository, RedisTaskRepository>();
            services.AddTransient<IJobRepository, RedisJobRepository>();

            services.AddSingleton(c => {
                var clientManager = c.GetRequiredService<IRedisClientsManager>();
                return clientManager.GetClient();
            });
            services.AddSingleton<IMessageProducer, RedisMessageProducer>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
