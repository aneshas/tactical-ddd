using System;
using Aperture.Core;
using Microsoft.Extensions.DependencyInjection;
using IPullEventStreamEventStore = Aperture.Core.IEventStore;

namespace Tactical.DDD.EventSourcing.Postgres.Aperture
{
    public static class ApertureServiceCollectionExtensions
    {
        public static void AddPostgresEventStream(this IServiceCollection services) =>
            services.AddScoped<IPullEventStreamEventStore, EventStore>();

        public static void AddPostgresAperture(
            this IServiceCollection services,
            PullEventStream.Config config = default,
            Func<ApertureAgent, ApertureAgent> configure = null)
        {
            services.AddScoped<PostgresOffsetTracker>();

            services.AddScoped<IStreamEvents>(
                ctx => new PullEventStream(
                    ctx.GetService<IPullEventStreamEventStore>(), config));

            var agent = ApertureAgentBuilder
                .CreateDefault();

            services.AddSingleton(
                ctx =>
                {
                    foreach (var projection in ctx.GetServices<IProjectEvents>())
                        agent.AddProjection(projection);

                    if (configure != null)
                        agent = configure(agent);

                    return agent
                        .UseEventStream(ctx.GetService<IStreamEvents>());
                });
        }
    }
}