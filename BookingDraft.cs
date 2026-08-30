namespace CapabilityComposedFluentApi;

internal sealed class BookingDraft(string cabinClass, string route)
{
    public string CabinClass { get; } = cabinClass;

    public string Route { get; } = route;

    public string? Seat { get; set; }

    public List<Bag> CabinBags { get; } = [];

    public List<Bag> CheckedBags { get; } = [];
}
