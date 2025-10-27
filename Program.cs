namespace webserver_cli;

class Program
{
    static void Main(string[] args)
    {
        Thread t_PortHandler = new(Control.PortHandler);
        Thread t_StopControl = new(CliControl);
        t_PortHandler.Start();
        t_StopControl.Start();

        t_StopControl.Join();    
        Console.WriteLine(Message.stop);
    }

    static void CliControl()
    {
        Thread.Sleep(2000);
        Console.WriteLine(Message.exit1);
        do
        {
            Control.RxMessage = Console.ReadLine() ?? "";
            if (!Control.RxMessage.Equals(Message.stopWord))
            {
                ComPort.Write(Control.RxMessage);
            }
        } while (!Control.RxMessage.Equals(Message.stopWord));
    }
}
