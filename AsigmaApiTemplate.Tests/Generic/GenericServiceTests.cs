using FakeItEasy;
using FluentAssertions;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.Repositories.GenericRepositories;
using AsigmaApiTemplate.Api.Services.GenericServices;
using System.Linq.Expressions;

namespace AsigmaApiTemplate.Tests.Generic;

public class GenericServiceTests
{
    private readonly IGenericService<IEntity> _genericService;
    private readonly IGenericRepository<IEntity> _repository;

    public GenericServiceTests()
    {
        _repository = A.Fake<IGenericRepository<IEntity>>();
        _genericService = new GenericService<IEntity>(_repository);
    }

    [Fact]
    public async Task GenericService_GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = new List<IEntity>();
        var totalCount = entities.Count;
        var paginatedList = new PaginatedList<IEntity>(entities, totalCount, 1, 10, totalCount);

        A.CallTo(() => _repository.GetAllAsync()).Returns(Task.FromResult(paginatedList));

        // Act
        var result = await _genericService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(paginatedList);
    }

    [Fact]
    public async Task GenericService_GetByIdAsync_ShouldReturnEntityById()
    {
        // Arrange

        var entity = A.Fake<IEntity>();
        var id = Guid.NewGuid();
        A.CallTo(() => _repository.GetByIdAsync(id)).Returns(Task.FromResult<IEntity?>(entity));

        // Act
        var result = await _genericService.GetByIdAsync(id);

        // Assert
        result.Should().Be(entity);
    }

    [Fact]
    public async Task GenericService_InsertAsync_ShouldAddNewEntity()
    {
        // Arrange

        var entity = A.Fake<IEntity>();
        A.CallTo(() => _repository.InsertAsync(entity)).Returns(Task.FromResult(entity));

        // Act
        var result = await _genericService.InsertAsync(entity);

        // Assert
        result.Should().Be(entity);
    }

    [Fact]
    public async Task GenericService_UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange

        var entity = A.Fake<IEntity>();
        A.CallTo(() => _repository.UpdateAsync(entity)).Returns(Task.FromResult(entity));

        // Act
        var result = await _genericService.UpdateAsync(entity);

        // Assert
        result.Should().Be(entity);
    }

    [Fact]
    public async Task GenericService_DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange

        var id = Guid.NewGuid();
        A.CallTo(() => _repository.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var act = new Func<Task>(async () => await _genericService.DeleteAsync(id));

        // Assert
        await act.Should().NotThrowAsync();
    }
}