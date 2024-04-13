using Anis.AccountUsers.Commands.Grpc.Protos;
using FluentValidation;

namespace Anis.AccountUsers.Commands.Grpc.Validators
{
    public class AssignUserToAccountRequestValidator : AbstractValidator<AssignUserToAccountRequest>
    {
        public AssignUserToAccountRequestValidator()
        {
            RuleFor(r => r.AccountId)
                .Must(accountId => Guid.TryParse(accountId, out _))
                .NotEqual(Guid.Empty.ToString());

            RuleFor(r => r.UserId)
                .Must(userId => Guid.TryParse(userId, out _))
                .NotEqual(Guid.Empty.ToString());
        }
    }
}
