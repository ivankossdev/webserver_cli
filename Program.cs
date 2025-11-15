namespace webserver_cli;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        OperatingSystem os = Environment.OSVersion;
        System.Console.WriteLine($"Операционная система {os}");
        Thread t_PortHandler = new(Control.PortHandler);
        Thread t_StopControl = new(CliControl);
        if (args.Length == 0)
        {
            System.Console.WriteLine("Запустите приложение с аргументами:");
            System.Console.WriteLine("\t[ action ] - Сервер обрабочтик событий");
            System.Console.WriteLine("\t[ comport ] - Программа настрой контроллера");
        }
        if (args.Length != 0)
        {
            foreach (string arg in args)
            {
                if (arg == "action")
                {
                    await SimpleListenerExample();
                }
                else if (arg == "comport")
                {
                    t_PortHandler.Start();
                    t_StopControl.Start();
                    t_StopControl.Join();    
                    Console.WriteLine(Message.stop);
                }
            }
        }
    }

    static async Task SendAction()
    {
       string uri = "http://192.168.0.219:8080/event/104/external1/activate"; 
        string username = "admin";     
        string password = "evidenceadmin";     

        var credentialCache = new CredentialCache
        {
            { new Uri(uri), "Digest", new NetworkCredential(username, password) }
        };

        var handler = new HttpClientHandler
        {
            Credentials = credentialCache
        };

        using var client = new HttpClient(handler);
        try
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP status code is not 2xx

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseBody}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
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

    public static async Task SimpleListenerExample()
    {
        while (true)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            HttpListener listener = new();
            listener.Prefixes.Add("http://*:10500/action/");
            listener.Start();

            Console.WriteLine("Ожидание запроса...");
            
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            
            HttpListenerResponse response = context.Response;

            await SendAction();

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes($"<HTML><BODY>OK</BODY></HTML>");
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
