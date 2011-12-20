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
        public event EventHandler Connected;

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
            GetID(); //fetches a new ID
            while (continueRestarts)
            {
                Listen();
                System.Threading.Thread.Sleep(1); //Not 100% sure if I need this, but it seems to break if I don't have it.
                //Ho ho ho, hackery hackery doo
            }
        }

        /// <summary>
        /// Gets a stranger ID from the Omegle service.
        /// </summary>
        public void GetID()
        {
            PostSubmitter post = new PostSubmitter();
            post.Url = "http://bajor.omegle.com/start";
            post.Type = PostSubmitter.PostTypeEnum.Post;
            ID = post.Post();
            ID = ID.TrimStart('"'); //gets rid of " at the start and end
            ID = ID.TrimEnd('"');
        }

        /// <summary>
        /// Sends a message to the connected stranger.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>The stranger response</returns>
        public string SendMessage(string message)
        {
            //Send Message format: http://bajor.omegle.com/send?id=ID&msg=MSG

            message = HttpUtility.UrlEncode(message); //URL encode it first

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
            //This method could potentially be used to send messages from another user.
            //One would have to acquire said users ID first.
            //TODO: Find a way to get a strangers ID
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

        private void Listen()
        {
            //Todo: Bloody hell get some proper parsing
            PostSubmitter eventlisten = new PostSubmitter();
            eventlisten.Url = "http://bajor.omegle.com/events";
            eventlisten.PostItems.Add("id", ID);
            eventlisten.Type = PostSubmitter.PostTypeEnum.Post;
            string response = eventlisten.Post();
            if (response.Contains("strangerDisconnected"))
            {
                if (this.StrangerDisconnected != null)
                {
                    this.StrangerDisconnected(this, new EventArgs());
                }
            }
            else if (response.Contains("connected"))
            {
                if (this.Connected != null)
                {
                    this.Connected(this, new EventArgs());
                }
            }
            else if (response.Contains("typing"))
            {
                if (this.StrangerTyping != null)
                {
                    this.StrangerTyping(this, new EventArgs());
                }
            }
            else if (response.Contains("waiting"))
            {
                if (this.WaitingForPartner != null)
                {
                    this.WaitingForPartner(this, new EventArgs());
                }
            }
            if (response.Contains("gotMessage"))
            {
                //Console.WriteLine(response);
                //Todo: Especially here :/
                response = response.TrimStart(new char[] { '[', '[', '"', 'g', 'o', 't', 'M', 'e', 's', 's', 'a', 'g', 'e', '"', ',', ' ', '"' });
                response = response.TrimEnd(new char[] { '"', ']', ']' });
                response = HttpUtility.UrlDecode(response);
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