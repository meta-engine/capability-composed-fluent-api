namespace CapabilityComposedFluentApi;

internal abstract class BookingBuilder<TSelf>(BookingDraft draft) :
    ISeatCapability<TSelf>,
    ICabinLuggageCapability<TSelf>
{
    protected BookingDraft Draft { get; } = draft;

    protected abstract TSelf Self { get; }

    public TSelf WithSeat(string seat)
    {
        Draft.Seat = seat;
        return Self;
    }

    public TSelf WithCabinBag(Bag bag)
    {
        Draft.CabinBags.Add(bag);
        return Self;
    }
}

internal sealed class EconomyBookingBuilder(BookingDraft draft) :
    BookingBuilder<IEconomyBookingBuilder>(draft),
    IEconomyBookingBuilder
{
    protected override IEconomyBookingBuilder Self => this;
}

internal sealed class BusinessBookingBuilder(BookingDraft draft) :
    BookingBuilder<IBusinessBookingBuilder>(draft),
    IBusinessBookingBuilder
{
    protected override IBusinessBookingBuilder Self => this;

    public IBusinessBookingBuilder WithCheckedBag(Bag bag)
    {
        Draft.CheckedBags.Add(bag);
        return this;
    }
}
