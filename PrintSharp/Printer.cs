using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSharp;

namespace PrintSharp
{
    internal class Printer
    {
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

        public bool Printing
        {
            set
            {
                if (!printing) { startJob(); }
                else { pauseJob(); }
                printing = value;
            }
            get { return printing; }
        }

        public Job Job = null;
        public int BufferSize = 16;

        private string port = null;
        private int baud = 115200;
        private SerialPort serialPort = null;
        private bool printing = false;
        private bool paused = false;
        private System.Timers.Timer jobTimer;
        private int bufferedCommands = 0;

        public void startJob()
        {
            jobTimer = new System.Timers.Timer(100);
            jobTimer.Elapsed += new System.Timers.ElapsedEventHandler(jobTimerElapsed);
            jobTimer.Start();

        }

        public void pauseJob() { }

        public void endJob() { }

        private void jobTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            while (bufferedCommands < BufferSize)
            {
                Command command = Job.GetNextCommand();
                if (command.Code != null)
                {
                    Console.WriteLine(command.ToString());
                    Send(command);
                    bufferedCommands++;
                }
                else
                {
                    Console.WriteLine("Skipping: " + command.Text);
                }
            }

            WaitFor("ok 0", 1000);// ToDo: Handle long, long waits like heating
            // Waiting for command responses needs to be handled differently.
            // Do all commands return an "ok" after executing?

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

        public void Print()
        {
            if (Job == null) { return; }

            if (!printing)
            {
                startJob();
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