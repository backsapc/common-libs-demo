namespace CommonLibsDemo.LinuxLibrary
{
    public class Class
    {
        public override string ToString()
        {
#if Linux
           return "Built on Linux!";
#elif OSX
            return "Built on macOS!";
#elif Windows
            return "Built in Windows!";
#endif

            return "Built in other environment";
        }
    }
}