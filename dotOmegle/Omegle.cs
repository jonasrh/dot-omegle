﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace dotOmegle
{
    /// <summary>
    /// Allows a user to interact with the Omegle website
    /// </summary>
    public class Omegle
    {
        /// <summary>
        /// Raised when a message from a stranger is received.
        /// </summary>
        public event MessageReceivedEvent MessageReceived;

        /// <summary>
        /// Raised when the Stranger disconnects.
        /// </summary>
        public event EventHandler StrangerDisconnected;

        /// <summary>
        /// Raised when the stranger is typing a message.
        /// </summary>
        public event EventHandler StrangerTyping;

        /// <summary>
        /// Raised when the application is still looking for a partner to connect to.
        /// </summary>
        public event EventHandler WaitingForPartner;

        /// <summary>
        /// The applications stranger ID.
        /// </summary>
        public string ID = null;

        /// <summary>
        /// As long as omegleMode is true, the application shall continue checking for events and reconnecting.
        /// Do not call it directly, call Close() instead.
        /// </summary>
        public bool omegleMode = true;

        /// <summary>
        /// As long as continueRestarts is enabled, the application will continue reconnecting if disconnected.
        /// </summary>
        public bool continueRestarts = true;
        public string response = null;

        /// <summary>
        /// Connects to the Omegle network.
        /// </summary>
        public void Start()
        {
            Console.Title = "OmegleAPI Test";
            while (omegleMode)
            {
                MainLoop();
            }
        }

        /// <summary>
        /// Connects to the Omegle network. Do not call directly, rather Start()
        /// </summary>
        public void MainLoop()
        {
            GetID();
            while (continueRestarts)
            {
                Listen();
                System.Threading.Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Gets a stranger ID from the Omegle service.
        /// </summary>
        public void GetID()
        {
            PostSubmitter post = new PostSubmitter();
            post.Url = "http://bajor.omegle.com/start";
            /*post.PostItems.Add("id", "hello");
            post.PostItems.Add("rel_code", "1102");
            post.PostItems.Add("FREE_TEXT", "c# jobs");
            post.PostItems.Add("SEARCH", "");*/
            post.Type = PostSubmitter.PostTypeEnum.Post;
            ID = post.Post();
            ID = ID.TrimStart('"');
            ID = ID.TrimEnd('"');
        }

        /// <summary>
        /// Sends a message to the connected stranger.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>The stranger response</returns>
        public string SendMessage(string message)
        {
            message = HttpUtility.UrlEncode(message);

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/send";
            sendPost.PostItems.Add("id", ID);
            sendPost.PostItems.Add("msg", message);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            return sendPost.Post();
        }

        /// <summary>
        /// Sends a messsage from the specified ID.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ownID"></param>
        /// <returns></returns>
        public string SendMessageAsID(string message, string ownID)
        {
            message = HttpUtility.UrlEncode(message);

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/send";
            sendPost.PostItems.Add("id", ownID);
            sendPost.PostItems.Add("msg", message);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            return sendPost.Post();
        }

        protected virtual void OnMessageReceived(MessageReceivedArgs e)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(this, e);
            }
        }

        /// <summary>
        /// Listen for events. Do not call directly.
        /// </summary>
        public void Listen()
        {
            PostSubmitter eventlisten = new PostSubmitter();
            eventlisten.Url = "http://bajor.omegle.com/events";
            eventlisten.PostItems.Add("id", ID);
            eventlisten.Type = PostSubmitter.PostTypeEnum.Post;
            string response = eventlisten.Post();
            //Console.WriteLine("response: " + response);
            if (response.Contains("strangerDisconnected"))
            {
                /*Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Stranger disconnected. Restarting loop.");
                Console.ForegroundColor = ConsoleColor.Gray;
                MainLoop();
                return;*/
                this.StrangerDisconnected(this, new EventArgs());
            }
            else if (response.Contains("typing"))
            {
                this.StrangerTyping(this, new EventArgs());
            }
            else if (response.Contains("waiting"))
            {
                this.WaitingForPartner(this, new EventArgs());
            }
            if (response.Contains("gotMessage"))
            {
                //Console.WriteLine(response);

                response = response.TrimStart(new char[] { '[', '[', '"', 'g', 'o', 't', 'M', 'e', 's', 's', 'a', 'g', 'e', '"', ',', ' ', '"' });
                response = response.TrimEnd(new char[] { '"', ']', ']' });
                response = HttpUtility.UrlDecode(response);
                /*
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(response);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n");
                GetResponse();*/
                this.MessageReceived(this, new MessageReceivedArgs(response));
            }
        }

        /// <summary>
        /// Disconnect from the Omegle network.
        /// </summary>
        public void Close()
        {
            continueRestarts = false;
            omegleMode = false;
        }
    }
}