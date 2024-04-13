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
using Anis.AccountUsers.Commands.Domain.Enums;
using Calzolari.Grpc.Net.Client.Validation;

namespace Anis.AccountUsers.Commands.Test.Test
{
    public class DeleteUserFromAccountTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper _dbContextHelper;
        private readonly GrpcClientHelper _grpcClientHelper;
        public DeleteUserFromAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsDefaultEnvironment();
            });

            _dbContextHelper = new DbContextHelper(factory.Services);
            _grpcClientHelper = new GrpcClientHelper(factory);
        }

        [Fact]
        public async Task DeleteUserFromAccount_WithValidData_ReturnValid()
        {
            // Arrange

            var userAssignedToAccount = new UserAssignedToAccountFaker().Generate();

            await _dbContextHelper.InsertAsync(userAssignedToAccount);

            var request = new DeleteUserFromAccountRequest
            {
                AccountId = userAssignedToAccount.AggregateId.ToString(),
                UserId = userAssignedToAccount.Data.UserId.ToString()
            };

            // Act
            var response = await _grpcClientHelper.Send(r => r.DeleteUserFromAccountAsync(request));
            var userUnassignedFromAccount = await _dbContextHelper.Query(db => db.Events
                                                                          .SingleOrDefaultAsync(e => e.Type == EventType.UserDeletedFromAccount));

            var outboxMessage = await _dbContextHelper.Query(db => db.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync());

            // Assert

            Assert.Equal(Phrases.UserDeleted, response.Message);

            EventsAssert.AssertEquality(request, userUnassignedFromAccount);

            EventsAssert.AssertEquality<UserDeletedFromAccount, UserDeletedFromAccountData>(userUnassignedFromAccount, outboxMessage);
        }

        [Fact]
        public async Task DeleteUserFromAccount_NoEventsInDatabaseForAggregate_ReturnNotFound()
        {
            // Arrange

            var request = new DeleteUserFromAccountRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };

            // Act

            var exception = await Assert.ThrowsAsync<RpcException>(
                  async () => await _grpcClientHelper.Send(x => x.DeleteUserFromAccountAsync(request: request)));

            var @event = await _dbContextHelper.Query(db => db.Events.SingleOrDefaultAsync());

            // Assert

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Equal(Phrases.AccountNotFound, exception.Status.Detail);
            Assert.Null(@event);
        }


        [Theory]
        [InlineData("28D3BE19-1F24-44E4-8D18", "28D3BE19-1F24-44E4-8D18-D7C09981C195", "AccountId")]
        [InlineData("28D3BE19-1F24-44E4-8D18-D7C09981C195", "28D3BE19-1F24-44E4-8D18", "UserId")]
        public async Task DeleteUserFromAccount_SendInvalidData_ReturnInvalidArgument(string accountId, string userId, string error)
        {
            // Arrange

            var request = new DeleteUserFromAccountRequest
            {
                AccountId = accountId,
                UserId = userId
            };

            // Act

            var exception = await Assert.ThrowsAsync<RpcException>(
                      async () => await _grpcClientHelper.Send(x => x.DeleteUserFromAccountAsync(request: request)));

            var @event = await _dbContextHelper.Query(db => db.Events.SingleOrDefaultAsync());

            // Assert

            Assert.NotEmpty(exception.Status.Detail);

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains(
                    exception.GetValidationErrors(),
                    e => e.PropertyName.EndsWith(error)
                           );

            Assert.Null(@event);
        }
    }
}
