# Backend.Fx

A well-thought modular application framework. 

This library provides some global utility classes without any specific application type in mind. You might want to check out [Backend.Fx.Execution](https://github.com/backend-fx/Backend.Fx.Execution) for a generic application framework and [Backend.Fx.Ddd](https://github.com/backend-fx/Backend.Fx.Ddd) for a foundation following the principles of Domain Driven Design.

# `Backend.Fx`: basic types

### Exceptions

Backend.Fx provides a basic Exceptions that indicates a caller's fault: the `ClientException`. It is an implementation of the [_Notification Pattern_](https://martinfowler.com/articles/replaceThrowWithNotification.html) and comes with a structure to transport various detailed errors that are intended to help the caller solving the problem. See the `Errors` type for details.

#### Exception types

There are more specific exceptions deriving from `ClientException` that resemble the HTTP 4xx error classes: 

| Exception | Intention |
| --- | --- |
| `ConflictedException` | An optimistic concurrency fault |
| `ForbiddenException` | The caller is not allowed to execute this action |
| `NotFoundException` | The requested entity was not found |
| `TooManyRequestsException` | The caller made too many requests in a short period of time |
| `UnauthorizedException` | The caller is not authenticated |
| `UnprocessableException` | The call was understood formally but cannot be processed |

#### Collecting multiple errors and throw a single exception

Each implementation of `ClientException` can be used with the `ExceptionBuilder` to collect possible errors and throw them as single exception.

```csharp
using (var builder = UnprocessableException.UseBuilder())
{

    // collect validation errors
    builder.AddIf(model.Age < 18, "User must be adult");
    builder.AddIfNull(someArgument, "Value is required");

    // enjoy poor man's try pattern, e.g. when the type does not provide a `TryParse` method.
    // any caught exception is converted to an error on the thrown UnprocessableException
    ParsedValue = builder.Try(() => Value.Parse(argument));

} // <-- the builder will throw on dispose when any error has been collected
```

### Logging

The `Microsoft.Extensions.Logging` facility is extended by some useful functions

#### stop watch like logging

```csharp
using (_logger.LogDebugDuration("Critical activity")) 
{
    // execute the critical activity here, the log will contain entry and exit logging with elapsed time
}
```

#### central logger service location

While generally considered an anti pattern, logging might want to bypass the application's injection framework to provide an independent logger factory. Initialize the logging with your preferred logging implementation, e.g. Serilog in your `Program.cs`:

```csharp
var loggerConfiguration = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Is(LogEventLevel.Information)
    .WriteTo.Console(outputTemplate: outputTemplate);

Log.Logger = loggerConfiguration.CreateLogger();
Backend.Fx.Logging.Log.Initialize(new SerilogLoggerFactory(Log.Logger));
```

and create instances of `Microsoft.Extensions.Logging.ILogger` wherever you need them

```csharp
private readonly ILogger _logger = Log.Create<MyDomainService>();
```

### [NodaTime](https://nodatime.org/)

Backend.Fx encourages the use of NodaTime over the date and time types provided in the base class library. NodaTime gives you compile time safety over time zoned values and not zoned time `Instance`s and is therefore superior.
