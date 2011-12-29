namespace CleverOmegleGUI
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textEntryBox = new System.Windows.Forms.TextBox();
            this.conversationBox = new System.Windows.Forms.WebBrowser();
            this.gbCleverbotOpts = new System.Windows.Forms.GroupBox();
            this.cbStartsBox = new System.Windows.Forms.CheckBox();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.shortcutsHelpBtn = new System.Windows.Forms.Button();
            this.fbQuestionButton = new System.Windows.Forms.Button();
            this.facebookButton = new System.Windows.Forms.Button();
            this.reconnectBtn = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.gbCleverbotOpts.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(699, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusBar";
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.gbCleverbotOpts);
            this.splitContainer.Panel2.Controls.Add(this.shortcutsHelpBtn);
            this.splitContainer.Panel2.Controls.Add(this.fbQuestionButton);
            this.splitContainer.Panel2.Controls.Add(this.facebookButton);
            this.splitContainer.Panel2.Controls.Add(this.reconnectBtn);
            this.splitContainer.Panel2.Controls.Add(this.connectButton);
            this.splitContainer.Size = new System.Drawing.Size(699, 420);
            this.splitContainer.SplitterDistance = 548;
            this.splitContainer.TabIndex = 18;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.textEntryBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.conversationBox, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(538, 410);
            this.tableLayoutPanel.TabIndex = 18;
            // 
            // textEntryBox
            // 
            this.textEntryBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textEntryBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEntryBox.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEntryBox.Location = new System.Drawing.Point(5, 383);
            this.textEntryBox.Margin = new System.Windows.Forms.Padding(4);
            this.textEntryBox.Name = "textEntryBox";
            this.textEntryBox.Size = new System.Drawing.Size(528, 20);
            this.textEntryBox.TabIndex = 18;
            this.textEntryBox.TextChanged += new System.EventHandler(this.textEntryBox_TextChanged);
            // 
            // conversationBox
            // 
            this.conversationBox.AllowNavigation = false;
            this.conversationBox.AllowWebBrowserDrop = false;
            this.conversationBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conversationBox.Location = new System.Drawing.Point(4, 4);
            this.conversationBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.conversationBox.Name = "conversationBox";
            this.conversationBox.Size = new System.Drawing.Size(530, 371);
            this.conversationBox.TabIndex = 17;
            this.conversationBox.TabStop = false;
            this.conversationBox.WebBrowserShortcutsEnabled = false;
            // 
            // gbCleverbotOpts
            // 
            this.gbCleverbotOpts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCleverbotOpts.Controls.Add(this.cbStartsBox);
            this.gbCleverbotOpts.Controls.Add(this.cbEnabled);
            this.gbCleverbotOpts.Location = new System.Drawing.Point(8, 93);
            this.gbCleverbotOpts.Name = "gbCleverbotOpts";
            this.gbCleverbotOpts.Size = new System.Drawing.Size(128, 67);
            this.gbCleverbotOpts.TabIndex = 20;
            this.gbCleverbotOpts.TabStop = false;
            this.gbCleverbotOpts.Text = "Cleverbot";
            // 
            // cbStartsBox
            // 
            this.cbStartsBox.AutoSize = true;
            this.cbStartsBox.Location = new System.Drawing.Point(10, 42);
            this.cbStartsBox.Name = "cbStartsBox";
            this.cbStartsBox.Size = new System.Drawing.Size(120, 17);
            this.cbStartsBox.TabIndex = 14;
            this.cbStartsBox.Text = "Starts conversation";
            this.cbStartsBox.UseVisualStyleBackColor = true;
            this.cbStartsBox.CheckedChanged += new System.EventHandler(this.cbStartsBox_CheckedChanged);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.Location = new System.Drawing.Point(10, 19);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(64, 17);
            this.cbEnabled.TabIndex = 10;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // shortcutsHelpBtn
            // 
            this.shortcutsHelpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shortcutsHelpBtn.Location = new System.Drawing.Point(13, 329);
            this.shortcutsHelpBtn.Name = "shortcutsHelpBtn";
            this.shortcutsHelpBtn.Size = new System.Drawing.Size(123, 38);
            this.shortcutsHelpBtn.TabIndex = 19;
            this.shortcutsHelpBtn.Text = "Keyboard Shortcuts";
            this.shortcutsHelpBtn.UseVisualStyleBackColor = true;
            this.shortcutsHelpBtn.Click += new System.EventHandler(this.shortcutsHelpBtn_Click);
            // 
            // fbQuestionButton
            // 
            this.fbQuestionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fbQuestionButton.Image = ((System.Drawing.Image)(resources.GetObject("fbQuestionButton.Image")));
            this.fbQuestionButton.Location = new System.Drawing.Point(99, 374);
            this.fbQuestionButton.Name = "fbQuestionButton";
            this.fbQuestionButton.Size = new System.Drawing.Size(35, 29);
            this.fbQuestionButton.TabIndex = 18;
            this.fbQuestionButton.UseVisualStyleBackColor = true;
            this.fbQuestionButton.Click += new System.EventHandler(this.fbQuestionButton_Click);
            // 
            // facebookButton
            // 
            this.facebookButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.facebookButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facebookButton.Location = new System.Drawing.Point(13, 373);
            this.facebookButton.Name = "facebookButton";
            this.facebookButton.Size = new System.Drawing.Size(76, 29);
            this.facebookButton.TabIndex = 17;
            this.facebookButton.Text = "FB URL";
            this.facebookButton.UseVisualStyleBackColor = true;
            this.facebookButton.Click += new System.EventHandler(this.facebookButton_Click);
            // 
            // reconnectBtn
            // 
            this.reconnectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reconnectBtn.Location = new System.Drawing.Point(13, 58);
            this.reconnectBtn.Name = "reconnectBtn";
            this.reconnectBtn.Size = new System.Drawing.Size(112, 29);
            this.reconnectBtn.TabIndex = 16;
            this.reconnectBtn.Text = "Reconnect";
            this.reconnectBtn.UseVisualStyleBackColor = true;
            this.reconnectBtn.Click += new System.EventHandler(this.reconnectBtn_Click);
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(13, 15);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(112, 29);
            this.connectButton.TabIndex = 15;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 442);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Main";
            this.Text = "CleverOmegle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.gbCleverbotOpts.ResumeLayout(false);
            this.gbCleverbotOpts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textEntryBox;
        private System.Windows.Forms.WebBrowser conversationBox;
        private System.Windows.Forms.GroupBox gbCleverbotOpts;
        private System.Windows.Forms.CheckBox cbStartsBox;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button shortcutsHelpBtn;
        private System.Windows.Forms.Button fbQuestionButton;
        private System.Windows.Forms.Button facebookButton;
        private System.Windows.Forms.Button reconnectBtn;
        private System.Windows.Forms.Button connectButton;
    }
}

