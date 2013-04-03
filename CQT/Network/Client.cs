using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CQT.Network
{
    class Client
    {
        IPEndPoint serverAddress;
        Socket sendingSocket;

        public Client()
        {
            serverAddress = null;
            sendingSocket = null;
        }

        /// <summary>
        /// Initialize the conncetion with the server.
        /// </summary>
        /// <param name="serverIP">The server's IP address</param>
        /// <param name="serverPort">The server's port</param>
        /// <param name="listeningPort">The port listend by the client</param>
        /// <returns></returns>
        public bool InitializeConnection(IPAddress serverIP, int serverPort, int listeningPort)
        {
            serverAddress = new IPEndPoint(serverIP, serverPort);
            sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.Out.WriteLine("Client sending message");
            // TODO : UDP can lose messages, repeat until timeout/answer from server
            send("Hello");

            // TODO : listen to server

            return false;
        }

        protected void send(String message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            sendingSocket.SendTo(buffer, serverAddress);
        }
    }
}
