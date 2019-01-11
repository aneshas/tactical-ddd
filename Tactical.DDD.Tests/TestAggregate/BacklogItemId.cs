using System;

namespace Tactical.DDD.Tests.TestAggregate
{
    public class BacklogItemId : DomainIdentity
    {
        private readonly Guid _id;

        public override string Identity => _id.ToString();

        public BacklogItemId()
        {
            _id = Guid.NewGuid();
        }

        public BacklogItemId(Guid id)
        {
            _id = id;
        }
    }
}