using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CQT.Network
{
    class Client
    {
        IPEndPoint serverAddress;
        Socket sendingSocket;
        UdpClient listener;
        Thread listeningThread;
        int listeningPort;

        public Client()
        {
            serverAddress = null;
            sendingSocket = null;
            listeningPort = 0;
        }

        /// <summary>
        /// Initialize the conncetion with the server.
        /// </summary>
        /// <param name="serverIP">The server's IP address</param>
        /// <param name="serverPort">The server's port</param>
        /// <param name="listeningPort">The port listend by the client</param>
        /// <returns></returns>
        public bool InitializeConnection(IPAddress serverIP, int serverPort, int _listeningPort)
        {
            listeningPort = _listeningPort;
            serverAddress = new IPEndPoint(serverIP, serverPort);
            sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.Out.WriteLine("Client sending message");
            // TODO : UDP can lose messages, repeat until timeout/answer from server
            send(listeningPort.ToString());

            // TODO : check for answer before entering loop
            listener = new UdpClient(listeningPort);
            listeningThread = new Thread(this.listen);
            listeningThread.Start();
            
            

            return false;
        }

        protected void listen()
        {
            Console.Out.WriteLine("Client listening to port " + listeningPort);
            while (true) // TODO : change
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, serverAddress.Port);
                byte[] byteArray;
                byteArray = listener.Receive(ref sender);
                String message = Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
                Console.Out.WriteLine("Client received : " + message.ToString() + " from " + sender.Address.ToString()
                    + ":" + sender.Port.ToString());

                // TODO : handle message
            }
        }

        protected void send(String message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            sendingSocket.SendTo(buffer, serverAddress);
        }
    }
}
