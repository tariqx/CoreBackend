using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreBackend.Model;
using CoreBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CoreBackend
{
    public class Startup
    {
       public static IHttpContextAccessor _httpContext { get; private set; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //register jwt authentication 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //initialize/config jwt token parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["TokenSpecs:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenSpecs:Key"])),
                    AudienceValidator = ValidateAudience //custom audience validator handler
                };
                //wire jwt hooks 
                options.Events = new JwtBearerEvents()
                {
                    //anytime there is an issue in authentication process_
                    //return custom message 
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        //in case token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.WriteAsync("Token Expired").Wait();
                        }
                        return Task.CompletedTask;
                    },
                    //on any incoming messages check for custom prefix
                    OnMessageReceived = context =>
                    {
                        //grab current auhorization header from the current httpcontext
                        string authorization = context.Request.Headers["Authorization"];

                        // If no authorization header found, nothing to process further
                        if (string.IsNullOrEmpty(authorization))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                        //check if it starts with prefix "CoreAPI" 
                        if (authorization.StartsWith("CoreAPI ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = authorization.Substring("CoreAPI ".Length).Trim();
                        }

                        // If no token found, authentication failed.
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                        return Task.CompletedTask;
                    },
                    //customize message before sent back to caller/UI
                    OnChallenge = ctx =>
                    {
                        if (ctx.Response.StatusCode != 401)
                        {
                            ctx.Response.StatusCode = 401;
                            ctx.Response.WriteAsync("Unauthorized").Wait();
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //registers our DBContext via dependency injection
            services.AddDbContext<ProductDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //add proper support for our JSON formated data
            services.AddControllers().AddNewtonsoftJson();
            //Dependency Inject our product service
            services.AddScoped<IProductsService, ProductsService>();
            //register httpcontext 
            services.AddHttpContextAccessor();

            //add support for CORS 
            services.AddCors(o => o.AddPolicy("corsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContext)
        {
            //initialize current context
            _httpContext = httpContext;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Custom audience validation handler. 
        /// </summary>
        /// <param name="audience"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>

        private bool ValidateAudience(IEnumerable<string> audience, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            //if requesterCheck==flag allows testing of api 
            bool requesterCheck = Convert.ToBoolean(Configuration["TokenSpecs:RequesterCheck"]);

            //check if the requester flag true or not
            if (requesterCheck)
            {
                //capture the origin header to compare where the call is coming from
                var headers = _httpContext.HttpContext.Request.Headers;
                var currAudience = headers["Origin"];

                if (!string.IsNullOrEmpty(currAudience))
                {
                    if(currAudience == audience)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //if is false that just let it pass through
                return true;
            }
            //default is false
            return false;
        }
    }
}
