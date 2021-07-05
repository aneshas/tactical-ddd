using System;
using System.Threading.Tasks;
using System.Transactions;
using Aperture.Core;

namespace Tactical.DDD.EventSourcing.Postgres.Aperture
{
    public class PostgresProjection : Projection
    {
        private readonly ITrackOffset _offsetTracker;

        protected PostgresProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            using var txScope = new TransactionScope(
                TransactionScopeOption.Required,
                TransactionScopeAsyncFlowOption.Enabled);

            await base.TrackAndHandleEventAsync(projection, eventData);

            txScope.Complete();
        }
    }
}