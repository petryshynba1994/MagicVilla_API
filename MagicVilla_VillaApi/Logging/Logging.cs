namespace MagicVilla_VillaApi.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if(type == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR - " + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(message);
                Console.BackgroundColor = ConsoleColor.Red;
            }
        }
    }
}
