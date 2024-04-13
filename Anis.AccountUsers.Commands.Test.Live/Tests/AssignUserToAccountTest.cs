using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Resourses;
using Anis.AccountUsers.Commands.Test.Asserts;
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
using Anis.AccountUsers.Commands.Test.Live.Asserts;
using Anis.AccountUsers.Commands.Test.Fakers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;


namespace Anis.AccountUsers.Commands.Test.Live.Tests
{
    public class AssignUserToAccountTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private const int _delay = 10000;
        private readonly DbContextHelper _dbContextHelper;
        private readonly GrpcClientHelper _grpcClientHelper;
        private readonly ListenerHelper _listenerHelper;
        public AssignUserToAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
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
        public async Task AssignUserToAccount_CreateAccount_ReturnValid()
        {
            // Arrange
            var listener = _listenerHelper.Listener;
            var request = new AssignUserToAccountRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
            };

            // Act
            var response = await _grpcClientHelper.Send(r => r.AssignUserToAccountAsync(request));
            await Task.Delay(_delay);
            await listener.CloseAsync();
            var userAssignedToAccount = await _dbContextHelper.Query(db => db.Events.OfType<UserAssignedToAccount>()
               .SingleOrDefaultAsync());

            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());
            var message = listener.Messages.SingleOrDefault();
            // Assert
            Assert.Null(outboxMessage);
            Assert.Equal(Phrases.UserAssigned, response.Message);
            EventsAssert.AssertEquality(request, userAssignedToAccount);           
            MessageAssert.AssertEquality(userAssignedToAccount, message);
        }
        [Fact]
        public async Task AssignUserToAccount_AssignUser_ReturnValid()
        {
            // Arrange
            var listener = _listenerHelper.Listener;
            var createEvent = new UserAssignedToAccountFaker().Generate();
            await _dbContextHelper.InsertAsync(createEvent);

            var request = new AssignUserToAccountRequest
            {
                AccountId = createEvent.AggregateId.ToString(),
                UserId = Guid.NewGuid().ToString(),
            };

            // Act
            var response = await _grpcClientHelper.Send(r => r.AssignUserToAccountAsync(request));
            await Task.Delay(_delay);
            await listener.CloseAsync();
            var userAssignedToAccount = await _dbContextHelper.Query(db => db.Events.OfType<UserAssignedToAccount>()
                                                                          .SingleOrDefaultAsync(e => e.Sequence == 2));
            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());
            var message = listener.Messages.SingleOrDefault();
            // Assert
            Assert.Null(outboxMessage);
            Assert.Equal(Phrases.UserAssigned, response.Message);

            EventsAssert.AssertEquality(request, userAssignedToAccount);
            MessageAssert.AssertEquality(userAssignedToAccount, message);

        }
    }
}
