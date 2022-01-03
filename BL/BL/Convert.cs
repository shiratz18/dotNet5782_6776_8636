using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BL
{
    public static class Convert
    {
        /// <summary>
        /// Converts an object to another object
        /// </summary>
        /// <typeparam name="T">Object type to</typeparam>
        /// <typeparam name="S">Object type from</typeparam>
        /// <param name="from">Object from</param>
        /// <param name="to">Object to</param>
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;

                object value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                {
                    propTo.SetValue(to, value);
                }
                else if (!(value is IEnumerable<ValueType>))
                {
                    object target = propTo.GetValue(to, null);
                    value.CopyPropertiesTo(target);
                }
            }
        }

        /// <summary>
        /// Converts a list of object types to a different type
        /// </summary>
        /// <typeparam name="T">Object type to</typeparam>
        /// <typeparam name="S">Object type from</typeparam>
        /// <param name="from">Object from</param>
        /// <param name="to">Object to</param>
        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to) where T : new()
        {
            foreach(S s in from)
            {
                T t = new T();
                s.CopyPropertiesTo(t);
                to.Add(t);
            }
        }

        public static object CopyPropertiesToNew<S>(this S from,Type type)
        {
            object to = Activator.CreateInstance(type);
            from.CopyPropertiesTo(to);
            return to;
        }
    }
}
