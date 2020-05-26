using AuthenricationDbContext;
using BasicAuthentication.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BasicAuthentication
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookiesAuth")
                .AddCookie("CookiesAuth", config =>
                 {
                     config.Cookie.Name = "Grandmas.Cookie";
                     config.LoginPath = "/Home/Authenticate";
                 });

            //this how default policy build up
            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //        .RequireAuthenticatedUser()
                //        .RequireClaim(ClaimTypes.DateOfBirth)
                //        .Build();
                //config.DefaultPolicy = defaultAuthPolicy;


                //config.AddPolicy("Claim.Mile", policyBilder =>
                // {
                //     policyBilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });

                //这里也可以添加认证
                //Controller也可以添加 [Authorize(Roles = "Admin")]
                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.Mile", policyBuilder =>
                 {
                     //policyBilder.AddRequirements(new CustomRequireClaims(ClaimTypes.DateOfBirth));
                     policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                 });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimsHandler>();

            services.AddControllersWithViews();

            var sqlConnection = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CommonDbContext>(config => config.UseSqlServer(sqlConnection));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //who you are?
            app.UseAuthentication();

            //are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
