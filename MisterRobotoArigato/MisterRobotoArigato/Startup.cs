using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MisterRobotoArigato.Data;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.Handlers;

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

            //Identity services for our users, claims and roles
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //this is needed to use OAUTH from Google and MS
            //these service requests are chained as suggested by the MS Docs
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

            //Which database to connect to
            services.AddDbContext<RobotoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection2")));

            //policies being enforced by Mister Roboto Arigato
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(ApplicationRoles.Admin));
                options.AddPolicy("IsDoge", policy => policy.Requirements.Add(new IsDogeRequirement("doge")));
            });

            services.AddScoped<IRobotoRepo, DevRobotoRepo>();
            services.AddScoped<IBasketRepo, DevBasketRepo>();
            services.AddScoped<IOrderRepo, DevOrderRepo>();

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

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("errrrrrooooooooooooor!");
            });
        }
    }
}