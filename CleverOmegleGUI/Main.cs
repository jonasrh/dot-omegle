﻿using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using ChatterBotAPI;
using CleverOmegleGUI.ChatterBotEx;
using dotOmegle;

namespace CleverOmegleGUI
{
    /// <summary>Main form.</summary>
    public partial class Main : Form
    {
        Omegle omegle = new Omegle();
        ChatterBotFactory factory = new ChatterBotFactory();

        ChatterBot bot;
        ChatterBotSession session;

        DateTime conversationStartTime; // Holds the start time for the current conversation.
        HtmlElement currentConversation;

        /// <summary>Initializes a new instance of the <see cref="Main"/> class.</summary>
        public Main()
        {
            InitializeComponent();
            statusStripLabel.Text = null;
            statusStripTime.Text = null;
        }

        /// <summary>
        /// Handles the Load event of the Main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Main_Load(object sender, EventArgs e)
        {
            bot = factory.Create(ChatterBotType.CLEVERBOT);

            prepareNewPage();

            // Tiny little 'hack' to prevent the textbox from resizing to fit the font
            textEntryBox.AutoSize = false;

            omegle.Throws = false;

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

            reconnectToolStripButton.Enabled = false;
        }

        /// <summary>Override for global keyboard shortcuts.</summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, that represents the Win32 message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values that represents the key to process.</param>
        /// <returns>
        /// true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool control = (keyData & Keys.Control) != 0;
            bool alt = (keyData & Keys.Alt) != 0;
            bool shift = (keyData & Keys.Shift) != 0;
            Keys key = (Keys)msg.WParam.ToInt32();

            if (control) switch (key)
                {
                    case Keys.S: saveToolStripButton.PerformClick(); return true;
                    case Keys.R: reconnectToolStripButton.PerformClick(); return true;
                    case Keys.P: printToolStripButton.PerformClick(); return true;
                    case Keys.O: connectToolStripButton.PerformClick(); return true;
                }
            else switch (key)
                {
                    case Keys.PageUp: botEnabledToolStripMenuItem.PerformClick(); return true;
                    case Keys.PageDown: botStartsConversationToolStripMenuItem.PerformClick(); return true;
                }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>Sets the status.</summary>
        /// <param name="text">The text.</param>
        /// <param name="showAnimation">if set to <c>true</c>, shows a loading animation.</param>
        protected void SetStatus(string text, bool? showAnimation = null)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                statusStripLabel.Text = text;
                if (showAnimation != null)
                    statusStripThinking.Visible = (bool)showAnimation;
            }));
        }

        /// <summary>
        /// Writes a line of text.
        /// </summary>
        /// <param name="text">The text.</param>
        protected void WriteText(string text)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerText = text;
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(true);
            }));
        }

        /// <summary>
        /// Writes HTML.
        /// </summary>
        /// <param name="html">The html.</param>
        protected void WriteHtml(string html)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerHtml = html;
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(true);
            }));
        }

        /// <summary>
        /// Writes a line of text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        protected void WriteText(string text, Color color)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("div");
                element.InnerText = text;
                element.Style = String.Format("color: '{0}';", ColorTranslator.ToHtml(color));
                conversationBox.Document.Body.AppendChild(element);
                element.ScrollIntoView(true);
            }));
        }

        /// <summary>Writes a new message.</summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="color">The color.</param>
        protected void WriteMessage(string source, string message, Color color)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement element = conversationBox.Document.CreateElement("span");
                HtmlElement el_time = conversationBox.Document.CreateElement("span");
                HtmlElement el_source = conversationBox.Document.CreateElement("span");
                HtmlElement el_message = conversationBox.Document.CreateElement("span");

                string now = DateTime.Now.ToShortTimeString();

                el_time.InnerText = "[" + now + "]";
                el_time.SetAttribute("class", "time");
                el_time.Style = "color: Gray; font-size: 0.8em;";

                el_source.InnerText = string.Format(" {0}: ", source);
                el_source.SetAttribute("class", "source");

                el_message.InnerText = message;
                el_message.SetAttribute("class", "message");

                element.Style = String.Format("color: '{0}';", ColorTranslator.ToHtml(color));

                element.AppendChild(el_time);
                element.AppendChild(el_source);
                element.AppendChild(el_message);
                element.AppendChild(conversationBox.Document.CreateElement("br"));

                currentConversation.AppendChild(element);

                element.ScrollIntoView(true);
            }));
        }

        /// <summary>
        /// Begins a conversation.
        /// </summary>
        protected void beginConversation()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                reconnectToolStripButton.Enabled = true;

                currentConversation = conversationBox.Document.CreateElement("div");
                currentConversation.InnerHtml = "<hr><span style='color: DarkGreen;'>You're now chatting with a random stranger. Say hi!<br/><br/></span>";

                conversationBox.Document.Body.AppendChild(currentConversation);

                currentConversation.ScrollIntoView(true);

                conversationStartTime = DateTime.Now;
                conversationTimer.Start();
            }));
        }

        /// <summary>
        /// Prepares a new page.
        /// </summary>
        protected void prepareNewPage()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                conversationBox.Navigate("about:blank");

                conversationBox.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(delegate
                {
                    HtmlDocument doc = conversationBox.Document.OpenNew(true);

                    doc.Write("<html><head></head><body><p>Welcome to CleverOmegle.<br/>\nPress 'Connect' to start!</p></body></html>");

                    doc.Body.Style = "background-color: white; font-family: 'Segoe UI', Times, serif; font-size:90%;";

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

        /// <summary>
        /// Handles the CaptchaRequired event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_CaptchaRequired(object sender, CaptchaRequiredArgs args)
        {
            SetStatus("Captcha required.", false);

            omegle.Stop();

            this.Invoke(new MethodInvoker(delegate
            {
                HtmlElement form = conversationBox.Document.CreateElement("form");
                HtmlElement recaptchaControl = conversationBox.Document.CreateElement("div");

                form.Id = "recaptcha_form";
                form.AppendChild(recaptchaControl);

                recaptchaControl.Id = "recaptcha_div";

                EventHandler onSubmit = null;

                form.AttachEventHandler("onsubmit", onSubmit = new EventHandler(delegate
                {
                    form.DetachEventHandler("onsubmit", onSubmit);

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
                    args.id, recaptchaControl.Id
                });

                textEntryBox.Enabled = false;
                conversationBox.Focus();

                this.Flash();
            }));
        }

        /// <summary>
        /// Handles the StrangerStoppedTyping event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_StrangerStoppedTyping(object sender, EventArgs e)
        {
            SetStatus("Stranger stopped typing.", false);
        }

        /// <summary>
        /// Handles the StrangerTyping event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_StrangerTyping(object sender, EventArgs e)
        {
            SetStatus("Stranger typing...", false);
        }

        /// <summary>
        /// Handles the CaptchaRefused event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_CaptchaRefused(object sender, EventArgs e)
        {
            //WriteText("Captcha refused.", Color.Red);
            SetStatus("Captcha refused...", false);
            //omegle.Reconnect();
            omegle.Disconnect();
            omegle.Connect();
            //this.Invoke(new MethodInvoker(delegate
            //{
            //    connectToolStripButton.Checked = false;
            //    reconnectToolStripButton.Enabled = true;
            //}));
            return;
        }

        /// <summary>
        /// Handles the WaitingForPartner event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            SetStatus("Waiting for partner...", true);
        }

        /// <summary>
        /// Handles the StrangerDisconnected event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            conversationTimer.Stop();

            WriteText("Stranger disconnected. Starting a new conversation.");
            SetStatus("Reconnecting...", true);

            omegle.Reconnect();
            return;
        }

        /// <summary>
        /// Handles the MessageReceived event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_MessageReceived(object sender, MessageReceivedArgs e)
        {
            SetStatus("");
            if (e.message != null && e.message.Trim().Length > 0)
            {
                WriteMessage("Stranger", e.message.Trim(), Color.SaddleBrown);
            }

            if (botEnabledToolStripMenuItem.Checked)
            {
                string botName = currentbotToolStripButton.Text;
                string answer = String.Empty; int retries = 4;

                SetStatus(botName + " is thinking...", true);

                DateTime thinkingStart = DateTime.Now;

                try
                {
                    while ((answer = session.Think(e.message).Trim()).Length == 0 && retries-- > 0)
                    {
                        WriteText("Error getting " + botName + "'s response, retrying.", Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    WriteText("Exception while getting " + botName + "'s response: " + ex.ToString(), Color.Red);
                }

                TimeSpan thinkingTime = DateTime.Now.Subtract(thinkingStart);

                if (answer.Length > 0)
                {
                    answer = HttpUtility.HtmlDecode(answer).Replace("  ", " ");

                    SetStatus(botName + " is typing...", false);

                    omegle.StartTyping();

                    //  Good Typist (90 wpm) --- 0.12 seconds
                    double durationMs = TimeSpan.FromSeconds(answer.Length * 0.12)
                        .Subtract(thinkingTime).TotalMilliseconds;

                    if (durationMs > 0)
                        System.Threading.Thread.Sleep((int)durationMs);

                    omegle.StopTyping();

                    SetStatus(botName + " stopped typing...");
                    if (botEnabledToolStripMenuItem.Checked) // Double check
                    {
                        omegle.SendMessageRaw(answer);
                        WriteMessage(botName, answer, Color.Teal);
                    }

                    SetStatus("");
                }
                else
                    SetStatus("Could not get " + botName + "'s response.", false);
            }
        }

        /// <summary>
        /// Handles the Connected event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void omegle_Connected(object sender, EventArgs e)
        {
            SetStatus("Connected.", false);

            session = bot.CreateSession();

            beginConversation();

            if (botStartsConversationToolStripMenuItem.Checked)
            {
                omegle_MessageReceived(null, new MessageReceivedArgs(string.Empty));
            }
        }

        /// <summary>
        /// Handles the WebException event of the omegle class.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="dotOmegle.WebExceptionEventArgs"/> instance containing the event data.</param>
        private void omegle_WebException(object sender, WebExceptionEventArgs e)
        {
            WriteText(e.Exception.Status + ": " + e.Exception.Message, Color.Red);
        }

        #endregion omegle eventhandler methods

        /// <summary>
        /// Handles the FormClosing event of the Main control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Handles the KeyUp event of the textEntryBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void textEntryBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (omegle.IsConnected == true && !talkToBotToolStripButton.Checked)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        omegle.SendMessage(textEntryBox.Text);
                        WriteMessage("You", textEntryBox.Text, Color.Navy);
                    }));
                }
                else if (talkToBotToolStripButton.Checked)
                {
                    WriteMessage(
                        (omegle.IsConnected ? "You (To " + currentbotToolStripButton.Text + ")" : "You"),
                            textEntryBox.Text, Color.Navy);

                    if (!omegle.IsConnected)
                        Task.Factory.StartNew(() =>
                            WriteMessage(currentbotToolStripButton.Text, session.Think(textEntryBox.Text), Color.Teal)
                        );
                    else
                    {
                        //TODO:
                    }
                }
                else
                    WriteText("Not connected to Omegle network. Click the \"Connect\" button.", Color.Red);

                textEntryBox.Clear();
            }
        }

        /// <summary>
        /// Handles the Click event of the connectToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void connectToolStripButton_Click(object sender, EventArgs e)
        {
            if (connectToolStripButton.Checked)
            {
                SetStatus("Connecting...", true);
                omegle.Connect();
            }
            else
            {
                omegle.Disconnect();
                reconnectToolStripButton.Enabled = false;
                SetStatus("Disconnected.", false);
            }
        }

        /// <summary>
        /// Handles the Click event of the reconnectToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void reconnectToolStripButton_Click(object sender, EventArgs e)
        {
            omegle.Reconnect();
            SetStatus("Reconnecting...");
        }

        /// <summary>
        /// Handles the Click event of the facebookToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void facebookToolStripButton_Click(object sender, EventArgs e)
        {
            if (omegle.IsConnected)
            {
                omegle.SendMessage("http://tinyurl.com/ye94pn7");
                WriteMessage("You", "http://tinyurl.com/ye94pn7", Color.Navy);
            }
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Html page|*.html;*.htm|Text document|*.txt|All files|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    if (dialog.FilterIndex == 1)
                    {
                        string header = String.Format(
                            "<html>\n\t<head>\n\t\t<title>CleverOmegle Chat Log ({0})\n\t\t</title>\n\t</head>\n\t<body>",
                            DateTime.Now.ToLongDateString());

                        writer.WriteLine(header);

                        foreach (HtmlElement el in conversationBox.Document.Body.Children)
                        {
                            if (!el.TagName.Equals("script", StringComparison.InvariantCultureIgnoreCase) &&
                                 el.Id != "recaptcha_form")
                            {
                                writer.WriteLine("\t\t" + el.OuterHtml.Trim());
                            }
                        }
                        writer.Write("\n\t</body>\n</html>");
                    }
                    else
                        writer.Write(conversationBox.Document.Body.InnerText);

                    writer.Flush();
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the printToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            conversationBox.ShowPrintPreviewDialog();
        }

        /// <summary>
        /// Handles the Click event of the copyToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            object document = conversationBox.Document.DomDocument;
            object iselection = document.GetProperty("selection");
            object range = iselection.InvokeMethod("createRange");
            object otext = range.GetProperty("text");

            string text = (otext != null ? otext as string : string.Empty);

            if (text == string.Empty)
            {
                foreach (HtmlElement el in conversationBox.Document.Body.Children)
                    text += el.InnerText + "\n";
            }

            Clipboard.SetText(text);
        }

        /// <summary>
        /// Handles the Tick event of the conversationTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void conversationTimer_Tick(object sender, EventArgs e)
        {
            if (omegle.IsConnected)
            {
                TimeSpan duration = DateTime.Now.Subtract(conversationStartTime);
                statusStripTime.Text =
                    duration.ToString("g").Split('.')[0].TrimStart(new char[] { '0', ':' });
            }
            else
            {
                statusStripTime.Text = null;
                conversationTimer.Stop();
            }
        }

        /// <summary>
        /// Handles the Click event of the botSelCleverbotToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void botSelCleverbotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot = factory.Create(ChatterBotType.CLEVERBOT);
            session = bot.CreateSession();

            currentbotToolStripButton.Image = botSelCleverbotToolStripMenuItem.Image;
            currentbotToolStripButton.Text = botSelCleverbotToolStripMenuItem.Text;

            botStartsConversationToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Handles the Click event of the botSelJabberwackyToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void botSelJabberwackyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bot = factory.Create(ChatterBotType.JABBERWACKY);
            session = bot.CreateSession();

            currentbotToolStripButton.Image = botSelJabberwackyToolStripMenuItem.Image;
            currentbotToolStripButton.Text = botSelJabberwackyToolStripMenuItem.Text;

            botStartsConversationToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Handles the Click event of the botsSeltPandoraBotToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void botsSeltPandoraBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PandoraBotSelectionForm sel_form = new PandoraBotSelectionForm())
            {
                sel_form.ShowDialog(this);

                if (sel_form.BotId == null || sel_form.BotName == null)
                    return;

                bot = factory.Create(ChatterBotType.PANDORABOTS, sel_form.BotId);
                session = bot.CreateSession();

                currentbotToolStripButton.Image = botSelPandoraBotToolStripMenuItem.Image;
                currentbotToolStripButton.Text = sel_form.BotName;

                botStartsConversationToolStripMenuItem.Checked = false;
                botStartsConversationToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the botSelSensationBotToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void botSelSensationBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null && sender != botSelSensationBotToolStripMenuItem)
            {
                ToolStripItem control = sender as ToolStripItem;

                switch (control.Tag.ToString())
                {
                    case "general": bot = new SensationBot(SensationBot.BotType.GeneralChat); break;
                    case "romantic": bot = new SensationBot(SensationBot.BotType.Romantic, 0); break;
                    case "romantic2": bot = new SensationBot(SensationBot.BotType.Romantic, 1); break;
                    case "smacktalk": bot = new SensationBot(SensationBot.BotType.SmackTalk); break;
                    case "adult1": bot = new SensationBot(SensationBot.BotType.AdultM2F); break;
                    case "adult2": bot = new SensationBot(SensationBot.BotType.AdultF2M); break;
                    case "adult3": bot = new SensationBot(SensationBot.BotType.AdultL); break;
                    case "adult4": bot = new SensationBot(SensationBot.BotType.AdultG); break;
                    default: break;
                }

                currentbotToolStripButton.Image = botSelSensationBotToolStripMenuItem.Image;
                currentbotToolStripButton.Text = botSelSensationBotToolStripMenuItem.Text.TrimEnd('.');

                session = bot.CreateSession();
            }
        }

        /// <summary>
        /// Handles the Click event of the restartBotToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void restartBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            session = bot.CreateSession();
        }

        private void talkToBotToolStripButton_Click(object sender, EventArgs e)
        {
            session = bot.CreateSession();

            if (currentConversation == null)
                beginConversation();
        }
    }
}