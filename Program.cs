namespace webserver_cli;

class Program
{
    static void Main(string[] args)
    {
        Thread threadPortHandler = new(Control.PortHandler);

        Console.WriteLine("Stop programm");
    }

    static void StopControl()
    {
        Control.OpenPort = Console.ReadLine();  
    }
}
