using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheUnauthorizedException
{
    [Fact]
    public void CanBeInstantiated()
    {
        _ = new UnauthorizedException();
        _ = new UnauthorizedException("With a message");
        _ = new UnauthorizedException("With a message and an inner", new Exception());
    }
}