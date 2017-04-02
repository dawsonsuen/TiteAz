using System;
using NEvilES.Pipeline;
using TiteAz.Domain;

namespace TiteAz.ReadModel
{
    public class Debt
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
            private readonly object _writer;
            // public Projector(IWriteData writer)
            // {
            //     _writer = writer;
            // }
            public void Project(Domain.Debt.Created message, ProjectorData data)
            {

                throw new NotImplementedException();
            }
        }
    }
}