using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ENet;

namespace CQT.Network
{
    class ENetServer
    {
        protected const int TIMEOUT = 1000;

        protected Thread listeningThread;
        protected List<ENet.Peer> clients;
        protected ENet.Host server;

        public ENetServer(int port, int maxClients)
        {
            clients = new List<ENet.Peer>();
            server = new ENet.Host();
            server.InitializeServer(port, maxClients * 2); // 2 channels per client
        }

        /// <summary>
        /// Launchs the server
        /// </summary>
        public void Launch()
        {
            listeningThread = new Thread(this.listen);
            listeningThread.Start();
        }

        protected unsafe void listen()
        {
            ENet.Event e = new ENet.Event();
            while (true) // TODO : change
            {
                server.Service(TIMEOUT, out e);
                //Console.Out.WriteLine("Message received. Type : " + e.Type);
                switch (e.Type)
                {
                        // needs more error handling
                    case ENet.EventType.Connect:
                        addClient(e.Peer);
                        break;
                    case ENet.EventType.Disconnect:
                        removeClient(e.Peer);
                        break;
                    case ENet.EventType.Receive:
                        String message = new String((sbyte*)e.Packet.Data.ToPointer(), 0, e.Packet.Length);
                        processMessage(message, e.Peer);
                        break;
                    case ENet.EventType.None:
                        break;
                }
            }
        }

        protected unsafe bool addClient(ENet.Peer newClient)
        {
            Console.Out.WriteLine("New connection from client " + newClient.NativeData->address.host + ":"
                + newClient.NativeData->address.port);
            if (clients.Contains(newClient))
            {
                return false;
            }
            clients.Add(newClient);
            return true;
        }

        protected void removeClient(ENet.Peer client)
        {
            clients.Remove(client);
        }

        public void Send(String message, ENet.Peer destination)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.UnreliableFragment);
            destination.Send((byte)(clients.IndexOf(destination)*2), packet);
        }

        public void SendReliable(String message, ENet.Peer destination)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.Reliable);
            destination.Send((byte)(clients.IndexOf(destination) * 2 + 1), packet);
        }

        protected unsafe void processMessage(String message, ENet.Peer sender)
        {
            Console.Out.WriteLine("Message from " + sender.NativeData->address.host + ":"
                + sender.NativeData->address.port + " : " + message);
            SendReliable("Well hello good sir !", sender);
        }
    }
}
