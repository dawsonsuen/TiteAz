using System;

namespace TiteAz.Api.Models
{
    public class BillInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //This is an edge case forget it for now and move on
        //public Payment[] Payments { get; set; }
        public Guid PaidByUserId { get; set; }
        public Guid[] People { get; set; }
        public SplitType SplitType { get; set; }
        public decimal Total { get; set; }

    }

    public enum SplitType
    {
        Equally,
        Custom
    }


    public class Payment
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}