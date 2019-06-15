using System;
using System.Collections.Generic;

namespace Tactical.DDD.Tests.TestAggregate
{
    public class BacklogItemId : EntityId 
    {
        private readonly Guid _id;

        public BacklogItemId()
        {
            _id = Guid.NewGuid();
        }

        public BacklogItemId(Guid id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }
    }
}