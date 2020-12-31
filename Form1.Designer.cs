namespace NetworkPlaybackSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelPreview = new System.Windows.Forms.Panel();
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.timerStat = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxOutput = new System.Windows.Forms.CheckBox();
            this.comboBoxVF = new System.Windows.Forms.ComboBox();
            this.comboBoxRenderer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxAF = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPreview
            // 
            this.panelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPreview.BackColor = System.Drawing.Color.Green;
            this.panelPreview.Location = new System.Drawing.Point(16, 66);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(320, 180);
            this.panelPreview.TabIndex = 0;
            this.panelPreview.UseWaitCursor = true;
            this.panelPreview.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // timerDelay
            // 
            this.timerDelay.Interval = 1000;
            this.timerDelay.Tick += new System.EventHandler(this.timerDelay_Tick);
            // 
            // timerStat
            // 
            this.timerStat.Tick += new System.EventHandler(this.TimerStat_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 171;
            this.label1.Text = "Server:";
            this.label1.UseWaitCursor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox1.Size = new System.Drawing.Size(168, 20);
            this.textBox1.TabIndex = 468;
            this.textBox1.Text = "tempvideoapp.com";
            this.textBox1.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(280, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 175;
            this.label2.Text = "not Connected";
            this.label2.UseWaitCursor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 266);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 165;
            this.label5.Text = "Video format:";
            this.label5.UseWaitCursor = true;
            // 
            // checkBoxOutput
            // 
            this.checkBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOutput.AutoSize = true;
            this.checkBoxOutput.Checked = true;
            this.checkBoxOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOutput.ForeColor = System.Drawing.Color.White;
            this.checkBoxOutput.Location = new System.Drawing.Point(16, 31);
            this.checkBoxOutput.Name = "checkBoxOutput";
            this.checkBoxOutput.Size = new System.Drawing.Size(83, 17);
            this.checkBoxOutput.TabIndex = 169;
            this.checkBoxOutput.Text = "Output Port:";
            this.checkBoxOutput.UseVisualStyleBackColor = true;
            this.checkBoxOutput.UseWaitCursor = true;
            this.checkBoxOutput.CheckedChanged += new System.EventHandler(this.checkBoxOutput_CheckedChanged_1);
            // 
            // comboBoxVF
            // 
            this.comboBoxVF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVF.DropDownWidth = 300;
            this.comboBoxVF.FormattingEnabled = true;
            this.comboBoxVF.Location = new System.Drawing.Point(84, 263);
            this.comboBoxVF.Name = "comboBoxVF";
            this.comboBoxVF.Size = new System.Drawing.Size(252, 21);
            this.comboBoxVF.TabIndex = 167;
            this.comboBoxVF.UseWaitCursor = true;
            this.comboBoxVF.SelectedIndexChanged += new System.EventHandler(this.comboBoxVF_SelectedIndexChanged);
            // 
            // comboBoxRenderer
            // 
            this.comboBoxRenderer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRenderer.FormattingEnabled = true;
            this.comboBoxRenderer.Location = new System.Drawing.Point(105, 29);
            this.comboBoxRenderer.Name = "comboBoxRenderer";
            this.comboBoxRenderer.Size = new System.Drawing.Size(231, 21);
            this.comboBoxRenderer.TabIndex = 170;
            this.comboBoxRenderer.UseWaitCursor = true;
            this.comboBoxRenderer.SelectedIndexChanged += new System.EventHandler(this.comboBoxRenderer_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 293);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 166;
            this.label4.Text = "Audio format:";
            this.label4.UseWaitCursor = true;
            // 
            // comboBoxAF
            // 
            this.comboBoxAF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAF.DropDownWidth = 300;
            this.comboBoxAF.FormattingEnabled = true;
            this.comboBoxAF.Location = new System.Drawing.Point(84, 290);
            this.comboBoxAF.Name = "comboBoxAF";
            this.comboBoxAF.Size = new System.Drawing.Size(252, 21);
            this.comboBoxAF.TabIndex = 168;
            this.comboBoxAF.UseWaitCursor = true;
            this.comboBoxAF.SelectedIndexChanged += new System.EventHandler(this.comboBoxAF_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 396);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 92);
            this.groupBox2.TabIndex = 179;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Application Server";
            this.groupBox2.UseWaitCursor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 469;
            this.label3.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(258, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 174;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxOutput);
            this.groupBox1.Controls.Add(this.comboBoxRenderer);
            this.groupBox1.Controls.Add(this.comboBoxVF);
            this.groupBox1.Controls.Add(this.panelPreview);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxAF);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 378);
            this.groupBox1.TabIndex = 183;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output Settings";
            this.groupBox1.UseWaitCursor = true;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 5000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(384, 503);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SDI Server";
            this.UseWaitCursor = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.Timer timerDelay;
        private System.Windows.Forms.Timer timerStat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxOutput;
        private System.Windows.Forms.ComboBox comboBoxAF;
        private System.Windows.Forms.ComboBox comboBoxRenderer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxVF;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer2;
    }
}

