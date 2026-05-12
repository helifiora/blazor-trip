using System.Collections;

namespace BlazorTrip.Web.Data;

public class DataSet<TKey, TEntity>(Func<TEntity, TKey> keySelectorFunc)
    : IEnumerable<TEntity>
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    private readonly Dictionary<TKey, TEntity> _entities = new();

    public void Add(TEntity entity)
    {
        var key = keySelectorFunc(entity);
        _entities.TryAdd(key, entity);
    }

    public bool Remove(TKey key)
    {
        return _entities.Remove(key);
    }

    public TEntity Get(TKey key)
    {
        return _entities[key] ?? throw new KeyNotFoundException();
    }

    public TEntity? GetOrNull(TKey key)
    {
        return _entities.GetValueOrDefault(key);
    }

    public void Update(TEntity entity)
    {
        var key = keySelectorFunc(entity);
        if (_entities.ContainsKey(key))
        {
            _entities[key] = entity;
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    public void Set(IEnumerable<TEntity> entities)
    {
        _entities.Clear();
        foreach (var entity in entities)
        {
            var key = keySelectorFunc(entity);
            _entities[key] = entity;
        }
    }

    public void Clear()
    {
        _entities.Clear();
    }

    public IEnumerator<TEntity> GetEnumerator()
    {
        return _entities.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}