using Calzolari.Grpc.AspNetCore.Validation;

namespace Anis.AccountUsers.Commands.Grpc.Validators.Main
{
    public static class ValidationContainer
    {
        public static IServiceCollection AddAppValidators(this IServiceCollection services)
        {
            services.AddGrpcValidation();
            services.AddScoped<GrpcValidator>();

            services.AddValidator<AssignUserToAccountRequestValidator>();
            services.AddValidator<DeleteUserFromAccountRequestValidator>();

            return services;
        }
    }
}
