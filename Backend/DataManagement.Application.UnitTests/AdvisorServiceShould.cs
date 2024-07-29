using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using CSharpFunctionalExtensions;
using DataManagement.Application.Contracts;
using DataManagement.Application.DTOs;
using DataManagement.Application.Services;
using DataManagement.Application.UnitTests.TestData;
using DataManagement.Domain;
using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Moq;
using Shouldly;
using Xunit;

namespace DataManagement.Application.UnitTests;

public class AdvisorServiceShould
{
    private readonly Mock<IAdvisorRepository> _mockAdvisoryRepo;
    private readonly AdvisorService _sut;

    public AdvisorServiceShould()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockAdvisoryRepo = fixture.Freeze<Mock<IAdvisorRepository>>();
        _sut = fixture.Create<AdvisorService>();
    }

    [Fact(DisplayName = "Return a list of advisors on request")]
    public async Task Succeed_Get()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Advisor, bool>>>()))
            .ReturnsAsync([]);

        var result = await _sut.GetAsync();

        result.ShouldBeOfType<List<AdvisorDto>>();
    }

    [Fact(DisplayName = "Return an error when looking for an advisor given their ID, if no match is found")]
    public async Task Fail_GetById_NotFound()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(Maybe<Advisor>.None);

        var result = await _sut.GetAsync(1);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBeEquivalentTo(DomainErrors.NotFound(1));
    }

    [Fact(DisplayName = "Return an advisor when looking for an advisor given their ID, if a match is found")]
    public async Task Succeed_GetById_Found()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(new AdvisorBuilder().WithDefaultData().Build().Value);

        var result = await _sut.GetAsync(1);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeOfType<AdvisorDto>();
    }

    [Fact(DisplayName = "Return an error when adding an advisor, if creation payload not valid")]
    public async Task Fail_OnAdd_InvalidPayload()
    {
        var result = await _sut.AddAsync(new AdvisorCreationDto());

        result.IsFailure.ShouldBeTrue();
        result.Error.Count.ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "Return an error when adding an advisor, if duplicate advisor info or SIN")]
    public async Task Fail_OnAdd_DuplicateInfoOrSIN()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Advisor, bool>>>()))
            .ReturnsAsync([new AdvisorBuilder().WithDefaultData().Build().Value]);
        var dto = new AdvisorCreationDtoBuilder()
            .WithDefaultData()
            .Build();

        var result = await _sut.AddAsync(dto);

        result.IsFailure.ShouldBeTrue();
        result.Error.First().ShouldBeEquivalentTo(DomainErrors.AlreadyExists());
    }

    [Fact(DisplayName = "Succeed when adding an advisor, if payload is valid, no duplicate advisor info or SIN")]
    public async Task Succeed_OnAdd_ValidDataAndNoDuplication()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Advisor, bool>>>()))
            .ReturnsAsync([]);
        var dto = new AdvisorCreationDtoBuilder()
            .WithDefaultData()
            .Build();

        var result = await _sut.AddAsync(dto);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeOfType<AdvisorDto>();
        _mockAdvisoryRepo.Verify(x => x.AddAsync(It.IsAny<Advisor>()), Times.Once);
        _mockAdvisoryRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact(DisplayName = "Return an error when updating an advisor, if no match found for ID provided")]
    public async Task Fail_OnUpdate_NotFound()
    {
        const int id = 1;
        var dto = new AdvisorUpdateDtoBuilder()
            .Build();
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(Maybe<Advisor>.None);

        var result = await _sut.UpdateAsync(id, dto);

        result.IsFailure.ShouldBeTrue();
        result.Error.First().ShouldBeEquivalentTo(DomainErrors.NotFound(id));
    }

    [Fact(DisplayName = "Return an error when updating an advisor, if update payload not valid")]
    public async Task Fail_OnUpdate_InvalidPayload()
    {
        const int id = 1;
        var dto = new AdvisorUpdateDtoBuilder()
            .Build();
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(new AdvisorBuilder().WithDefaultData().Build().Value);

        var result = await _sut.UpdateAsync(id, dto);

        result.IsFailure.ShouldBeTrue();
        result.Error.Count.ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "Return an error when updating an advisor, if duplicate advisor info or SIN")]
    public async Task Fail_OnUpdate_DuplicateInfoOrSIN()
    {
        const int id = 1;
        var dto = new AdvisorUpdateDtoBuilder()
            .WithDefaultData()
            .Build();
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(new AdvisorBuilder().WithDefaultData().Build().Value);
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Advisor, bool>>>()))
            .ReturnsAsync([new AdvisorBuilder().WithDefaultData().Build().Value]);

        var result = await _sut.UpdateAsync(id, dto);

        result.IsFailure.ShouldBeTrue();
        result.Error.First().ShouldBeEquivalentTo(DomainErrors.AlreadyExists());
    }

    [Fact(DisplayName = "Succeed when updating an advisor, if payload is valid, no duplicate advisor info or SIN")]
    public async Task Succeed_OnUpdate_ValidDataAndNoDuplication()
    {
        const int id = 1;
        var dto = new AdvisorUpdateDtoBuilder()
            .WithDefaultData()
            .Build();
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(new AdvisorBuilder().WithDefaultData().Build().Value);
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Advisor, bool>>>()))
            .ReturnsAsync([]);

        var result = await _sut.UpdateAsync(id, dto);

        result.IsSuccess.ShouldBeTrue();
        _mockAdvisoryRepo.Verify(x => x.Update(It.IsAny<Advisor>()), Times.Once);
        _mockAdvisoryRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact(DisplayName = "Return an error when deleting an advisor, if no match found for ID provided")]
    public async Task Fail_OnDelete_NotFound()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(Maybe<Advisor>.None);

        var result = await _sut.DeleteAsync(1);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBeEquivalentTo(DomainErrors.NotFound(1));
    }

    [Fact(DisplayName = "Succeed deleting an advisor, if match found for ID provided")]
    public async Task Succeed_OnDelete()
    {
        _mockAdvisoryRepo
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(new AdvisorBuilder().WithDefaultData().Build().Value);

        var result = await _sut.DeleteAsync(1);

        result.IsSuccess.ShouldBeTrue();
        _mockAdvisoryRepo.Verify(x => x.Delete(It.IsAny<Advisor>()), Times.Once);
        _mockAdvisoryRepo.Verify(x => x.SaveAsync(), Times.Once);
    }
}