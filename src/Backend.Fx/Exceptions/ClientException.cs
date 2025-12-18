using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Backend.Fx.Exceptions;

    [PublicAPI]
    public class ClientException : Exception
    {
        public ClientException()
            : base("Bad request.")
        {
        }

        /// <param name="message">When using one of the middlewares in Backend.Fx.AspNetCore.ErrorHandling, the message is not sent
        /// to the client to not provide internal details to an attacker. Write the exception message with a developer in mind, since
        /// the application log will contain the message. To provide the user with functional feedback to correct their input, use
        /// the AddError(s) overloads.</param>
        public ClientException(string message)
            : base(message)
        {
        }

        /// <param name="message">When using one of the middlewares in Backend.Fx.AspNetCore.ErrorHandling, the message is not sent
        /// to the client to not provide internal details to an attacker. Write the exception message with a developer in mind, since
        /// the application log will contain the message. To provide the user with functional feedback to correct their input, use
        /// the AddError(s) overloads.</param>
        /// <param name="innerException"></param>
        public ClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public Errors Errors { get; } = new();

        public bool HasErrors()
        {
            return Errors.Any();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string exceptionType = GetType().ToString();

            string message = string.IsNullOrEmpty(Message)
                                 ? exceptionType
                                 : exceptionType + ": " + Message;

            string? innerException = InnerException != null
                                        ? " ---> "
                                          + InnerException
                                          + Environment.NewLine
                                          + "   End of inner exception stack trace"
                                        : null;

            return string.Join(Environment.NewLine,
                               new[] {message, Errors.ToString(), innerException, StackTrace}.Where(s => s != null));
        }
        
        /// <summary>
        /// Provides an RFC7807 compliant title for the exception.
        /// </summary>
        public virtual string Rfc7807Title => "Bad Request";
        
        /// <summary>
        /// Provides an RFC7807 compliant status code for the exception.
        /// </summary>
        public virtual int? Rfc7807Status => 400;
    
        /// <summary>
        /// Provides an RFC7807 compliant detail for the exception. This will be the generic error, the first error, or
        /// the Rfc7807Title of the exception in order of prevalence. 
        /// </summary>
        public string Rfc7807Detail
        {
            get
            {
                if (Errors.TryGetValue(Errors.GenericErrorKey, out var errors))
                {
                    return string.Join(". ", errors.Select(err => new string(err.TrimEnd(".").ToArray())));
                }

                if (Errors.Any())
                {
                    return string.Join(". ", Errors.First().Value.Select(err => new string(err.TrimEnd(".").ToArray())));
                }

                return Rfc7807Title;

            }
        }

        /// <summary>
        /// Provides the errors dictionary as an instance of `IDictionary{string, object}` that can be used in RFC7807's
        /// ProblemDetail.Extensions property
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> Rfc7807ExtensionsDictionary
            => Errors.ToDictionary(kvp => kvp.Key, kvp => (object) kvp.Value);

        /// <summary>
        /// Used to build an <see cref="ClientException"/> with multiple possible error messages. The builder will throw on disposal
        /// when at least one error was added. Using the AddIf methods is quite comfortable when there are several criteria to be validated
        /// before executing a business case. 
        /// </summary>
        public static IExceptionBuilder UseBuilder()
        {
            return new ExceptionBuilder<ClientException>();
        }
    }

    [PublicAPI]
    public static class ClientExceptionEx
    {
        public static TEx AddError<TEx>(this TEx clientException, [LocalizationRequired] string errorMessage) where TEx : ClientException
        {
            clientException.Errors.Add(errorMessage);
            return clientException;
        }

        public static TEx AddError<TEx>(this TEx clientException, string key, [LocalizationRequired] string errorMessage) where TEx : ClientException
        {
            clientException.Errors.Add(key, errorMessage);
            return clientException;
        }

        public static TEx AddErrors<TEx>(this TEx clientException, [LocalizationRequired] IEnumerable<string> errorMessages) where TEx : ClientException
        {
            clientException.Errors.Add(errorMessages);
            return clientException;
        }


        public static TEx AddErrors<TEx>(this TEx clientException, string key, [LocalizationRequired] IEnumerable<string> errorMessages) where TEx : ClientException
        {
            clientException.Errors.Add(key, errorMessages);
            return clientException;
        }
    }
