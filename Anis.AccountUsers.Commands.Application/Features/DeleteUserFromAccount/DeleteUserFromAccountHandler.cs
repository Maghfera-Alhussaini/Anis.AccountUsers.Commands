using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Application.Services.BaseServices;
using Anis.AccountUsers.Commands.Domain.Exceptions;
using Anis.AccountUsers.Commands.Domain.Models;
using Anis.AccountUsers.Commands.Domain.Resourses;
using MediatR;

namespace Anis.AccountUsers.Commands.Application.Features.DeleteUserFromAccount
{
    public class DeleteUserFromAccountHandler : IRequestHandler<DeleteUserFromAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommitEventService _commitEventService;
      

        public DeleteUserFromAccountHandler(IUnitOfWork unitOfWork, ICommitEventService commitEventService)
        {
            _unitOfWork = unitOfWork;
            _commitEventService = commitEventService;
        }
        public async Task Handle(DeleteUserFromAccountCommand command, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.Events.GetAllByAggregateIdAsync(command.AccountId, cancellationToken);
            Account account;

            if (events.Any())
            {
                account = Account.LoadFromHistory(events);

                account.DeleteUser(command);
                await _commitEventService.CommitNewEventsAsync(account);
            }
            else
            {
                throw new AppException(ExceptionStatusCode.NotFound, Phrases.AccountNotFound);
            }



        }
    }
}
