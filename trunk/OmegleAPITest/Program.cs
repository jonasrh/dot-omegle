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

            omegle.continueRestarts = true;
            omegle.omegleMode = true;
            Console.WriteLine("Connecting...");
            omegle.Start();
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
            omegle.MainLoop();
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