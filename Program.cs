namespace webserver_cli;
using System.Net;

class Program
{
    static void Main(string[] args)
    {

        Thread t_PortHandler = new(Control.PortHandler);
        Thread t_StopControl = new(CliControl);
        Thread t_Listener = new(SimpleListenerExample);

        foreach (string arg in args)
        {
            if (arg == "listener")
            {
                t_Listener.Start();
            }
            else
            {
                t_PortHandler.Start();
            }
        }
        
        t_StopControl.Start();
        t_StopControl.Join();    
        Console.WriteLine(Message.stop);
    }

    static void CliControl()
    {
        while (!Control.isWork) { }

        if (Control.isWork)
        {
            Console.WriteLine(Message.exit1);
            do
            {
                if (ComPort.isException) break;

                Control.RxMessage = Console.ReadLine() ?? "";
                if (!Control.RxMessage.Equals(Message.stopWord))
                {
                    ComPort.Write(Control.RxMessage);
                }
            } while (!Control.RxMessage.Equals(Message.stopWord));
        }
    }

    public static void SimpleListenerExample()
    {
        while (true)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // if (prefixes == null || prefixes.Length == 0)
            //     throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add("http://*:10500/");
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = $"<HTML><BODY>OK</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            
            Console.WriteLine(request.UserAgent);
            Console.WriteLine(request.UserHostName);
            Console.WriteLine(request.UserLanguages);
            Console.WriteLine(request.Headers);

            output.Close();
            listener.Stop();
        }
    }
    
}
