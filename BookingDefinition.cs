namespace CapabilityComposedFluentApi;

public sealed class BookingDefinition
{
    private readonly List<Booking> _bookings = [];

    public IReadOnlyList<Booking> Bookings => _bookings;

    public BookingDefinition Economy(
        string route,
        Action<IEconomyBookingBuilder> configure)
    {
        var draft = new BookingDraft("Economy", route);
        configure(new EconomyBookingBuilder(draft));
        Materialize(draft);
        return this;
    }

    public BookingDefinition Business(
        string route,
        Action<IBusinessBookingBuilder> configure)
    {
        var draft = new BookingDraft("Business", route);
        configure(new BusinessBookingBuilder(draft));
        Materialize(draft);
        return this;
    }

    private void Materialize(BookingDraft draft)
    {
        _bookings.Add(new Booking(
            draft.CabinClass,
            draft.Route,
            draft.Seat,
            [.. draft.CabinBags],
            [.. draft.CheckedBags]));
    }
}
