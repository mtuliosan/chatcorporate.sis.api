using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using IdentityService.Repository;
using IdentityService.Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IdentityService.Core
{
    /// <summary>
    ///
    /// </summary>
    public static partial class DependencyInjector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterDependencyInjectionContainers(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ISessionRepository, SessionRepository>();
            services.AddSingleton<IAudienceRepository, AudienceRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<BaseRepository>();

            services.AddSingleton<IMailerService, MailerService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAudienceService, AudienceService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
