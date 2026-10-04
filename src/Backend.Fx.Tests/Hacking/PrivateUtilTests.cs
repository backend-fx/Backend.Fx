using System;
using Backend.Fx.Hacking;
using JetBrains.Annotations;
using Xunit;

namespace Backend.Fx.Tests.Hacking;

public class PrivateUtilTests
{
    private sealed class WithPrivateSetter
    {
        public string Value { get; [UsedImplicitly] private set; } = "initial";
    }

    [UsedImplicitly]
    private sealed class WithPrivateConstructor
    {
        private WithPrivateConstructor() => Value = "constructed";
        public string Value { get; }
    }

    [UsedImplicitly]
    private sealed class WithoutPrivateConstructor;

    [Fact]
    public void SetPrivateSetsPropertyWithPrivateSetter()
    {
        var instance = new WithPrivateSetter();

        instance.SetPrivate(x => x.Value, "changed");

        Assert.Equal("changed", instance.Value);
    }

    [Fact]
    public void SetPrivateThrowsOnNullInstance()
    {
        WithPrivateSetter? instance = null;

        Assert.Throws<InvalidOperationException>(() => instance!.SetPrivate(x => x.Value, "x"));
    }

    [Fact]
    public void CreateInstanceFromPrivateDefaultConstructorInvokesPrivateConstructor()
    {
        var instance = PrivateUtil.CreateInstanceFromPrivateDefaultConstructor<WithPrivateConstructor>();

        Assert.Equal("constructed", instance.Value);
    }

    [Fact]
    public void CreateInstanceFromPrivateDefaultConstructorThrowsWhenNoneExists()
    {
        Assert.Throws<InvalidOperationException>(
            PrivateUtil.CreateInstanceFromPrivateDefaultConstructor<WithoutPrivateConstructor>);
    }
}
