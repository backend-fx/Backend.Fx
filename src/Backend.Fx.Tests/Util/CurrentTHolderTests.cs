using System;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class CurrentTHolderTests
{
    private sealed class StringHolder : CurrentTHolder<string>
    {
        private readonly string _provided;

        public StringHolder(string provided) => _provided = provided;

        public int ProvideCount { get; private set; }

        public override string ProvideInstance()
        {
            ProvideCount++;
            return _provided;
        }

        protected override string Describe(string instance) => instance;
    }

    private sealed class Disposable : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }

    private sealed class DisposableHolder : CurrentTHolder<Disposable>
    {
        private readonly Disposable _provided;

        public DisposableHolder(Disposable provided) => _provided = provided;

        public override Disposable ProvideInstance() => _provided;

        protected override string Describe(Disposable instance) => "disposable";
    }

    [Fact]
    public void CurrentProvidesInstanceLazilyAndCachesIt()
    {
        var holder = new StringHolder("value");

        Assert.Equal(0, holder.ProvideCount);
        Assert.Equal("value", holder.Current);
        Assert.Equal("value", holder.Current);
        Assert.Equal(1, holder.ProvideCount);
    }

    [Fact]
    public void ReplaceCurrentChangesTheHeldInstance()
    {
        var holder = new StringHolder("initial");

        holder.ReplaceCurrent("replaced");

        Assert.Equal("replaced", holder.Current);
        Assert.Equal(0, holder.ProvideCount);
    }

    [Fact]
    public void ClearCurrentCausesReprovisioning()
    {
        var holder = new StringHolder("value");
        _ = holder.Current;

        holder.ClearCurrent();
        _ = holder.Current;

        Assert.Equal(2, holder.ProvideCount);
    }

    [Fact]
    public void ClearCurrentDisposesDisposableInstance()
    {
        var disposable = new Disposable();
        var holder = new DisposableHolder(disposable);
        _ = holder.Current;

        holder.ClearCurrent();

        Assert.True(disposable.IsDisposed);
    }
}
