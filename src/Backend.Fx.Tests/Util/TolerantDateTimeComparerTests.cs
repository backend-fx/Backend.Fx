using System;
using Backend.Fx.Util;
using NodaTime;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class TolerantDateTimeComparerTests
{
    private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

    private readonly TolerantDateTimeComparer _sut = new(OneSecond);

    [Fact]
    public void ConsidersValuesWithinEpsilonEqual()
    {
        var now = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.True(_sut.Equals(now, now.AddMilliseconds(500)));
    }

    [Fact]
    public void ConsidersValuesOutsideEpsilonUnequal()
    {
        var now = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.False(_sut.Equals(now, now.AddSeconds(2)));
    }

    [Fact]
    public void ConsidersTwoNullsEqual()
        => Assert.True(_sut.Equals(null, null));

    [Fact]
    public void ConsidersNullAndValueUnequal()
    {
        var now = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.False(_sut.Equals(null, now));
        Assert.False(_sut.Equals(now, null));
    }

    [Fact]
    public void GetHashCodeReturnsZeroForNull()
        => Assert.Equal(0, _sut.GetHashCode(null));
}

public class TolerantDateTimeOffsetComparerTests
{
    private readonly TolerantDateTimeOffsetComparer _sut = new(TimeSpan.FromSeconds(1));

    [Fact]
    public void ConsidersValuesWithinEpsilonEqual()
    {
        var now = new DateTimeOffset(2020, 1, 1, 12, 0, 0, TimeSpan.Zero);
        Assert.True(_sut.Equals(now, now.AddMilliseconds(500)));
    }

    [Fact]
    public void ConsidersValuesOutsideEpsilonUnequal()
    {
        var now = new DateTimeOffset(2020, 1, 1, 12, 0, 0, TimeSpan.Zero);
        Assert.False(_sut.Equals(now, now.AddSeconds(2)));
    }

    [Fact]
    public void ConsidersTwoNullsEqual()
        => Assert.True(_sut.Equals(null, null));

    [Fact]
    public void GetHashCodeReturnsZeroForNull()
        => Assert.Equal(0, _sut.GetHashCode(null));
}

public class TolerantInstantComparerTests
{
    private readonly TolerantInstantComparer _sut = new(Duration.FromSeconds(1));

    [Fact]
    public void ConsidersValuesWithinEpsilonEqual()
    {
        var now = Instant.FromUtc(2020, 1, 1, 12, 0);
        Assert.True(_sut.Equals(now, now.Plus(Duration.FromMilliseconds(500))));
    }

    [Fact]
    public void ConsidersValuesOutsideEpsilonUnequal()
    {
        var now = Instant.FromUtc(2020, 1, 1, 12, 0);
        Assert.False(_sut.Equals(now, now.Plus(Duration.FromSeconds(2))));
    }

    [Fact]
    public void ConsidersTwoNullsEqual()
        => Assert.True(_sut.Equals(null, null));

    [Fact]
    public void ConsidersNullAndValueUnequal()
    {
        var now = Instant.FromUtc(2020, 1, 1, 12, 0);
        Assert.False(_sut.Equals(null, now));
        Assert.False(_sut.Equals(now, null));
    }

    [Fact]
    public void GetHashCodeReturnsZeroForNull()
        => Assert.Equal(0, _sut.GetHashCode(null));
}
