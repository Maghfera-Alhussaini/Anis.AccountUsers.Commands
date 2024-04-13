using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Resourses;
using Anis.AccountUsers.Commands.Test.Asserts;
using Anis.AccountUsers.Commands.Test.Fakers;
using Anis.AccountUsers.Commands.Test.Helpers;
using Anis.AccountUsers.Commands.Test.Live.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Anis.AccountUsers.Commands.Test.Protos;
using Anis.AccountUsers.Commands.Domain.Enums;
using Anis.AccountUsers.Commands.Test.Live.Asserts;

namespace Anis.AccountUsers.Commands.Test.Live.Tests
{
    public class DeleteUserFromAccountTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private const int _delay = 10000;
        private readonly DbContextHelper _dbContextHelper;
        private readonly GrpcClientHelper _grpcClientHelper;
        private readonly ListenerHelper _listenerHelper;
        public DeleteUserFromAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetLiveTestsDefaultEnvironment();
            });

            _dbContextHelper = new DbContextHelper(factory.Services);
            _grpcClientHelper = new GrpcClientHelper(factory);
            _listenerHelper = new ListenerHelper(factory.Services);
        }
        [Fact]
        public async Task DeleteUserFromAccount_WithValidData_ReturnValid()
        {
            // Arrange
            var listener = _listenerHelper.Listener;

            var userAssignedToAccount = new UserAssignedToAccountFaker().Generate();

            await _dbContextHelper.InsertAsync(userAssignedToAccount);

            var request = new DeleteUserFromAccountRequest
            {
                AccountId = userAssignedToAccount.AggregateId.ToString(),
                UserId = userAssignedToAccount.Data.UserId.ToString()
            };

            // Act         
            var response = await _grpcClientHelper.Send(r => r.DeleteUserFromAccountAsync(request));
 
            await Task.Delay(_delay);

            await listener.CloseAsync();

            var userDeletedFromAccount = await _dbContextHelper.Query(db => db.Events.OfType<UserDeletedFromAccount>()
                                                                          .SingleOrDefaultAsync());

            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());

            var message = listener.Messages.SingleOrDefault();
            // Assert
            Assert.Null(outboxMessage);
            Assert.Equal(Phrases.UserDeleted, response.Message);
            EventsAssert.AssertEquality(request, userDeletedFromAccount);
            MessageAssert.AssertEquality(userDeletedFromAccount, message);
        }
    }
}
