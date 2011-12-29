using System;
using System.Drawing;
using System.Windows.Forms;
using ChatterBotAPI;
using dotOmegle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleverOmegleGUI
{
    public partial class Main : Form
    {
        public static Omegle omegle = new Omegle();
        public bool cleverbotEnabled;
        public static ChatterBotSession cleverbot;
        public static bool cbStarts = false;
        ChatterBotFactory factory = new ChatterBotFactory();

        protected static KeyEventHandler userTextEvent;

        public Main()
        {
            InitializeComponent();
        }

        protected void SetStatus(string text)
        {
            this.Invoke(new MethodInvoker(delegate { statusStripLabel.Text = text; }));
        }

        protected void WriteText(string text)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerText = text;
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(false);
            }));
        }

        protected void WriteHtml(string text)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerHtml = text;
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(true);
            }));
        }

        protected void WriteText(string text, Color color)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerText = text;
                element.Style = String.Format("color: '{0}';", ColorTranslator.ToHtml(color));
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(false);
            }));
        }

        protected void beginConversation()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("hr");
                conversationBox.Document.Body.AppendChild(element);
                element = conversationBox.Document.CreateElement("div");
                element.InnerHtml = "You're now chatting with a random stranger. Say hi!<br/>";
                element.Style = String.Format("color: '{0}';", ColorTranslator.ToHtml(Color.DarkGreen));
                conversationBox.Document.Body.AppendChild(element);
            }));
        }


        private void Main_Load(object sender, EventArgs e)
        {
            ChatterBot bot = factory.Create(ChatterBotType.CLEVERBOT);
            cleverbot = bot.CreateSession();

            prepareNewPage();

            // Tiny little 'hack' to prevent the textbox from resizing to fit the font
            textEntryBox.AutoSize = false;

            userTextEvent = new KeyEventHandler(textEntryBox_KeyUp);

            textEntryBox.KeyUp += userTextEvent;

            #region omegle eventhandlers

            omegle.Connected += new EventHandler(omegle_Connected);
            omegle.CaptchaRefused += new EventHandler(omegle_CaptchaRefused);
            omegle.CaptchaRequired += new CaptchaRequiredEvent(omegle_CaptchaRequired);
            omegle.MessageReceived += new MessageReceivedEvent(omegle_MessageReceived);
            omegle.StrangerDisconnected += new EventHandler(omegle_StrangerDisconnected);
            omegle.StrangerTyping += new EventHandler(omegle_StrangerTyping);
            omegle.StrangerStoppedTyping += new EventHandler(omegle_StrangerStoppedTyping);
            omegle.WaitingForPartner += new EventHandler(omegle_WaitingForPartner);
            omegle.WebException += new WebExceptionEvent(omegle_WebException);

            #endregion omegle eventhandlers

            reconnectBtn.Enabled = false;
            cleverbotEnabled = true;
            cbStarts = false;
        }

        protected void prepareNewPage()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                conversationBox.Navigate("about:blank");

                conversationBox.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(delegate
                {
                    HtmlDocument doc = conversationBox.Document.OpenNew(true);

                    doc.Write("<html><head></head><body><p>Welcome to CleverOmegle.<br/>Press 'Connect' to start!</p></body></html>");

                    doc.Body.Style = "background-color: white; font-family: 'Segoe UI', Times, serif; font-size:16px;";

                    HtmlElement captchaScript = doc.CreateElement("script");

                    captchaScript.SetAttribute("type", "text/javascript");
                    captchaScript.SetAttribute("src", "http://www.google.com/recaptcha/api/js/recaptcha_ajax.js");

                    doc.GetElementsByTagName("head")[0].AppendChild(captchaScript);

                    HtmlElement script = doc.CreateElement("script");
                    script.SetAttribute("type", "text/javascript");

                    IHTMLScriptElement iscript_elem = script.DomElement as IHTMLScriptElement;

                    iscript_elem.Text = @"
                        function do_recaptcha(key, elem_id)
                        {
                            // HACK: Workaround for: http://markmail.org/message/afaioxiycjuhyfwo
                            Recaptcha._get_api_server = function(){return 'http://www.google.com/recaptcha/api';};
                            Recaptcha.create(key, elem_id,
                                {
                                    theme: 'clean',
                                    callback: Recaptcha.focus_response_field
                                }
                            );
                        }
                        function get_response() { return Recaptcha.get_response(); }
                        function get_challenge() { return Recaptcha.get_challenge(); }
                        function destroy_recaptcha() { return Recaptcha.destroy(); }";

                    doc.Body.AppendChild(script);
                });
            }));
        }

        #region omegle eventhandler methods

        private void omegle_CaptchaRequired(object sender, CaptchaRequiredArgs args)
        {

            SetStatus("Captcha required.");

            omegle.Stop();

            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement form = conversationBox.Document.CreateElement("form");
                HtmlElement recaptchaControl = conversationBox.Document.CreateElement("div");
                
                form.Id = "recaptchaForm";
                form.AppendChild(recaptchaControl);

                recaptchaControl.Id = "recaptcha_div";

                form.AttachEventHandler("onsubmit", new EventHandler(delegate
                {
                    string response = conversationBox.Document.InvokeScript("get_response").ToString();
                    string challenge = conversationBox.Document.InvokeScript("get_challenge").ToString();

                    conversationBox.Document.InvokeScript("destroy_recaptcha");
                    form.Parent.InvokeMember("removeChild", new object[] { form.DomElement });

                    omegle.SendCaptcha(challenge, response);
                    omegle.Start();

                    textEntryBox.Enabled = true;
                }));

                conversationBox.Document.Body.AppendChild(form);
                conversationBox.Document.InvokeScript("do_recaptcha", new object[] 
                {
                    args.id,
                    recaptchaControl.Id
                });

                textEntryBox.Enabled = false;
                conversationBox.Focus();

                this.Flash();
            }));
        }

        private void omegle_StrangerStoppedTyping(object sender, EventArgs e)
        {
            SetStatus("Stranger stopped typing.");
        }

        private void omegle_StrangerTyping(object sender, EventArgs e)
        {
            SetStatus("Stranger typing...");
        }

        private void omegle_CaptchaRefused(object sender, EventArgs e)
        {
            WriteText("Captcha refused.", Color.Red);
            //SetStatus("Reconnecting...");
            //omegle.Reconnect();
            omegle.Disconnect();
            this.Invoke(new MethodInvoker(delegate
            {
                connectButton.Enabled = true;
                reconnectBtn.Enabled = false;
            }));
            return;
        }

        private void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            SetStatus("Waiting for partner...");
        }

        private void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            WriteText("Stranger disconnected. Restarting main loop...");
            SetStatus("Reconnecting...");
            omegle.Reconnect();
            return;
        }

        private void omegle_MessageReceived(object sender, MessageReceivedArgs e)
        {
            SetStatus("");
            if (e.message != null && e.message.Trim().Length > 0)
            {
                WriteText("Stranger: " + e.message.Trim(), Color.SaddleBrown);
            }

            if (cleverbotEnabled)
            {
                omegle.StartTyping();
                string answer;
                while ((answer = cleverbot.Think(e.message).Trim()).Length == 0)
                {
                    WriteText("Error getting Cleverbot's response, retrying.", Color.Red);
                }
                omegle.StopTyping();
                omegle.SendMessageRaw(answer);
                WriteText("Cleverbot: " + answer, Color.Teal);
            }
        }

        private void omegle_Connected(object sender, EventArgs e)
        {
            SetStatus("Connected.");

            beginConversation();

            if (cbStarts)
            {
                omegle_MessageReceived(null, new MessageReceivedArgs(string.Empty));
            }
        }

        private void omegle_WebException(object sender, WebExceptionEventArgs e)
        {
            WriteText("Exception: " + e.ToString(), Color.Red);
            WriteText("Disconnected..");
            //omegle.Disconnect();
            this.Invoke(new MethodInvoker(delegate {
                connectButton.Enabled = true;
                reconnectBtn.Enabled = false;
            }));
        }

        #endregion omegle eventhandler methods

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            reconnectBtn.Enabled = true;
            connectButton.Enabled = false;
            SetStatus("Connecting...");
            omegle.Connect();
        }

        private void reconnectBtn_Click(object sender, EventArgs e)
        {
            omegle.Reconnect();
            SetStatus("Reconnecting...");
            return;
        }

        private void textEntryBox_KeyUp(object sender, KeyEventArgs e)
        {
            ExcecuteShortcuts(e);

            if ((e.KeyCode == Keys.Enter) && (omegle.IsConnected == true))
            {
                //omegle.StopTyping();
                omegle.SendMessage(textEntryBox.Text);
                WriteText("You: " + textEntryBox.Text, Color.Navy);
                textEntryBox.Clear();
            }
            if ((!omegle.IsConnected) && (e.KeyCode == Keys.Enter))
            {
                WriteText("Not connected to Omegle network. Click the \"Connect\" button.", Color.Red);
                textEntryBox.Clear();
            }
        }

        private void fbQuestionButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This sends the stranger a Facebook Profile URL.\nIt looks just like any other" +
                " profile URL, but it\n actually redirects the stranger to their own profile.\nYou can use this" +
                " to troll the stranger, saying things like \n\"I haxxored ur facebook, i know who you are, lololol\",\nor whatever.");
        }

        private void facebookButton_Click(object sender, EventArgs e)
        {
            if (omegle.IsConnected)
            {
                omegle.SendMessage("http://tinyurl.com/ye94pn7");
                WriteText("You: http://tinyurl.com/ye94pn7", Color.Navy);
            }
        }

        private void textEntryBox_TextChanged(object sender, EventArgs e)
        {
            //if ((omegle.IsConnected) && (textEntryBox.Text.Length > 0))
            //{
            //    omegle.StartTyping();
            //}
        }

        public void ExcecuteShortcuts(KeyEventArgs e)
        {
            //TODO: Make this work universally in the form, not just in the entry box.
            if (e.KeyCode == Keys.PageUp)
            {
                cleverbotEnabled = !cleverbotEnabled;
                cbEnabled.Checked = cleverbotEnabled;
            }
            if (e.KeyCode == Keys.PageDown)
            {
                cbStarts = !cbStarts;
                cbStartsBox.Checked = cbStarts;
            }
        }

        private void cbStartsBox_CheckedChanged(object sender, EventArgs e)
        {
            cbStarts = cbStartsBox.Checked;
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cleverbotEnabled = cbEnabled.Checked;
        }

        private void shortcutsHelpBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press PgUp to toggle CleverBot.\nPress PgDown to toggle whether CleverBot initiates conversations.");
        }


    }
}