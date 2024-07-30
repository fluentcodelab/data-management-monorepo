using CSharpFunctionalExtensions;

namespace DataManagement.Application.Contracts;

public interface ICacheService
{
    Maybe<TValue> Get<TValue>(string key);
    void Put<TValue>(string key, TValue value);
    void Delete<TValue>(string key);
}