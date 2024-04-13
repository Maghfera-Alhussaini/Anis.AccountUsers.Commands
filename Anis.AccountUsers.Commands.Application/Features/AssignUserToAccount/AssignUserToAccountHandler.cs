using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Application.Services.BaseServices;
using Anis.AccountUsers.Commands.Domain.Models;
using MediatR;
using System.Security.Principal;

namespace Anis.AccountUsers.Commands.Application.Features.AssignUserToAccount
{
    public class AssignUserToAccountHandler : IRequestHandler<AssignUserToAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommitEventService _commitEventService;

        public AssignUserToAccountHandler(IUnitOfWork unitOfWork, ICommitEventService commitEventService)
        {
            _unitOfWork = unitOfWork;
            _commitEventService = commitEventService;
        }
        public async Task Handle(AssignUserToAccountCommand command, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.Events.GetAllByAggregateIdAsync(command.AccountId, cancellationToken);
            Account account;

            if (events.Any())
            {
                account = Account.LoadFromHistory(events);

                account.AssignUser(command);
            }
            else
            {
                account = Account.Create(command);
            }

            await _commitEventService.CommitNewEventsAsync(account);
         
        }
    }
}
