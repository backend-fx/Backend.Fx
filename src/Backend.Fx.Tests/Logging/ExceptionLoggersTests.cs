using System;
using System.Linq;
using Backend.Fx.Logging;
using Xunit;

namespace Backend.Fx.Tests.Logging;

public class ExceptionLoggersTests
{
    private sealed class RecordingExceptionLogger : IExceptionLogger
    {
        public Exception? Logged { get; private set; }
        public void LogException(Exception exception) => Logged = exception;
    }

    private sealed class ThrowingExceptionLogger : IExceptionLogger
    {
        public void LogException(Exception exception) => throw new InvalidOperationException("inner logger failed");
    }

    [Fact]
    public void DelegatesToAllRegisteredLoggers()
    {
        var first = new RecordingExceptionLogger();
        var second = new RecordingExceptionLogger();
        var sut = new ExceptionLoggers(first, second);
        var exception = new Exception("boom");

        sut.LogException(exception);

        Assert.Same(exception, first.Logged);
        Assert.Same(exception, second.Logged);
    }

    [Fact]
    public void SwallowsExceptionsThrownByInnerLoggers()
    {
        var healthy = new RecordingExceptionLogger();
        var sut = new ExceptionLoggers(new ThrowingExceptionLogger(), healthy);
        var exception = new Exception("boom");

        sut.LogException(exception);

        Assert.Same(exception, healthy.Logged);
    }

    [Fact]
    public void SupportsCollectionOperations()
    {
        var sut = new ExceptionLoggers();
        var logger = new RecordingExceptionLogger();

        sut.Add(logger);
        Assert.Single(sut);
        bool contains = sut.Contains(logger);
        Assert.True(contains);
        Assert.Contains(logger, sut.ToList());
        Assert.False(sut.IsReadOnly);

        bool removed = sut.Remove(logger);
        Assert.True(removed);
        Assert.Empty(sut);

        sut.Add(logger);
        sut.Clear();
        Assert.Empty(sut);
    }
}
