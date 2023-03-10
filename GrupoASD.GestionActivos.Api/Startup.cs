using GrupoASD.GestionActivos.Api.Models;
using GrupoASD.GestionActivos.Api.Servicios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api
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
            ////Deshabilita la validación automatica
            //services.Configure<ApiBehaviorOptions>(options
            //        => options.SuppressModelStateInvalidFilter = true);

            //configure acces to database
            services.AddDbContext<ActivosASDContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("db")));
            services.AddControllers().AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                     );

            // configure DI for application services
            services.AddScoped<ILogsErrorReposotorio, LogsErrorReposotorio>();
            services.AddScoped<IActivosReposotorio, ActivosReposotorio>();
            services.AddScoped<IEstadosActivosRepositorio, EstadosActivosRepositorio>();
            services.AddScoped<ITipoActivoRepositorio, TipoActivoRepositorio>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {                
                c.SwaggerEndpoint(Configuration.GetValue<string>("PathSwagger"), "API Gestion Activos ASD");             
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
