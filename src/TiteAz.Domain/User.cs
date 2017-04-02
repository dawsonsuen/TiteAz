using System;
using NEvilES;

namespace TiteAz.Domain
{
    public abstract class User
    {

        public class Details
        {
            public Details(string email, string password, string firstName, string lastName)
            {
                Email = email;
                Password = password;
                FirstName = firstName;
                LastName = lastName;
            }

            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class NewUser : ICommand
        {
            public Guid StreamId { get; set; }
            public Details Details { get; set; }

        }
        public class Created : NewUser, IEvent
        {
        }

        public class CorrectUserDetails : ICommand
        {
            public Guid StreamId { get; set; }
            public Details Details { get; set; }
        }
        public class UserDetailsCorrected : CorrectUserDetails, IEvent
        {

        }

        public class Aggregate : AggregateBase,
              IHandleAggregateCommand<NewUser, UniqueNameValidator>,
              IHandleAggregateCommand<CorrectUserDetails, UniqueNameValidator>
        {
            public void Handle(NewUser command, UniqueNameValidator uniqueNameValidator)
            {
                if (uniqueNameValidator.Dispatch(command).IsValid)
                    Raise<Created>(command);
            }

            public void Handle(CorrectUserDetails command, UniqueNameValidator uniqueNameValidator)
            {
                if (uniqueNameValidator.Dispatch(command).IsValid)
                    Raise<UserDetailsCorrected>(command);
            }

            //-------------------------------------------------------------------

            private void Apply(Created e)
            {
            }
            private void Apply(UserDetailsCorrected e)
            {
            }
        }

        public class UniqueNameValidator :
            INeedExternalValidation<NewUser>,
            INeedExternalValidation<CorrectUserDetails>
        {
            public CommandValidationResult Dispatch(NewUser command)
            {
                return Validate(command.Details);
            }

            public CommandValidationResult Dispatch(CorrectUserDetails command)
            {
                return Validate(command.Details);
            }

            private CommandValidationResult Validate(Details commandDetails)
            {
                // Check ReadModel
                return new CommandValidationResult(true);
            }
        }
    }
}