namespace NLogClient
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
            this.components = new System.ComponentModel.Container();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSendStatus = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.nclients = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnSetupAck = new System.Windows.Forms.Button();
            this.btnSendCaptum = new System.Windows.Forms.Button();
            this.btnSendReply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nclients)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(0, 0);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSendStatus
            // 
            this.btnSendStatus.Location = new System.Drawing.Point(102, 0);
            this.btnSendStatus.Name = "btnSendStatus";
            this.btnSendStatus.Size = new System.Drawing.Size(75, 23);
            this.btnSendStatus.TabIndex = 1;
            this.btnSendStatus.Text = "sendStatus";
            this.btnSendStatus.UseVisualStyleBackColor = true;
            this.btnSendStatus.Click += new System.EventHandler(this.btnSendStatus_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(183, 0);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 2;
            this.btnSendData.Text = "send data";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(407, 0);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(75, 23);
            this.btnclear.TabIndex = 3;
            this.btnclear.Text = "clear";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 86);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(798, 316);
            this.textBox1.TabIndex = 4;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(0, 29);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 0;
            this.btnDisconnect.Text = "disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // nclients
            // 
            this.nclients.Location = new System.Drawing.Point(488, 0);
            this.nclients.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nclients.Name = "nclients";
            this.nclients.Size = new System.Drawing.Size(70, 20);
            this.nclients.TabIndex = 6;
            this.nclients.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(582, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 7;
            // 
            // btnSetupAck
            // 
            this.btnSetupAck.Location = new System.Drawing.Point(102, 29);
            this.btnSetupAck.Name = "btnSetupAck";
            this.btnSetupAck.Size = new System.Drawing.Size(109, 23);
            this.btnSetupAck.TabIndex = 0;
            this.btnSetupAck.Text = "sendSetupAck";
            this.btnSetupAck.UseVisualStyleBackColor = true;
            this.btnSetupAck.Click += new System.EventHandler(this.btnSetupAck_Click);
            // 
            // btnSendCaptum
            // 
            this.btnSendCaptum.Location = new System.Drawing.Point(277, 0);
            this.btnSendCaptum.Name = "btnSendCaptum";
            this.btnSendCaptum.Size = new System.Drawing.Size(107, 23);
            this.btnSendCaptum.TabIndex = 2;
            this.btnSendCaptum.Text = "send captum";
            this.btnSendCaptum.UseVisualStyleBackColor = true;
            this.btnSendCaptum.Click += new System.EventHandler(this.btnSendCaptum_Click);
            // 
            // btnSendReply
            // 
            this.btnSendReply.Location = new System.Drawing.Point(277, 38);
            this.btnSendReply.Name = "btnSendReply";
            this.btnSendReply.Size = new System.Drawing.Size(107, 23);
            this.btnSendReply.TabIndex = 2;
            this.btnSendReply.Text = "send reply";
            this.btnSendReply.UseVisualStyleBackColor = true;
            this.btnSendReply.Click += new System.EventHandler(this.btnSendReply_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.nclients);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnclear);
            this.Controls.Add(this.btnSendReply);
            this.Controls.Add(this.btnSendCaptum);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.btnSendStatus);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnSetupAck);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nclients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSendStatus;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.NumericUpDown nclients;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnSetupAck;
        private System.Windows.Forms.Button btnSendCaptum;
        private System.Windows.Forms.Button btnSendReply;
    }
}

