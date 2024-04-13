using Anis.AccountUsers.Commands.Application.Features.AssignUserToAccount;
using Anis.AccountUsers.Commands.Application.Features.DeleteUserFromAccount;
using Anis.AccountUsers.Commands.Grpc.Protos;

namespace Anis.AccountUsers.Commands.Grpc.Extensions
{
    public static class CommandExtensions
    {
        public static AssignUserToAccountCommand ToCommand(this AssignUserToAccountRequest request)
          => new(
              Guid.Parse(request.AccountId),
              Guid.Parse(request.UserId));
        public static DeleteUserFromAccountCommand ToCommand(this DeleteUserFromAccountRequest request)
         => new(
             Guid.Parse(request.AccountId),
             Guid.Parse(request.UserId));
    }
}
