namespace Tactical.DDD.Tests.TestAggregate
{
    public class Assignee : ValueObject
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        
        protected int SomethingProtected { get; set; }
        private int SomethingPrivate { get; set; }

        public Assignee(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}