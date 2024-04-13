using Anis.AccountUseres.Commands.Domain.Models;
using Anis.AccountUsers.Commands.Domain.Commands;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Exceptions;
using Anis.AccountUsers.Commands.Domain.Extensions;
using Anis.AccountUsers.Commands.Domain.Resourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.AccountUsers.Commands.Domain.Models
{
    public class Account : Aggregate<Account>
    {
        private HashSet<Guid> _users = new();
        public IReadOnlyCollection<Guid> Users => _users;
        public static Account Create(IAssignUserToAccountCommand command)
        {
            var account = new Account();

            var @event = command.ToEvent();

            account.ApplyChange(@event);

            return account;
        }
        public void AssignUser(IAssignUserToAccountCommand command)
        {
            if (_users.Any(c => c == command.UserId))
                throw new AppException(ExceptionStatusCode.AlreadyExists, Phrases.UserAlreadyExist);

            var @event = command.ToEvent(Sequence + 1);

            ApplyChange(@event);
        }
        public void DeleteUser(IDeleteUserFromAccountCommand command)
        {
            if (!_users.Any(c => c == command.UserId))
                throw new AppException(ExceptionStatusCode.NotFound, Phrases.UserNotFound);

            var @event = command.ToEvent(Sequence + 1);

            ApplyChange(@event);
        }

        public void Apply(UserAssignedToAccount @event)
        {
            _users.Add(@event.Data.UserId);
        }
        public void Apply(UserDeletedFromAccount @event)
        {
            _users.Remove(@event.Data.UserId);
        }
    }
}
