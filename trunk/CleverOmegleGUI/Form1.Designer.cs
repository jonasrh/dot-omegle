namespace CleverOmegleGUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.conversationBox = new System.Windows.Forms.TextBox();
            this.textEntryBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.reconnectBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.facebookButton = new System.Windows.Forms.Button();
            this.fbQuestionButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbStartsBox = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 433);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(747, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // conversationBox
            // 
            this.conversationBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.conversationBox.Location = new System.Drawing.Point(13, 15);
            this.conversationBox.Multiline = true;
            this.conversationBox.Name = "conversationBox";
            this.conversationBox.ReadOnly = true;
            this.conversationBox.Size = new System.Drawing.Size(590, 372);
            this.conversationBox.TabIndex = 1;
            // 
            // textEntryBox
            // 
            this.textEntryBox.Location = new System.Drawing.Point(14, 401);
            this.textEntryBox.Name = "textEntryBox";
            this.textEntryBox.Size = new System.Drawing.Size(588, 20);
            this.textEntryBox.TabIndex = 2;
            this.textEntryBox.TextChanged += new System.EventHandler(this.textEntryBox_TextChanged);
            this.textEntryBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEntryBox_KeyUp);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(616, 15);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(114, 28);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // reconnectBtn
            // 
            this.reconnectBtn.Location = new System.Drawing.Point(616, 58);
            this.reconnectBtn.Name = "reconnectBtn";
            this.reconnectBtn.Size = new System.Drawing.Size(114, 28);
            this.reconnectBtn.TabIndex = 4;
            this.reconnectBtn.Text = "Reconnect";
            this.reconnectBtn.UseVisualStyleBackColor = true;
            this.reconnectBtn.Click += new System.EventHandler(this.reconnectBtn_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(616, 113);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(114, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "CleverBot Enabled";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // facebookButton
            // 
            this.facebookButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facebookButton.Location = new System.Drawing.Point(616, 325);
            this.facebookButton.Name = "facebookButton";
            this.facebookButton.Size = new System.Drawing.Size(78, 28);
            this.facebookButton.TabIndex = 10;
            this.facebookButton.Text = "FB URL";
            this.facebookButton.UseVisualStyleBackColor = true;
            this.facebookButton.Click += new System.EventHandler(this.facebookButton_Click);
            // 
            // fbQuestionButton
            // 
            this.fbQuestionButton.Image = ((System.Drawing.Image)(resources.GetObject("fbQuestionButton.Image")));
            this.fbQuestionButton.Location = new System.Drawing.Point(700, 325);
            this.fbQuestionButton.Name = "fbQuestionButton";
            this.fbQuestionButton.Size = new System.Drawing.Size(30, 28);
            this.fbQuestionButton.TabIndex = 11;
            this.fbQuestionButton.UseVisualStyleBackColor = true;
            this.fbQuestionButton.Click += new System.EventHandler(this.fbQuestionButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(617, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 28);
            this.button1.TabIndex = 12;
            this.button1.Text = "Keyboard Shortcuts";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbStartsBox
            // 
            this.cbStartsBox.AutoSize = true;
            this.cbStartsBox.Location = new System.Drawing.Point(616, 138);
            this.cbStartsBox.Name = "cbStartsBox";
            this.cbStartsBox.Size = new System.Drawing.Size(137, 17);
            this.cbStartsBox.TabIndex = 13;
            this.cbStartsBox.Text = "CB starts conversations";
            this.cbStartsBox.UseVisualStyleBackColor = true;
            this.cbStartsBox.CheckedChanged += new System.EventHandler(this.cbStartsBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 455);
            this.Controls.Add(this.cbStartsBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fbQuestionButton);
            this.Controls.Add(this.facebookButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.reconnectBtn);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.textEntryBox);
            this.Controls.Add(this.conversationBox);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "CleverOmegle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;
        private System.Windows.Forms.TextBox conversationBox;
        private System.Windows.Forms.TextBox textEntryBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button reconnectBtn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button facebookButton;
        private System.Windows.Forms.Button fbQuestionButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbStartsBox;
    }
}

