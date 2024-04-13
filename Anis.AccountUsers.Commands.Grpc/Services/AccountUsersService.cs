using Anis.AccountUsers.Commands.Domain.Resourses;
using Anis.AccountUsers.Commands.Grpc.Extensions;
using Anis.AccountUsers.Commands.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Anis.AccountUsers.Commands.Grpc.Services
{
    public class AccountUsersService : Protos.AccountUsers.AccountUsersBase
    {
        private readonly IMediator _mediator;

        public AccountUsersService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Response> AssignUserToAccount(AssignUserToAccountRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            await _mediator.Send(command);

            return new Response
            {
                Message = Phrases.UserAssigned
            };
        }
        public override async Task<Response> DeleteUserFromAccount(DeleteUserFromAccountRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            await _mediator.Send(command);

            return new Response
            {
                Message = Phrases.UserDeleted
            };
        }
    }
}
