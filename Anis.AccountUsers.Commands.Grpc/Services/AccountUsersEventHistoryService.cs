using Anis.AccountUsers.Commands.Application.Contracts.Repositories;
using Anis.AccountUsers.Commands.Grpc.Extensions;
using Anis.AccountUsers.Commands.Grpc.Protos.History;
using Grpc.Core;

namespace Anis.AccountUsers.Commands.Grpc.Services
{
    public class AccountUsersEventHistoryService: AccountUsersEventHistory.AccountUsersEventHistoryBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountUsersEventHistoryService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public async override Task<Response> GetEvents(GetEventsRequest request, ServerCallContext context)
        {
            var events = await _unitOfWork.Events.GetAsPaginationAsync(request.CurrentPage, request.PageSize);

            var response = new Response();

            response.Events.ToOutputEvent(events);

            return response;
        }
     }    
}
