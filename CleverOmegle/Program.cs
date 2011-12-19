using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatterBotAPI;
using dotOmegle;

namespace CleverOmegle
{
    internal class Program
    {
        public static ChatterBotFactory factory = new ChatterBotFactory();
        public static Omegle omegle = new Omegle();
        public static ChatterBotSession bot;

        public static void Main(string[] args)
        {
            Console.Title = "CleverOmegle";
            ChatterBot cake = factory.Create(ChatterBotType.CLEVERBOT);
            bot = cake.CreateSession();

            omegle.MessageReceived += new MessageReceivedEvent(omegle_MessageReceived);
            omegle.StrangerTyping += new EventHandler(omegle_StrangerTyping);
            omegle.StrangerDisconnected += new EventHandler(omegle_StrangerDisconnected);
            omegle.WaitingForPartner += new EventHandler(omegle_WaitingForPartner);
            omegle.Start();
            omegle.omegleMode = true;
            omegle.continueRestarts = true;
        }

        private static void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            Console.WriteLine("Waiting for partner...");
        }

        private static void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Stranger is disconnected. Restarting main loop.");
            Console.ForegroundColor = ConsoleColor.Gray;
            omegle.MainLoop();
            return;
        }

        private static void omegle_StrangerTyping(object sender, EventArgs e)
        {
            //Console.WriteLine("Stranger typing...");
        }

        public static void omegle_MessageReceived(object sender, MessageReceivedArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Stranger: " + e.message);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string response = bot.Think(e.message);
            omegle.SendMessage(response);
            Console.WriteLine("Cleverbot: " + response);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}