using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Backend.Fx.Tests;

public sealed class TestLogger : ILogger
{
    public List<(LogLevel Level, string Message, Exception? Exception)> Entries { get; } = new();

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) => Entries.Add((logLevel, formatter(state, exception), exception));

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose() { }
    }
}

public sealed class TestLoggerFactory : ILoggerFactory
{
    public List<string> CreatedCategories { get; } = new();

    public TestLogger Logger { get; } = new();

    public ILogger CreateLogger(string categoryName)
    {
        CreatedCategories.Add(categoryName);
        return Logger;
    }

    public void AddProvider(ILoggerProvider provider) { }

    public void Dispose() { }
}
