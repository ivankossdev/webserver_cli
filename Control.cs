using System.IO.Ports;
namespace webserver_cli;

class Control : ComPort
{
    public static void Prog()
    {
        string[] ports = SerialPort.GetPortNames();
        if (ports.Length > 0)
        {
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine($"[ {i} ] {ports[i]}");
            }
        }
        if (PortChoice(ports))
        {
            try
            {

                Open();
                
                Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Порт занят другой программой\n{e}");
            }
        }
    }

    private static bool PortChoice(string[] ports)
    {
        ConsoleKeyInfo pressKey;
        bool state = false;
        do
        {
            Console.WriteLine(Message.exit);
            Console.WriteLine(Message.enterNumPort);
            pressKey = Console.ReadKey();

            int num = Convert.ToInt32(pressKey.KeyChar) & 0x0f;

            if (num <= ports.Length - 1)
            {
                Console.WriteLine($"[ {num} ] {ports[num]}");
                Init(ports[num]);

                state = true;
                break;
            }
            else if (pressKey.Key != ConsoleKey.Escape)
            {
                Console.WriteLine(Message.errorNumPort);
            }
        } while (pressKey.Key != ConsoleKey.Escape);

        return state;
    }
}