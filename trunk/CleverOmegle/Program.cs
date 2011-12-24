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
using System.IO;
using System.Linq;
using System.Text;
using ChatterBotAPI;
using dotOmegle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public static bool BotInitiates = false;
        public static string captchaURL = null;
        public static CaptchaWindow captcha = new CaptchaWindow();

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
            captcha.Hide();
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
            omegle.UnhandledResponse += new UnhandledResponseEvent(omegle_UnhandledResponse);
            omegle.WebException += new WebExceptionEvent(omegle_WebException);
            omegle.CaptchaRequired += new CaptchaRequiredEvent(omegle_CaptchaRequired);
            omegle.CaptchaRefused += new EventHandler(omegle_CaptchaRefused);
            Console.WriteLine("Should the bot start conversations? (y/n)");
            string answerBool = Console.ReadLine();
            if (string.Equals(answerBool, "y", StringComparison.CurrentCultureIgnoreCase)) //y and Y will both yield true
            {
                BotInitiates = true;
            }
            else
            {
                BotInitiates = false;
            }
            Console.Clear();
            omegle.Start();
            omegle.omegleMode = true;
            omegle.continueRestarts = true;
        }

        private static void omegle_WebException(object sender, WebExceptionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception met: {0}", e.exception.ToString());
            Console.ResetColor();
            Console.WriteLine("Restarting..");
            //Console.ReadKey();
            omegle.MainLoop();
            return;
        }

        private static void omegle_CaptchaRequired(object sender, CaptchaRequiredArgs e)
        {
            Console.ResetColor();
            Console.WriteLine("Captcha Required. Press any key to launch browser.");
            Console.ReadKey();

            PostSubmitter post = new PostSubmitter();
            post.Url = "http://www.google.com/recaptcha/api/challenge";
            post.PostItems.Add("k", e.id);
            post.PostItems.Add("ajax", "1");
            post.Type = PostSubmitter.PostTypeEnum.Get;

            string response = "{" + post.Post().Split(new char[] { '{', '}' })[1] + "}";
            string challenge = JsonConvert.DeserializeObject<JObject>(response)["challenge"].ToString();
            captchaURL = "http://www.google.com/recaptcha/api/image?c=" + challenge;
            //System.Diagnostics.Process.Start("http://www.google.com/recaptcha/api/image?c=" + challenge);
            captcha.ShowDialog();
            Console.Write("Please Input Captcha: ");
            string userInput = Console.ReadLine();

            if (userInput != string.Empty)
                omegle.SendCaptcha(challenge, userInput);
            else
                omegle.MainLoop();
        }

        public static void omegle_CaptchaRefused(object sender, EventArgs e)
        {
            Console.WriteLine("Captcha invalid.");
            omegle.MainLoop();
        }

        private static void omegle_UnhandledResponse(object sender, UnhandledResponseEventArgs e)
        {
            Console.WriteLine(e.response);
        }

        private static void omegle_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected.");
            Log("\r\nConnected");
            if (BotInitiates)
                omegle_MessageReceived(null, new MessageReceivedArgs(string.Empty));
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
            if (e.message != string.Empty)
            {
                Console.WriteLine("Stranger: " + e.message);
                Log(time + "Stranger: " + e.message);
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            omegle.StartTyping();
            string response = bot.Think(e.message).Replace("Cleverbot", "Jasmin"); //MWAHAHAHA
            omegle.StopTyping();
            omegle.SendMessageRaw(response); //Already URI encoded
            Console.WriteLine("Cleverbot: " + response);
            Log(time + "Cleverbot: " + response);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}