using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CQT.Network
{
    class Client : PacketProcessor
    {
        IPEndPoint serverAddress;
        Thread listeningThread;
        UdpCommunication communicator;

        public Client()
        {
            serverAddress = null;
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
            communicator = new UdpCommunication(listeningPort, this);

            Console.Out.WriteLine("Client sending message");

            if (communicator.SecureSend(listeningPort.ToString(), serverAddress))
            {
                listeningThread = new Thread(communicator.Listen);
                listeningThread.Start();
                return true;
            }
            return false;
        }

        public void ProcessMessage(IPEndPoint sender, String message)
        {
            
        }
    }
}
