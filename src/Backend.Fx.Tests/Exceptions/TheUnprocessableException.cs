using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheUnprocessableException
{
    [Fact]
    public void CanBeInstantiated()
    {
        _ = new UnprocessableException();
        _ = new UnprocessableException("With a message");
        _ = new UnprocessableException("With a message and an inner", new Exception());
    }
}
