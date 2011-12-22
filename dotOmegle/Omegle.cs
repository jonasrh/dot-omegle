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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotOmegle
{
    /// <summary>
    /// Allows a user to interact with the Omegle website
    /// </summary>
    public class Omegle
    {
        private string[] servers = new string[]
        {
            "bajor","quarks" //finish servers implementation
        };
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
        public event EventHandler StrangerStoppedTyping;
        public event EventHandler Count;
        public event EventHandler WebException;
        public event UnhandledResponseEvent UnhandledResponse;
        public event CaptchaRequiredEvent CaptchaRequired;
        public event EventHandler CaptchaRefused;

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
        /// <summary>
        /// Sends a message to the connected stranger.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>The stranger response</returns>
        public string SendMessage(string message)
        {
            message = HttpUtility.UrlEncode(message); //URL encode it first

            return SendMessageRaw(message);
        }

        public string SendMessageRaw(string message)
        {
            //Send Message format: [url]http://bajor.omegle.com/send?id=ID&msg=MSG[/url]

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/send";
            sendPost.PostItems.Add("id", ID);
            sendPost.PostItems.Add("msg", message);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            return sendPost.Post();
        }

        public string SendCaptcha(string challenge, string response)
        {
            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/recaptcha";
            sendPost.PostItems.Add("id", ID);
            sendPost.PostItems.Add("challenge", challenge);
            sendPost.PostItems.Add("response", response);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            return sendPost.Post();
        }

        public void StartTyping()
        {
            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/typing";
            sendPost.PostItems.Add("id", ID);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            sendPost.Post();
        }

        public void StopTyping()
        {
            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = "http://bajor.omegle.com/stoppedtyping";
            sendPost.PostItems.Add("id", ID);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            sendPost.Post();
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

        protected virtual void UnhandledResponseEvent(UnhandledResponseEventArgs e)
        {
            if (this.UnhandledResponse != null)
            {
                this.UnhandledResponse(this, e);
            }
        }

        //Huge thanks to voodooattack for this and lots more. Cannot stress enough how thankful I am.
        //Here, have a cookie: ( # ) <- cookie
        private void Parse(string response)
        {
            JArray events = JsonConvert.DeserializeObject<JArray>(response);

            if (events != null)
            {
                foreach (JToken ev in events)
                {
                    string event_ = ev[0].ToString();
                    switch (event_)
                    {
                        //we need to prefix and suffix each one with a literal " character
                        case "\"connected\"":
                            if (this.Connected != null)
                                this.Connected(this, new EventArgs());
                            break;
                        case "\"strangerDisconnected\"":
                            if (this.StrangerDisconnected != null)
                                this.StrangerDisconnected(this, new EventArgs());
                            break;
                        case "\"gotMessage\"":
                            if (this.MessageReceived != null)
                                this.MessageReceived(this, new MessageReceivedArgs(ev[1].ToString().TrimStart('"').TrimEnd('"')));
                            break;
                        case "\"waiting\"":
                            if (this.WaitingForPartner != null)
                                this.WaitingForPartner(this, new EventArgs());
                            break;
                        case "\"typing\"":
                            if (this.StrangerTyping != null)
                                this.StrangerTyping(this, new EventArgs());
                            break;
                        case "\"stoppedTyping\"":
                            if (this.StrangerStoppedTyping != null)
                                this.StrangerStoppedTyping(this, new EventArgs());
                            break;
                        case "\"count\"":
                            if (this.Count != null)
                                this.Count(this, new EventArgs()); //I'm a cheapskate, ev[1] holds user count though.
                            break;
                        case "\"recaptchaRequired\"":
                            if (this.CaptchaRequired != null)
                                this.CaptchaRequired(this, new CaptchaRequiredArgs(ev[1].ToString()));
                            break;
                        case "\"recaptchaRejected":
                            if (this.CaptchaRefused != null)
                                this.CaptchaRefused(this, new EventArgs());
                            break;
                        case "\"error\"": // should probably handle this one
                        case "\"spyMessage\"":
                        case "\"spyTyping\"":
                        case "\"spyStoppedTyping\"":
                        case "\"spyDisconnected\"":
                        case "\"question\"":
                        case "\"suggestSpyee\"":
                        default:
                            if (this.UnhandledResponse != null)
                            {
                                this.UnhandledResponse(this, new UnhandledResponseEventArgs(ev.ToString()));
                            }
                            break;
                    }
                }
            }
        }

        private void Listen()
        {
            PostSubmitter eventlisten = new PostSubmitter();
            eventlisten.Url = "http://bajor.omegle.com/events";
            eventlisten.PostItems.Add("id", ID);
            eventlisten.Type = PostSubmitter.PostTypeEnum.Post;

            string response = eventlisten.Post();

            Parse(response);
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