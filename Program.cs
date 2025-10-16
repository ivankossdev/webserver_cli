namespace webserver_cli;

class Program
{
    static void Main(string[] args)
    {
        Thread thProg = new Thread(Control.Prog);
        Thread thMyPrint = new Thread(MyPrint);
        
        thProg.Start();
        thMyPrint.Start();

        thMyPrint.Join();

        Console.WriteLine("Stop programm");
    }
    
    static void MyPrint()
    {
        int i = 0; 
        while (i < 10)
        {
            System.Console.WriteLine($"i = {i}");
            Thread.Sleep(2000);
            i++; 
        }
    }
}
