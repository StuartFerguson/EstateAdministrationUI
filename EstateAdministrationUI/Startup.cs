using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EstateAdministrationUI
{
    using System.IdentityModel.Tokens.Jwt;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    using Shared.General;
    using TokenManagement;

    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(webHostEnvironment.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                      .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<CookiePolicyOptions>(options =>
                                                    {
                                                        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                                                        options.CheckConsentNeeded = context => true;
                                                        options.MinimumSameSitePolicy = SameSiteMode.None;
                                                    });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "mvchybridautorefresh";
            }).AddAutomaticTokenManagement().AddOpenIdConnect("oidc",
                                                                                                      options =>
                                                                                                      {
                                                                                                          options.SignInScheme = "Cookies";
                                                                                                          options.Authority = ConfigurationReader.GetValue("Authority");

                                                                                                          options.RequireHttpsMetadata = false;

                                                                                                          options.ClientSecret =
                                                                                                              ConfigurationReader.GetValue("ClientSecret");
                                                                                                          options.ClientId = ConfigurationReader.GetValue("ClientId");

                                                                                                          options.ResponseType = "code id_token";

                                                                                                          options.Scope.Clear();
                                                                                                          options.Scope.Add("openid");
                                                                                                          options.Scope.Add("profile");
                                                                                                          options.Scope.Add("email");
                                                                                                          options.Scope.Add("offline_access");

                                                                                                          options.ClaimActions.MapAllExcept("iss",
                                                                                                                                            "nbf",
                                                                                                                                            "exp",
                                                                                                                                            "aud",
                                                                                                                                            "nonce",
                                                                                                                                            "iat",
                                                                                                                                            "c_hash");

                                                                                                          options.GetClaimsFromUserInfoEndpoint = true;
                                                                                                          options.SaveTokens = true;

                                                                                                          options.TokenValidationParameters = new TokenValidationParameters
                                                                                                                                              {
                                                                                                                                                  NameClaimType = JwtClaimTypes.Name,
                                                                                                                                                  RoleClaimType = JwtClaimTypes.Role,
                                                                                                                                                  ValidateIssuer = false
                                                                                                                                              };
                                                                                                      });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigurationReader.Initialise(Startup.Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapAreaControllerRoute("Account", "Account", "Account/{controller=Home}/{action=Index}/{id?}");
                                 endpoints.MapAreaControllerRoute("Estate", "Estate", "Estate/{controller=Home}/{action=Index}/{id?}");

                                 endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");


            });

            
        }
    }
}
