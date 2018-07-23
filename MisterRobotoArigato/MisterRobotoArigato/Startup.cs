using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MisterRobotoArigato.Controllers;
using MisterRobotoArigato.Data;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.Handlers;
using Newtonsoft.Json.Linq;

namespace MisterRobotoArigato
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(google =>
            {
                google.ClientId = Configuration["Authentication:Google:ClientId"];
                google.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
                microsoftOptions.CallbackPath = new PathString("/signin-microsoft");
            });

            services.AddDbContext<RobotoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection2")));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(ApplicationRoles.Admin));
                options.AddPolicy("IsDoge", policy => policy.Requirements.Add(new IsDogeRequirement("doge")));
            });

            services.AddScoped<IRobotoRepo, DevRobotoRepo>();
            services.AddScoped<IBasketRepo, DevBasketRepo>();
            services.AddScoped<ICheckoutRepo, DevCheckoutRepo>();

            services.AddSingleton<IAuthorizationHandler, IsDogeHandler>();
            services.AddScoped<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            //app.UseMvc(route =>
            //{
            //    route.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            //app.Run((context) =>
            //{
            //    context.Response.Redirect("/");
            //    return Task.FromResult<object>(null);
            //});

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("errrrrrooooooooooooor!");
            });
        }
    }
}
