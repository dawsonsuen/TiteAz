using System;
using NEvilES;

namespace TiteAz.Domain
{
    public abstract class Debt
    {
        public abstract class NewDebt
        {
            protected NewDebt() { }
            protected NewDebt(Guid streamId, Guid debitUserId, Guid creditUserId, Guid billId, decimal amount)
            {
                StreamId = streamId;
                DebitUserId = debitUserId;
                CreditUserId = creditUserId;
                BillId = billId;
                Amount = amount;
            }
            public Guid StreamId { get; set; }
            public Guid DebitUserId { get; set; }
            public Guid CreditUserId { get; set; }

            public Guid BillId { get; set; }
            public decimal Amount { get; set; }
        }

        public class IOweYou : NewDebt, ICommand
        {
            public IOweYou(Guid streamId, Guid me, Guid you, Guid billId, decimal amount) : base(streamId, me, you, billId, amount) { }
        }


        public class YouOweMe : NewDebt, ICommand
        {
            public YouOweMe(Guid streamId, Guid you, Guid me, Guid billId, decimal amount) : base(streamId, you, me, billId, amount) { }
        }

        public class Created : NewDebt, IEvent
        {
        }

        public class Accept : ICommand
        {
            public Accept() { }
            public Accept(Guid streamId)
            {
                StreamId = streamId;
            }
            public Guid StreamId { get; set; }
        }

        public class Accepted : Accept, IEvent
        {
            public Accepted() { }
            public Accepted(Guid streamId) : base(streamId)
            {
            }
        }

        public class Dispute : ICommand
        {
            public Dispute() { }
            public Dispute(Guid streamId, string reason)
            {
                StreamId = streamId;
                Reason = reason;
            }
            public Guid StreamId { get; set; }
            public string Reason { get; set; }
        }

        public class Disputed : Dispute, IEvent
        {
            public Disputed() { }
            public Disputed(Guid streamId, string reason) : base(streamId, reason)
            {
            }
        }

        public class Aggregate : AggregateBase,
            IHandleAggregateCommand<IOweYou>,
            IHandleAggregateCommand<YouOweMe>,
            IHandleAggregateCommand<Accept>,
            IHandleAggregateCommand<Dispute>
        {
            public void Handle(IOweYou command)
            {
                Raise<Created>(command);
            }

            public void Handle(YouOweMe command)
            {
                Raise<Created>(command);
            }

            public void Handle(Accept command)
            {
                if (state == DebtStatus.Cancelled && state != DebtStatus.Created)
                    throw new DomainAggregateException(this, "Can't accept a cancelled request");

                Raise<Accepted>(command);
            }
            public void Handle(Dispute command)
            {
                if (state == DebtStatus.Cancelled && state != DebtStatus.Created)
                    throw new DomainAggregateException(this, "Can't dispute a cancelled request");

                Raise<Disputed>(command);
            }

            //-------------------------------------------------------------------
            private DebtStatus state;
            public void Apply(Created e)
            {
                state = DebtStatus.Created;
            }

            private void Apply(Accepted e)
            {
                state = DebtStatus.Accepted;
            }

            private void Apply(Disputed e)
            {
                state = DebtStatus.Disputted;
            }
        }
    }

    public enum DebtStatus
    {
        Created,
        Accepted,
        Disputted,
        Cancelled
    }
}