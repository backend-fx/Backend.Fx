using Backend.Fx.Exceptions;
using JetBrains.Annotations;
using Xunit;

namespace Backend.Fx.Tests.Exceptions;

public class TheNotFoundException
{
    [Fact]
    public void CanBeThrownWithoutAnyParameters()
    {
        var exception = new NotFoundException();
        Assert.Null(exception.EntityName);
    }
        
    [Fact]
    public void FillsNameAndIdProperties()
    {
        var exception = new NotFoundException<SomeEntity>(4711);
        Assert.Equal("SomeEntity", exception.EntityName);
        Assert.Equal(4711, exception.Id);
    }


    [UsedImplicitly]
    private class SomeEntity(int id)
    {
        [UsedImplicitly]
        public int Id { get; } = id;
    }
}