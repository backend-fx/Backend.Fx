using System;
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
            if (instance == null)
            {
                throw new InvalidOperationException("Cannot set a private property value on a null reference");
            }
            
            instance.GetType().GetTypeInfo().GetDeclaredProperty(GetName(propertyExpression)).SetValue(instance, value, null);
        }

        private static string GetName<T, TValue>(Expression<Func<T, TValue>> exp)
        {
            return exp.Body switch
            {
                MemberExpression memberExpression => memberExpression.Member.Name,
                MethodCallExpression methodCallExpression => methodCallExpression.Method.Name,
                UnaryExpression { Operand: MemberExpression operand } => operand.Member.Name,
                ConstantExpression constantExpression => constantExpression.Value.ToString(),
                _ => throw new ArgumentException("Expression type not supported")
            };
        }
        
        public static T CreateInstanceFromPrivateDefaultConstructor<T>()
        {
            var constructor = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).SingleOrDefault(ci => ci.GetParameters().Length == 0);
            if (constructor == null)
            {
                throw new InvalidOperationException($"No private default constructor found in {typeof(T).Name}");
            }

            var instance = (T) constructor.Invoke(null);
            return instance;
        }
    }
}