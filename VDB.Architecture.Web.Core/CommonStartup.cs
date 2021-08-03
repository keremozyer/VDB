using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VDB.Architecture.AppException.Manager;
using VDB.Architecture.Concern.GenericValidator;
using VDB.Architecture.Concern.Options;

namespace VDB.Architecture.Web.Core
{
    public static class CommonStartup
    {
        public static void CommonServiceConfiguration(ServiceConfigurationOptions options)
        {
            ApplyAuth(options.Services, options.Config.GetSection(nameof(TokenSettings)).Get<TokenSettings>());

            options.Services
                .AddControllers()
                .AddNewtonsoftJson(options => 
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            options.Services.AddLogging();

            ConfigureRedis(options.Services, options.Config);

            options.Services.AddSingleton(typeof(Validator));
            options.Services.AddSingleton(typeof(ExceptionParser));

            if (options.UseSwagger)
            {
                ConfigureSwagger(options);
            }

            if (options.AutoMapperProfile != null)
            {
                ConfigureAutoMapper(options.Services, options.AutoMapperProfile);
            }
        }

        private static void ConfigureSwagger(ServiceConfigurationOptions options)
        {
            options.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });
        }

        private static void ApplyAuth(IServiceCollection services, TokenSettings settings)
        {
            services
                .AddAuthentication(options => 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SecurityKey)),
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                        TokenDecryptionKey = new X509SecurityKey(new X509Certificate2(settings.EncryptionCertificate.PrivateKeyPath, settings.EncryptionCertificate.PrivateKeyPassword))
                    };
                });
        }

        public static void CommonAppConfiguration(AppConfigurationOptions options)
        {
            if (options.HostEnvironment.IsDevelopment())
            {
                options.App.UseDeveloperExceptionPage();
            }

            options.App.UseExceptionHandler($"/{options.ErrorHandlerEndpoint}");

            options.App.UseRouting();

            options.App.UseAuthentication();
            options.App.UseAuthorization();

            options.App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (options.SwaggerSettings != null)
            {
                ConfigureSwagger(options);
            }
        }

        private static void ConfigureSwagger(AppConfigurationOptions options)
        {
            options.App.UseSwagger(c => 
            {
                c.PreSerializeFilters.Add((swagger, request) => swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = options.SwaggerSettings.Server } } );
            });
            options.App.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(options.SwaggerSettings.SwaggerEndpoint, options.SwaggerSettings.APIName);                
            });
        }

        private static void ConfigureAutoMapper(IServiceCollection services, Profile mappingProfile)
        {
            MapperConfiguration mapperConfig = new(config =>
            {
                config.AddProfile(mappingProfile);
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void ConfigureRedis(IServiceCollection services, IConfiguration config)
        {
            RedisSettings redisSettings = config.GetSection(nameof(RedisSettings)).Get<RedisSettings>();
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisSettings.Host);
            services.AddSingleton(s => redis.GetDatabase());
        }
    }
}
