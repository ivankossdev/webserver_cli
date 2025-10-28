using System.IO.Ports;
namespace webserver_cli;

class Control : ComPort
{
    public static string? RxMessage;
    public static bool isWork; 
    public static void PortHandler()
    {
        string openSerialPort = string.Empty;
        string[] ports = SerialPort.GetPortNames();
        if (ports.Length > 0)
        {
            for (int i = 0; i < ports.Length; i++)
            {
                openSerialPort = ports[i];
                Console.WriteLine($"[ {i} ] {openSerialPort}");
            }
        }
        isWork = PortChoice(ports);
        if (isWork)
        {
            try
            {

                Open();
                do
                {
                    ReadLine();
                    if (RxMessage == Message.stopWord) break; // Выход из программы 
                    else if (RxMessage == Message.clear)      // Очищает экран консоли 
                    {
                        Console.Clear();
                        RxMessage = string.Empty;
                    }
                } while (_serialPort.IsOpen);
                Close();

            }
            catch (Exception e) when (e.Message.Contains($"Access to the port '{openSerialPort}' is denied"))
            {
                Console.Clear();
                Console.WriteLine($"Порт занят другой программой\n");
            }
        }
        isWork = true;
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
            Console.Clear();

            if (num <= ports.Length - 1)
            {
                Console.WriteLine($"Открыт порт {ports[num]}");
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