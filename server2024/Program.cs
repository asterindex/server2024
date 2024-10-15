// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication_1_1
{
    internal class Program
    {
        public static void SocketExecutor(object obj)
        {
            Socket clientSocket = (Socket)obj;// todo:
            // Отримуємо дані від клієнта
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = clientSocket.Receive(buffer);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (bytesRead == 0)
                    break;
                Console.WriteLine($"Отримано від клієнта: {dataReceived}");

                // Відправляємо дані назад клієнту
                dataReceived = "Hello, Client!";
                clientSocket.Send(Encoding.ASCII.GetBytes(dataReceived));
            }
            // Закриваємо з'єднання з клієнтом
            Console.WriteLine("Закриваємо з'єднання з клієнтом");
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
           
        public static void Main(string[] args)
        {
            // Створюємо сокет для сервера
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // Задаємо адресу та порт для прослуховування
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 6116;
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            // Прив'язуємо сокет до адреси та порту
            serverSocket.Bind(endPoint);

            // Починаємо прослуховування з'єднань
            serverSocket.Listen(10);
            Console.WriteLine("Сервер готовий до прийому клієнтів...");

            int iterator = 0;

            while (true)
            {
                // Очікуємо на підключення клієнта
                Socket clientSocket = serverSocket.Accept();
                iterator++;
                Console.WriteLine(
                    $"З'єднано з клієнтом {((IPEndPoint)clientSocket.RemoteEndPoint).Address}:{((IPEndPoint)clientSocket.RemoteEndPoint).Port}" +
                    " iterator " + iterator);
                // todo:
                Thread thread = new Thread(new ParameterizedThreadStart(SocketExecutor));
                thread.Start(clientSocket);

            }

            
        }
    }
}