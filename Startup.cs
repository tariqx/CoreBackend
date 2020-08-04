using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreBackend.Model;
using CoreBackend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreBackend
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
            //add proper support for our JSON formated data
            services.AddControllers().AddNewtonsoftJson();
            //Dependency Inject our product servce
            services.AddSingleton<IProductsService, ProductsService>();

            //add support for CORS 
            services.AddCors(o => o.AddPolicy("corsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("corsPolicy");

            //add exception handler just in case if anything goes wrong in early lifecycle of our app
            app.UseExceptionHandler(
               options =>
               {
                   options.Run(
                       async context =>
                       {
                           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           context.Response.ContentType = "text/html";
                           var ex = context.Features.Get<IExceptionHandlerFeature>();
                           if (ex != null)
                           {
                               var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                               await context.Response.WriteAsync(err).ConfigureAwait(false);
                           }
                       });
               }
           );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
