using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CQT.Network
{
    class Server : PacketProcessor
    {
        Thread listeningThread;
        List<IPEndPoint> clients;
        UdpCommunication communicator;

        public Server(int port)
        {
            communicator = new UdpCommunication(port, this);
            clients = new List<IPEndPoint>();
        }

        /// <summary>
        /// Launchs the server
        /// </summary>
        public void Launch()
        {
            listeningThread = new Thread(communicator.Listen);
            listeningThread.Start();
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
               communicator.Send(message, client);
            }
        }

        public void ProcessMessage(IPEndPoint sender, String message)
        {
            //if (message.Equals("Hello"))
            {
                sender.Port = int.Parse(message);
                Thread.Sleep(1000);
                Console.Out.WriteLine("Adding new client");
                addClient(sender);
                communicator.Send("I love u bro", sender);
            }
        }
    }
}
