using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FrockRaytracer.Utils
{
    public static class Generics
    {
        /// <summary>
        /// Cache dat shizzle
        /// </summary>
        private static Dictionary<Type, int> _sizes = new Dictionary<Type, int>();

        /// <summary>
        /// Size of a given type in bytes
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int SizeOf(Type type)
        {
            int size;
            if (_sizes.TryGetValue(type, out size))
            {
                return size;
            }

            size = SizeOfType(type);
            _sizes.Add(type, size);
            return size;            
        }

        /// <summary>
        /// Size of a given type in bytes
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int SizeOfType(Type type)
        {
            var dm = new DynamicMethod("SizeOfType", typeof(int), new Type[] { });
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, type);
            il.Emit(OpCodes.Ret);
            return (int)dm.Invoke(null, null);
        }
    }
}