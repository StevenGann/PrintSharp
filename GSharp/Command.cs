using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp
{
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
}
