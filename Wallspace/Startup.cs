namespace Wallspace
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Wallspace.Infrastructure;
    using Wallspace.Models;
    using Wallspace.Models.Database;

    internal class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<WallspaceDbContext>(options => options.UseNpgsql(_configuration["Data:Identity:ConnectionString"]));

            services.AddIdentity<WallspaceUser, IdentityRole>(options => options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-._@/ ")
                    .AddEntityFrameworkStores<WallspaceDbContext>()
                    .AddDefaultTokenProviders();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "WallspaceClient/dist");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WallspaceClient";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}