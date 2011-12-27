using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ChatterBotAPI;
using dotOmegle;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleverOmegleGUI
{
    public partial class Form1 : Form
    {
        //My issue is with the threading. If omegle runs on the same thread as the UI, the UI freezes as soon as omegle.Start is called.
        //So I run it on another thread. The omegle thread or the methods it calls (like event handlers) cannot access
        //the UI. So it cannot update the conversation box. Curiously, the SetStatus method works (when CheckForblabla is false).
        //As a side note, I've been thinking of addding something like an activity tracker. Whenever someone launches CleverOmegle,
        //a message gets sent to a server or whatever. Just to let me know how many people are using CleverOmegle.
        // - Naarkie
        public static Omegle omegle = new Omegle();
        public bool connected;
        public bool cleverbotEnabled;
        public static ChatterBotSession cleverbot;
        public static bool captchaMode = false;
        public static bool cbStarts = false;
        public string challenge;
        public static string captchaResponse = null;
        public static Thread omegleThread = new Thread(new ThreadStart(omegle.Start));
        ChatterBotFactory factory = new ChatterBotFactory();

        public Form1()
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            //This is the only way I know how to get rid of that CrossThreadCall error. Like I said, I suck at threading.
            InitializeComponent();
        }

        public void SetStatus(string text)
        {
            statusStripLabel.Text = text;
        }

        public void Write(string text)
        {
            conversationBox.AppendText(text + "\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            omegle.omegleMode = true;
            omegle.continueRestarts = true;

            ChatterBot bot = factory.Create(ChatterBotType.CLEVERBOT);
            cleverbot = bot.CreateSession();

            #region omegle eventhandlers

            omegle.Connected += new EventHandler(omegle_Connected);
            omegle.CaptchaRefused += new EventHandler(omegle_CaptchaRefused);
            omegle.CaptchaRequired += new CaptchaRequiredEvent(omegle_CaptchaRequired);
            omegle.MessageReceived += new MessageReceivedEvent(omegle_MessageReceived);
            omegle.StrangerDisconnected += new EventHandler(omegle_StrangerDisconnected);
            omegle.StrangerTyping += new EventHandler(omegle_StrangerTyping);
            omegle.StrangerStoppedTyping += new EventHandler(omegle_StrangerStoppedTyping);
            omegle.WaitingForPartner += new EventHandler(omegle_WaitingForPartner);

            #endregion omegle eventhandlers

            reconnectBtn.Enabled = false;
            cleverbotEnabled = true;
            cbStarts = false;
            connected = false;
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
            Write("Captcha refused. Reconnecting...");
            omegle.MainLoop();
            SetStatus("Reconnecting...");
            connected = false;
            return;
        }

        #region omegle eventhandler methods

        private void omegle_WaitingForPartner(object sender, EventArgs e)
        {
            SetStatus("Waiting for partner...");
        }

        private void omegle_StrangerDisconnected(object sender, EventArgs e)
        {
            Write("Stranger disconnected. Restarting main loop...");
            omegle.MainLoop();
            SetStatus("Reconnecting...");
            connected = false;
            return;
        }

        private void omegle_MessageReceived(object sender, MessageReceivedArgs e)
        {
            SetStatus("");
            if (e.message != null)
            {
                Write("Stranger: " + e.message);
            }
            if (cleverbotEnabled)
            {
                omegle.StartTyping();
                string answer = cleverbot.Think(e.message);
                omegle.StopTyping();
                omegle.SendMessageRaw(answer);
                Write("Cleverbot: " + answer);
            }
        }

        private void omegle_CaptchaRequired(object sender, CaptchaRequiredArgs e)
        {
            SetStatus("Captcha required.");
            PostSubmitter post = new PostSubmitter();
            post.Url = "http://www.google.com/recaptcha/api/challenge";
            post.PostItems.Add("k", e.id);
            post.PostItems.Add("ajax", "1");
            post.Type = PostSubmitter.PostTypeEnum.Get;

            string response = "{" + post.Post().Split(new char[] { '{', '}' })[1] + "}";
            challenge = JsonConvert.DeserializeObject<JObject>(response)["challenge"].ToString();
            System.Diagnostics.Process.Start("http://www.google.com/recaptcha/api/image?c=" + challenge);

            captchaMode = true;
        }

        private void omegle_Connected(object sender, EventArgs e)
        {
            SetStatus("Connected.");
            connected = true;
            if (cbStarts)
            {
                omegle_MessageReceived(null, new MessageReceivedArgs(string.Empty));
            }
        }

        #endregion omegle eventhandler methods

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //omegleThread.Abort();
            Environment.Exit(0);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            reconnectBtn.Enabled = true;
            connectButton.Enabled = false;
            omegleThread.Start();
            SetStatus("Connecting...");
        }

        private void reconnectBtn_Click(object sender, EventArgs e)
        {
            omegle.MainLoop();
            SetStatus("Reconnecting...");
            connected = false;
            return;
        }

        private void textEntryBox_KeyUp(object sender, KeyEventArgs e)
        {
            ExcecuteShortcuts(e);
            if (!captchaMode)
            {
                if ((e.KeyCode == Keys.Enter) && (connected == true))
                {
                    //omegle.StopTyping();
                    omegle.SendMessage(textEntryBox.Text);
                    Write("You: " + textEntryBox.Text);
                    textEntryBox.Clear();
                }
                if ((!connected) && (e.KeyCode == Keys.Enter))
                {
                    Write("Not connected to Omegle network. Click the \"Connect\" button.");
                    textEntryBox.Clear();
                }
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    captchaResponse = textEntryBox.Text;
                    omegle.SendCaptcha(challenge, captchaResponse);
                    Write("Sent catpcha: " + textEntryBox.Text);
                    textEntryBox.Clear();
                    captchaMode = false;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cleverbotEnabled = checkBox1.Checked;
        }

        private void fbQuestionButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This sends the stranger a Facebook Profile URL.\nIt looks just like any other" +
                " profile URL, but it\n actually redirects the stranger to their own profile.\nYou can use this" +
                " to troll the stranger, saying things like \n\"I haxxored ur facebook, i know who you are, lololol\",\nor whatever.");
        }

        private void facebookButton_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                omegle.SendMessage("http://tinyurl.com/ye94pn7");
                Write("You: http://tinyurl.com/ye94pn7");
            }
        }

        private void textEntryBox_TextChanged(object sender, EventArgs e)
        {
            /*if ((connected) && (textEntryBox.Text != ""))
            {
                omegle.StartTyping();
            }*/
            //Makes the entry bar "laggy". Can't be arsed to find a proper solution tbh.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press PgUp to toggle CleverBot.\nPress PgDown to toggle whether CleverBot initiates conversations.");
        }

        public void ExcecuteShortcuts(KeyEventArgs e)
        {
            //TODO: Make this work universally in the form, not just in the entry box.
            if (e.KeyCode == Keys.PageUp)
            {
                cleverbotEnabled = !cleverbotEnabled;
                checkBox1.Checked = cleverbotEnabled;
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
    }
}