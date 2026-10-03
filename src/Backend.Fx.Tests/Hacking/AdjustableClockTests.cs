using Backend.Fx.Hacking;
using NodaTime;
using Xunit;

namespace Backend.Fx.Tests.Hacking;

public class AdjustableClockTests
{
    private sealed class FixedClock : IClock
    {
        private readonly Instant _instant;
        public FixedClock(Instant instant) => _instant = instant;
        public Instant GetCurrentInstant() => _instant;
    }

    private static readonly Instant Base = Instant.FromUtc(2020, 1, 1, 0, 0);

    [Fact]
    public void DelegatesToUnderlyingClockByDefault()
    {
        var sut = new AdjustableClock(new FixedClock(Base));
        Assert.Equal(Base, sut.GetCurrentInstant());
    }

    [Fact]
    public void OverrideUtcNowReplacesCurrentInstant()
    {
        var sut = new AdjustableClock(new FixedClock(Base));
        var overridden = Instant.FromUtc(2030, 6, 15, 12, 0);

        sut.OverrideUtcNow(overridden);

        Assert.Equal(overridden, sut.GetCurrentInstant());
    }

    [Fact]
    public void AdvanceMovesClockForward()
    {
        var sut = new AdjustableClock(new FixedClock(Base));

        var result = sut.Advance(Duration.FromHours(2));

        Assert.Equal(Base.Plus(Duration.FromHours(2)), result);
        Assert.Equal(Base.Plus(Duration.FromHours(2)), sut.GetCurrentInstant());
    }

    [Fact]
    public void AddSkewShiftsCurrentInstantAndClearSkewResetsIt()
    {
        var sut = new AdjustableClock(new FixedClock(Base));

        sut.AddSkew(Duration.FromMinutes(30));
        Assert.Equal(Base.Plus(Duration.FromMinutes(30)), sut.GetCurrentInstant());

        sut.ClearSkew();
        Assert.Equal(Base, sut.GetCurrentInstant());
    }
}
