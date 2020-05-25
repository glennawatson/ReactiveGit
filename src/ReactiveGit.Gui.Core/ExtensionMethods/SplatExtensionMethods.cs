// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Splat;

namespace ReactiveGit.Gui.Core.ExtensionMethods
{
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
            Func<IMutableDependencyResolver, object> func = RegisterCache<TConcrete>.GetRegisterFunc();
            resolver.Register(() => func(resolver), typeof(TInterface));
        }

        private static class RegisterCache<TConcrete>
        {
            private static Func<IMutableDependencyResolver, object> _cachedFunc;

            public static Func<IMutableDependencyResolver, object> GetRegisterFunc()
            {
                return _cachedFunc ?? (_cachedFunc = GenerateFunc());
            }

            private static Func<IMutableDependencyResolver, object> GenerateFunc()
            {
                ParameterExpression funcParameter = Expression.Parameter(typeof(IMutableDependencyResolver));

                Type concreteType = typeof(TConcrete);

                // Must be a single constructor
                ConstructorInfo constructor =
                    concreteType.GetTypeInfo().DeclaredConstructors.Single(x => x.GetParameters().Length > 0);

                MethodInfo getServiceMethodInfo =
                    typeof(IDependencyResolver).GetTypeInfo().GetDeclaredMethod("GetService");

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
                return Expression.Lambda<Func<IMutableDependencyResolver, object>>(converted, funcParameter).Compile();
            }
        }
    }
}