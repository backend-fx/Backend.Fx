using System;
using JetBrains.Annotations;

namespace Backend.Fx.Exceptions
{
    [PublicAPI]
    public interface IExceptionBuilder : IDisposable
    {
        bool HasError { get; }
        void Add(string error);
        void Add(string key, string error);
        void AddIf(bool condition, string error);
        void AddIf(string key, bool condition, string error);
        void AddIfNull(object toCheck, string error);
        void AddIfNull(object toCheck, string key, string error);
        void CheckAndMaybeThrowNow();
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

        public void AddIfNull(object? toCheck, string key, string error)
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

        public T Collect<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Add(ex.Message);
                return default!;
            }
        }

        public T Collect<T>(string key, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Add(key, ex.Message);
                return default!;
            }
        }

        public T CollectError<T>(string key, Func<T> func, Func<Exception, string> provideErrorMessage)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Add(key, provideErrorMessage.Invoke(ex));
                return default!;
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
}
