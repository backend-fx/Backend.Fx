using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class StringExTests
{
    [Fact]
    public void CutReturnsEmptyUnchanged()
        => Assert.Equal(string.Empty, string.Empty.Cut(5));

    [Fact]
    public void CutReturnsNullUnchanged()
        => Assert.Null(((string)null!).Cut(5));

    [Fact]
    public void CutKeepsStringShorterThanLength()
        => Assert.Equal("abc", "abc".Cut(5));

    [Fact]
    public void CutKeepsStringOfExactLength()
        => Assert.Equal("abc", "abc".Cut(3));

    [Fact]
    public void CutTruncatesLongerStringAndAppendsEllipsis()
        => Assert.Equal("ab\u2026", "abcd".Cut(3));

    [Fact]
    public void CutToLengthOneReturnsOnlyEllipsis()
        => Assert.Equal("\u2026", "abcde".Cut(1));

    [Fact]
    public void ToUnixLineEndingNormalizesAllVariants()
        => Assert.Equal("a\nb\nc\nd", "a\r\nb\rc\nd".ToUnixLineEnding());

    [Fact]
    public void ToMacintoshLineEndingNormalizesAllVariants()
        => Assert.Equal("a\rb\rc\rd", "a\r\nb\rc\nd".ToMacintoshLineEnding());
}
