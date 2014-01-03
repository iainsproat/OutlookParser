using System;
using System.IO;

namespace EmailVisualiser.ConsoleApp
{
    public class IndentingConsole : System.IO.TextWriter
    {
        System.IO.TextWriter oldConsole;
        bool doIndent;

        public IndentingConsole()
        {
            oldConsole = System.Console.Out;
            System.Console.SetOut(this);
        }

        public int Indent { get; set; }

        public override void Write(char ch)
        {
            if (this.doIndent)
            {
                this.doIndent = false;
                for (int ix = 0; ix < Indent; ++ix)
                {
                    oldConsole.Write("  ");
                }
            }

            oldConsole.Write(ch);
            if (ch == '\n')
            {
                this.doIndent = true;
            }
        }

        public override System.Text.Encoding Encoding
        {
            get 
            { 
                return oldConsole.Encoding; 
            }
        }
    }
}
