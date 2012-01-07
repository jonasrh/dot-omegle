using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CleverOmegle
{
    public partial class CaptchaWindow : Form
    {
        public CaptchaWindow()
        {
            InitializeComponent();
        }

        private void CaptchaWindow_Load(object sender, EventArgs e)
        {
            captchaPictureBox.WaitOnLoad = true;
            captchaPictureBox.LoadAsync(Program.captchaURL);
        }

        private void CaptchaWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}