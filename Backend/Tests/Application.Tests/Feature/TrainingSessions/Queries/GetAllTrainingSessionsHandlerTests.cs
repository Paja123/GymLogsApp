using Application.Common.Interfaces;
using Application.Feature.TrainingSessions.Queries.GetAll;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Feature.TrainingSessions.Queries
{
    public class GetAllTrainingSessionsHandlerTests
    {
        private readonly Mock<ITrainingSessionRepository> _repoMock = new();
        private readonly Mock<ICurrentUserService> _userMock = new();
        private readonly GetAllTrainingSessionsHandler _handler;

        public GetAllTrainingSessionsHandlerTests()
        {
            _handler = new GetAllTrainingSessionsHandler(
                _repoMock.Object, 
                _userMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSessionsForCurrentUser()
        {
            var userId = "user-123";
            _userMock.Setup(x => x.UserId).Returns(userId);

            var fakeSessions = new List<TrainingSession>()
            {
                new () {Id = Guid.NewGuid(), Duration = 60, TrainingType = TrainingType.Cardio},
                new () {Id = Guid.NewGuid(), Duration = 45, TrainingType = TrainingType.Swimming }
            };
            _repoMock.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(fakeSessions);

            var result = await _handler.Handle(
                new GetAllTrainingSessionsQuery(), CancellationToken.None);

            result.Should().HaveCount(2);
            result[0].Duration.Should().Be(60);
            result[1].Duration.Should().Be(45);   
        }

        [Fact]
        public async Task Handle_WhenNoSessions_returnsEmptyList()
        {
            //Arrange
            _userMock.Setup(x => x.UserId).Returns("user-123");
            _repoMock.Setup(x => x.GetAllAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<TrainingSession>());
            
            //Act
            var result = await _handler.Handle(
                new GetAllTrainingSessionsQuery(), CancellationToken.None);
            
            //Assert
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task Handle_WhenUserNotAuthenticated_ThrowsUnauthorizedException()
        {
            _userMock.Setup(x => x.UserId).Returns((string?)null);

            var act = async () => await _handler.Handle(
                new GetAllTrainingSessionsQuery(), CancellationToken.None);
            
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task Handle_CallsRepositoryWithCorrectUserId()
        {
            var userId = "user-123";
            _userMock.Setup(x => x.UserId).Returns(userId);
            _repoMock.Setup(x => x.GetAllAsync(userId))
                .ReturnsAsync(new List<TrainingSession>());

            await _handler.Handle(new GetAllTrainingSessionsQuery(), CancellationToken.None);

            // Assert — verify repo was called with the right userId
            _repoMock.Verify(x => x.GetAllAsync(userId), Times.Once());
        }
    }
}
