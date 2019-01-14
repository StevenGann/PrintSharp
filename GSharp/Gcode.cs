using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp
{
    public static class Gcode
    {
        private static readonly string[,] table = new string[,] {
            { "G0", "X,Y,Z,E,F,S", "Move" },
            { "G1", "X,Y,Z,E,F,S", "Move" },
            { "G2", "X,Y,I,J,E,F", "Controlled Arc Move Clockwise" },
            { "G3", "X,Y,I,J,E,F", "Controlled Arc Move Counter-Clockwise" },
            { "G4", "P,S", "Dwell" },
            { "G6", "A,B,C,R", "Direct Stepper Move" },
            { "G10", "S", "Retract" },
            { "G11", "", "Unretract" },
            { "G12", "P,S,T", "Clean Tool" },
            { "G17", "", "Plane Selection: XY" },
            { "G18", "", "Plane Selection: ZX" },
            { "G19", "", "Plane Selection: YZ" },
            { "G20", "", "Set Units to Inches" },
            { "G21", "", "Set Units to Millimeters" },
            { "G22", "", "Firmware-Controlled Retract" },
            { "G23", "", "Firmware-Controlled Precharge" },
            { "G26", "", "Mesh Validation Pattern" },
            { "G28", "X,Y,Z", "Move to Origin" },
            { "G29", "S", "Auto Bed Leveling" },
            { "G30", "P,X,Y,Z,H,S", "Single Z-Probe" },
            { "G31", "P,X,Y,Z,C,S,T", "Set of report Current Probe Status" },
            { "G32", "S,P", "Probe Z and Calculate Z Plane" },
            { "G33", "L,R,X,Y,Z", "Measure/List/Adjust Distortion Matrix" },
            { "G34", "", "Set Delta Height Calculated From Toolhead Position" },
            { "G38", "", "Straight Probe" },
            { "G40", "", "Compensation Off" },
            { "G42", "I,J,P", "Move to Grid Point" },
            { "G53", "", "Coordinate System Select" },
            { "G54", "", "Coordinate System Select" },
            { "G55", "", "Coordinate System Select" },
            { "G56", "", "Coordinate System Select" },
            { "G57", "", "Coordinate System Select" },
            { "G58", "", "Coordinate System Select" },
            { "G59", "", "Coordinate System Select" },
            { "G60", "S", "Save current Position To Slot" },
            { "G61", "X,Y,Z,E,F,S", "Apply/Restore Saved Coordinates to Active Extruder" },
            { "G80", "", "Cancel Canned Cycle" },
            { "G90", "", "Set to Absolute Positioning" },
            { "G91", "", "Set to Relative Positioning" },
            { "G92", "X,Y,Z,E", "Set Position" },
            { "M20", "S,P", "List SD Card" },
            { "M21", "P", "Initialize SD Card" },
            { "M22", "P", "Release SD Card" },
            { "M23", "", "Select SD File" },
            { "M24", "", "Start/Resume SD Print" },
            { "M25", "", "Pause SD Print" },
            { "M26", "S,P", "Set SD Position" },
            { "M27", "C,S", "Report SD Print Status" },
            { "M42", "P,S", "Switch I/O Pin" },
            { "M80", "S", "ATX Power On" },
            { "M81", "P,R,S", "ATX Power Off" },
            { "M82", "", "Set Extruder to Absolute Mode" },
            { "M83", "", "Set Extruder to Relative Mode" },
            { "M84", "I", "Stop Idle Hold" },
            { "M85", "S", "Set Inactivity Shutdown Timer" },
            { "M104", "S", "Set Extruder Temperature" },
            { "M105", "R,S", "Get Extruder Temperature" },
            { "M106", "P,S,I,F,L,X,BmH,R,T,C", "Fan On" },
            { "M107", "", "Fan Off" },
            { "M109", "S,R", "Set Extruder Temperature and Wait" },
            { "M112", "", "Emergency Stop" },
            { "M114", "", "Get Current Position" },
            { "M115", "P", "Get Firmware Version and Capabilities" },
            { "M116", "P,H,C", "Wait" },
            { "M117", "", "Display Message" },
            { "M119", "", "Get Endstop Status" },
            { "M140", "P,H,S,R", "Set Bed Temperature (Fast)" },
            { "M155", "S", "Automatically Send Temperatures" },
            { "M190", "S,R", "Wait For Bed Temperature to Reach Target Temperature" },
            { "M205", "", "Output EEPROM Settings" },
            { "M206", "T,P,S,X", "Set EEPROM Value" },
            { "M226", "P,S", "Wait for Pin State" },
            { "M300", "S,P", "Play Beep Sound" },
            { "M320", "S", "Activate Autolevel" },
            { "M321", "S", "Deactivate Autolevel" },
            { "M322", "S", "Reset Autolevel Matrix" },
            { "M323", "S,P", "Distortion Correction On/Off" },
            { "M340", "P,S", "Control Servos" },
            { "M355", "S", "Turn Case Lights On/Off" },
            { "M360", "", "Report Firmware Configuration" },
            { "M400", "", "Wait For Current Moves to Finish" }
        };

        public static string GetDescription(string _command)
        {
            string result = null;
            string command = _command.ToUpper();
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i, 0] == command)
                {
                    result = table[i, 0];
                    break;
                }
            }

            return result;
        }

        public static Command ParseLine(string _line)
        {
            Command result = new Command();
            string text = _line;
            string code = "";
            string comment = "";
            string[] parameters = null; ;
            string t = text;

            if (t.Contains(";"))
            {
                string[] tt = t.Split(';');
                t = tt[0];
                if (tt.Length > 1)
                {
                    for (int i = 1; i < tt.Length; i++)
                    {
                        comment += tt[i];
                    }
                }
            }

            string[] cc = t.Split(' ');
            code = cc[0].ToUpper();

            if (cc.Length > 1)
            {
                for (int i = 1; i < cc.Length; i++)
                {
                    cc[i] = cc[i].Trim();
                }

                int paramcount = 0;
                for (int i = 1; i < cc.Length; i++)
                {
                    if (cc[i].Length != 0)
                    {
                        paramcount++;
                    }
                }

                parameters = new string[paramcount];
                for (int i = 1; i < cc.Length; i++)
                {
                    if (cc[i].Length != 0)
                    {
                        parameters[i - 1] = cc[i];//.ToUpper();
                    }
                }
            }

            result.Text = text;
            result.Code = code;
            result.Comment = comment;
            result.Parameters = parameters;

            return result;
        }
    }

    public class Command
    {
        public string Text = null;
        public string Code = null;
        public string Comment = null;
        public string[] Parameters = null;
        public bool Debug = false;

        public override string ToString()
        {
            string result = "";
            if (Code != null) { result += Code; }

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    result += " ";
                    if (Debug) { result += "("; }
                    result += Parameters[i];
                    if (Debug) { result += ")"; }
                }
            }

            if (Comment != null)
            {
                result += " ;" + Comment;
                if (Debug && Text != null)
                {
                    result += " [" + Text + "]";
                }
            }
            else if (Debug && Text != null)
            {
                result += " ;[" + Text + "]";
            }

            if (result == "")
            { result = ";EMPTY"; }

            return result;
        }

        public string GetClean()
        {
            string result = "";
            if (Code != null)
            {
                result += Code;

                if (Parameters != null)
                {
                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        result += " ";
                        result += Parameters[i];
                    }
                }
            }
            return result;
        }
    }

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
    }
}