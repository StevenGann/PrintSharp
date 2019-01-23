using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GSharp
{
    public class Job
    {
        public string Name;
        public string SourcePath;
        public List<Command> Commands;
        public int Index = 0;
        public double Progress = 0;

        public Command GetNextCommand()
        {
            Command result = Commands[Index];
            Index++;
            return result;
        }

        public List<Command> GetNextCommands(int _count)
        {
            List<Command> result = new List<Command>();

            for (int i = 0; i < _count && i < Commands.Count; i++)
            {
                result.Add(Commands[Index]);
                Index++;
            }

            return result;
        }

        public Command PeekNextCommand()
        {
            Command result = Commands[Index];
            return result;
        }

        public List<Command> PeekNextCommands(int _count)
        {
            List<Command> result = new List<Command>();

            for (int i = 0; i < _count && i < Commands.Count; i++)
            {
                result.Add(Commands[Index]);
            }

            return result;
        }

        public void Load(string _path)
        {
            //ToDo: Add lazy loading of gcode files to reduce RAM usage

            Commands = new List<Command>();
            using (FileStream fs = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Commands.Add(Gcode.ParseLine(line));
                }
            }

            SourcePath = _path;
            Index = 0;
            Progress = 0;
        }
    }
}
