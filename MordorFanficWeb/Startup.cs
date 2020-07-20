using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MordorFanficWeb.Models;
using MordorFanficWeb.Persistence.AppDbContext;
using MordorFanficWeb.ViewModels;
using MordorFanficWeb.ViewModels.FluentValidation;
using MordorFanficWeb.BusinessLogic.Services;
using MordorFanficWeb.BusinessLogic.Interfaces;
using System;
using MordorFanficWeb.PresentationAdapters.AccountAdapter;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MordorFanficWeb.Common.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MordorFanficWeb.Common.Helper;
using MordorFanficWeb.BusinessLogic.Policies;
using Microsoft.AspNetCore.Authorization;
using MordorFanficWeb.ViewModels.CompositionViewModels;
using MordorFanficWeb.ViewModels.CompositionViewModels.FluentValidation;
using MordorFanficWeb.PresentationAdapters.CompositionAdapter;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using MordorFanficWeb.ViewModels.ChapterViewModels.FluentValidation;
using MordorFanficWeb.PresentationAdapters.ChapterAdapter;

namespace MordorFanficWeb
{
    public class Startup
    {
        private const string SecretKey = "ugWlbE2f9-uyfxAjZuaSIoQORme42UW937LbvkxDT8mBISZaD7XTWOLDMLiY679AJFJZdbKxeweM";
        private readonly SymmetricSecurityKey signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MordorFanficWeb")));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            var jwtAppSettingsOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccountUser", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess);
                });

                options.AddPolicy("Admin", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(Constants.Strings.JwtClaimIdentifiersAdmin.Rol, Constants.Strings.JwtClaims.ApiAccess);
                });

                options.AddPolicy("RegisteredUsers", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new RegisteredUserRequirement());
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)],

                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signinKey,

                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddIdentity<AppUser, IdentityRole>(
                 options =>
                 {
                     options.Password.RequireDigit = true;
                     options.Password.RequireLowercase = true;
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequireUppercase = true;
                     options.Password.RequiredLength = 6;
                     options.Password.RequiredUniqueChars = 1;

                     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                     options.Lockout.MaxFailedAccessAttempts = 5;
                     options.Lockout.AllowedForNewUsers = true;

                     options.User.AllowedUserNameCharacters =
                     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                     options.User.RequireUniqueEmail = true;
                 })
                 .AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders();

            services.AddSingleton<IAuthorizationHandler, UserRegisteredHandler>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountAdapter, AccountAdapter>();
            services.AddScoped<ICompositionService, CompositionService>();
            services.AddScoped<ICompositionAdapter, CompositionAdapter>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IChapterAdapter, ChapterAdapter>();
            services.AddScoped<IAppDbContext, AppDbContext>();

            services.AddTransient<IValidator<ChangeUserPasswordViewModel>, ChangeUserPasswordValidator>();
            services.AddTransient<IValidator<RegistrationViewModel>, RegistrationViewModelValidator>();
            services.AddTransient<IValidator<UpdateUserViewModel>, UpdateUserViewModelValidator>();
            services.AddTransient<IValidator<CredentialsViewModel>, CredentialsViewModelValidator>();
            services.AddTransient<IValidator<CompositionViewModel>, CompositionViewModelValidator>();
            services.AddTransient<IValidator<ChapterViewModel>, ChapterViewModelValidator>();

            services.AddSingleton(Common.AutoMapper.AutoMapper.Configure());

            services.AddMvc(
                options => options.EnableEndpointRouting = false)
                .AddFluentValidation();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();
            app.UseMvc();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
