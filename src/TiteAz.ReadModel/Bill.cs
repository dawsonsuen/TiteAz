using System;
using NEvilES.Pipeline;
using TiteAz.Domain;

namespace TiteAz.ReadModel
{
    public class Bill
    {

        public Guid Id { get; set; }
        public string Description { get; set; }



        public class Projector :
            IProject<Domain.Bill.Created>
        {
            public void Project(Domain.Bill.Created message, ProjectorData data)
            {

            }
        }
    }
}