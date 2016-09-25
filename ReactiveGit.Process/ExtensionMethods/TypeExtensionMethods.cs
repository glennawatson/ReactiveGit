namespace ReactiveGit.Process.ExtensionMethods
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods related to Type's.
    /// </summary>
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Gets the element type of the specified IEnumerable type.
        /// </summary>
        /// <param name="seqType">The type to find the element type of.</param>
        /// <returns>The element type.</returns>
        public static Type GetElementType(this Type seqType)
        {
            Type enumerableType = FindIEnumerable(seqType);
            return enumerableType == null ? seqType : enumerableType.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type seqType)
        {
            while (true)
            {
                if (seqType == null || seqType == typeof(string))
                {
                    return null;
                }

                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
                }

                if (seqType.IsGenericType)
                {
                    foreach (Type arg in seqType.GetGenericArguments())
                    {
                        Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                        if (ienum.IsAssignableFrom(seqType))
                        {
                            return ienum;
                        }
                    }
                }

                Type[] interfaces = seqType.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    foreach (Type interfaceType in interfaces)
                    {
                        Type enumerableType = FindIEnumerable(interfaceType);
                        if (enumerableType != null)
                        {
                            return enumerableType;
                        }
                    }
                }

                if (seqType.BaseType == null || seqType.BaseType == typeof(object))
                {
                    return null;
                }

                seqType = seqType.BaseType;
            }
        }
    }
}