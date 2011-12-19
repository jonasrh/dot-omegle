using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChatterBotAPI;
using dotOmegle;

namespace CleverOmegle
{
    public class Program
    {
        public static ChatterBotFactory factory = new ChatterBotFactory();
        public static Omegle omegle = new Omegle();
        public static DateTime now = DateTime.Now;
        public static string day;
        public static string month;
        public static string year;
        public static string fileLocation;
        public static ChatterBotSession bot;

        public static void Log(string text)
        {
            if (!File.Exists(fileLocation))
            {
                FileStream fs = File.Create(fileLocation);
                fs.Close();
            }
            File.AppendAllText(fileLocation, "\r\n" + text);
        }

        public static void Main(string[] args)
        {
            Console.Title = "CleverOmegle";
            if (!Directory.Exists(@"logs/"))
            {
                Directory.CreateDirectory(@"logs/");
            }
            ChatterBot cake = factory.Create(ChatterBotType.CLEVERBOT);
            day = now.Day.ToString();
            month = now.Month.ToString();
            year = now.Year.ToString();
            fileLocation = @"logs\" + String.Format("{0}-{1}-{2}.txt", day, month, year);
            bot = cake.CreateSession();

            omegle.MessageReceived += new MessageReceivedEvent(omegle_MessageReceived);
            omegle.StrangerTyping += new EventHandler(omegle_StrangerTyping);
            omegle.StrangerDisconnected += new EventHandler(omegle_StrangerDisconnected);
            omegle.WaitingForPartner += new EventHandler(omegle_WaitingForPartner);
            omegle.Connected += new EventHandler(omegle_Connected);
            omegle.Start();
            omegle.omegleMode = true;
            omegle.continueRestarts = true;
        }

        private static void omegle_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected.");
            Log("\r\nConnected");
        }

        private static void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            Console.WriteLine("Waiting for partner...");
            Log("Waiting for partner...");
        }

        private static void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Stranger is disconnected. Restarting main loop.");
            Log("Stranger is disconnected. Restarting main loop.");
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
            DateTime temporaryNow = DateTime.Now;
            string time = string.Format("{0}:{1}:{2} ", temporaryNow.Hour, temporaryNow.Minute, temporaryNow.Second);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Stranger: " + e.message);
            Log(time + "Stranger: " + e.message);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string response = bot.Think(e.message);
            omegle.SendMessage(response);
            Console.WriteLine("Cleverbot: " + response);
            Log(time + "Cleverbot: " + response);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}