namespace BlazorTrip.Infrastructure.InMemory;

public class InMemoryDao<T, TKey>(Func<T, TKey> keySelector) where T : class where TKey : IEquatable<TKey>
{
    private Dictionary<TKey, T> _items = new();

    public IQueryable<T> Items => _items.Values.AsQueryable();

    public bool Exists(params TKey[] keys)
    {
        return keys.All(_items.ContainsKey);
    }

    public void Set(IEnumerable<T> data)
    {
        _items = data.ToDictionary(keySelector);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public T? Get(TKey key)
    {
        return _items.GetValueOrDefault(key);
    }
    
    public T? GetWhere(Func<T, bool> predicate)
    {
        return _items.Values.FirstOrDefault(predicate);
    }

    public bool Add(T item)
    {
        var key = keySelector(item);
        return _items.TryAdd(key, item);
    }

    public bool Update(T item)
    {
        var key = keySelector(item);
        if (!_items.ContainsKey(key)) return false;
        _items[key] = item;
        return true;
    }

    public bool UpdateWhere(Func<T, bool> predicate, Func<T, T> updater)
    {
        var keysToUpdate = _items.Where(s => predicate(s.Value)).Select(s => s.Key).ToList();
        if (keysToUpdate.Count == 0) return false;
        foreach (var key in keysToUpdate)
        {
            var existing = _items[key];
            _items[key] = updater(existing);
        }

        return true;
    }

    public bool Remove(TKey key)
    {
        return _items.Remove(key);
    }

    public bool RemoveWhere(Func<T, bool> predicate)
    {
        var keysToRemove = _items.Where((s) => predicate(s.Value)).Select(s => s.Key).ToList();
        if (keysToRemove.Count == 0) return false;
        keysToRemove.ForEach(key => _items.Remove(key));
        return true;
    }
}