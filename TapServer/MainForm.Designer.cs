namespace TapServer
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ServerLocalTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ServerRemoteTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ClientLocalTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ClientRemoteTextbox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.LogTextbox = new System.Windows.Forms.TextBox();
            this.StartWorker = new System.ComponentModel.BackgroundWorker();
            this.NatCheckbox = new System.Windows.Forms.CheckBox();
            this.ClientModeCheckbox = new System.Windows.Forms.CheckBox();
            this.NetshCheckbox = new System.Windows.Forms.CheckBox();
            this.LogCheckbox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.HostTextbox = new System.Windows.Forms.TextBox();
            this.StartClientWorker = new System.ComponentModel.BackgroundWorker();
            this.StartServerWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local IP (Server):";
            // 
            // ServerLocalTextbox
            // 
            this.ServerLocalTextbox.Location = new System.Drawing.Point(108, 10);
            this.ServerLocalTextbox.Name = "ServerLocalTextbox";
            this.ServerLocalTextbox.Size = new System.Drawing.Size(100, 20);
            this.ServerLocalTextbox.TabIndex = 1;
            this.ServerLocalTextbox.Text = "10.4.0.2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Remote IP (Server):";
            // 
            // ServerRemoteTextbox
            // 
            this.ServerRemoteTextbox.Location = new System.Drawing.Point(321, 10);
            this.ServerRemoteTextbox.Name = "ServerRemoteTextbox";
            this.ServerRemoteTextbox.Size = new System.Drawing.Size(100, 20);
            this.ServerRemoteTextbox.TabIndex = 3;
            this.ServerRemoteTextbox.Text = "10.4.0.1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(428, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Local IP (Client):";
            // 
            // ClientLocalTextbox
            // 
            this.ClientLocalTextbox.Location = new System.Drawing.Point(518, 10);
            this.ClientLocalTextbox.Name = "ClientLocalTextbox";
            this.ClientLocalTextbox.Size = new System.Drawing.Size(100, 20);
            this.ClientLocalTextbox.TabIndex = 5;
            this.ClientLocalTextbox.Text = "10.3.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(624, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Remote IP (Client):";
            // 
            // ClientRemoteTextbox
            // 
            this.ClientRemoteTextbox.Location = new System.Drawing.Point(725, 10);
            this.ClientRemoteTextbox.Name = "ClientRemoteTextbox";
            this.ClientRemoteTextbox.Size = new System.Drawing.Size(100, 20);
            this.ClientRemoteTextbox.TabIndex = 7;
            this.ClientRemoteTextbox.Text = "10.3.0.2";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(859, 35);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(155, 22);
            this.StartButton.TabIndex = 8;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // LogTextbox
            // 
            this.LogTextbox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogTextbox.Location = new System.Drawing.Point(16, 63);
            this.LogTextbox.Multiline = true;
            this.LogTextbox.Name = "LogTextbox";
            this.LogTextbox.Size = new System.Drawing.Size(998, 589);
            this.LogTextbox.TabIndex = 9;
            // 
            // StartWorker
            // 
            this.StartWorker.WorkerReportsProgress = true;
            this.StartWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.StartWorker_DoWork);
            // 
            // NatCheckbox
            // 
            this.NatCheckbox.AutoSize = true;
            this.NatCheckbox.Checked = true;
            this.NatCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NatCheckbox.Location = new System.Drawing.Point(16, 37);
            this.NatCheckbox.Name = "NatCheckbox";
            this.NatCheckbox.Size = new System.Drawing.Size(80, 17);
            this.NatCheckbox.TabIndex = 11;
            this.NatCheckbox.Text = "Magic NAT";
            this.NatCheckbox.UseVisualStyleBackColor = true;
            this.NatCheckbox.CheckedChanged += new System.EventHandler(this.NatCheckbox_CheckedChanged);
            // 
            // ClientModeCheckbox
            // 
            this.ClientModeCheckbox.AutoSize = true;
            this.ClientModeCheckbox.Location = new System.Drawing.Point(278, 37);
            this.ClientModeCheckbox.Name = "ClientModeCheckbox";
            this.ClientModeCheckbox.Size = new System.Drawing.Size(82, 17);
            this.ClientModeCheckbox.TabIndex = 12;
            this.ClientModeCheckbox.Text = "Client Mode";
            this.ClientModeCheckbox.UseVisualStyleBackColor = true;
            this.ClientModeCheckbox.CheckedChanged += new System.EventHandler(this.ClientModeCheckbox_CheckedChanged);
            // 
            // NetshCheckbox
            // 
            this.NetshCheckbox.AutoSize = true;
            this.NetshCheckbox.Checked = true;
            this.NetshCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NetshCheckbox.Location = new System.Drawing.Point(103, 37);
            this.NetshCheckbox.Name = "NetshCheckbox";
            this.NetshCheckbox.Size = new System.Drawing.Size(76, 17);
            this.NetshCheckbox.TabIndex = 13;
            this.NetshCheckbox.Text = "Use Netsh";
            this.NetshCheckbox.UseVisualStyleBackColor = true;
            this.NetshCheckbox.CheckedChanged += new System.EventHandler(this.NetshCheckbox_CheckedChanged);
            // 
            // LogCheckbox
            // 
            this.LogCheckbox.AutoSize = true;
            this.LogCheckbox.Checked = true;
            this.LogCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LogCheckbox.Location = new System.Drawing.Point(186, 37);
            this.LogCheckbox.Name = "LogCheckbox";
            this.LogCheckbox.Size = new System.Drawing.Size(86, 17);
            this.LogCheckbox.TabIndex = 14;
            this.LogCheckbox.Text = "Log Packets";
            this.LogCheckbox.UseVisualStyleBackColor = true;
            this.LogCheckbox.CheckedChanged += new System.EventHandler(this.LogCheckbox_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(832, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Connect/Bind:";
            // 
            // HostTextbox
            // 
            this.HostTextbox.Location = new System.Drawing.Point(914, 10);
            this.HostTextbox.Name = "HostTextbox";
            this.HostTextbox.Size = new System.Drawing.Size(100, 20);
            this.HostTextbox.TabIndex = 16;
            this.HostTextbox.Text = "0.0.0.0";
            // 
            // StartClientWorker
            // 
            this.StartClientWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.StartClientWorker_DoWork);
            // 
            // StartServerWorker
            // 
            this.StartServerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.StartServerWorker_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 664);
            this.Controls.Add(this.HostTextbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LogCheckbox);
            this.Controls.Add(this.NetshCheckbox);
            this.Controls.Add(this.ClientModeCheckbox);
            this.Controls.Add(this.NatCheckbox);
            this.Controls.Add(this.LogTextbox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ClientRemoteTextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ClientLocalTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ServerRemoteTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerLocalTextbox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Tap Sandbox: Katy Perry Edition";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ServerLocalTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ServerRemoteTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ClientLocalTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ClientRemoteTextbox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox LogTextbox;
        private System.ComponentModel.BackgroundWorker StartWorker;
        private System.Windows.Forms.CheckBox NatCheckbox;
        private System.Windows.Forms.CheckBox ClientModeCheckbox;
        private System.Windows.Forms.CheckBox NetshCheckbox;
        private System.Windows.Forms.CheckBox LogCheckbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox HostTextbox;
        private System.ComponentModel.BackgroundWorker StartClientWorker;
        private System.ComponentModel.BackgroundWorker StartServerWorker;
    }
}

