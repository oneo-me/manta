using Manta.Logging;

namespace Manta.Tests.Logging;

/// <summary>
/// Tests for the Log system (Manta/Logging/Log.cs)
/// Priority: MEDIUM - Logging failures could hide critical errors
/// </summary>
public class LogTests
{
    private class TestLogger : ILogger
    {
        public List<LogEntry> Entries { get; } = new();

        public void WriteLine(DateTimeOffset time, LogLevel level, string messageTemplate, params object?[] propertyValues)
        {
            Entries.Add(new LogEntry(time, level, messageTemplate, propertyValues));
        }
    }

    private record LogEntry(DateTimeOffset Time, LogLevel Level, string MessageTemplate, object?[] PropertyValues);

    public LogTests()
    {
        ClearLoggers();
    }

    private void ClearLoggers()
    {
        var field = typeof(Log).GetField("_loggers",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var loggers = field?.GetValue(null) as System.Collections.IList;
        loggers?.Clear();
    }

    #region AddLogger Tests

    [Fact]
    public void AddLogger_ShouldRegisterLogger()
    {
        // Arrange
        var logger = new TestLogger();

        // Act
        Log.AddLogger(logger);
        Log.Information("Test");

        // Assert
        logger.Entries.Should().HaveCount(1);
    }

    [Fact]
    public void AddLogger_MultipleLoggers_ShouldAllReceiveMessages()
    {
        // Arrange
        var logger1 = new TestLogger();
        var logger2 = new TestLogger();
        var logger3 = new TestLogger();

        Log.AddLogger(logger1);
        Log.AddLogger(logger2);
        Log.AddLogger(logger3);

        // Act
        Log.Information("Test message");

        // Assert
        logger1.Entries.Should().HaveCount(1);
        logger2.Entries.Should().HaveCount(1);
        logger3.Entries.Should().HaveCount(1);
    }

    #endregion

    #region Log Level Tests

    [Fact]
    public void Verbose_ShouldLogWithVerboseLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Verbose("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Verbose);
    }

    [Fact]
    public void Debug_ShouldLogWithDebugLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Debug("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Debug);
    }

    [Fact]
    public void Information_ShouldLogWithInformationLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Information("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Information);
    }

    [Fact]
    public void Warning_ShouldLogWithWarningLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Warning("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Warning);
    }

    [Fact]
    public void Error_ShouldLogWithErrorLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Error("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Error);
    }

    [Fact]
    public void Fatal_ShouldLogWithFatalLevel()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        // Act
        Log.Fatal("Test");

        // Assert
        logger.Entries[0].Level.Should().Be(LogLevel.Fatal);
    }

    #endregion

    #region FormatTime Tests

    [Fact]
    public void FormatTime_None_ShouldReturnEmptyString()
    {
        // Arrange
        var time = new DateTimeOffset(2025, 12, 29, 14, 30, 45, 123, TimeSpan.Zero);

        // Act
        var result = Log.FormatTime(time, LogTimeType.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FormatTime_Time_ShouldFormatTimeOnly()
    {
        // Arrange
        var time = new DateTimeOffset(2025, 12, 29, 14, 30, 45, 123, TimeSpan.Zero);

        // Act
        var result = Log.FormatTime(time, LogTimeType.Time);

        // Assert
        result.Should().Be("14:30:45.123");
    }

    [Fact]
    public void FormatTime_DateTime_ShouldFormatFullDateTime()
    {
        // Arrange
        var time = new DateTimeOffset(2025, 12, 29, 14, 30, 45, 123, TimeSpan.Zero);

        // Act
        var result = Log.FormatTime(time, LogTimeType.DateTime);

        // Assert
        result.Should().Be("2025-12-29 14:30:45.123");
    }

    #endregion

    #region FormatLevel Tests

    [Fact]
    public void FormatLevel_Verbose_ShouldReturnV()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Verbose);

        // Assert
        result.Should().Be('V');
    }

    [Fact]
    public void FormatLevel_Debug_ShouldReturnD()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Debug);

        // Assert
        result.Should().Be('D');
    }

    [Fact]
    public void FormatLevel_Information_ShouldReturnI()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Information);

        // Assert
        result.Should().Be('I');
    }

    [Fact]
    public void FormatLevel_Warning_ShouldReturnW()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Warning);

        // Assert
        result.Should().Be('W');
    }

    [Fact]
    public void FormatLevel_Error_ShouldReturnE()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Error);

        // Assert
        result.Should().Be('E');
    }

    [Fact]
    public void FormatLevel_Fatal_ShouldReturnF()
    {
        // Act
        var result = Log.FormatLevel(LogLevel.Fatal);

        // Assert
        result.Should().Be('F');
    }

    #endregion

    #region FormatMessage Tests

    [Theory]
    [InlineData("Simple message", new object[] { }, null, "Simple message")]
    [InlineData("Hello {0}", new object[] { "World" }, null, "Hello 'World'")]
    [InlineData("{0} {1}", new object[] { 1, 2 }, null, "'1' '2'")]
    [InlineData("{0} {1} {2}", new object[] { "a", "b", "c" }, null, "'a' 'b' 'c'")]
    [InlineData("Test {name}", new object[] { "value" }, null, "Test 'value'")]
    [InlineData("Hello {{0}}", new object[] { "World" }, null, "Hello {0}")]
    public void FormatMessage_VariousTemplates_ShouldFormatCorrectly(
        string template, object[] args, string? area, string expected)
    {
        // Act
        var result = Log.FormatMessage(area, template, args);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FormatMessage_WithArea_ShouldIncludeArea()
    {
        // Act
        var result = Log.FormatMessage("TestArea", "Message {0}", new object[] { "test" });

        // Assert
        result.Should().Be("[TestArea] Message 'test'");
    }

    [Fact]
    public void FormatMessage_WithoutArea_ShouldNotIncludeArea()
    {
        // Act
        var result = Log.FormatMessage(null, "Message {0}", new object[] { "test" });

        // Assert
        result.Should().Be("Message 'test'");
    }

    [Fact]
    public void FormatMessage_MissingParameter_ShouldHandleGracefully()
    {
        // Act
        var result = Log.FormatMessage(null, "{0} {1}", new object[] { 1 });

        // Assert
        result.Should().Be("'1' ''");
    }

    [Fact]
    public void FormatMessage_ExtraParameters_ShouldIgnore()
    {
        // Act
        var result = Log.FormatMessage(null, "{0}", new object[] { 1, 2, 3 });

        // Assert
        result.Should().Be("'1'");
    }

    [Fact]
    public void FormatMessage_NullParameter_ShouldFormatAsEmpty()
    {
        // Act
        var result = Log.FormatMessage(null, "{0}", new object?[] { null });

        // Assert
        result.Should().Be("''");
    }

    [Fact]
    public void FormatMessage_EscapedBraces_ShouldNotReplace()
    {
        // Act
        var result = Log.FormatMessage(null, "{{escaped}}", Array.Empty<object>());

        // Assert
        result.Should().Be("{escaped}");
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public void AddLogger_Concurrent_ShouldHandleSafely()
    {
        // Arrange
        var tasks = new List<Task>();
        var loggers = new System.Collections.Concurrent.ConcurrentBag<TestLogger>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var logger = new TestLogger();
                loggers.Add(logger);
                Log.AddLogger(logger);
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert - All loggers should receive the message
        Log.Information("Test");
        loggers.Should().AllSatisfy(logger => logger.Entries.Should().HaveCount(1));
    }

    [Fact]
    public void WriteLine_Concurrent_ShouldDistributeToAllLoggers()
    {
        // Arrange
        var logger = new TestLogger();
        Log.AddLogger(logger);

        var tasks = new List<Task>();

        // Act - Multiple threads logging concurrently
        for (int i = 0; i < 100; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                Log.Information($"Message {index}");
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        logger.Entries.Should().HaveCount(100);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void FullWorkflow_MultipleLoggersAndLevels_ShouldWork()
    {
        // Arrange
        var logger1 = new TestLogger();
        var logger2 = new TestLogger();
        Log.AddLogger(logger1);
        Log.AddLogger(logger2);

        // Act
        Log.Verbose("Verbose message");
        Log.Debug("Debug message");
        Log.Information("Info message");
        Log.Warning("Warning message");
        Log.Error("Error message");
        Log.Fatal("Fatal message");

        // Assert
        logger1.Entries.Should().HaveCount(6);
        logger2.Entries.Should().HaveCount(6);

        logger1.Entries[0].Level.Should().Be(LogLevel.Verbose);
        logger1.Entries[1].Level.Should().Be(LogLevel.Debug);
        logger1.Entries[2].Level.Should().Be(LogLevel.Information);
        logger1.Entries[3].Level.Should().Be(LogLevel.Warning);
        logger1.Entries[4].Level.Should().Be(LogLevel.Error);
        logger1.Entries[5].Level.Should().Be(LogLevel.Fatal);
    }

    #endregion
}
