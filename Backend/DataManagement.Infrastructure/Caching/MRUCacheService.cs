using CSharpFunctionalExtensions;
using DataManagement.Application.Contracts;

namespace DataManagement.Infrastructure.Caching;

public class MRUCacheService : ICacheService
{
    private readonly Dictionary<string, LinkedListNode<(string key, object value)>> _cache;
    private readonly int _capacity;
    private readonly LinkedList<(string key, object value)> _mruList;

    public MRUCacheService(int capacity = 5)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        _capacity = capacity;
        _cache = new Dictionary<string, LinkedListNode<(string key, object value)>>();
        _mruList = [];
    }

    public Maybe<TValue> Get<TValue>(string key)
    {
        if (!_cache.TryGetValue(key, out var node)) return Maybe<TValue>.None;

        _mruList.Remove(node);
        _mruList.AddFirst(node);

        return Maybe<TValue>.From((TValue)node.Value.value);
    }

    public void Put<TValue>(string key, TValue value)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            // Update the value and move the node to the front
            node.Value = (key, value);
            _mruList.Remove(node);
            _mruList.AddFirst(node);
        }
        else
        {
            if (_cache.Count >= _capacity)
            {
                // Remove the most recently used item (last node)
                var mruNode = _mruList.First;
                _mruList.RemoveFirst();
                _cache.Remove(mruNode!.Value.key);
            }

            // Add the new key-value pair
            var newNode = new LinkedListNode<(string key, object value)>((key, value));
            _mruList.AddFirst(newNode);
            _cache[key] = newNode;
        }
    }

    public void Delete<TValue>(string key)
    {
        if (!_cache.TryGetValue(key, out var node)) return;

        _mruList.Remove(node);
        _cache.Remove(key);
    }
}