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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptchaForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.captchaImageBox = new System.Windows.Forms.PictureBox();
            this.answerBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.captchaImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.54626F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.45374F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(0, 112);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // captchaImageBox
            // 
            this.captchaImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.captchaImageBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("captchaImageBox.InitialImage")));
            this.captchaImageBox.Location = new System.Drawing.Point(12, 12);
            this.captchaImageBox.Name = "captchaImageBox";
            this.captchaImageBox.Size = new System.Drawing.Size(302, 59);
            this.captchaImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.captchaImageBox.TabIndex = 5;
            this.captchaImageBox.TabStop = false;
            this.captchaImageBox.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.captchaImageBox_LoadCompleted);
            this.captchaImageBox.LoadProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.captchaImageBox_LoadProgressChanged);
            // 
            // answerBox
            // 
            this.answerBox.AcceptsReturn = true;
            this.answerBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.answerBox.Location = new System.Drawing.Point(12, 80);
            this.answerBox.Name = "answerBox";
            this.answerBox.Size = new System.Drawing.Size(302, 20);
            this.answerBox.TabIndex = 2;
            this.answerBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.answerBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.answerBox_KeyUp);
            // 
            // CaptchaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 112);
            this.Controls.Add(this.answerBox);
            this.Controls.Add(this.captchaImageBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptchaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Captcha Required";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptchaForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptchaForm_Load);
            this.Shown += new System.EventHandler(this.CaptchaForm_Shown);
            this.VisibleChanged += new System.EventHandler(this.CaptchaForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.captchaImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox captchaImageBox;
        private System.Windows.Forms.TextBox answerBox;

    }
}