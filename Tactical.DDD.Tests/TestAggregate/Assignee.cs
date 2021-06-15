using System.Collections.Generic;

namespace Tactical.DDD.Tests.TestAggregate
{
    public sealed record Assignee 
    {
        public string Name { get; private set; }
        
        public string DisplayName { get; private set; }

        private int SomethingProtected { get; set; }
        
        private int SomethingPrivate { get; set; }

        public Assignee(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}