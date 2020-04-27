using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{

    const int MAX_CONNECTION = 10;
    const int PORT_NUMBER = 8888;

    static TcpListener listener;

    static Dictionary<string, string> _data =
        new Dictionary<string, string>
    {
        {"0","Zero"},
        {"1","One"},
        {"2","Two"},
        {"3","Three"},
        {"4","Four"},
        {"5","Five"},
        {"6","Six"},
        {"7","Seven"},
        {"8","Eight"},
        {"9","Nine"},
        {"10","Ten"},
    };

    public static void Main()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");

        listener = new TcpListener(address, PORT_NUMBER);
        Console.WriteLine("Waiting for connection...");
        listener.Start();

        for (int i = 0; i < MAX_CONNECTION; i++)
        {
            Socket soc = listener.AcceptSocket();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string IPConnected = sb1.ToString();
            if (IPConnected.Contains(soc.RemoteEndPoint.ToString()))
            {
                Console.WriteLine("429 Too Many Request");
                sb.Append("#IP:Port of Client: " + soc.RemoteEndPoint + "-" + "Disconnect At: " + DateTime.Now + "-"+"Reason : 429 Too Many Request");
                File.AppendAllText("E:\\KY 6\\LAP TRINH MANG\\Week\\Access.log", sb.ToString());
                sb.Clear();
            }
            else
            {
                new Thread(Process).Start();
                sb1.Append(soc.RemoteEndPoint);
                File.AppendAllText("ConnectedIP.txt", sb1.ToString());
            }
            
        }
    }

    static void Process()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();

            Console.WriteLine("Connection received from: {0}",
                              soc.RemoteEndPoint);

            StringBuilder sb = new StringBuilder();
            sb.Append("#IP:Port of Client: " + soc.RemoteEndPoint + "-" + "Connect At: " + DateTime.Now);
            File.AppendAllText("E:\\KY 6\\LAP TRINH MANG\\Week\\Access.log", sb.ToString());
            try
            {   
                var stream = new NetworkStream(soc);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;


                while (true)
                {
                    string id = reader.ReadLine();

                    if (id.ToUpper() == "EXIT")
                        writer.WriteLine("Bye");

                    if (_data.ContainsKey(id))
                        writer.WriteLine("Number you've entered: '{0}'", _data[id]);
                    else
                        writer.WriteLine("Number is not valid !");
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.WriteLine("Client disconnected: {0}",
                              soc.RemoteEndPoint);
            soc.Close();
        }
    }
}
