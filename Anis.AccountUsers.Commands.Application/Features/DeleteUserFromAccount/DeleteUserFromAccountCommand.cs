using Anis.AccountUsers.Commands.Domain.Commands;
using MediatR;

namespace Anis.AccountUsers.Commands.Application.Features.DeleteUserFromAccount
{
    public record DeleteUserFromAccountCommand(Guid AccountId, Guid UserId) : IRequest, IDeleteUserFromAccountCommand;

}
