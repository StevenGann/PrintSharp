using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSharp;

namespace PrintSharp
{
    interface PrinterConnectionInterface
    {

        bool IsConnected
        {
            get;
            set;
        }

        void Connect();

        void Disconnect();

        void Send(Command command);
    }
}
