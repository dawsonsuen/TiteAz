using System;
using NEvilES.Pipeline;
using TiteAz.Common;
using TiteAz.Domain;

namespace TiteAz.ReadModel
{
    public class Debt : IHaveIdentity
    {
        public Guid Id { get; set; }

        public Guid BillId { get; set; }
        public Guid DebitUserId { get; set; }
        public Guid CreditUserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }


        public class Projector :
            IProject<Domain.Debt.Created>
        {
            private readonly IWriteReadModel _writer;

            public Projector(IWriteReadModel writer)
            {
                _writer = writer;
            }
            public void Project(Domain.Debt.Created message, ProjectorData data)
            {

                _writer.Insert(new Debt
                {
                    Id = message.StreamId,
                    BillId = message.BillId,
                    DebitUserId = message.DebitUserId,
                    CreditUserId = message.CreditUserId,
                    Amount = message.Amount,
                    Status = "Created"
                });
            }
        }
    }
}