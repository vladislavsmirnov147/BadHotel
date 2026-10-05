namespace BadHotel.Core;

public class RepositoryReactions<T> where T : HotelWorker
{
    private readonly Repository<T> _repository;
    private readonly Dictionary<T, WorkerSubscriptions> _subscriptions = new();

    public event Action<string>? ReactionCreated;

    public RepositoryReactions(Repository<T> repository)
    {
        _repository = repository;

        _repository.ItemAdded += OnItemAdded;
        _repository.ItemRemoved += OnItemRemoved;
    }

    private void OnItemAdded(object? sender, RepositoryEventArgs<T> e)
    {
        foreach (T worker in _repository.GetAll())
        {
            if (!ReferenceEquals(worker, e.Actor))
            {
                ReactionCreated?.Invoke(worker.ReactToAdded(e.Actor));
            }
        }

        EventHandler<RepositoryEventArgs<T>> addedHandler = (_, args) =>
        {
            if (!ReferenceEquals(e.Actor, args.Actor))
            {
                ReactionCreated?.Invoke(e.Actor.ReactToAdded(args.Actor));
            }
        };

        EventHandler<RepositoryEventArgs<T>> removedHandler = (_, args) =>
        {
            if (!ReferenceEquals(e.Actor, args.Actor))
            {
                ReactionCreated?.Invoke(e.Actor.ReactToRemoved(args.Actor));
            }
        };

        EventHandler<RepositoryEventArgs<T>> crazyHandler = (_, args) =>
        {
            if (!ReferenceEquals(e.Actor, args.Actor))
            {
                ReactionCreated?.Invoke(e.Actor.ReactToCrazyAction(args.Actor));
            }
        };

        _subscriptions[e.Actor] = new WorkerSubscriptions(
            addedHandler,
            removedHandler,
            crazyHandler);

        _repository.ItemAdded += addedHandler;
        _repository.ItemRemoved += removedHandler;
        _repository.CrazyActionPerformed += crazyHandler;
    }

    private void OnItemRemoved(object? sender, RepositoryEventArgs<T> e)
    {
        if (_subscriptions.TryGetValue(e.Actor, out WorkerSubscriptions? subscription))
        {
            _repository.ItemAdded -= subscription.AddedHandler;
            _repository.ItemRemoved -= subscription.RemovedHandler;
            _repository.CrazyActionPerformed -= subscription.CrazyHandler;

            _subscriptions.Remove(e.Actor);
        }
    }

    private sealed class WorkerSubscriptions
    {
        public EventHandler<RepositoryEventArgs<T>> AddedHandler { get; }

        public EventHandler<RepositoryEventArgs<T>> RemovedHandler { get; }

        public EventHandler<RepositoryEventArgs<T>> CrazyHandler { get; }

        public WorkerSubscriptions(
            EventHandler<RepositoryEventArgs<T>> addedHandler,
            EventHandler<RepositoryEventArgs<T>> removedHandler,
            EventHandler<RepositoryEventArgs<T>> crazyHandler)
        {
            AddedHandler = addedHandler;
            RemovedHandler = removedHandler;
            CrazyHandler = crazyHandler;
        }
    }
}