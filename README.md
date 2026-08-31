# Capability-composed fluent API

A deliberately small .NET 10 example of a fluent API where each builder exposes
only the operations its domain supports.

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
`WithCheckedBag` is absent from the economy callback's static type.

This follows a callback-scoped builder shape: concrete builders are internal,
receive a mutable draft, and `BookingDefinition` materializes that draft when the
callback returns. There is intentionally no terminal `.Build()` method.

## Verify

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download). The sample is a
console app, not a library: the SDK is required to build, run, and prove the
compile contract.

```bash
./scripts/verify.sh
```

The script builds Release with warnings treated as errors, checks the valid
example's output, and compiles the excluded negative example to prove that an
economy booking with `WithCheckedBag` fails with `CS1061`.

## License

[MIT](LICENSE)
