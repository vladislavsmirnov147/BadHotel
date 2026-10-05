using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BadHotel.Core;

public abstract class HotelWorker : INotifyPropertyChanged
{
    private int _energy;

    public string Name { get; }

    public int Energy
    {
        get => _energy;
        private set
        {
            if (_energy == value)
            {
                return;
            }

            _energy = value;
            OnPropertyChanged();
        }
    }

    protected HotelWorker(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        Name = name.Trim();
        Energy = 100;
    }

    public abstract string Work();

    public abstract string CrazyAction();

    public virtual string ReactToAdded(HotelWorker worker)
    {
        return $"{Name} welcomes {worker.Name} to the Bad Hotel.";
    }

    public virtual string ReactToRemoved(HotelWorker worker)
    {
        return $"{Name} says goodbye to {worker.Name}.";
    }

    public virtual string ReactToCrazyAction(HotelWorker worker)
    {
        RestoreEnergy(5);
        return $"{Name} reacts to {worker.Name}'s crazy action and gains 5 energy. Energy: {Energy}.";
    }

    protected bool TryUseEnergy(int amount)
    {
        if (amount <= 0 || Energy < amount)
        {
            return false;
        }

        Energy -= amount;
        return true;
    }

    private void RestoreEnergy(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Energy = Math.Min(100, Energy + amount);
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