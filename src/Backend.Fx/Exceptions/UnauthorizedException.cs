using System;
using JetBrains.Annotations;

namespace Backend.Fx.Exceptions;

[PublicAPI]
public class UnauthorizedException : ClientException
{
    public UnauthorizedException()
        : base("Unauthorized")
    {
    }

    /// <inheritdoc />
    public UnauthorizedException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public override string Rfc7807Title => "Unauthorized";
    
    public override int? Rfc7807Status => 401;
}