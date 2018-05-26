namespace FrockRaytracer.Utils
{
    public static class Mem
    {
        /// <summary>
        /// Swaps two varibales of the same type
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <typeparam name="T"></typeparam>
        public static void Swap<T> (ref T lhs, ref T rhs) {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}