using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFlogger;
using CoreFlogger.Middleware;
using CoreTodosMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTodosMVC
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
      services.AddDbContext<ToDoDbContext>(options =>
          options.UseSqlServer(Environment.GetEnvironmentVariable("TODO_CONNSTR")));

      // Securing ASP.NET Core with OAuth2 and OpenID Connect by Kevin Dockx
      services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      }).AddCookie()
      .AddOpenIdConnect(options => {
        options.Authority = Environment.GetEnvironmentVariable("AUTHORITY");
        options.ClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        options.ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

        options.SaveTokens = true;
        options.RequireHttpsMetadata = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code id_token";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api");
        options.Scope.Add("email");
        options.Scope.Add("offline_access");

        options.ClaimActions.Remove("amr");
      });

      //services.Configure<CookiePolicyOptions>(options =>
      //{
      //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
      //        options.CheckConsentNeeded = context => true;
      //  options.MinimumSameSitePolicy = SameSiteMode.None;
      //});


      services.AddMvc(options => 
            options.Filters.Add(new TrackPerformanceFilter("ToDos", "Core MVC"))
        ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseAuthentication();

      // replace whole conditional with something like
      // app.UseCustomerExcetptionHandler("ToDos", "Core MVC", "/Home/Error");
      app.UseCustomExceptionHandler("ToDos", "Core MVC", "/Home/Error");
      //if (env.IsDevelopment())
      //{
      //  app.UseDeveloperExceptionPage();
      //  app.UseBrowserLink();
      //}
      //else
      //{
      //  app.UseExceptionHandler("/Home/Error");
      //  app.UseHsts();
      //}

      //app.UseHttpsRedirection();
      app.UseStaticFiles();
      //app.UseCookiePolicy();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
