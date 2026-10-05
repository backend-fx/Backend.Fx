using System.Collections.Generic;
using Backend.Fx.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Backend.Fx.Tests.Logging;

public class DurationLoggerTests
{
    [Fact]
    public void LogsBeginMessageOnConstructionAndEndMessageOnDispose()
    {
        var messages = new List<string>();

        var sut = new DurationLogger(messages.Add, "doing work");
        Assert.Single(messages);
        Assert.Equal("doing work", messages[0]);

        sut.Dispose();
        Assert.Equal(2, messages.Count);
        Assert.StartsWith("doing work", messages[1]);
        Assert.Contains("Duration:", messages[1]);
    }

    [Fact]
    public void UsesDistinctBeginAndEndMessages()
    {
        var messages = new List<string>();

        using (new DurationLogger(messages.Add, "begin", "end"))
        {
            Assert.Equal("begin", messages[0]);
        }

        Assert.StartsWith("end", messages[1]);
    }

    [Fact]
    public void ExtensionMethodLogsThroughLoggerAtInformationLevel()
    {
        var logger = new TestLogger();

        using (logger.LogInformationDuration("activity")) { }

        Assert.Equal(2, logger.Entries.Count);
        Assert.All(logger.Entries, e => Assert.Equal(LogLevel.Information, e.Level));
        Assert.Equal("activity", logger.Entries[0].Message);
        Assert.Contains("Duration:", logger.Entries[1].Message);
    }

    [Fact]
    public void ExtensionMethodLogsAtDebugAndTraceLevels()
    {
        var logger = new TestLogger();

        using (logger.LogDebugDuration("d-begin", "d-end")) { }

        using (logger.LogTraceDuration("t")) { }

        Assert.Equal(LogLevel.Debug, logger.Entries[0].Level);
        Assert.Equal(LogLevel.Trace, logger.Entries[2].Level);
    }
}
