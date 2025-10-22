using System.Diagnostics;
using System.IO.Ports;
namespace webserver_cli;
// dotnet add package System.IO.Ports --version 9.0.10

public class ComPort
{
    protected static SerialPort _serialPort = new();
    protected static void Init(string comport)
    {
        _serialPort.PortName = comport;
        _serialPort.BaudRate = 115200;
        _serialPort.DataBits = 8;
        
        _serialPort.DtrEnable = true;
        _serialPort.RtsEnable = true;

        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;
    }

    protected static bool Open()
    {   
        _serialPort.Open();
        return _serialPort.IsOpen;
    }

    protected static bool Close()
    {
        _serialPort.Close();
        return _serialPort.IsOpen;
    }

    protected static void Write(string message){
        _serialPort.WriteLine(message);
    }

    protected static void ReadLine()
    {
        try
        {
            string message = _serialPort.ReadLine();
            Console.WriteLine(message);
        }
        catch (TimeoutException) { }
    }
}
