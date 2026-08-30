namespace CapabilityComposedFluentApi;

public sealed record Booking(
    string CabinClass,
    string Route,
    string? Seat,
    IReadOnlyList<Bag> CabinBags,
    IReadOnlyList<Bag> CheckedBags);
