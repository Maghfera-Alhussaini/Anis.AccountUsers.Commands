using Anis.AccountUsers.Commands.Domain.Commands;
using MediatR;

namespace Anis.AccountUsers.Commands.Application.Features.AssignUserToAccount
{
    public record AssignUserToAccountCommand(Guid AccountId, Guid UserId) : IRequest, IAssignUserToAccountCommand;

}
