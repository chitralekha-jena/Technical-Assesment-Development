using AutoMapper;
using Demo_Api.Application.Handler;
using Demo_Api.Application.Interfaces;
using Demo_Api.Application.Queries.GetTargetAssets;
using Demo_API.Application.Exceptions;
using Demo_API.Core.Entities;
using Demo_API.Infrastructure.ExternalServices.TargetAsset.Response;
using Demo_API.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Demo_Api.Application.UnitTests
{
    public class GetTargetAssetsQueryHandlerTests
    {
        private readonly Mock<ITargetAssetService> _targetAssetServiceMock;
        private readonly Mock<ITargetAssetRules> _targetAssetRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetTargetAssetsQueryHandler>> _loggerMock;

        private readonly GetTargetAssetsQueryHandler _handler;

        public GetTargetAssetsQueryHandlerTests()
        {
            _targetAssetServiceMock = new Mock<ITargetAssetService>();
            _targetAssetRulesMock = new Mock<ITargetAssetRules>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetTargetAssetsQueryHandler>>();

            _handler = new GetTargetAssetsQueryHandler(_targetAssetServiceMock.Object, _targetAssetRulesMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenServiceReturnsEmptyList()
        {
            // Arrange
            _targetAssetServiceMock.Setup(svc => svc.GetTargetAssets()).ReturnsAsync(new List<TargetAssetApiResponse>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new GetTargetAssetsQuery(), CancellationToken.None));
            _targetAssetServiceMock.Verify(svc => svc.GetTargetAssets(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Call_IsStartable_With_Correct_Arguments()
        {
            // Arrange
            var request = new GetTargetAssetsQuery();
            var targetAssetApiResponse = new TargetAssetApiResponse();
            var targetAssetDto = new TargetAssetDto();
            var targetAssetDtos = new List<TargetAssetDto> { targetAssetDto };
            var targetAssetApiResponses = new List<TargetAssetApiResponse> { targetAssetApiResponse };
            _targetAssetServiceMock.Setup(x => x.GetTargetAssets()).ReturnsAsync(targetAssetApiResponses);
            _mapperMock.Setup(x => x.Map<List<TargetAssetDto>>(targetAssetApiResponses)).Returns(targetAssetDtos);

            _targetAssetRulesMock.Setup(x => x.IsStartable(It.IsAny<string>())).Returns(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _targetAssetRulesMock.Verify(x => x.IsStartable(targetAssetDto.status), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Call_CalculateParentTargetAssetCount_With_Correct_Arguments()
        {
            // Arrange
            var request = new GetTargetAssetsQuery();
            var targetAssetApiResponse = new TargetAssetApiResponse { parentId = 1 };
            var targetAssetDto = new TargetAssetDto { id = 2, parentId = 1 };
            var targetAssetDtos = new List<TargetAssetDto?> { targetAssetDto };
            var targetAssetApiResponses = new List<TargetAssetApiResponse> { targetAssetApiResponse };
            _targetAssetServiceMock.Setup(x => x.GetTargetAssets()).ReturnsAsync(targetAssetApiResponses);
            _mapperMock.Setup(x => x.Map<List<TargetAssetDto?>>(targetAssetApiResponses)).Returns(targetAssetDtos);

            _targetAssetRulesMock.Setup(x => x.CalculateParentTargetAssetCount(targetAssetDtos, targetAssetDto.parentId)).Returns(2);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _targetAssetRulesMock.Verify(x => x.CalculateParentTargetAssetCount(targetAssetDtos, targetAssetDto.parentId), Times.Once);
        }

        //[Fact]
        //public async Task Handle_SetsIsStartableAndParentTargetAssetCountPropertiesOnEachDto()
        //{
        //    // Arrange
        //    var targetAssets = new List<TargetAssetApiResponse>
        //    {
        //       new TargetAssetApiResponse { id = 1, status = "InProgress", parentId = null },
        //       new TargetAssetApiResponse { id = 2, status = "Completed", parentId = 1 },
        //       new TargetAssetApiResponse { id = 3, status = "NotStarted", parentId = 1 },
        //       new TargetAssetApiResponse { id = 4, status = "NotStarted", parentId = 2 }
        //    };
        //    var dtoResult = new List<TargetAssetDto?>
        //    {
        //       new TargetAssetDto { id = 1 },
        //       new TargetAssetDto { id = 2 },
        //       new TargetAssetDto { id = 3 },
        //       new TargetAssetDto { id = 4 }
        //    };
        //    var expectedParentCounts = new Dictionary<int, int> { { 1, 0 }, { 2, 1 }, { 3, 1 }, { 4, 2 } };
        //    _targetAssetServiceMock.Setup(x => x.GetTargetAssets()).ReturnsAsync(targetAssets);
        //    _mapperMock.Setup(x => x.Map<List<TargetAssetDto?>>(targetAssets)).Returns(dtoResult);
        //    _targetAssetRulesMock.Setup(x => x.IsStartable("InProgress")).Returns(true);
        //    _targetAssetRulesMock.Setup(x => x.IsStartable("Completed")).Returns(false);
        //    _targetAssetRulesMock.Setup(x => x.IsStartable("NotStarted")).Returns(true);
        //    _targetAssetRulesMock.Setup(x => x.CalculateParentTargetAssetCount(dtoResult, It.IsAny<int?>())).Returns<int?, int>((id, _) => expectedParentCounts[id.Value]);

        //    // Act
        //    var result = await _handler.Handle(new GetTargetAssetsQuery(), CancellationToken.None);

        //    // Assert
        //    _targetAssetRulesMock.Verify(x => x.IsStartable(It.IsAny<string>()), Times.Exactly(4));
        //    _targetAssetRulesMock.Verify(x => x.CalculateParentTargetAssetCount(It.IsAny<List<TargetAssetDto?>>(), It.IsAny<int?>()), Times.Exactly(4));
        //}

        [Fact]
        public async Task Handle_Should_SetIsStartableAndParentTargetAssetCount_When_ResponseDtoIsNotNull()
        {
            // Arrange
            var targetAssets = new List<TargetAssetApiResponse>
            {
              new TargetAssetApiResponse { id = 1, status = "Active", parentId = 2 },
              new TargetAssetApiResponse { id = 2, status = "Inactive" }
            };
            var targetAssetDtos = new List<TargetAssetDto?>
            {
               new TargetAssetDto { id = 1, status = "Active", parentId = 2 },
               new TargetAssetDto { id = 2, status = "Inactive" }
            };
            var parentTargetAssetCount = 1;
            _targetAssetServiceMock.Setup(x => x.GetTargetAssets()).ReturnsAsync(targetAssets);
            _mapperMock.Setup(x => x.Map<List<TargetAssetDto?>>(targetAssets)).Returns(targetAssetDtos);
            _targetAssetRulesMock.Setup(x => x.IsStartable(It.IsAny<string>())).Returns(true);
            _targetAssetRulesMock.Setup(x => x.CalculateParentTargetAssetCount(targetAssetDtos, It.IsAny<int?>())).Returns(parentTargetAssetCount);

            // Act
            var result = await _handler.Handle(new GetTargetAssetsQuery(), CancellationToken.None);

            // Assert
            _targetAssetRulesMock.Verify(x => x.IsStartable("Active"), Times.Once);
            _targetAssetRulesMock.Verify(x => x.CalculateParentTargetAssetCount(targetAssetDtos, 2), Times.Once);
            Assert.Equal(true, result[0]?.isStartable);
            Assert.Equal(parentTargetAssetCount, result[0]?.parentTargetAssetCount);
        }
    }
}



