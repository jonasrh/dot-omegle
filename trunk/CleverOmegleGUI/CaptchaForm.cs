using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CleverOmegleGUI
{
    public partial class CaptchaForm : Form
    {
        public string captchaUrl { get; set; }
        public string userResponse { get; protected set; }
        public bool cancelled { get; protected set; }

        private bool closing = false;

        public CaptchaForm(string captchaUrl)
        {
            InitializeComponent();
            captchaImageBox.LoadAsync(this.captchaUrl = captchaUrl);
        }

        private void answerBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && answerBox.Text.Trim().Length > 0)
            {
                userResponse = answerBox.Text.Trim();
                closing = true;
                this.Close();
            }
        }

        private void CaptchaForm_VisibleChanged(object sender, EventArgs e)
        {
            /*if (this.Visible == true)
            {
                try
                {
                    captchaImageBox.LoadAsync(Form1.captchaURL);
                }
                catch (Exception f)
                {
                    Console.WriteLine("Exception: " + f.Message);
                }
            }*/
        }

        private void CaptchaForm_Shown(object sender, EventArgs e)
        {
        }

        private void CaptchaForm_Load(object sender, EventArgs e)
        {
        }

        private void captchaImageBox_LoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void captchaImageBox_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        private void CaptchaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !closing)
            {
                cancelled = true;
            }
        }

    }
}