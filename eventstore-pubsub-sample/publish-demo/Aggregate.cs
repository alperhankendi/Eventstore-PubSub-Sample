using System;
using System.Collections.Generic;

namespace console_app
{
    public abstract class Aggregate
    {
        public Guid Id { get; set; }
        
        public int Version { get; private set; } = -1;
        
        private readonly IList<object> changes = new List<object>();

        public IList<object> GetChanges() => changes;
        
        public void Load(object[] history)
        {
            foreach (var @event in history)
            {
                When(@event);
                
                Console.WriteLine($"Order State=> {ToString()}");
                Version++;
            }
        }

        protected void Apply(object @event)
        {
            When(@event);
            changes.Add(@event);
        }

        protected abstract void When(object @event);
    }
}