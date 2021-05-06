using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Trill.Core.Repositories;

namespace Trill.Core.Mongo
{
    internal static class Extensions
    {
        internal static IServiceCollection AddMongo(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetRequiredService<IConfiguration>();
            }
            
            var section = configuration.GetSection("mongo");
            services.Configure<MongoOptions>(section);
            var mongoOptions = new MongoOptions();
            section.Bind(mongoOptions);
            services.AddSingleton(mongoOptions);
            
            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetRequiredService<MongoOptions>();
                return new MongoClient(options.ConnectionString);
            });
            services.AddTransient(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });

            services.AddTransient<IStoryRepository, MongoStoryRepository>();
            
            return services;
        }
    }
}