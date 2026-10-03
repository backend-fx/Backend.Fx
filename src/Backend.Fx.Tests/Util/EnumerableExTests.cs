using System.Collections.Generic;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class EnumerableExTests
{
    [Fact]
    public void ForAllInvokesActionForEveryItemInOrder()
    {
        var collected = new List<int>();

        new[] { 1, 2, 3 }.ForAll(collected.Add);

        Assert.Equal(new[] { 1, 2, 3 }, collected);
    }

    [Fact]
    public void ForAllDoesNothingForEmptySequence()
    {
        var invoked = false;

        System.Array.Empty<int>().ForAll(_ => invoked = true);

        Assert.False(invoked);
    }
}
