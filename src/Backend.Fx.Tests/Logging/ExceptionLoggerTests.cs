using System;
using Backend.Fx.Exceptions;
using Backend.Fx.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Backend.Fx.Tests.Logging;

public class DebugExceptionLoggerTests
{
    private readonly DebugExceptionLogger _sut = new();

    [Fact]
    public void LogsClientExceptionWithoutThrowing() =>
        _sut.LogException(new ClientException("client"));

    [Fact]
    public void LogsGenericExceptionWithoutThrowing() =>
        _sut.LogException(new InvalidOperationException("server"));
}

public class ExceptionLoggerTests
{
    [Fact]
    public void LogsClientExceptionAsWarning()
    {
        var logger = new TestLogger();
        var sut = new ExceptionLogger(logger);

        sut.LogException(new ClientException("client"));

        Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Warning, logger.Entries[0].Level);
    }

    [Fact]
    public void LogsServerExceptionAsError()
    {
        var logger = new TestLogger();
        var sut = new ExceptionLogger(logger);

        sut.LogException(new InvalidOperationException("server"));

        Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Error, logger.Entries[0].Level);
    }
}
