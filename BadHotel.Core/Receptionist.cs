namespace BadHotel.Core;

public class Receptionist : HotelWorker, IServe
{
	public Receptionist(string name) : base(name)
	{
	}

	public override string Work()
	{
		if (!TryUseEnergy(10))
		{
			return $"{Name} is too tired to work.";
		}

		return $"{Name} checks in a new hotel guest. Energy: {Energy}.";
	}

	public string ServeGuest()
	{
		if (!TryUseEnergy(5))
		{
			return $"{Name} is too tired to serve a guest.";
		}

		return $"{Name} gives the guest a room key. Energy: {Energy}.";
	}

	public override string CrazyAction()
	{
		if (!TryUseEnergy(15))
		{
			return $"{Name} is too tired for a crazy action.";
		}

		return $"{Name} accidentally gives a guest the key to the storage room! Energy: {Energy}.";
	}
}