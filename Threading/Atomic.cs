namespace Threading
{
    partial class Program
    {
        private static int _Int;
        private static long _Long;
        private static string _String;
        private static object _Object;

        public static void AtomicOperations()
        {
            _Int = 42;
            _Int++;

            _Long = 42L;
            _Long ^= _Long;

            _String = "Foo";
            _String += "Bar";

            if (_Object != null)
            {
                
            }
        }

    }
}