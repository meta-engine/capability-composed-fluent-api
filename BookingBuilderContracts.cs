namespace CapabilityComposedFluentApi;

public interface ISeatCapability<out TSelf>
{
    TSelf WithSeat(string seat);
}

public interface ICabinLuggageCapability<out TSelf>
{
    TSelf WithCabinBag(Bag bag);
}

public interface ICheckedLuggageCapability<out TSelf>
{
    TSelf WithCheckedBag(Bag bag);
}

public interface IEconomyBookingBuilder :
    ISeatCapability<IEconomyBookingBuilder>,
    ICabinLuggageCapability<IEconomyBookingBuilder>
{
}

public interface IBusinessBookingBuilder :
    ISeatCapability<IBusinessBookingBuilder>,
    ICabinLuggageCapability<IBusinessBookingBuilder>,
    ICheckedLuggageCapability<IBusinessBookingBuilder>
{
}
