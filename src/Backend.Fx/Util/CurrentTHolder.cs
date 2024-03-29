﻿using System;
using Backend.Fx.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Backend.Fx.Util
{
    /// <summary>
    /// Holds a current instance of T that might be replaced during the scope
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    public interface ICurrentTHolder<T>
    {
        T Current { get; }

        void ReplaceCurrent(T newCurrentInstance);
        
        void ClearCurrent();

        T ProvideInstance();
    }

    [PublicAPI]
    public abstract class CurrentTHolder<T> : ICurrentTHolder<T>
    {
        private readonly ILogger _logger = Log.Create<CurrentTHolder<T>>();
        private T? _current;

        protected CurrentTHolder()
        { }

        protected CurrentTHolder(T initial)
        {
            _current = initial;
        }
        
        public T Current
        {
            get
            {
                if (_current == null)
                {
                    _logger.LogDebug("Providing initial {HeldTypeName} instance", typeof(T).Name);
                    _current = ProvideInstance();
                    _logger.LogDebug("Initial instance of {HeldTypeName} is: {HeldInstanceDescription}", typeof(T).Name, DescribeSafe(_current));
                }

                return _current;
            }
        }

        public void ReplaceCurrent(T newCurrentInstance)
        {
            if (Equals(_current, newCurrentInstance)) return;

            _logger.LogDebug(
                "Replacing current instance of {HeldTypename} ({HeldInstanceDescription}) with another instance ({NewInstanceDescription})",
                typeof(T).Name,
                DescribeSafe(_current),
                DescribeSafe(newCurrentInstance));
            _current = newCurrentInstance;
        }

        public void ClearCurrent()
        {
            if (Equals(_current, null)) return;

            _logger.LogDebug(
                "Clearing current instance of {HeldTypename} ({HeldInstanceDescription})",
                typeof(T).Name,
                DescribeSafe(_current));
            
            if (_current is IDisposable disposable)
            {
                _logger.LogDebug(
                    "Disposing current instance of {HeldTypename} ({HeldInstanceDescription})",
                    typeof(T).Name,
                    DescribeSafe(_current));
                disposable.Dispose();
            }
            
            _current = default;
        }

        private string DescribeSafe(T? instance)
        {
            return instance == null ? "<NULL>" : Describe(instance);   
        }

        public abstract T ProvideInstance();

        protected abstract string Describe(T instance);
    }
}