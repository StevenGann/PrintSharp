using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSharp;

namespace PrintSharp
{
    class SerialPrinterConnection : PrinterConnectionInterface
    {

        private string port;
        private int baud = 115200;
        private SerialPort serialPort = null;

        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                Connect();
            }
        }

        public bool IsConnected
        {
            get
            {
                if (serialPort != null) { return serialPort.IsOpen; } else return false;
            }
            set
            {
                if (value) { Connect(); }
                else { Disconnect(); }
            }
        }

        public void Connect()
        {
            if (serialPort != null)
            {
                Disconnect();
            }
            else
            {
                serialPort = new SerialPort(port, baud);
            }

            if (port != null)
            {
                serialPort.ReadTimeout = 1000;
                serialPort.WriteTimeout = 1000;

                serialPort.Open();

                if (serialPort.IsOpen)
                {
                    WaitFor("wait", 5000);
                    Send(Gcode.ParseLine("M117 PrintSharp"));
                    WaitFor("ok 0", 5000);
                }
                else
                {
                    throw new Exception("Failed to open COM port " + port);
                }
            }
            else
            {
                throw new Exception("Cannot connect to NULL port");
            }
        }

        public void Disconnect()
        {
            if (serialPort != null)
            {
                serialPort.Close();
            }
        }

        public void Send(Command _command)
        {
            if (!IsConnected) { Connect(); }

            if (IsConnected)
            {
                string command = _command.GetClean();
                serialPort.WriteLine(command);
            }
        }

        private void WaitFor(string _token, int _timeout)
        {
            if (!IsConnected) { Connect(); }

            if (IsConnected)
            {
                bool done = false;
                int count = 0;
                int oldReadTimeout = serialPort.ReadTimeout;
                serialPort.ReadTimeout = _timeout;

                while (!done && count < _timeout)
                {
                    string read = serialPort.ReadLine().Trim();
                    Console.WriteLine(read);
                    if (read.ToUpper() == _token.ToUpper())
                    {
                        done = true;
                    }
                    System.Threading.Thread.Sleep(10);
                    count += 10;
                }

                serialPort.ReadTimeout = oldReadTimeout;

                if (!done) { throw new Exception("Wait for token \"" + _token + "\" timed out"); }
            }
        }
    }
}
