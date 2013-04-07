namespace CQT.View
{
    partial class MainMenu
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
            this.bStart = new System.Windows.Forms.Button();
            this.bJoin = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(197, 63);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(75, 23);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "New Game";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // bJoin
            // 
            this.bJoin.Location = new System.Drawing.Point(197, 92);
            this.bJoin.Name = "bJoin";
            this.bJoin.Size = new System.Drawing.Size(75, 23);
            this.bJoin.TabIndex = 1;
            this.bJoin.Text = "Join Game";
            this.bJoin.UseVisualStyleBackColor = true;
            this.bJoin.Click += new System.EventHandler(this.bJoin_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(90, 211);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 2;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tbIp
            // 
            this.tbIp.Location = new System.Drawing.Point(65, 66);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(100, 20);
            this.tbIp.TabIndex = 3;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(65, 95);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "I.P";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbIp);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bJoin);
            this.Controls.Add(this.bStart);
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Interface Close Quarter Tactics";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bJoin;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}