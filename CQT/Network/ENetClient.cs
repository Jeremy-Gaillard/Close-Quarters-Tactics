using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

using ENet;

namespace CQT.Network
{
    class ENetClient
    {
        protected const int TIMEOUT = 1000;
        protected const int CONNECTIONTIMEOUT = 10000;

        protected Thread listeningThread;
        protected ENet.Peer server;
        protected ENet.Host client;

        public ENetClient()
        {
            client = new ENet.Host();
            client.InitializeClient(2); // 2 channels per client
        }

        public bool Connect(IPEndPoint serverAddress)
        {
            client.Connect(serverAddress, 0, 2);
            ENet.Event e = new ENet.Event();
            if (client.Service(CONNECTIONTIMEOUT, out e) && e.Type == ENet.EventType.Connect)
            {
                Console.Out.WriteLine("Successful connection to " + serverAddress.Address.ToString() + ":" + serverAddress.Port);
                server = e.Peer;
                listeningThread = new Thread(this.listen);
                listeningThread.Start();
                return true;
            }
            else
            {
                Console.Out.WriteLine("Connection failure to " + serverAddress.Address.ToString() + ":" + serverAddress.Port);
                return false;
            }

        }

        protected unsafe void listen()
        {
            ENet.Event e = new ENet.Event();
            while (true) // TODO : change
            {
                client.Service(TIMEOUT, out e);
                //Console.Out.WriteLine("Message received. Type : " + e.Type);
                switch (e.Type)
                {
                        // needs more error handling
                    case ENet.EventType.Connect:
                        break;
                    case ENet.EventType.Disconnect:
                        // TODO
                        break;
                    case ENet.EventType.Receive:
                        String message = new String((sbyte*)e.Packet.Data.ToPointer(), 0, e.Packet.Length);
                        processMessage(message);
                        break;
                    case ENet.EventType.None:
                        break;
                }
            }
        }

        public void Send(String message)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.UnreliableFragment);
            server.Send(0, packet);
        }

        public void SendReliable(String message)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.Reliable);
            server.Send(1, packet);
        }

        protected unsafe void processMessage(String message)
        {
            Console.Out.WriteLine("Message from " + server.NativeData->address.host + ":"
                + server.NativeData->address.port + " : " + message);
        }
    }
}
