using System;
using JetBrains.Annotations;

namespace Backend.Fx.Exceptions;

[PublicAPI]
public class ForbiddenException : ClientException
{
    public ForbiddenException()
        : base("Forbidden")
    {
    }

    /// <inheritdoc />
    public ForbiddenException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public override string Rfc7807Title => "Forbidden";
    
    public override int? Rfc7807Status => 403;
}