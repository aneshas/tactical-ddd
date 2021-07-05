using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Tactical.DDD.EventSourcing.Postgres
{
    public static class EventStoreServiceCollectionExtensions
    {
        public static void AddPostgresEventStore(this IServiceCollection services, string connString)
        {
            services.AddTransient(_ => new NpgsqlConnection(connString));
            services.AddSingleton<EventStoreMigrator>();
            services.AddScoped<IEventStore, EventStore>(); 
        }
    }
}