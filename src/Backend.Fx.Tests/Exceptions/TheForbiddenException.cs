using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheForbiddenException
{
    [Fact]
    public void CanBeInstantiated()
    {
        _ = new ForbiddenException();
        _ = new ForbiddenException("With a message");
        _ = new ForbiddenException("With a message and an inner", new Exception());
    }
}