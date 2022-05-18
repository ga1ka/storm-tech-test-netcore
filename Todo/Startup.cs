using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Services;
using System;
using Polly.Extensions.Http;
using Polly;
using System.Net.Http;
using Polly.Timeout;

namespace Todo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllers();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            AddGravatar(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }

        private void AddGravatar(IServiceCollection services)
        {
            var retryPolicy = HttpPolicyExtensions
              .HandleTransientHttpError()
              .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner execution times out
              .RetryAsync(3);

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

            services.AddHttpClient<GravatarApiClient>(x =>
            {
                x.BaseAddress = new Uri("https://gravatar.com/");
                x.DefaultRequestHeaders.UserAgent.ParseAdd("ga1ka-storm-tech-test");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(2))
             .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);
        }
    }
}
