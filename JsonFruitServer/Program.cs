using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using JsonFruit;

namespace JsonFruitServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fruit server ready");

            TcpListener listener = new TcpListener(IPAddress.Any, 10002);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("New client");

                Task.Run(() =>
                {
                    HandleClient(socket);
                });
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            string message = reader.ReadLine();

            Fruit fromJson = JsonSerializer.Deserialize<Fruit>(message);
            Console.WriteLine("Frugt type: " + fromJson.typeOfFruit);
            writer.WriteLine("Frugt modtaget");
            writer.Flush();
            socket.Close();
        }
    }
}
