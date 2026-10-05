using System;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class StringEnumUtilTests
{
    public enum Color
    {
        Red,
        Green,
        Blue,
    }

    [Theory]
    [InlineData("Red", Color.Red)]
    [InlineData("Green", Color.Green)]
    [InlineData("Blue", Color.Blue)]
    public void ParsesExactName(string value, Color expected) =>
        Assert.Equal(expected, value.Parse<Color>());

    [Theory]
    [InlineData("red", Color.Red)]
    [InlineData("GREEN", Color.Green)]
    [InlineData("bLuE", Color.Blue)]
    public void ParsesCaseInsensitively(string value, Color expected) =>
        Assert.Equal(expected, value.Parse<Color>());

    [Fact]
    public void ThrowsArgumentExceptionForInvalidValue()
    {
        var exception = Assert.Throws<ArgumentException>(() => "Purple".Parse<Color>());
        Assert.Contains("Purple", exception.Message);
        Assert.Contains("Red", exception.Message);
    }
}
