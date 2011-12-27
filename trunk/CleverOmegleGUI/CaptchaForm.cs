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
        public CaptchaForm()
        {
            InitializeComponent();
        }

        private void answerBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            }
        }

        private void CaptchaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
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
    }
}