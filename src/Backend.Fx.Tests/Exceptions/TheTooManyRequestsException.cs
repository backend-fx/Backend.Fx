using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheTooManyRequestsException
{
    [Fact]
    public void CanBeInstantiated()
    {
        _ = new TooManyRequestsException(5);
        _ = new TooManyRequestsException(5, "With a message");
        _ = new TooManyRequestsException(5, "With a message and an inner", new Exception());
    }

    [Fact]
    public void KeepsRetryAfter()
    {
        var exception1 = new TooManyRequestsException(5);
        Assert.Equal(5, exception1.RetryAfter);
    }
}
