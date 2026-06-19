using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using System.Reflection;

namespace BitcoinMarketLoader.UnitTests.Mocks;

/// <summary>
/// Helper class for creating mocked instances for testing 
/// </summary>
public static class MockHelper
{
    public const string DefaultInstrumentName = "BTC-EUR";
    public const string DefaultCurrencyName = "BTC-EUR";

    public static MarketTick CreateMarketTick(long ccseq, string? instrumentName = DefaultInstrumentName) =>
        CreateMockedInstance<MarketTick>(true, t =>
        {
            t.Ccseq = ccseq;
            t.Instrument = instrumentName;
            t.QuoteCurrency = new MarketCurrency { Name = DefaultCurrencyName };
        });

    public static MarketTickDto CreateMarketTickDto(long ccseq, ICollection<string>? instruments = null) =>
        new()
        {
            Data = (instruments ?? [DefaultInstrumentName])
                .ToDictionary(name => name, name => CreateMockedInstance<InstrumentDataDto>(true, i =>
                {
                    i.Ccseq = ccseq;
                    i.Instrument = name;
                }))
        };

    /// <summary>
    /// Create an instance with filled scalar values
    /// </summary>
    /// <param name="randomNumbers">True to randomize values of numeric properties. False to set them all to 1.</param>
    /// <param name="updateAction">Optional update action to run on the new instance. Useful for fluent generation of mocked instances.</param>
    /// <typeparam name="T">Type of the instance to create. Has to have a parameterless constructor</typeparam>
    public static T CreateMockedInstance<T>(bool randomNumbers = true, Action<T>? updateAction = null) where T : new()
    {
        var instance = new T();

        foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            object? value;
            var valueCreated = randomNumbers
                ? TryCreateRandomScalarValue(property.PropertyType, property.Name, out value)
                : TryCreateScalarValue(property.PropertyType, property.Name, out value);
            if (property.SetMethod?.IsPublic != true ||
                property.GetIndexParameters().Length != 0 ||
                !valueCreated)
            {
                continue;
            }

            property.SetValue(instance, value);
        }

        updateAction?.Invoke(instance);

        return instance;
    }

    private static bool TryCreateScalarValue(Type type, string propertyName, out object? value)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        value =
            type == typeof(string) ? $"{propertyName}Mock" :
            type == typeof(bool) ? true :
            type == typeof(byte) ? (byte)1 :
            type == typeof(sbyte) ? (sbyte)1 :
            type == typeof(short) ? (short)1 :
            type == typeof(ushort) ? (ushort)1 :
            type == typeof(int) ? 1 :
            type == typeof(uint) ? (uint)1 :
            type == typeof(long) ? 1L :
            type == typeof(ulong) ? 1L :
            type == typeof(float) ? 1.0 :
            type == typeof(double) ? 1.0D :
            type == typeof(decimal) ? 1M :
            type == typeof(char) ? 'M' :
            type == typeof(Guid) ? Guid.NewGuid() :
            type == typeof(DateTime) ? DateTime.UtcNow :
            type == typeof(DateTimeOffset) ? DateTimeOffset.UtcNow :
            type == typeof(TimeSpan) ? TimeSpan.FromMinutes(1) :
            null;

        return value is not null;
    }
    
    private static bool TryCreateRandomScalarValue(Type type, string propertyName, out object? value)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        value =
            type == typeof(string) ? $"{propertyName}Mock" :
            type == typeof(bool) ? true :
            type == typeof(byte) ? (byte)Random.Shared.Next(byte.MaxValue) :
            type == typeof(sbyte) ? (sbyte)Random.Shared.Next(sbyte.MaxValue) :
            type == typeof(short) ? (short)Random.Shared.Next(1000) :
            type == typeof(ushort) ? (ushort)Random.Shared.Next(1000) :
            type == typeof(int) ? Random.Shared.Next(1000) :
            type == typeof(uint) ? (uint)Random.Shared.NextInt64(1000) :
            type == typeof(long) ? Random.Shared.NextInt64(1000) :
            type == typeof(ulong) ? (ulong)Random.Shared.NextInt64(1000) :
            type == typeof(float) ? Random.Shared.NextSingle() * 1000F :
            type == typeof(double) ? Random.Shared.NextDouble() * 1000D :
            type == typeof(decimal) ? (decimal)Random.Shared.NextDouble() * 1000M :
            type == typeof(char) ? 'M' :
            type == typeof(Guid) ? Guid.NewGuid() :
            type == typeof(DateTime) ? DateTime.UtcNow :
            type == typeof(DateTimeOffset) ? DateTimeOffset.UtcNow :
            type == typeof(TimeSpan) ? TimeSpan.FromMinutes(1) :
            null;

        return value is not null;
    }

}
