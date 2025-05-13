using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Infrastructure.Authentication
{
    internal static class Startup
    {
        internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
        {
            // Register and validate options
            services.AddOptions<GoogleAuthConfig>()
                .Bind(config.GetSection(GoogleAuthConfig.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddDataProtection();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            })
            .AddGoogle(options =>
            {
                var googleOptions = services.BuildServiceProvider()
                    .GetRequiredService<IOptions<GoogleAuthConfig>>().Value;

                if (string.IsNullOrEmpty(googleOptions.ClientId))
                    throw new InvalidOperationException("Google ClientId is missing in configuration");

                if (string.IsNullOrEmpty(googleOptions.ClientSecret))
                    throw new InvalidOperationException("Google ClientSecret is missing in configuration");

                options.ClientId = googleOptions.ClientId;
                options.ClientSecret = googleOptions.ClientSecret;
                options.CallbackPath = googleOptions.CallbackPath;
                options.SaveTokens = googleOptions.SaveTokens;

                //this is only for logging and debugging purposes .. A.Hassan
                options.Events = new OAuthEvents
                {
                    OnRedirectToAuthorizationEndpoint = context =>
                    {
                        Console.WriteLine($"Redirecting to: {context.RedirectUri}");
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    },
                    OnTicketReceived = context =>
                    {
                        Console.WriteLine("Ticket received!");
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        Console.WriteLine($"Remote failure: {context.Failure?.Message}");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        internal static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            return app;
        }
    }
}

