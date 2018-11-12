using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Data;
using ServiceStack.Redis;

namespace Server
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
            var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");       
            services.AddSingleton<IRedisClientsManager> (c =>
                new RedisManagerPool($"{redisHost}:{redisPort}"));                 

            var pgUser = Environment.GetEnvironmentVariable("PGUSER");
            var pgHost = Environment.GetEnvironmentVariable("PGHOST");
            var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
            var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");
            var pgPort = Environment.GetEnvironmentVariable("PGPORT");
            services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(options 
                => options.UseNpgsql($"Server={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword}"));
            services.AddTransient<DatabaseService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DatabaseService databaseService)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.1
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });            
            app.UseMvc();
            databaseService.InitializeDatabase();
        }
    }
}
