# Capability-composed fluent API

A small .NET 10 example of a fluent API where each builder exposes only the
operations its domain supports. It accompanies
[Make illegal calls unrepresentable](https://www.metaengine.eu/articles/capability-composed-fluent-api).

```csharp
var definition = new BookingDefinition()
    .Economy("MAD-LHR", builder => builder
        .WithSeat("12A")
        .WithCabinBag(Bag.Small))
    .Business("MAD-JFK", builder => builder
        .WithSeat("2A")
        .WithCabinBag(Bag.Small)
        .WithCheckedBag(Bag.Large));
```

`IEconomyBookingBuilder` composes seat and cabin-luggage capabilities.
`IBusinessBookingBuilder` also composes checked luggage. Consequently,
`WithCheckedBag` is absent from the economy callback's static type, including
after a chained call such as `WithSeat`.

Concrete builders are internal and receive a mutable draft. `BookingDefinition`
materializes a booking when the callback returns, copying its luggage lists.
The enclosing definition owns that boundary, so there is no terminal `.Build()`
method.

## Run the sample

Install [Git](https://git-scm.com/downloads) and the
[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). The project is a
console app; it requires the SDK to build and run.

```bash
git clone https://github.com/meta-engine/capability-composed-fluent-api.git
cd capability-composed-fluent-api
dotnet run --configuration Release
```

Expected output:

```text
Economy MAD-LHR | seat 12A | cabin bags 1 | checked bags 0
Business MAD-JFK | seat 2A | cabin bags 1 | checked bags 1
```

## Verify the compile contract

The verification scripts require Bash on macOS, Linux, or Windows through WSL
(with the SDK installed inside WSL). Run both commands used by CI:

```bash
./scripts/verify.sh
./scripts/test-verifier.sh
```

`verify.sh` builds Release with warnings treated as errors, checks the valid
example's output, and compiles the excluded negative example. It requires
`CS1061` for `WithCheckedBag` on `IEconomyBookingBuilder` in that fixture and
rejects every unrelated compiler error, including another `CS1061`.

`test-verifier.sh` checks the verifier against temporary copies of the sample:
with an extra missing-member error, with a different missing member, and with
an economy chain that compiles. Each must be rejected for the intended reason.
The original sources remain unchanged.

## Read the implementation

| File | Responsibility |
| --- | --- |
| [Program.cs](Program.cs) | The two valid fluent chains and their output |
| [BookingBuilderContracts.cs](BookingBuilderContracts.cs) | Small capability interfaces and the public surface for each cabin class |
| [BookingBuilders.cs](BookingBuilders.cs) | Internal builders that update an injected draft and return their public interface |
| [BookingDefinition.cs](BookingDefinition.cs) | Callback lifecycle and materialization |
| [BookingDraft.cs](BookingDraft.cs), [Booking.cs](Booking.cs), [Bag.cs](Bag.cs) | Mutable configuration and the resulting data |
| [CompileContracts/InvalidEconomyBooking.cs](CompileContracts/InvalidEconomyBooking.cs) | The economy call that must not compile |

## Scope

The fare rules are a teaching model: economy allows cabin luggage, and business
also allows checked luggage. They do not describe a particular airline.

The compiler restricts which methods callers can use through the typed API.
It does not validate seat names, routes, bag weights, availability, or input
received as JSON. This sample omits those runtime rules so the capability
composition remains easy to follow.

Calls do not transition through successive state types. Each callback keeps a
fixed public interface; the self type on each capability preserves that
interface throughout the fluent chain.

## License

[MIT](LICENSE)
