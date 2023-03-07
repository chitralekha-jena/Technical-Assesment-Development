using Demo_Api.Application.Interfaces;
using Demo_API.Core.Entities;
using Demo_API.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace Demo_Api.Application.UnitTests
{
    public class TargetAssetRulesTests
    {
        private readonly Mock<IDateTimeService> _dateTimeServiceMock;
        private readonly TargetAssetRules _targetAssetRules;

        public TargetAssetRulesTests()
        {
            _dateTimeServiceMock = new Mock<IDateTimeService>();
            _targetAssetRules = new TargetAssetRules(_dateTimeServiceMock.Object);
        }

        [Fact]
        public void IsStartable__WhenStatusIsRunningAndCurrentDateIsThirdDayOfMonth_ReturnsTrue_()
        {
            // Arrange
            var currentDate = new DateTime(2023, 3, 3);
            _dateTimeServiceMock.Setup(m => m.GetCurrentDate()).Returns(currentDate);

            var status = "Running";

            // Act
            var result = _targetAssetRules.IsStartable(status);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsStartable_WhenStatusIsNotRunning_ReturnsFalse()
        {
            // Arrange
            var currentDate = new DateTime(2023, 3, 3);
            _dateTimeServiceMock.Setup(m => m.GetCurrentDate()).Returns(currentDate);

            var status = "Stopped";

            // Act
            var result = _targetAssetRules.IsStartable(status);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsStartable_WhenCurrentDateIsNotThirdDayOfMonth_ReturnsFalse()
        {
            // Arrange
            var currentDate = new DateTime(2023, 3, 4);
            _dateTimeServiceMock.Setup(m => m.GetCurrentDate()).Returns(currentDate);

            var status = "Running";

            // Act
            var result = _targetAssetRules.IsStartable(status);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CalculateParentTargetAssetCount_WithTargetAssets_ReturnsCorrectCount()
        {
            // Arrange
            var targetAssets = new List<TargetAssetDto?>
            {
               new TargetAssetDto { id = 6, parentId = 5 },
               new TargetAssetDto { id = 5, parentId = 1 },
               new TargetAssetDto { id = 1, parentId = null }
            };

            var targetAssetParentId = 5;

            // Act
            var result = _targetAssetRules.CalculateParentTargetAssetCount(targetAssets, targetAssetParentId);

            // Assert
            result.Should().Be(3);

        }

        [Fact]
        public void CalculateParentTargetAssetCount_WithTargetAssetsDTOObjects_ShouldReturnCorrectCount()
        {
            // Arrange
            var targetAssets = new List<TargetAssetDto?>
            {
                 new TargetAssetDto { id = 1, parentId = null },
                 new TargetAssetDto { id = 2, parentId = 1 },
                 new TargetAssetDto { id = 3, parentId = 2 },
                 new TargetAssetDto { id = 4, parentId = 3 }
            };

            int targetAssetParentId = 4;
            int expectedCount = 5;

            // Act
            int actualCount = _targetAssetRules.CalculateParentTargetAssetCount(targetAssets, targetAssetParentId);

            // Assert
            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public void CalculateParentTargetAssetCount_ShouldHandleNullParentId()
        {
            // Arrange
            var targetAssets = new List<TargetAssetDto?>
            {
                   new TargetAssetDto { id = 1, parentId = null },
                   new TargetAssetDto { id = 2, parentId = 1 },
                   new TargetAssetDto { id = 3, parentId = 2 },
                   new TargetAssetDto { id = 4, parentId = 3 }
            };

            int? targetAssetParentId = null;
            int expectedCount = 1;

            // Act
            int actualCount = _targetAssetRules.CalculateParentTargetAssetCount(targetAssets, targetAssetParentId);

            // Assert
            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public void CalculateParentTargetAssetCount_ShouldHandleCircularDependencies_ReturnsZeroCount()
        {
            // Arrange
            var targetAssets = new List<TargetAssetDto?>
            {
                    new TargetAssetDto { id = 1, parentId = 2 },
                    new TargetAssetDto { id = 2, parentId = 3 },
                    new TargetAssetDto { id = 3, parentId = 1 },
                    new TargetAssetDto { id = 4, parentId = 3 }
            };

            int targetAssetParentId = 4;
            int expectedCount = 0;

            // Act
            int actualCount = _targetAssetRules.CalculateParentTargetAssetCount(targetAssets, targetAssetParentId);

            // Assert
            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public void CalculateParentTargetAssetCount_ShouldHandleInconsistentData_ReturnsTwo()
        {
            // Arrange
            var targetAssets = new List<TargetAssetDto?>
            {
                 new TargetAssetDto { id = 1, parentId = 2 },
                 new TargetAssetDto { id = 3, parentId = 4 },
                 new TargetAssetDto { id = 4, parentId = 5 },
                 new TargetAssetDto { id = 5, parentId = null }
            };
            int targetAssetParentId = 1;
            int expectedCount = 2;

            // Act
            int actualCount = _targetAssetRules.CalculateParentTargetAssetCount(targetAssets, targetAssetParentId);

            // Assert
            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public void CalculateParentTargetAssetCount_WithEmptyListAndTargetAssetParentID_ReturnsEZeroCount()
        {
            // Arrange
            var targetAssetsDtolist = new List<TargetAssetDto?>();

            // Act
            var result = _targetAssetRules.CalculateParentTargetAssetCount(targetAssetsDtolist,null);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void GetParentTargetAssetCountLatest_WithNoParentIds_ReturnsTargetAssetsWithZeroParentTargetAssetCount()
        {
            // Arrange
            var targetAssetsDtolist = new List<TargetAssetDto?>
            {
                 new TargetAssetDto { id = 1, parentId = null },
                 new TargetAssetDto { id = 2, parentId = null },
                 new TargetAssetDto { id = 3, parentId = null }
            };

            // Act
            var result = _targetAssetRules.CalculateParentTargetAssetCount(targetAssetsDtolist, null);

            // Assert
            result.Should().Be(1);
        }
    }
}

