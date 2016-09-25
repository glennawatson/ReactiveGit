namespace ReactiveGit.Gui.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Splat;

    /// <summary>
    /// Extension methods related to the splat framework.
    /// </summary>
    public static class SplatExtensionMethods
    {
        /// <summary>
        /// Version of get service that uses a generic to resolve.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="resolver">The resolver to use to get the service.</param>
        /// <returns>The service.</returns>
        public static T GetService<T>(this IDependencyResolver resolver)
        {
            return (T)resolver.GetService(typeof(T));
        }

        /// <summary>
        /// Helper class for having a object's constructor automatically assigned by a "GetService" request.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <typeparam name="TConcrete">The concrete class type.</typeparam>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        public static void Register<TConcrete, TInterface>(this IMutableDependencyResolver resolver)
        {
            Func<IDependencyResolver, object> func = RegisterCache<TConcrete>.GetRegisterFunc();
            resolver.Register(() => func(resolver), typeof(TInterface));
        }

        private static class RegisterCache<TConcrete>
        {
            private static Func<IDependencyResolver, object> cachedFunc;

            public static Func<IDependencyResolver, object> GetRegisterFunc()
            {
                return cachedFunc ?? (cachedFunc = GenerateFunc());
            }

            private static Func<IDependencyResolver, object> GenerateFunc()
            {
                ParameterExpression funcParameter = Expression.Parameter(typeof(IDependencyResolver));

                var concreteType = typeof(TConcrete);

                // Must be a single constructor
                var constructor =
                    concreteType.GetTypeInfo().DeclaredConstructors.Single(x => x.GetParameters().Length > 0);

                var getServiceMethodInfo = typeof(IDependencyResolver).GetTypeInfo().GetDeclaredMethod("GetService");

                IList<Expression> parameterExpressions =
                    constructor.GetParameters().Select(
                        parameter =>
                            Expression.Convert(
                                Expression.Call(
                                    funcParameter,
                                    getServiceMethodInfo,
                                    Expression.Constant(parameter.ParameterType),
                                    Expression.Convert(Expression.Constant(null), typeof(string))),
                                parameter.ParameterType)).Cast<Expression>().ToList();

                NewExpression newValue = Expression.New(constructor, parameterExpressions);
                Expression converted = Expression.Convert(newValue, typeof(object));
                return Expression.Lambda<Func<IDependencyResolver, object>>(converted, funcParameter).Compile();
            }
        }
    }
}