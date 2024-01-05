using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheForbiddenException
{
    [Fact]
    public void CanBeInstantiated()
    {
        var unused1 = new ForbiddenException();
        var unused2 = new ForbiddenException("With a message");
        var unused3 = new ForbiddenException("With a message and an inner", new Exception());
    }
}