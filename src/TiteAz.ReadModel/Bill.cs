using System;
using NEvilES.Pipeline;
using TiteAz.Common;
using TiteAz.Domain;

namespace TiteAz.ReadModel
{
    public class Bill : IHaveIdentity
    {

        public Guid Id { get; set; }
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public class Projector :
            IProject<Domain.Bill.Created>
        {
            private readonly IWriteReadModel _writer;

            public Projector(IWriteReadModel writer)
            {
                _writer = writer;
            }

            public void Project(Domain.Bill.Created message, ProjectorData data)
            {
                _writer.Insert(new Bill
                {
                    Id = message.StreamId,
                    Description = message.Description,
                    Amount = message.Amount
                });
            }
        }
    }
}