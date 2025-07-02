namespace MagicVilla_villaApi.Logging
{
    public class Logger:Ilogger
    {
        public void log(string message, string type)
        {
            if (type == "error") { 
                Console.WriteLine("Error - " + message);
            }
            else { 
                Console.WriteLine(message);
            }
        }
    }
}
