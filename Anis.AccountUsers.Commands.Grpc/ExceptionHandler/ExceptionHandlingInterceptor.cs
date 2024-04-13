using Anis.AccountUsers.Commands.Domain.Exceptions;
using Grpc.Core.Interceptors;
using Grpc.Core;
using Anis.AccountUsers.Commands.Grpc.Extensions;

namespace Anis.AccountUsers.Commands.Grpc.ExceptionHandler
{
    public class ExceptionHandlingInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionHandlingInterceptor> _logger;

        public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
        {
            _logger = logger;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case AppException appException:
                        throw new RpcException(new Status(appException.StatusCode.ToRpcStatuseCode(), appException.Message));
                    case RpcException:
                        throw;
                    default:
                        _logger.LogError(e, "An error occured when calling {Method}", context.Method);
                        throw new RpcException(new Status(StatusCode.Unknown, e.Message));
                }
            }
        }
    }
}
