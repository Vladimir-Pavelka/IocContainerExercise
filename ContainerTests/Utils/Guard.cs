namespace ContainerTests.Utils
{
    using System;

    public static class Guard
    {
        public static void NotNull(object argument, string argName)
        {
            if (argument == null) throw new ArgumentNullException(argName);
        }
    }
}