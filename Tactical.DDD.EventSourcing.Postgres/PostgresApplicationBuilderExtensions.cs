using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tactical.DDD.EventSourcing.Postgres
{
    public static class PostgresApplicationBuilderExtensions
    {
        public static void EnsurePostgresEventStoreCreated(this IApplicationBuilder app)
        {
            var svc = app.ApplicationServices.GetRequiredService<EventStoreMigrator>();

            svc.EnsureEventStoreCreated();
        }
    }
}