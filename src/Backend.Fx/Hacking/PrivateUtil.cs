using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Backend.Fx.Hacking
{
    [PublicAPI]
    public static class PrivateUtil
    {
        public static void SetPrivate<T, TValue>(this T instance, Expression<Func<T, TValue>> propertyExpression, TValue value)
        {
            instance.GetType().GetTypeInfo().GetDeclaredProperty(GetName(propertyExpression)).SetValue(instance, value, null);
        }

        private static string GetName<T, TValue>(Expression<Func<T, TValue>> exp)
        {
            if (exp.Body is not MemberExpression body)
            {
                var unaryExpression = (UnaryExpression) exp.Body;
                body = unaryExpression.Operand as MemberExpression;
            }

            Debug.Assert(body != null, "body != null");
            return body.Member.Name;
        }
        
        public static T CreateInstanceFromPrivateDefaultConstructor<T>()
        {
            ConstructorInfo constructor = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).SingleOrDefault(ci => ci.GetParameters().Length == 0);
            if (constructor == null)
            {
                throw new InvalidOperationException($"No private default constructor found in {typeof(T).Name}");
            }

            var instance = (T) constructor.Invoke(null);
            return instance;
        }
    }
}