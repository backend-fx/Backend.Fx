using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheUnprocessableException
{
    [Fact]
    public void CanBeInstantiated()
    {
        var unused1 = new UnprocessableException();
        var unused2 = new UnprocessableException("With a message");
        var unused3 = new UnprocessableException("With a message and an inner", new Exception());
    }
}