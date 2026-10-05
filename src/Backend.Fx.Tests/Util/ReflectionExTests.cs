using System.Collections.Generic;
using System.Linq;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class ReflectionExTests
{
    private interface IAnimal;

    // ReSharper disable once UnusedTypeParameter
    private interface IRepository<T>;

    private class Dog : IAnimal;

    private abstract class AbstractAnimal : IAnimal;

    private class Cat : AbstractAnimal;

    private class IntRepository : IRepository<int>;

    [Fact]
    public void GetImplementingTypesReturnsConcreteImplementations()
    {
        var assemblies = new[] { typeof(ReflectionExTests).Assembly };

        var types = assemblies.GetImplementingTypes<IAnimal>().ToList();

        Assert.Contains(typeof(Dog), types);
        Assert.Contains(typeof(Cat), types);
        Assert.DoesNotContain(typeof(AbstractAnimal), types);
        Assert.DoesNotContain(typeof(IAnimal), types);
    }

    [Fact]
    public void GetImplementingTypesForSingleAssemblyWorks()
    {
        var types = typeof(ReflectionExTests)
            .Assembly.GetImplementingTypes(typeof(IAnimal))
            .ToList();

        Assert.Contains(typeof(Dog), types);
    }

    [Fact]
    public void IsImplementationOfOpenGenericInterfaceDetectsImplementation()
    {
        Assert.True(
            typeof(IntRepository).IsImplementationOfOpenGenericInterface(typeof(IRepository<>))
        );
        Assert.False(typeof(Dog).IsImplementationOfOpenGenericInterface(typeof(IRepository<>)));
    }

    [Fact]
    public void GetDetailedTypeNameReturnsSimpleNameForNonGeneric() =>
        Assert.Equal("Dog", typeof(Dog).GetDetailedTypeName());

    [Fact]
    public void GetDetailedTypeNameIncludesGenericArguments()
    {
        Assert.Equal("IRepository<Int32>", typeof(IRepository<int>).GetDetailedTypeName());
        Assert.Equal(
            "Dictionary<String,Int32>",
            typeof(Dictionary<string, int>).GetDetailedTypeName()
        );
    }

    [Fact]
    public void IsOpenGenericDetectsOpenGenerics()
    {
        Assert.True(typeof(IRepository<>).IsOpenGeneric());
        Assert.True(typeof(List<>).IsOpenGeneric());
    }

    [Fact]
    public void IsOpenGenericReturnsFalseForClosedOrNonGeneric()
    {
        Assert.False(typeof(IRepository<int>).IsOpenGeneric());
        Assert.False(typeof(Dog).IsOpenGeneric());
        Assert.False(((System.Type?)null).IsOpenGeneric());
    }
}
