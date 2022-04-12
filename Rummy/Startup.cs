using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Rummy.Helpers;
using Services;
using Services.CacheManager;
using Services.Common;
using Services.Jwt;
using Services.Rummy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rummy
{
    public class Startup
    {
        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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

            //services.AddCors(options =>
            //{
            //    //options.AddPolicy("Policy1",
            //    //    builder =>
            //    //    {
            //    //        builder.WithOrigins("https://test555.worldserviceprovider.com",
            //    //                            "http://www.contoso.com");
            //    //    });

            //    options.AddPolicy("AnotherPolicy",
            //        builder =>
            //        {
            //            builder.WithOrigins("35.177.165.206")
            //                                .AllowAnyHeader()
            //                                .AllowAnyMethod();
            //        });
            //});


            // Register Handler

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
            services.AddSingleton<IRummyService, RummyService>();
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
            //    services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            //    var applicationOptions = Configuration
            //.GetSection("ApplicationOptions")
            //.Get<ApplicationOptions>();

            //    // Register Policy
            //    services.AddAuthorization(options =>
            //    {
            //        options.AddPolicy("RestrictIP", policy =>
            //            policy.Requirements.Add(new IPRequirement(applicationOptions)));

            //    });
            //    services.AddSingleton<IAuthorizationHandler, IPAddressHandler>();
            //services.AddSingleton(Configuration);

        }
        public class ApplicationOptions
        {
            public List<string> Whitelist { get; set; }
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
            app.UseStaticFiles();

            //app.UseMiddleware<AdminSafeListMiddleware>(Configuration["AdminSafeList"]);
            app.UseAuthorization();
            app.UseCors("AnotherPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
