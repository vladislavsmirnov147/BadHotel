namespace BadHotel.Core;

public class RepositoryEventArgs<T> : EventArgs where T : HotelWorker
{
    public T Actor { get; }

    public RepositoryEventArgs(T actor)
    {
        Actor = actor;
    }
}