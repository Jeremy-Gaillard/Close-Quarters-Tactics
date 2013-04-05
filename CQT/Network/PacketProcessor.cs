using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CQT.Network
{
    interface PacketProcessor
    {
        void ProcessMessage(IPEndPoint sender, String message);
    }
}
