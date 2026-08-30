using CapabilityComposedFluentApi;

var definition = new BookingDefinition()
    .Economy("MAD-LHR", builder => builder
        .WithSeat("12A")
        .WithCabinBag(Bag.Small))
    .Business("MAD-JFK", builder => builder
        .WithSeat("2A")
        .WithCabinBag(Bag.Small)
        .WithCheckedBag(Bag.Large));

foreach (var booking in definition.Bookings)
{
    Console.WriteLine(
        $"{booking.CabinClass} {booking.Route} | " +
        $"seat {booking.Seat} | " +
        $"cabin bags {booking.CabinBags.Count} | " +
        $"checked bags {booking.CheckedBags.Count}");
}
