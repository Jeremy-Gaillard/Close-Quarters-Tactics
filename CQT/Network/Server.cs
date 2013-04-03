using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CQT.Network
{
    class Server
    {
        Thread listeningThread;
        Socket serverSocket;
        List<IPEndPoint> clients;
        int serverPort;
        UdpClient listener;

        public Server(int port)
        {
            serverPort = port;
            listener = new UdpClient(port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clients = new List<IPEndPoint>();
        }

        /// <summary>
        /// Launchs the server
        /// </summary>
        public void Launch()
        {
            listeningThread = new Thread(this.listen);
            listeningThread.Start();
        }

        protected void listen()
        {
            Console.Out.WriteLine("Server listening to port " + serverPort);
            while (true) // TODO : change
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, serverPort);
                byte[] byteArray;
                byteArray = listener.Receive(ref sender);
                String message = Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
                Console.Out.WriteLine("Server received : " + message.ToString() + " from " + sender.Address.ToString()
                    + ":" + sender.Port.ToString());

                //if (message.Equals("Hello"))
                {
                    sender.Port = int.Parse(message);
                    Thread.Sleep(1000);
                    Console.Out.WriteLine("Adding new client");
                    addClient(sender);
                    send("I love u bro", sender);
                }
            }
        }
        
        protected bool addClient(IPEndPoint newClient)
        {
            foreach(IPEndPoint client in clients)
            {
                if( client == newClient )
                {
                    Console.Out.WriteLine("Client " + client.Address.ToString() + ":" + client.Port.ToString() + " already registered");
                    return false;
                }
            }
            clients.Add(newClient);
            return true;
        }

        protected bool addClient(IPAddress clientIp, int clientPort)
        {
            foreach (IPEndPoint client in clients)
            {
                // Better error handling ?
                if (client.Address == clientIp && client.Port == clientPort)
                {
                    Console.Out.WriteLine("Client " + clientIp.ToString() + ":" + clientPort + " already registered");
                    return false;
                }
            }
            clients.Add(new IPEndPoint(clientIp, clientPort));
            return true;
        }

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendToAllClients(String message)
        {
            foreach (IPEndPoint client in clients)
            {
                send(message, client);
            }
        }

        protected void send(String message, IPEndPoint client)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            serverSocket.SendTo(buffer, client);
        }
    }
}
