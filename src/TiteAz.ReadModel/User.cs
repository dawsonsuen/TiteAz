using System;
using NEvilES.DataStore;
using NEvilES.Pipeline;
using TiteAz.Common;
using TiteAz.Domain;

namespace TiteAz.ReadModel
{
    public class User : IHaveIdentity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Created { get; set; }


        public class Projectors :
            IProject<Domain.User.Created>
        {
            private readonly IWriteReadModel _writer;
            public Projectors(IWriteReadModel writer)
            {
                _writer = writer;
            }
            public void Project(Domain.User.Created message, ProjectorData data)
            {
                _writer.Insert(new User
                {
                    Id = message.StreamId,
                    Email = message.Details.Email,
                    FirstName = message.Details.FirstName,
                    LastName = message.Details.LastName,
                    Created = data.TimeStamp
                });
            }
        }
    }
}