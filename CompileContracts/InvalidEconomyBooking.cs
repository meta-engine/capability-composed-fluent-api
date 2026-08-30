using CapabilityComposedFluentApi;

namespace CompileContracts;

internal static class InvalidEconomyBooking
{
    public static void Configure(BookingDefinition definition)
    {
        definition.Economy("MAD-LHR", builder => builder
            .WithSeat("12A")
            .WithCabinBag(Bag.Small)
            .WithCheckedBag(Bag.Large));
    }
}
