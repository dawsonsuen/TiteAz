using System;
using NEvilES;

namespace TiteAz.Domain
{
    public abstract class Bill
    {

        public class Created : IEvent
        {
            public Guid StreamId { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
        }


        public class Aggregate : AggregateBase,
             IHandleStatelessEvent<Created>
        {
        }
    }
}