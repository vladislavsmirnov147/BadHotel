using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BadHotel.Core;

namespace BadHotel.Wpf;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Repository<Receptionist> _receptionists = new();
    private readonly Repository<Cleaner> _cleaners = new();
    private readonly Repository<Cook> _cooks = new();

    private readonly RepositoryReactions<Receptionist> _receptionistReactions;
    private readonly RepositoryReactions<Cleaner> _cleanerReactions;
    private readonly RepositoryReactions<Cook> _cookReactions;

    private string _newWorkerName = string.Empty;
    private string? _selectedWorkerType;
    private string _selectedRepository = "All";
    private HotelWorker? _selectedWorker;
    private string _searchText = string.Empty;
    private bool _sortByName;

    public ObservableCollection<HotelWorker> Workers { get; } = new();

    public ObservableCollection<HotelWorker> DisplayedWorkers { get; } = new();

    public ObservableCollection<string> ActivityLog { get; } = new();

    public ObservableCollection<string> WorkerSummary { get; } = new();

    public ObservableCollection<string> WorkerTypes { get; } = new()
    {
        "Receptionist",
        "Cleaner",
        "Cook"
    };

    public ObservableCollection<string> RepositoryTypes { get; } = new()
    {
        "All",
        "Receptionist",
        "Cleaner",
        "Cook"
    };

    public string NewWorkerName
    {
        get => _newWorkerName;
        set
        {
            _newWorkerName = value;
            OnPropertyChanged();
            AddCommand.RaiseCanExecuteChanged();
        }
    }

    public string? SelectedWorkerType
    {
        get => _selectedWorkerType;
        set
        {
            _selectedWorkerType = value;
            OnPropertyChanged();
            AddCommand.RaiseCanExecuteChanged();
        }
    }

    public string SelectedRepository
    {
        get => _selectedRepository;
        set
        {
            _selectedRepository = value;
            OnPropertyChanged();
            SelectedWorker = null;
            UpdateDisplayedWorkers();
        }
    }

    public HotelWorker? SelectedWorker
    {
        get => _selectedWorker;
        set
        {
            _selectedWorker = value;
            OnPropertyChanged();
            WorkCommand.RaiseCanExecuteChanged();
            SpecialAbilityCommand.RaiseCanExecuteChanged();
            RemoveCommand.RaiseCanExecuteChanged();
            CrazyActionCommand.RaiseCanExecuteChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            UpdateDisplayedWorkers();
        }
    }

    public bool SortByName
    {
        get => _sortByName;
        set
        {
            _sortByName = value;
            OnPropertyChanged();
            UpdateDisplayedWorkers();
        }
    }

    public RelayCommand AddCommand { get; }

    public RelayCommand WorkCommand { get; }

    public RelayCommand SpecialAbilityCommand { get; }

    public RelayCommand RemoveCommand { get; }

    public RelayCommand CrazyActionCommand { get; }

    public MainViewModel()
    {
        _receptionistReactions = new RepositoryReactions<Receptionist>(_receptionists);
        _cleanerReactions = new RepositoryReactions<Cleaner>(_cleaners);
        _cookReactions = new RepositoryReactions<Cook>(_cooks);

        _receptionistReactions.ReactionCreated += AddReactionToLog;
        _cleanerReactions.ReactionCreated += AddReactionToLog;
        _cookReactions.ReactionCreated += AddReactionToLog;

        AddCommand = new RelayCommand(AddWorker, CanAddWorker);
        WorkCommand = new RelayCommand(
            DoWork,
            () => SelectedWorker is not null);
        SpecialAbilityCommand = new RelayCommand(
            DoSpecialAbility,
            () => SelectedWorker is not null);
        RemoveCommand = new RelayCommand(
            RemoveWorker,
            () => SelectedWorker is not null);
        CrazyActionCommand = new RelayCommand(
            DoCrazyAction,
            () => SelectedWorker is not null);

        UpdateDisplayedWorkers();
        UpdateWorkerSummary();
    }

    private void AddReactionToLog(string message)
    {
        ActivityLog.Add(message);
    }

    private bool CanAddWorker()
    {
        return !string.IsNullOrWhiteSpace(NewWorkerName)
            && !string.IsNullOrWhiteSpace(SelectedWorkerType);
    }

    private void AddWorker()
    {
        string name = NewWorkerName.Trim();

        switch (SelectedWorkerType)
        {
            case "Receptionist":
                Receptionist receptionist = new(name);
                _receptionists.Add(receptionist);
                Workers.Add(receptionist);
                ActivityLog.Add(
                    $"{receptionist.Name} joined the hotel as Receptionist.");
                SelectedWorker = receptionist;
                break;

            case "Cleaner":
                Cleaner cleaner = new(name);
                _cleaners.Add(cleaner);
                Workers.Add(cleaner);
                ActivityLog.Add(
                    $"{cleaner.Name} joined the hotel as Cleaner.");
                SelectedWorker = cleaner;
                break;

            case "Cook":
                Cook cook = new(name);
                _cooks.Add(cook);
                Workers.Add(cook);
                ActivityLog.Add(
                    $"{cook.Name} joined the hotel as Cook.");
                SelectedWorker = cook;
                break;
        }

        NewWorkerName = string.Empty;
        SelectedWorkerType = null;

        UpdateDisplayedWorkers();
        UpdateWorkerSummary();
    }

    private void DoWork()
    {
        if (SelectedWorker is null)
        {
            return;
        }

        ActivityLog.Add(SelectedWorker.Work());
    }

    private void DoSpecialAbility()
    {
        if (SelectedWorker is null)
        {
            return;
        }

        string message;

        if (SelectedWorker is ICook cook)
        {
            message = cook.CookMeal();
        }
        else if (SelectedWorker is IClean cleaner)
        {
            message = cleaner.CleanRoom();
        }
        else if (SelectedWorker is IServe server)
        {
            message = server.ServeGuest();
        }
        else
        {
            message = $"{SelectedWorker.Name} has no special ability.";
        }

        ActivityLog.Add(message);
    }

    private void RemoveWorker()
    {
        if (SelectedWorker is Receptionist receptionist)
        {
            _receptionists.Remove(receptionist);
        }
        else if (SelectedWorker is Cleaner cleaner)
        {
            _cleaners.Remove(cleaner);
        }
        else if (SelectedWorker is Cook cook)
        {
            _cooks.Remove(cook);
        }
        else
        {
            return;
        }

        HotelWorker removedWorker = SelectedWorker;

        Workers.Remove(removedWorker);
        ActivityLog.Add($"{removedWorker.Name} left the Bad Hotel.");

        SelectedWorker = null;

        UpdateDisplayedWorkers();
        UpdateWorkerSummary();
    }

    private void DoCrazyAction()
    {
        if (SelectedWorker is Receptionist receptionist)
        {
            ActivityLog.Add(
                _receptionists.PerformCrazyAction(receptionist));
        }
        else if (SelectedWorker is Cleaner cleaner)
        {
            ActivityLog.Add(
                _cleaners.PerformCrazyAction(cleaner));
        }
        else if (SelectedWorker is Cook cook)
        {
            ActivityLog.Add(
                _cooks.PerformCrazyAction(cook));
        }
    }

    private void UpdateDisplayedWorkers()
    {
        IEnumerable<HotelWorker> result = Workers;

        if (SelectedRepository != "All")
        {
            result = result.Where(
                worker => worker.GetType().Name == SelectedRepository);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            result = result.Where(
                worker => worker.Name.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (SortByName)
        {
            result = result.OrderBy(worker => worker.Name);
        }

        DisplayedWorkers.Clear();

        foreach (HotelWorker worker in result)
        {
            DisplayedWorkers.Add(worker);
        }
    }

    private void UpdateWorkerSummary()
    {
        var groups = Workers
            .GroupBy(worker => worker.GetType().Name)
            .OrderBy(group => group.Key);

        WorkerSummary.Clear();

        foreach (var group in groups)
        {
            WorkerSummary.Add($"{group.Key}: {group.Count()}");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}