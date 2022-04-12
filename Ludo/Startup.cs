using Data;
using Ludo.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Models.Ludo;
using Services;
using Services.CacheManager;
using Services.Common;
using Services.Ludo;
using Services.Jwt;
using System;
using System.Linq;
using System.Text;

namespace Ludo
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            //Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).ConfigureApiBehaviorOptions(options =>
            //{
            //    options.InvalidModelStateResponseFactory = actionContext =>
            //    {
            //        return CustomErrorResponse(actionContext);
            //    };

            //});
            services.AddSingleton<RedisCon>();
            services.AddSingleton<IRedis, Redis>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<Config>();
            services.AddSingleton<ErrorLog>();
            services.AddSingleton<ErrorLogService>();
            services.AddSingleton<HttpHelper>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISqlClient, SqlClient>();
            services.AddSingleton<ILudoService, LudoService>();
            services.AddControllers();


            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = Configuration.GetValue<string>("RedisString");
            //    options.InstanceName = Configuration.GetValue<string>("db");
            //});
            //services.AddDistributedRedisCache(option => option.Configuration = Configuration.GetValue<string>("RedisString"));
            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                        ValidIssuer = jwtOptions.Issuer,
                        ValidateIssuer = true,

                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.Configure<JwtOptions>(jwtSection);
            services.AddMvc();
            //services.AddSingleton(Configuration);

        }
        //private ObjectResult CustomErrorResponse(ActionContext actionContext)
        //{
        //    if (actionContext != null && actionContext.ModelState != null)
        //    {
        //        var dd = actionContext.ModelState
        //     .Where(modelError => modelError.Value.Errors.Count > 0)
        //     .Select(modelError => new
        //     {
        //         msg = modelError.Value.Errors.FirstOrDefault().ErrorMessage
        //     }).ToList();
        //        if (dd.LastOrDefault().msg.Contains("JSON"))
        //            return new ObjectResult(new GlobalValErr { status = 400, msg = dd.LastOrDefault().msg.ToString() });
        //        else
        //            return new ObjectResult(new GlobalValErr { status = 100, msg = dd.LastOrDefault().msg.ToString() });
        //    }
        //    return null;
        //    //return new ObjectResult(actionContext.ModelState
        //    // .Where(modelError => modelError.Value.Errors.Count > 0)
        //    // .Select(modelError => new
        //    // {
        //    //     status = 100,
        //    //     msg = modelError.Value.Errors.FirstOrDefault().ErrorMessage
        //    // }).ToList());
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    var controller = context.Request.RouteValues["controller"];
            //    await next();
            //});
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
