using System;
using System.IO;

namespace OutlookParserConsoleApp
{
    public class IndentingConsole : TextWriter
    {
        TextWriter oldConsole;
        bool doIndent;

        public IndentingConsole()
        {
            oldConsole = Console.Out;
            Console.SetOut(this);
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
