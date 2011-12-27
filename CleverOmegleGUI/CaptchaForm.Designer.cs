namespace CleverOmegleGUI
{
    partial class CaptchaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.captchaImageBox = new System.Windows.Forms.PictureBox();
            this.answerBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.captchaImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // captchaImageBox
            // 
            this.captchaImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.captchaImageBox.Location = new System.Drawing.Point(39, 12);
            this.captchaImageBox.Name = "captchaImageBox";
            this.captchaImageBox.Size = new System.Drawing.Size(300, 57);
            this.captchaImageBox.TabIndex = 0;
            this.captchaImageBox.TabStop = false;
            // 
            // answerBox
            // 
            this.answerBox.Location = new System.Drawing.Point(13, 86);
            this.answerBox.Name = "answerBox";
            this.answerBox.Size = new System.Drawing.Size(356, 20);
            this.answerBox.TabIndex = 1;
            this.answerBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.answerBox_KeyUp);
            // 
            // CaptchaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 119);
            this.Controls.Add(this.answerBox);
            this.Controls.Add(this.captchaImageBox);
            this.Name = "CaptchaForm";
            this.Text = "CaptchaForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaptchaForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.CaptchaForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.captchaImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox captchaImageBox;
        private System.Windows.Forms.TextBox answerBox;
    }
}