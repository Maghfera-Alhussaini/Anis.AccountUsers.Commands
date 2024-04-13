using Anis.AccountUsers.Commands.Domain.Events.DataTypes;
using Anis.AccountUsers.Commands.Domain.Events;
using Anis.AccountUsers.Commands.Domain.Models;
using Anis.AccountUsers.Commands.Domain.Resourses;
using Anis.AccountUsers.Commands.Test.Asserts;
using Anis.AccountUsers.Commands.Test.Helpers;
using Anis.AccountUsers.Commands.Test.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Anis.AccountUsers.Commands.Test.Fakers;

namespace Anis.AccountUsers.Commands.Test.Test
{
    public class AssignUserToAccountTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper _dbContextHelper;
        private readonly GrpcClientHelper _grpcClientHelper;
        public AssignUserToAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsDefaultEnvironment();
            });

            _dbContextHelper = new DbContextHelper(factory.Services);
            _grpcClientHelper = new GrpcClientHelper(factory);
        }


        [Fact]
        public async Task AssignUserToAccount_CreateAccount_ReturnValid()
        {
            // Arrange
            var request = new AssignUserToAccountRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
            };

            // Act
            var response = await _grpcClientHelper.Send(r => r.AssignUserToAccountAsync(request));
            var userAssignedToccount = await _dbContextHelper.Query(db => db.Events
               .SingleOrDefaultAsync(e => e.Type == Domain.Enums.EventType.UserAssignedToAccount));

            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());

            // Assert

            Assert.Equal(Phrases.UserAssigned, response.Message);

            EventsAssert.AssertEquality(request,userAssignedToccount);

            EventsAssert.AssertEquality<UserAssignedToAccount, UserAssignedToAccountData>(userAssignedToccount, outboxMessage);
        }
        [Fact]
        public async Task AssignUserToAccount_AssignUser_ReturnValid()
        {
            // Arrange
            var createEvent = new UserAssignedToAccountFaker().Generate();
            await _dbContextHelper.InsertAsync(createEvent);

            var request = new AssignUserToAccountRequest
            {
                AccountId = createEvent.AggregateId.ToString(),
                UserId = Guid.NewGuid().ToString(),
            };

            // Act
            var response = await _grpcClientHelper.Send(r => r.AssignUserToAccountAsync(request));
            var userAssignedToccount = await _dbContextHelper.Query(db => db.Events
                                                                          .SingleOrDefaultAsync(e => e.Sequence == 2));

            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());

            // Assert

            Assert.Equal(Phrases.UserAssigned, response.Message);

            EventsAssert.AssertEquality(request, userAssignedToccount);

            EventsAssert.AssertEquality<UserAssignedToAccount, UserAssignedToAccountData>(userAssignedToccount, outboxMessage);
        }
        [Fact]
        public async Task AssignUserToAccount_Already_Exist_EventsInDatabaseForAggregate_ReturnAlreadExist()
        {
            // Arrange

            var createEvent = new UserAssignedToAccountFaker().Generate();
            await _dbContextHelper.InsertAsync(createEvent);

            var request = new AssignUserToAccountRequest
            {
                AccountId = createEvent.AggregateId.ToString(),
                UserId = createEvent.Data.UserId.ToString(),
            };

            // Act

            var exception = await Assert.ThrowsAsync<RpcException>(
                  async () => await _grpcClientHelper.Send(x => x.AssignUserToAccountAsync(request: request)));

            var @event = await _dbContextHelper.Query(db => db.Events.SingleOrDefaultAsync());

            // Assert

            Assert.Equal(StatusCode.AlreadyExists, exception.StatusCode);
            Assert.Equal(Phrases.UserAlreadyExist, exception.Status.Detail);
            Assert.NotNull(@event);
            Assert.Equal(1, @event.Sequence);

        }
        [Theory]
        [InlineData("28D3BE19-1F24-44E4-8D18", "28D3BE19-1F24-44E4-8D18-D7C09981C195", "AccountId")]
        [InlineData("28D3BE19-1F24-44E4-8D18-D7C09981C195", "28D3BE19-1F24-44E4-8D18", "UserId")]
        public async Task AssignUserToAccount_SendInvalidData_ReturnInvalidArgument(string accountId, string userId, string error)
        {
            //Arrange
            var request= new AssignUserToAccountRequest {              
            AccountId = accountId,
            UserId = userId
            };

            //Act
            var exception = await Assert.ThrowsAsync<RpcException>(
                     async () => await _grpcClientHelper.Send(x => x.AssignUserToAccountAsync(request: request)));

            var @event = await _dbContextHelper.Query(db => db.Events.SingleOrDefaultAsync());
            //Assert
            Assert.NotEmpty(exception.Status.Detail);
            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Null(@event);

        }
    }
}
