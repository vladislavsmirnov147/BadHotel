namespace BadHotel.Core;

public class Repository<T> : IRepository<T> where T : HotelWorker
{
    private readonly List<T> _items = new();

    public event EventHandler<RepositoryEventArgs<T>>? ItemAdded;

    public event EventHandler<RepositoryEventArgs<T>>? ItemRemoved;

    public event EventHandler<RepositoryEventArgs<T>>? CrazyActionPerformed;

    public void Add(T item)
    {
        _items.Add(item);
        ItemAdded?.Invoke(this, new RepositoryEventArgs<T>(item));
    }

    public bool Remove(T item)
    {
        bool removed = _items.Remove(item);

        if (removed)
        {
            ItemRemoved?.Invoke(this, new RepositoryEventArgs<T>(item));
        }

        return removed;
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public T? Find(Func<T, bool> predicate)
    {
        return _items.FirstOrDefault(predicate);
    }

    public string PerformCrazyAction(T actor)
    {
        string message = actor.CrazyAction();
        CrazyActionPerformed?.Invoke(this, new RepositoryEventArgs<T>(actor));
        return message;
    }
}