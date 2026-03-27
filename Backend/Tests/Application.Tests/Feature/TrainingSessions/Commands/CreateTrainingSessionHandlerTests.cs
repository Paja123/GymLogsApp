using Application.Common.Interfaces;
using Application.Exceptions;
using Application.Feature.TrainingSessions.Commands.Create;
using Domain.Entities;
using Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Tests.Feature.TrainingSessions.Commands
{
    public class CreateTrainingSessionHandlerTests
    {
        private readonly Mock<ITrainingSessionRepository> _repoMock = new();
        private readonly Mock<ICurrentUserService> _userMock = new();
        private readonly CreateTrainingSessionHandler _handler;

        public CreateTrainingSessionHandlerTests()
        {
            _handler = new CreateTrainingSessionHandler(
                _repoMock.Object,
                _userMock.Object);
        }

        [Fact]
        public async Task Handle_Should_CreateSession_WhenNoOverlap()
        {
            var userId = "user-123";
            var command = new CreateTrainingSessionCommand(
                 TrainingType.Cardio,
                60,
                250,
                7,
                8,
                DateTime.Now,
                "good workout");

            var expectedId = Guid.NewGuid();

            _userMock.Setup(x => x.UserId).Returns(userId);
            _repoMock
                .Setup(x => x.OverlapingSessionExists(userId, command.Date, command.Duration))
                .ReturnsAsync(false);

            _repoMock
                .Setup(x => x.AddAsync(It.IsAny<TrainingSession>()))
                .ReturnsAsync(expectedId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(expectedId, result);

            _repoMock.Verify(x => x.AddAsync(It.Is<TrainingSession>(s =>
                s.UserId == userId &&
                s.Duration == command.Duration &&
                s.TrainingType == command.TrainingType)), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenOverlapExists()
        {
            var userId = "user-123";
            var command = new CreateTrainingSessionCommand(
                TrainingType.Cardio,
               60,
               250,
               7,
               8,
               DateTime.Now,
               "good workout");

            _userMock.Setup(x => x.UserId).Returns(userId);

            _repoMock
                .Setup(x => x.OverlapingSessionExists(userId, command.Date, command.Duration))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<TrainingSessionOverlapException>(() =>
            _handler.Handle(command, CancellationToken.None));

            _repoMock.Verify(x => x.AddAsync(It.IsAny<TrainingSession>()), Times.Never);
        }

    }
}
