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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DatabaseService databaseService)
        {
            app.UseMvc();
            databaseService.InitializeDatabase();
        }
    }
}
