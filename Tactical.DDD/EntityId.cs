using System;
using System.Collections.Generic;

namespace Tactical.DDD
{
    public abstract class EntityId : ValueObject, IEntityId
    {
        public abstract override string ToString();

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ToString();
        }
    }
}