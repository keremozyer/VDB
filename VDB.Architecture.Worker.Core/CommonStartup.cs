using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using VDB.Architecture.Concern.Options;

namespace VDB.Architecture.Worker.Core
{
    public static class CommonStartup
    {
        public static void ConfigureAutoMapper(IServiceCollection services, Profile mappingProfile)
        {
            MapperConfiguration mapperConfig = new(config =>
            {
                config.AddProfile(mappingProfile);
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureRedis(IServiceCollection services, IConfiguration config)
        {
            RedisSettings redisSettings = config.GetSection(nameof(RedisSettings)).Get<RedisSettings>();
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisSettings.Host);
            services.AddSingleton(s => redis.GetDatabase());
        }
    }
}
