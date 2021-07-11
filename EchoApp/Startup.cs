using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using EchoApp.Services;
using EchoApp.Services.Impl;
using EchoApp.Persistence.Repositories;
using EchoApp.Persistence.Repositories.Impl;

namespace EchoApp
{
    public class Startup
    {
	public Startup(IWebHostEnvironment env)
	{
	    var builder = new ConfigurationBuilder()
		    .SetBasePath(env.ContentRootPath)
		    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

	    builder.AddEnvironmentVariables();
	    Configuration = builder.Build();
	}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi", Version = "v1" });
            });

	    services.AddAuthentication(options => {
			    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
			    })
	    .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

	    services.AddControllersWithViews(options =>
			    {
			    var policy = new AuthorizationPolicyBuilder()
			    .RequireAuthenticatedUser()
			    .Build();

			    options.Filters.Add(new AuthorizeFilter(policy));
			    });

            services.AddRazorPages(options => {
			    	options.Conventions.AddPageRoute("/EchoList", "");
			    });

	    services.AddSingleton<IEchoService, EchoServiceImpl>();
	    services.AddSingleton<ITranslator, TranslatorImpl>();
	    services.AddSingleton<IEchoRepository, EchoRepositoryImpl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
		// TODO Using these security features will require me to figure out how to share certificates
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

	    // TODO Using these security features will require me to figure out how to share certificates
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

	    app.UseAuthentication();
            app.UseAuthorization();

	    app.UseCookiePolicy(new CookiePolicyOptions {
			    MinimumSameSitePolicy = SameSiteMode.None,
			    Secure = CookieSecurePolicy.Always,
			    });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
