using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DataManagement.Application.Contracts;
using DataManagement.Application.DTOs;
using DataManagement.Domain;
using DataManagement.Domain.Core;

namespace DataManagement.Application.Services;

public class AdvisorService(IAdvisorRepository repository) : IAdvisorService
{
    public async Task<List<AdvisorDto>> GetAsync()
    {
        var list = await repository.GetAsync();
        return list.ToDto();
    }

    public async Task<Result<AdvisorDto, Error>> GetAsync(int id)
    {
        var maybeAdvisor = await repository.GetAsync(id);
        if (maybeAdvisor.HasNoValue) return DomainErrors.NotFound(id);
        return maybeAdvisor.Value.ToDto();
    }

    public async Task<Result<AdvisorDto, List<Error>>> AddAsync(AdvisorCreationDto dto)
    {
        var validation = ValidateCreationPayload(dto);
        if (validation.Errors.Count > 0) return validation.Errors;

        // Check if advisor with same key attributes but different ID already exists
        Expression<Func<Advisor, bool>> sinAlreadyUsed = x => x.SIN.Number == validation.SIN.Number;
        var expression = CombineExpressions(validation.AdvisorWithSameInfo, sinAlreadyUsed,
            ExpressionType.OrElse);

        var advisorsWithSameInfoOrSameSin = await repository.GetAsync(expression);
        if (advisorsWithSameInfoOrSameSin.Count > 0) return new List<Error> { DomainErrors.AlreadyExists() };

        var advisor = Advisor.Create(validation.FullName, validation.SIN, validation.Address, validation.Phone);

        await repository.AddAsync(advisor);
        await repository.SaveAsync();

        return advisor.ToDto();
    }

    public async Task<UnitResult<List<Error>>> UpdateAsync(int id, AdvisorUpdateDto dto)
    {
        var maybeAdvisor = await repository.GetAsync(id);
        if (maybeAdvisor.HasNoValue) return new List<Error> { DomainErrors.NotFound(id) };

        var validation = ValidateUpdatePayload(dto);
        if (validation.Errors.Count > 0) return validation.Errors;

        // Check if advisor with same key attributes but different ID already exists
        Expression<Func<Advisor, bool>> advisorWithDifferentId = x => x.Id != id;
        var differentIdButSameInfo =
            CombineExpressions(validation.AdvisorWithSameInfo, advisorWithDifferentId, ExpressionType.AndAlso);
        var advisorsWithSameInfo = await repository.GetAsync(differentIdButSameInfo);
        if (advisorsWithSameInfo.Count > 0) return new List<Error> { DomainErrors.AlreadyExists() };

        var advisor = maybeAdvisor.GetValueOrDefault();

        advisor.Update(validation.FullName, validation.Address, validation.Phone);

        repository.Update(advisor);
        await repository.SaveAsync();

        return UnitResult.Success<List<Error>>();
    }

    public async Task<UnitResult<Error>> DeleteAsync(int id)
    {
        var maybeAdvisor = await repository.GetAsync(id);
        if (maybeAdvisor.HasNoValue) return DomainErrors.NotFound(id);

        var advisor = maybeAdvisor.GetValueOrDefault();

        repository.Delete(advisor);
        await repository.SaveAsync();

        return UnitResult.Success<Error>();
    }

    private static AdvisorValidationDto ValidateCreationPayload(AdvisorCreationDto dto)
    {
        return ValidatePayload(dto);
    }

    private static AdvisorValidationDto ValidateUpdatePayload(AdvisorUpdateDto dto)
    {
        return ValidatePayload(dto, false);
    }

    private static AdvisorValidationDto ValidatePayload(CommonAdvisorDto dto, bool isCreation = true)
    {
        var validationDto = new AdvisorValidationDto();

        var (_, nameFailure, fullName, nameErrors) = FullName.Create(dto.FirstName, dto.LastName);
        if (nameFailure) validationDto.Errors.AddRange(nameErrors);
        else validationDto.FullName = fullName;

        if (isCreation)
        {
            dto = (AdvisorCreationDto)dto;
            var (_, sinFailure, sin, sinError) = SIN.Create(((AdvisorCreationDto)dto).SIN);
            if (sinFailure) validationDto.Errors.Add(sinError);
            else validationDto.SIN = sin;
        }

        var (_, addressFailure, address, addressError) = Address.Create(dto.Address);
        if (addressFailure) validationDto.Errors.Add(addressError);
        else validationDto.Address = address;

        var (_, phoneFailure, phone, phoneError) = Phone.Create(dto.Phone);
        if (phoneFailure) validationDto.Errors.Add(phoneError);
        else validationDto.Phone = phone;

        if (validationDto.Errors.Count == 0)
            validationDto.AdvisorWithSameInfo = x =>
                x.FullName.FirstName == fullName.FirstName
                && x.FullName.LastName == fullName.LastName
                && x.Address.Value == address.Value
                && x.Phone.Number == phone.Number;

        return validationDto;
    }

    private static Expression<Func<Advisor, bool>> CombineExpressions(Expression<Func<Advisor, bool>> expr1,
        Expression<Func<Advisor, bool>> expr2, ExpressionType combinationType)
    {
        var parameter = Expression.Parameter(typeof(Advisor), "x");

        var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        Expression combined = combinationType switch
        {
            ExpressionType.AndAlso => Expression.AndAlso(left, right),
            ExpressionType.OrElse => Expression.OrElse(left, right),
            _ => throw new NotSupportedException($"Combination type '{combinationType}' is not supported.")
        };

        return Expression.Lambda<Func<Advisor, bool>>(combined, parameter);
    }
}

internal class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == oldParameter ? newParameter : base.VisitParameter(node);
    }
}