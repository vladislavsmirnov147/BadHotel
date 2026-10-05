namespace BadHotel.Core;

public interface IRepository<T> where T : HotelWorker
{
    void Add(T item);

    bool Remove(T item);

    IEnumerable<T> GetAll();

    T? Find(Func<T, bool> predicate);
}