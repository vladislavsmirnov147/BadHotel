namespace BadHotel.Core;

public class Cleaner : HotelWorker, IClean
{
    public Cleaner(string name) : base(name)
    {
    }

    public override string Work()
    {
        if (!TryUseEnergy(10))
        {
            return $"{Name} is too tired to work.";
        }

        return $"{Name} cleans a hotel room. Energy: {Energy}.";
    }

    public string CleanRoom()
    {
        if (!TryUseEnergy(5))
        {
            return $"{Name} is too tired to clean the room.";
        }

        return $"{Name} makes the room perfectly clean. Energy: {Energy}.";
    }

    public override string CrazyAction()
    {
        if (!TryUseEnergy(20))
        {
            return $"{Name} is too tired for a crazy action.";
        }

        return $"{Name} turns the vacuum cleaner to maximum power and accidentally sucks up a guest's sock! Energy: {Energy}.";
    }
}