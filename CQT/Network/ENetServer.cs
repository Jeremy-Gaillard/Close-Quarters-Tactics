using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Microsoft.Xna.Framework;

using ENet;

using CQT.Engine;
using CQT.Model;

namespace CQT.Network
{
    public class ENetServer
    {
        protected const int TIMEOUT = 1000;

        protected Thread listeningThread;
        protected List<ENet.Peer> clients;
        protected ENet.Host server;

        protected ServerEngine engine;

        protected Dictionary<ENet.Peer, Player> clientMap;

        protected bool end = false;

        public ENetServer(int port, int maxClients, ServerEngine se)
        {
            engine = se;
            clients = new List<ENet.Peer>();
            clientMap = new Dictionary<Peer, Player>();
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
                        engine.SendCurrentState(e.Peer);
                        break;
                    case ENet.EventType.Disconnect:
                        removeClient(e.Peer);
                        break;
                    case ENet.EventType.Receive:
                        byte[] bytes = new byte[e.Packet.Length];
                        for (int i = 0; i < e.Packet.Length; i++)
                        {
                            bytes[i] = *((byte*)(e.Packet.Data.ToPointer())+i);
                        }
                        dispatchMessage(bytes, e.Peer);
                        break;
                    case ENet.EventType.None:
                        break;
                }
            }
        }

        private void dispatchMessage(byte[] bytes, Peer peer)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryFormatter formater = new BinaryFormatter();
            NetFrame frame = (NetFrame)formater.Deserialize(stream);
            switch (frame.type)
            {
                case NetFrame.FrameType.player:
                    Player newPlayer = engine.AddPlayer((LightPlayer)frame.content);
                    clientMap.Add(peer, newPlayer);
                    break;
                case NetFrame.FrameType.position:
                    engine.UpdatePosition(clientMap[peer], (Vector2)frame.content);
                    break;
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

        public void SendReliable(Object message, NetFrame.FrameType type, ENet.Peer destination)
        {
            ENet.Packet packet = new ENet.Packet();
            NetFrame f = new NetFrame(message, type);
            MemoryStream stream = new MemoryStream(512); // TODO : buffer size ?
            BinaryFormatter formater = new BinaryFormatter();
            formater.Serialize(stream, f); 
            packet.Initialize(stream.GetBuffer(), ENet.PacketFlags.Reliable);
            destination.Send((byte)(clients.IndexOf(destination) * 2 + 1), packet);
            server.Flush();
        }


        public void SendReliable(byte[] message, ENet.Peer destination)
        {
            ENet.Packet packet = new ENet.Packet();
            packet.Initialize(message, ENet.PacketFlags.Reliable);
            destination.Send((byte)(clients.IndexOf(destination) * 2 + 1), packet);
        }

        public void SendReliable(String message)
        {
            ENet.Packet packet = new ENet.Packet();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            packet.Initialize(buffer, ENet.PacketFlags.Reliable);
            server.Broadcast(1 /*TODO : change channel ?*/, ref packet);
        }
    }
}
