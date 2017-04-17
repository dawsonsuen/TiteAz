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
            IProject<Domain.Debt.Created>,
            IProject<Domain.Debt.Accepted>
        {
            private readonly IWriteReadModel _writer;
            private readonly IReadFromReadModel _reader;

            public Projector(IReadFromReadModel reader, IWriteReadModel writer)
            {
                _writer = writer;
                _reader = reader;
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

            public void Project(Domain.Debt.Accepted message, ProjectorData data)
            {
                var debt = _reader.Get<Debt>(message.StreamId);

                debt.Status = DebtStatus.Accepted.ToString();

                _writer.Update(debt);
            }
        }
    }
}