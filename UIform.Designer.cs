namespace NavisTimelinerPlugin
{
    partial class UIform
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
            this.startAssocButton = new System.Windows.Forms.Button();
            this.bSkipCurrentTask = new System.Windows.Forms.Button();
            this.bAcceptCurrent = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentTaskBox = new System.Windows.Forms.TextBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StartDataInputButton = new System.Windows.Forms.Button();
            this.CurrentViewTaskBox = new System.Windows.Forms.TextBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startAssocButton
            // 
            this.startAssocButton.Location = new System.Drawing.Point(6, 48);
            this.startAssocButton.Name = "startAssocButton";
            this.startAssocButton.Size = new System.Drawing.Size(219, 23);
            this.startAssocButton.TabIndex = 0;
            this.startAssocButton.Text = "Начать назначение в ручном режиме";
            this.startAssocButton.UseVisualStyleBackColor = true;
            this.startAssocButton.Click += new System.EventHandler(this.startAssocButton_Click);
            // 
            // bSkipCurrentTask
            // 
            this.bSkipCurrentTask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bSkipCurrentTask.Enabled = false;
            this.bSkipCurrentTask.Location = new System.Drawing.Point(136, 120);
            this.bSkipCurrentTask.Name = "bSkipCurrentTask";
            this.bSkipCurrentTask.Size = new System.Drawing.Size(89, 40);
            this.bSkipCurrentTask.TabIndex = 3;
            this.bSkipCurrentTask.Text = "Пропустить";
            this.bSkipCurrentTask.UseVisualStyleBackColor = false;
            this.bSkipCurrentTask.Click += new System.EventHandler(this.bSkipCurrentTask_Click);
            // 
            // bAcceptCurrent
            // 
            this.bAcceptCurrent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.bAcceptCurrent.Enabled = false;
            this.bAcceptCurrent.Location = new System.Drawing.Point(6, 120);
            this.bAcceptCurrent.Name = "bAcceptCurrent";
            this.bAcceptCurrent.Size = new System.Drawing.Size(124, 40);
            this.bAcceptCurrent.TabIndex = 2;
            this.bAcceptCurrent.Text = "Присоединить текущее выделение";
            this.bAcceptCurrent.UseVisualStyleBackColor = false;
            this.bAcceptCurrent.Click += new System.EventHandler(this.bAcceptCurrent_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CurrentTaskBox);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Controls.Add(this.bSkipCurrentTask);
            this.groupBox1.Controls.Add(this.startAssocButton);
            this.groupBox1.Controls.Add(this.bAcceptCurrent);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 173);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Назначение заданиям выборок:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Текущее задание:";
            // 
            // CurrentTaskBox
            // 
            this.CurrentTaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentTaskBox.Location = new System.Drawing.Point(6, 94);
            this.CurrentTaskBox.Name = "CurrentTaskBox";
            this.CurrentTaskBox.ReadOnly = true;
            this.CurrentTaskBox.Size = new System.Drawing.Size(219, 20);
            this.CurrentTaskBox.TabIndex = 4;
            this.CurrentTaskBox.TabStop = false;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(6, 19);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(219, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Загрузить данные о связях";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonNext);
            this.groupBox2.Controls.Add(this.CurrentViewTaskBox);
            this.groupBox2.Controls.Add(this.StartDataInputButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 200);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Назначение %% выполнения";
            // 
            // StartDataInputButton
            // 
            this.StartDataInputButton.Location = new System.Drawing.Point(6, 19);
            this.StartDataInputButton.Name = "StartDataInputButton";
            this.StartDataInputButton.Size = new System.Drawing.Size(219, 23);
            this.StartDataInputButton.TabIndex = 0;
            this.StartDataInputButton.Text = "Начать!";
            this.StartDataInputButton.UseVisualStyleBackColor = true;
            this.StartDataInputButton.Click += new System.EventHandler(this.StartDataInputButton_Click);
            // 
            // CurrentViewTaskBox
            // 
            this.CurrentViewTaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentViewTaskBox.Location = new System.Drawing.Point(6, 48);
            this.CurrentViewTaskBox.Name = "CurrentViewTaskBox";
            this.CurrentViewTaskBox.ReadOnly = true;
            this.CurrentViewTaskBox.Size = new System.Drawing.Size(219, 20);
            this.CurrentViewTaskBox.TabIndex = 7;
            this.CurrentViewTaskBox.TabStop = false;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(6, 74);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 8;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // UIform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 597);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UIform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TimelinerPlugin";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startAssocButton;
        private System.Windows.Forms.Button bAcceptCurrent;
        private System.Windows.Forms.Button bSkipCurrentTask;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox CurrentTaskBox;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button StartDataInputButton;
        private System.Windows.Forms.TextBox CurrentViewTaskBox;
        private System.Windows.Forms.Button buttonNext;
    }
}