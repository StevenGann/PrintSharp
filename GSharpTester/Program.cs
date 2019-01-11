using GSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string raw = "G1 F900 X49.728 Y37.435 E2.17868 ;Test";

            Command command = Gcode.ParseLine(raw);
            command.Debug = true;

            Console.WriteLine(command.ToString());

            Console.ReadLine();
        }
    }
}
