/*Copyright (C) 2011 Naarkie (naarkie@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotOmegle;

namespace OmegleAPITest
{
    internal class Program
    {
        public static Omegle omegle = new Omegle();

        private static void Main(string[] args)
        {
            omegle.MessageReceived += new MessageReceivedEvent(omegle_MessageReceived);
            omegle.StrangerDisconnected += new EventHandler(omegle_StrangerDisconnected);
            omegle.StrangerTyping += new EventHandler(omegle_StrangerTyping);
            omegle.WaitingForPartner += new EventHandler(omegle_WaitingForPartner);

            Console.WriteLine("Connecting...");
            omegle.Connect();

        }

        private static void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            Console.WriteLine("Waiting for partner...");
        }

        private static void omegle_StrangerTyping(object sender, EventArgs e)
        {
            Console.WriteLine("Stranger typing...");
        }

        private static void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Stranger is disconnected. Restarting main loop.");
            return;
            /*Console.WriteLine("Stranger is disconnected. Closing connection.");
             *omegle.Close();
             *System.Environment.Exit(0);
             */
        }

        private static void omegle_MessageReceived(object sender, MessageReceivedArgs e)
        {
            Console.WriteLine("Message received: " + e.message + "\n");
            Console.Write(">");
            string answer = Console.ReadLine();
            omegle.SendMessage(answer);
        }
    }
}