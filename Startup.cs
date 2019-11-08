using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Attributes;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureAutoDI(services);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        public void ConfigureAutoDI(IServiceCollection services)
        {
            var assembly = typeof(Startup).Assembly.ExportedTypes;
            var types = assembly
                .Where(x => !x.IsAbstract && !x.IsInterface);

            foreach (var type in types)
            {
                if (type.GetCustomAttributes(typeof(SingletonAttribute), true).Length > 0)
                {
                    if (type.GetInterface($"I{type.Name}") != null)
                    {
                        services.AddSingleton(type.GetInterface($"I{type.Name}"), type);
                    }
                    else
                    {
                        services.AddSingleton(type);
                    }
                }
                else if (type.GetCustomAttributes(typeof(ScopedAttribute), true).Length > 0)
                {
                    if (type.GetInterface($"I{type.Name}") != null)
                    {
                        services.AddScoped(type.GetInterface($"I{type.Name}"), type);
                    }
                    else
                    {
                        services.AddScoped(type);
                    }
                }
                else if (type.GetCustomAttributes(typeof(TransientAttribute), true).Length > 0)
                {
                    if (type.GetInterface($"I{type.Name}") != null)
                    {
                        services.AddTransient(type.GetInterface($"I{type.Name}"), type);
                    }
                    else
                    {
                        services.AddTransient(type);
                    }
                }
            }

            //services.AddTransient<IExemploService, ExemploService>();
            //services.AddSingleton<IExemploRepository, ExemploRepository>();
        }
    }
}
