using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spy_detection.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Core.Web.Filters;
using spy_detection.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using spy_detection.Api;

namespace spy_detection
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<SpyService>();
            services.AddScoped<ISpyRepository, SpyRepository>();
            services.AddSingleton<SpyDetector>();

            ////
            //// Authentication
            ////
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //    .AddCookie(options =>
            //    {
            //        //options.LoginPath = "/auth/login";
            //        //options.LogoutPath = "/auth/logout";
            //        options.Events.OnRedirectToLogin = context =>
            //        {
            //            if (context.Request.Path.Value.StartsWith("/api"))
            //            {
            //                context.Response.Clear();
            //                context.Response.StatusCode = 401;
            //                return Task.FromResult(0);
            //            }
            //            context.Response.Redirect(context.RedirectUri);
            //            return Task.FromResult(0);
            //        };
            //    });

            //
            // Authorization
            //
            services.AddAuthorization(o =>
            {
                o.AddPolicy(Policies.Authenticated, policy => policy.RequireAuthenticatedUser());
            });


            services.AddMvc(options =>
            {
                // options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add<ErrorResponseExceptionFilter>();
            })
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    // opts.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                    opts.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //
            // Swagger support
            //
            //services.AddSwaggerGen(c =>
            //{
            //    // Swagger meta information
            //    c.SwaggerDoc("v1", new Info { Title = Configuration["Application:Title"], Description = Configuration["Application:Description"], Version = "v1" });

            //    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "spy-detection.xml");
            //    c.IncludeXmlComments(filePath);

            //    c.DescribeAllEnumsAsStrings();

            //    c.EnableAnnotations();
            //    c.ExampleFilters();
            //})
            //.AddSwaggerExamples();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // app.UseIdentity();
            app.UseAuthentication();
            
            //
            // Swagger
            //
            app.UseSwagger(opts =>
            {
                opts.RouteTemplate = "spy/swagger/{documentName}/swagger.json";
            })
            .UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("v1/swagger.json", $"{Configuration["Application:Title"]} v1");
                opts.RoutePrefix = "spy/swagger";
            });

            app.UseMvc();
        }
    }

    public class Policies
    {
        public const string Authenticated = "Authenticated";
    }
}
