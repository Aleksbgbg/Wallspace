namespace Wallspace
{
    using System;
    using System.Text;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    using Swashbuckle.AspNetCore.Swagger;

    using Wallspace.Infrastructure;
    using Wallspace.Infrastructure.Jwt;
    using Wallspace.Models;
    using Wallspace.Models.Database;
    using Wallspace.Services;

    internal class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
                    {
                        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                        options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<WallspaceDbContext>(options => options.UseNpgsql(_configuration["Data:Identity:ConnectionString"]));

            services.AddIdentity<WallspaceUser, IdentityRole>(options => options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-._@/ ")
                    .AddEntityFrameworkStores<WallspaceDbContext>()
                    .AddDefaultTokenProviders();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "WallspaceClient/dist");

            services.AddTransient<IJwtService, JwtService>();

            IConfigurationSection jwtOptions = _configuration.GetSection(nameof(JwtIssuerOptions));

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["SecurityKey"]));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)],
                        ValidateAudience = true,
                        ValidAudience = jwtOptions[nameof(JwtIssuerOptions.Audience)],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    });

            services.AddSwaggerGen(options => options.SwaggerDoc("v1",
                                                                 new Info
                                                                 {
                                                                     Title = "API",
                                                                     Version = "v1"
                                                                 }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

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