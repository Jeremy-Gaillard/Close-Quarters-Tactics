using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CQT.Network
{
    class UdpCommunication
    {
        Socket sendingSocket;
        UdpClient listener;
        PacketProcessor processor;

        public UdpCommunication(int port, PacketProcessor packetProcessor)
        {
            processor = packetProcessor;
            listener = new UdpClient(port);
            sendingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Send(String message, IPEndPoint destination)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            sendingSocket.SendTo(buffer, destination);
        }

        public bool SecureSend(String message, IPEndPoint destination)
        {
            Send(message, destination);
            return true; // TODO real implementation
        }

        public void Listen()
        {
            Console.Out.WriteLine("Listening to port " + ((IPEndPoint)listener.Client.LocalEndPoint).Port);
            while (true) // TODO : change
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                byte[] byteArray;
                byteArray = listener.Receive(ref sender);
                String message = Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
                Console.Out.WriteLine("Received : " + message.ToString() + " from " + sender.Address.ToString()
                    + ":" + sender.Port.ToString());

                processor.ProcessMessage(sender, message);
            }
        }
    }
}
