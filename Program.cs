namespace webserver_cli;

class Program
{
    static void Main(string[] args)
    {
        Thread t_PortHandler = new(Control.PortHandler);
        Thread t_StopControl = new(StopControl);
        t_PortHandler.Start();
        t_StopControl.Start();

        t_StopControl.Join();    
        Console.WriteLine(Message.stop);
    }

    static void StopControl()
    {
        do
        {
            Thread.Sleep(2000);
            Console.WriteLine(Message.exit1);
            Control.OpenPort = Console.ReadLine() ?? "";
        } while (!Control.OpenPort.Equals(Message.stopWord));
    }
    
}
