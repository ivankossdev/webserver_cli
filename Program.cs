namespace webserver_cli;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Thread t_PortHandler = new(Control.PortHandler);
        Thread t_StopControl = new(CliControl);

        if (args.Length >= 1)
        {
            foreach (string arg in args)
            {
                if (arg == "action")
                {
                    await SimpleListenerExample();
                }
            }
        }
        else
        {
            t_PortHandler.Start();
            t_StopControl.Start();
            t_StopControl.Join();    
            Console.WriteLine(Message.stop);
        }
    }

    static async Task SendAction()
    {
       string uri = "http://192.168.0.219:8080/event/104/external1/activate"; // Replace with your target URI
        string username = "admin";     // Replace with your username
        string password = "evidenceadmin";     // Replace with your password

        var credentialCache = new CredentialCache();
        credentialCache.Add(new Uri(uri), "Digest", new NetworkCredential(username, password));

        var handler = new HttpClientHandler
        {
            Credentials = credentialCache
        };

        using (var client = new HttpClient(handler))
        {
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
            // if (prefixes == null || prefixes.Length == 0)
            //     throw new ArgumentException("prefixes");

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://192.168.0.175:10500/action/");
            listener.Start();

            Console.WriteLine("Listening...");
            
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            
            HttpListenerResponse response = context.Response;
            string responseString = $"<HTML><BODY>OK</BODY></HTML>";

            await SendAction();

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
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
