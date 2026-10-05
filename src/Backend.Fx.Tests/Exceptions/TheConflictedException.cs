using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheConflictedException
{
    [Fact]
    public void CanBeInstantiated()
    {
        _ = new ConflictedException();
        _ = new ConflictedException("With a message");
        _ = new ConflictedException("With a message and an inner", new Exception());
    }
}
