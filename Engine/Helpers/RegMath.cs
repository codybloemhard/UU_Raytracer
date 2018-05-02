namespace Engine.Helpers
{
    public class RegMath
    {
        public static double Smooth(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static double Lerp(double a, double b, double x)
        {
            return a + x * (b - a);
        }
    }
}