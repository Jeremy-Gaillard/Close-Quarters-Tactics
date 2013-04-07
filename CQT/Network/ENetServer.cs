using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ENet;

using CQT.Engine;

namespace CQT.Network
{
    public class ENetServer
    {
        protected const int TIMEOUT = 1000;

        protected Thread listeningThread;
        protected List<ENet.Peer> clients;
        protected ENet.Host server;

        protected ServerEngine engine;

        protected bool end = false;

        public ENetServer(int port, int maxClients, ServerEngine se)
        {
            engine = se;
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

        public void Shutdown()
        {
            foreach (ENet.Peer p in clients)
            {
                p.DisconnectLater(0);
            }
            clients.Clear();
            end = true;
            listeningThread.Join();
            Console.Out.WriteLine("Server down");
        }


        protected unsafe void listen()
        {
            ENet.Event e = new ENet.Event();
            while (!end) // TODO : change
            {
                server.Service(TIMEOUT, out e);
                //Console.Out.WriteLine("Message received. Type : " + e.Type);
                switch (e.Type)
                {
                        // needs more error handling
                    case ENet.EventType.Connect:
                        addClient(e.Peer);
                        engine.SendCurrentState(e.Peer);    // TODO : change the way this works ?
                        break;
                    case ENet.EventType.Disconnect:
                        removeClient(e.Peer);
                        break;
                    case ENet.EventType.Receive:
                        String message = new String((sbyte*)e.Packet.Data.ToPointer(), 0, e.Packet.Length);
                        engine.processMessage(message, e.Peer);
                        //processMessage(message, e.Peer);
                        break;
                    case ENet.EventType.None:
                        break;
                }
            }
        }

        protected bool addClient(ENet.Peer newClient)
        {
            Console.Out.WriteLine("New connection from client " + newClient.GetRemoteAddress().Address.ToString() + ":"
                + newClient.GetRemoteAddress().Port);
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

        public void Send(String message)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.UnreliableFragment);
            server.Broadcast(0 /*TODO: change channel ?*/,ref packet);
        }

        public void SendReliable(String message, ENet.Peer destination)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.Reliable);
            destination.Send((byte)(clients.IndexOf(destination) * 2 + 1), packet);
        }

        public void SendReliable(String message)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.Reliable);
            server.Broadcast(1 /*TODO : change channel ?*/, ref packet);
        }

        protected void processMessage(String message, ENet.Peer sender)
        {
            Console.Out.WriteLine("Message from " + sender.GetRemoteAddress().Address.ToString() + ":"
                + sender.GetRemoteAddress().Port + " : " + message);
            SendReliable("Well hello good sir !", sender);
        }
    }
}
