namespace BadHotel.Core;

public abstract class HotelWorker
{
    public string Name { get; }

    public int Energy { get; private set; }

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

    protected bool TryUseEnergy(int amount)
    {
        if (amount <= 0 || Energy < amount)
        {
            return false;
        }

        Energy -= amount;
        return true;
    }
}