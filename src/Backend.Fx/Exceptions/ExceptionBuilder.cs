using System;
using JetBrains.Annotations;

namespace Backend.Fx.Exceptions;

/// <summary>
/// Notification pattern implementation for collecting errors in a <see cref="ClientException"/>.
/// On disposal, it checks if there are any errors collected and throws an exception if there are.
/// </summary>
[PublicAPI]
public interface IExceptionBuilder : IDisposable
{
    /// <summary>
    /// Checks if there are any errors collected so far.
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// Adds a general error message to the collection of errors.
    /// </summary>
    void Add(string error);

    /// <summary>
    /// Adds an error message related to a specific argument value identified by the given key.
    /// </summary>
    void Add(string key, string error);

    /// <summary>
    /// Adds a general error message if the provided object is null.
    /// </summary>
    void AddIfNull(object? toCheck, string error);

    /// <summary>
    /// Adds an error message related to a specific argument value identified by the given key if the provided object is null.
    /// </summary>
    void AddIfNull(string key, object? toCheck, string error);

    /// <summary>
    /// Adds a general error message if the provided condition is true.
    /// </summary>
    void AddIf(bool condition, string error);

    /// <summary>
    /// Adds an error message related to a specific argument value identified by the given key if the provided condition is true.
    /// </summary>
    void AddIf(string key, bool condition, string error);

    /// <summary>
    /// Returns the result of the provided function or collects a general error based on the exception message if an exception occurs.
    /// </summary>
    T? Try<T>(Func<T> func);

    /// <summary>
    /// Returns the result of the provided function or collects a general error. A function must be provided to generate the error message based on the exception that occurred.
    /// </summary>
    T? Try<T>(Func<T> func, Func<Exception, string> provideErrorMessage);

    /// <summary>
    /// Returns the result of the provided function or collects an error message related to a specific argument value identified by the given key based on the exception message if an exception occurs.
    /// </summary>
    T? Try<T>(string key, Func<T> func);

    /// <summary>
    /// Returns the result of the provided function, or collects an error message related to a specific argument value identified by the given key. A function must be provided to generate the error message based on the exception that occurred.
    /// </summary>
    T? Try<T>(string key, Func<T> func, Func<Exception, string> provideErrorMessage);

    /// <summary>
    /// Checks if there are any errors collected so far and throws the <see cref="ClientException"/> if there are.
    /// </summary>
    void CheckAndMaybeThrowNow();

    /// <summary>
    /// Accesses the collection of errors collected so far.
    /// </summary>
    Errors GetErrors();
}

[PublicAPI]
public class ExceptionBuilder<TEx> : IExceptionBuilder where TEx : ClientException, new()
{
    private readonly TEx _clientException = new();

    public bool HasError => _clientException.HasErrors();

    public void Add(string error)
    {
        _clientException.Errors.Add(error);
    }

    public void Add(string key, string error)
    {
        _clientException.Errors.Add(key, error);
    }

    public void AddIfNull(object? toCheck, string error)
    {
        if (toCheck == null)
        {
            Add(error);
        }
    }

    public void AddIfNull(string key, object? toCheck, string error)
    {
        if (toCheck == null)
        {
            Add(key, error);
        }
    }

    public void Dispose() => CheckAndMaybeThrowNow();

    public void AddIf(bool condition, string error)
    {
        if (condition)
        {
            _clientException.Errors.Add(error);
        }
    }

    public void AddIf(string key, bool condition, string error)
    {
        if (condition)
        {
            _clientException.Errors.Add(key, error);
        }
    }

    public T? Try<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch (ClientException ex)
        {
            foreach (var error in ex.Errors)
            {
                _clientException.Errors.Add(error.Key, error.Value);
            }
                
            return default;
        }
        catch (Exception ex)
        {
            Add(ex.Message);
            return default;
        }
    }

    public T? Try<T>(Func<T> func, Func<Exception, string> provideErrorMessage)
    {
        try
        {
            return func();
        }
        catch (ClientException ex)
        {
            foreach (var error in ex.Errors)
            {
                _clientException.Errors.Add(error.Key, error.Value);
            }
                
            return default;
        }
        catch (Exception ex)
        {
            try
            {
                var error = provideErrorMessage.Invoke(ex);
                Add(error);
            }
            catch (Exception innerEx)
            {
                Add(
                    $"Error generation failed! Original exception message: [{ex.Message}]. Error generation exception message: [{innerEx.Message}]");
            }

            return default;
        }
    }


    public T? Try<T>(string key, Func<T> func)
    {
        try
        {
            return func();
        }
        catch (ClientException ex)
        {
            foreach (var error in ex.Errors)
            {
                _clientException.Errors.Add(error.Key, error.Value);
            }
                
            return default;
        }
        catch (Exception ex)
        {
            Add(key, ex.Message);
            return default;
        }
    }

    public T? Try<T>(string key, Func<T> func, Func<Exception, string> provideErrorMessage)
    {
        try
        {
            return func();
        }
        catch (ClientException ex)
        {
            foreach (var error in ex.Errors)
            {
                _clientException.Errors.Add(error.Key, error.Value);
            }
                
            return default;
        }
        catch (Exception ex)
        {
            try
            {
                var error = provideErrorMessage.Invoke(ex);
                Add(error);
            }
            catch (Exception innerEx)
            {
                Add(
                    key,
                    $"Error generation failed! Original exception message: [{ex.Message}]. Error generation exception message: [{innerEx.Message}]");
            }

            return default;
        }
    }

    public void CheckAndMaybeThrowNow()
    {
        if (_clientException.HasErrors())
        {
            throw _clientException;
        }
    }

    public Errors GetErrors()
    {
        return _clientException.Errors;
    }
}