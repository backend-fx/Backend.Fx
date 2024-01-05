using System;
using Backend.Fx.Exceptions;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheConflictedException
{
    [Fact]
    public void CanBeInstantiated()
    {
        var unused1 = new ConflictedException();
        var unused2 = new ConflictedException("With a message");
        var unused3 = new ConflictedException("With a message and an inner", new Exception());
    }
}