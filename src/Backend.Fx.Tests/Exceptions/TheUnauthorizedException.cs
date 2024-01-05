using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheUnauthorizedException
{
    [Fact]
    public void CanBeInstantiated()
    {
        var unused1 = new UnauthorizedException();
        var unused2 = new UnauthorizedException("With a message");
        var unused3 = new UnauthorizedException("With a message and an inner", new Exception());
    }
}