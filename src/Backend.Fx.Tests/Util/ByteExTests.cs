using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class ByteExTests
{
    [Theory]
    [InlineData(1, 1_000)]
    [InlineData(2, 2_000)]
    [InlineData(0, 0)]
    public void KbReturnsDecimalKilobytes(int input, int expected)
        => Assert.Equal(expected, input.KB());

    [Theory]
    [InlineData(1, 1_000_000)]
    [InlineData(2, 2_000_000)]
    public void MbReturnsDecimalMegabytes(int input, int expected)
        => Assert.Equal(expected, input.MB());

    [Theory]
    [InlineData(1, 1_000_000_000)]
    public void GbReturnsDecimalGigabytes(int input, int expected)
        => Assert.Equal(expected, input.GB());

    [Theory]
    [InlineData(1, 1_000_000_000_000L)]
    [InlineData(2, 2_000_000_000_000L)]
    public void TbReturnsDecimalTerabytes(int input, long expected)
        => Assert.Equal(expected, input.TB());

    [Theory]
    [InlineData(1, 1_024)]
    [InlineData(2, 2_048)]
    [InlineData(0, 0)]
    public void KiBReturnsBinaryKibibytes(int input, int expected)
        => Assert.Equal(expected, input.KiB());

    [Theory]
    [InlineData(1, 1_048_576)]
    [InlineData(2, 2_097_152)]
    public void MiBReturnsBinaryMebibytes(int input, int expected)
        => Assert.Equal(expected, input.MiB());

    [Theory]
    [InlineData(1, 1_073_741_824)]
    public void GiBReturnsBinaryGibibytes(int input, int expected)
        => Assert.Equal(expected, input.GiB());

    [Theory]
    [InlineData(1, 1_099_511_627_776L)]
    [InlineData(2, 2_199_023_255_552L)]
    public void TiBReturnsBinaryTebibytes(int input, long expected)
        => Assert.Equal(expected, input.TiB());

    [Theory]
    [InlineData(0UL, "0 B")]
    [InlineData(500UL, "500 B")]
    // 1000 is the (decimal) threshold, but the switch only matches values strictly greater than it
    [InlineData(1000UL, "1000 B")]
    // values are scaled by the binary unit (1024), so 1024 bytes is exactly 1 KB
    [InlineData(1024UL, "1 KB")]
    [InlineData(2048UL, "2 KB")]
    [InlineData(1_048_576UL, "1 MB")]
    [InlineData(1_073_741_824UL, "1 GB")]
    [InlineData(1_099_511_627_776UL, "1 TB")]
    public void ToHumanReadableFormatsUlong(ulong size, string expected)
        => Assert.Equal(expected, size.ToHumanReadable());

    [Fact]
    public void ToHumanReadableRoundsToOneDecimalPlace()
    {
        Assert.Equal("1 KB", 1024.ToHumanReadable());
        Assert.Equal("1.5 KB", 1555.ToHumanReadable());
        Assert.Equal("1.6 KB", 1600.ToHumanReadable());
        Assert.Equal("1.5 MB", 1_600_000.ToHumanReadable());
    }

    [Fact]
    public void ToHumanReadableDelegatesForAllIntegerOverloads()
    {
        Assert.Equal("2 KB", ((short)2048).ToHumanReadable());
        Assert.Equal("2 KB", ((ushort)2048).ToHumanReadable());
        Assert.Equal("1 MB", 1_048_576.ToHumanReadable());
        Assert.Equal("1 GB", 1_073_741_824u.ToHumanReadable());
        Assert.Equal("1 TB", 1_099_511_627_776L.ToHumanReadable());
    }
}
