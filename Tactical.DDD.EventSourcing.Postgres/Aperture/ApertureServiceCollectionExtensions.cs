using Aperture.Core;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using IPullEventStreamEventStore = Aperture.Core.IEventStore;

namespace Tactical.DDD.EventSourcing.Postgres.Aperture
{
    public static class ApertureServiceCollectionExtensions
    {
        public static void AddPostgresAperture(
            this IServiceCollection services,
            string connString,
            PullEventStream.Config config = default
        )
        {
            services.AddTransient(_ => NpgsqlDataSource.Create(connString));
            services.AddTransient<IPullEventStreamEventStore, EventStore>();
            services.AddSingleton<ITrackOffset, PostgresOffsetTracker>();

            services.AddTransient<IStreamEvents>(
                ctx => new PullEventStream(
                    ctx.GetService<IPullEventStreamEventStore>(), config));
        }
    }
}