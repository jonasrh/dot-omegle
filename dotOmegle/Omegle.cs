﻿/*Copyright (C) 2011 Naarkie (naarkie@gmail.com)

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
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotOmegle
{
    /// <summary>Allows a user to interact with the Omegle website</summary>
    public class Omegle
    {
        public string[] serverList = new string[]
        {
            "bajor","quarks" //finish serverList implementation
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

        /// <summary>
        /// Raised a stranger is connected.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Raised when the stranger stops typing a message.
        /// </summary>
        public event EventHandler StrangerStoppedTyping;

        /// <summary>
        /// Raised when a user count message is received.
        /// </summary>
        public event EventHandler Count;

        /// <summary>
        /// Raised when the background event processor encounters an exception.
        /// </summary>
        public event WebExceptionEvent WebException;

        /// <summary>
        /// Raised when an unknown or unhandled event is received.
        /// </summary>
        public event UnhandledResponseEvent UnhandledResponse;

        /// <summary>
        /// Raised when the server asks for a captcha response.
        /// </summary>
        public event CaptchaRequiredEvent CaptchaRequired;

        /// <summary>
        /// Raised when the server refuses a captcha.
        /// </summary>
        public event EventHandler CaptchaRefused;

        /// <summary>
        /// Raised when the application is still looking for a partner to connect to.
        /// </summary>
        public event EventHandler WaitingForPartner;

        /// <summary>
        /// The applications stranger Id.
        /// </summary>
        /// <value>
        /// The user's id.
        /// </value>
        public string Id { get; protected set; }

        /// <summary>
        /// What server to connect to?
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Omegle"/> throws
        /// exceptions or passes them through the <see cref="WebException"/> event handler.
        /// (except for exceptions from <see cref="Listen()"/>, which are always passed through.)
        /// </summary>
        /// <value><c>true</c> if throws; otherwise, <c>false</c>.</value>
        public bool Throws { get; set; }

        /// <summary>
        /// Event processor status.
        /// </summary>
        public enum status
        {
            Stopped,
            Started
        }

        /// <summary>
        /// Gets the current status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public status Status { get; protected set; }

        /// <summary>
        /// Gets a boolean specifying whether or not there's an active connection.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Omegle"/> class.
        /// </summary>
        public Omegle()
        {
            Id = null;
            Status = status.Stopped;
            IsConnected = false;
            Throws = true;
            Server = serverList[0];
        }

        /// <summary>
        /// Connects to the Omegle network and starts processing events.
        /// </summary>
        public void Connect()
        {
            GetID();
            Start();
        }

        /// <summary>
        /// Starts event processing.
        /// </summary>
        public void Start()
        {
            if (Status != status.Started)
            {
                Status = status.Started;
                ThreadPool.QueueUserWorkItem(new WaitCallback(WorkerProc));
            }
        }

        /// <summary>
        /// Stops event processing.
        /// </summary>
        public void Stop()
        {
            Status = status.Stopped;
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public void Disconnect()
        {
            Stop();
            SendDisconnect();
            Id = null;
            IsConnected = false;
        }

        /// <summary>
        /// Reconnects and begins a new conversation.
        /// </summary>
        public void Reconnect()
        {
            Disconnect();
            Connect();
        }

        /// <summary>
        /// Gets a new Id from the Omegle service.
        /// </summary>
        public void GetID()
        {
            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/start", Server);
            sendPost.PostItems.Add("rcs", "1");
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            Id = sendPost.Post();
            Id = Id.TrimStart('"'); //gets rid of " at the start and end
            Id = Id.TrimEnd('"');
        }

        /// <summary>
        /// Sends a message to the connected stranger.
        /// </summary>
        /// <param name="message">The message to be sent</param>
        /// <returns>
        /// The stranger's response
        /// </returns>
        public string SendMessage(string message)
        {
            message = HttpUtility.UrlEncode(message); //URL encode it first

            return SendMessageRaw(message);
        }

        /// <summary>
        /// Sends a message to the connected stranger. This version does not html-encode the message.
        /// </summary>
        /// <param name="message">The message to be sent</param>
        /// <returns></returns>
        public string SendMessageRaw(string message)
        {
            if (!IsConnected)
                return null;

            //Send Message format: [url]http://bajor.omegle.com/send?id=Id&msg=MSG[/url]

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/send", Server);
            sendPost.PostItems.Add("id", Id);
            sendPost.PostItems.Add("msg", message);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            return sendPost.Post();
        }

        /// <summary>
        /// Sends a captcha response to the Server.
        /// </summary>
        /// <param name="challenge">The challenge value</param>
        /// <param name="response">The response string</param>
        /// <returns></returns>
        public string SendCaptcha(string challenge, string response)
        {
            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/recaptcha", Server);
            sendPost.PostItems.Add("id", Id);
            sendPost.PostItems.Add("challenge", challenge);
            sendPost.PostItems.Add("response", response);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            return sendPost.Post();
        }

        /// <summary>
        /// Notifies the Server that the user has started typing.
        /// </summary>
        public void StartTyping()
        {
            if (!IsConnected)
                return;

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/typing", Server);
            sendPost.PostItems.Add("id", Id);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            sendPost.Post();
        }

        /// <summary>
        /// Notifies the Server that the user has finished typing.
        /// </summary>
        public void StopTyping()
        {
            if (!IsConnected)
                return;

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/stoppedtyping", Server);
            sendPost.PostItems.Add("id", Id);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            sendPost.Post();
        }

        /// <summary>
        /// Sends a message from the specified Id.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ownID">The ID.</param>
        /// <returns></returns>
        public string SendMessageAsID(string message, string ownID)
        {
            if (!IsConnected)
                return null;

            //This method could potentially be used to send messages from another user.
            //One would have to acquire said users Id first.
            //TODO: Find a way to get a strangers Id
            message = HttpUtility.UrlEncode(message);

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/send", Server);
            sendPost.PostItems.Add("id", ownID);
            sendPost.PostItems.Add("msg", message);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            return sendPost.Post();
        }

        /// <summary>
        /// Sends a disconnect message to the server, ending the conversation.
        /// </summary>
        /// <returns></returns>
        public string SendDisconnect()
        {
            if (!IsConnected)
                return null;

            PostSubmitter sendPost = new PostSubmitter();
            sendPost.Url = String.Format("http://{0}.omegle.com/disconnect", Server);
            sendPost.PostItems.Add("id", Id);
            sendPost.Type = PostSubmitter.PostTypeEnum.Post;

            if (!Throws)
                sendPost.WebExceptionEvent += WebException;

            return sendPost.Post();
        }

        /// <summary>Internal event parsing.</summary>
        /// <param name="response">The response.</param>
        /// <returns>True if events were processed.</returns>
        private bool Parse(string response)
        {
            if (response == null || response.Trim() == string.Empty)
                return false;

            JArray events = JsonConvert.DeserializeObject<JArray>(response);

            if (events == null)
                return false;
            else
            {
                foreach (JToken ev in events)
                {
                    string event_ = ev[0].ToString();
                    switch (event_)
                    {
                        //we need to prefix and suffix each one with a literal " character
                        case "connected":
                            IsConnected = true;
                            if (this.Connected != null)
                                this.Connected(this, new EventArgs());
                            break;
                        case "strangerDisconnected":
                            IsConnected = false;
                            if (this.StrangerDisconnected != null)
                                this.StrangerDisconnected(this, new EventArgs());
                            return true;
                        case "gotMessage":
                            if (this.MessageReceived != null && IsConnected)
                                this.MessageReceived(this, new MessageReceivedArgs(ev[1].ToString().TrimStart('"').TrimEnd('"')));
                            break;
                        case "waiting":
                            if (this.WaitingForPartner != null)
                                this.WaitingForPartner(this, new EventArgs());
                            break;
                        case "typing":
                            if (this.StrangerTyping != null && IsConnected)
                                this.StrangerTyping(this, new EventArgs());
                            break;
                        case "stoppedTyping":
                            if (this.StrangerStoppedTyping != null && IsConnected)
                                this.StrangerStoppedTyping(this, new EventArgs());
                            break;
                        case "count":
                            if (this.Count != null)
                                this.Count(this, new EventArgs()); //I'm a cheapskate, ev[1] holds user count though.
                            break;
                        case "recaptchaRequired":
                            if (this.CaptchaRequired != null)
                                this.CaptchaRequired(this, new CaptchaRequiredArgs(ev[1].ToString()));
                            break;
                        case "recaptchaRejected":
                            if (this.CaptchaRefused != null)
                                this.CaptchaRefused(this, new EventArgs());
                            break;
                        case "error": // should probably handle this one
                        case "suggestSpyee":
                        case "spyMessage":
                        case "spyTyping":
                        case "spyStoppedTyping":
                        case "spyDisconnected":
                        case "question":
                        default:
                            if (this.UnhandledResponse != null)
                                this.UnhandledResponse(this, new UnhandledResponseEventArgs(ev.ToString()));
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>Checks the Server for a new response.</summary>
        /// <returns>True if events were parsed, otherwise false.</returns>
        private bool Listen()
        {
            PostSubmitter eventlisten = new PostSubmitter();
            eventlisten.Url = String.Format("http://{0}.omegle.com/events", Server);
            eventlisten.PostItems.Add("id", Id);
            eventlisten.Type = PostSubmitter.PostTypeEnum.Post;

            eventlisten.WebExceptionEvent += WebException;

            string response = eventlisten.Post();

            if (response != null && response != "null")
                return Parse(response);
            else
                return false;
        }

        /// <summary>Worker thread.</summary>
        /// <param name="info">Unused.</param>
        protected void WorkerProc(object info)
        {
            if (Listen() == false)
                Thread.Sleep(5000);

            if (Status == status.Started)
                ThreadPool.QueueUserWorkItem(WorkerProc);
        }
    }
}