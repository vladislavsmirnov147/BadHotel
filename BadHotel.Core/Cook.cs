namespace BadHotel.Core;

public class Cook : HotelWorker, ICook, IServe
{
    public Cook(string name) : base(name)
    {
    }

    public override string Work()
    {
        if (!TryUseEnergy(10))
        {
            return $"{Name} is too tired to work.";
        }

        return $"{Name} prepares breakfast for the hotel guests. Energy: {Energy}.";
    }

    public string CookMeal()
    {
        if (!TryUseEnergy(10))
        {
            return $"{Name} is too tired to cook.";
        }

        return $"{Name} cooks today's special meal. Energy: {Energy}.";
    }

    public string ServeGuest()
    {
        if (!TryUseEnergy(5))
        {
            return $"{Name} is too tired to serve a guest.";
        }

        return $"{Name} serves a meal to a hungry guest. Energy: {Energy}.";
    }

    public override string CrazyAction()
    {
        if (!TryUseEnergy(20))
        {
            return $"{Name} is too tired for a crazy action.";
        }

        return $"{Name} invents a mystery soup from everything left in the hotel fridge! Energy: {Energy}.";
    }
}