﻿namespace NavisTimelinerPlugin
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
            this.SkipCurrentTaskButton = new System.Windows.Forms.Button();
            this.AcceptCurrentSelectionButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CurrentAssocTaskBox = new System.Windows.Forms.TextBox();
            this.SaveAssocNowButton = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StartDataInputButton = new System.Windows.Forms.Button();
            this.ButtonAcceptCompletionProgress = new System.Windows.Forms.Button();
            this.UnitsComboBox = new System.Windows.Forms.ComboBox();
            this.CompletionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.CurrentViewTaskBox = new System.Windows.Forms.TextBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startAssocButton
            // 
            this.startAssocButton.Location = new System.Drawing.Point(12, 12);
            this.startAssocButton.Name = "startAssocButton";
            this.startAssocButton.Size = new System.Drawing.Size(171, 23);
            this.startAssocButton.TabIndex = 0;
            this.startAssocButton.Text = "Переназначить";
            this.startAssocButton.UseVisualStyleBackColor = true;
            this.startAssocButton.Click += new System.EventHandler(this.startAssocButton_Click);
            // 
            // SkipCurrentTaskButton
            // 
            this.SkipCurrentTaskButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.SkipCurrentTaskButton.Location = new System.Drawing.Point(231, 45);
            this.SkipCurrentTaskButton.Name = "SkipCurrentTaskButton";
            this.SkipCurrentTaskButton.Size = new System.Drawing.Size(120, 23);
            this.SkipCurrentTaskButton.TabIndex = 3;
            this.SkipCurrentTaskButton.Text = "Пропустить";
            this.SkipCurrentTaskButton.UseVisualStyleBackColor = false;
            this.SkipCurrentTaskButton.Click += new System.EventHandler(this.SkipCurrentTaskButton_Click);
            // 
            // AcceptCurrentSelectionButton
            // 
            this.AcceptCurrentSelectionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.AcceptCurrentSelectionButton.Location = new System.Drawing.Point(6, 45);
            this.AcceptCurrentSelectionButton.Name = "AcceptCurrentSelectionButton";
            this.AcceptCurrentSelectionButton.Size = new System.Drawing.Size(219, 23);
            this.AcceptCurrentSelectionButton.TabIndex = 2;
            this.AcceptCurrentSelectionButton.Text = "Присоединить текущее выделение";
            this.AcceptCurrentSelectionButton.UseVisualStyleBackColor = false;
            this.AcceptCurrentSelectionButton.Click += new System.EventHandler(this.AcceptCurrentSelectionButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CurrentAssocTaskBox);
            this.groupBox1.Controls.Add(this.SkipCurrentTaskButton);
            this.groupBox1.Controls.Add(this.AcceptCurrentSelectionButton);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(189, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 76);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Назначение заданиям выборок:";
            // 
            // CurrentAssocTaskBox
            // 
            this.CurrentAssocTaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentAssocTaskBox.Location = new System.Drawing.Point(6, 19);
            this.CurrentAssocTaskBox.Name = "CurrentAssocTaskBox";
            this.CurrentAssocTaskBox.ReadOnly = true;
            this.CurrentAssocTaskBox.Size = new System.Drawing.Size(345, 20);
            this.CurrentAssocTaskBox.TabIndex = 4;
            this.CurrentAssocTaskBox.TabStop = false;
            // 
            // SaveAssocNowButton
            // 
            this.SaveAssocNowButton.Location = new System.Drawing.Point(99, 41);
            this.SaveAssocNowButton.Name = "SaveAssocNowButton";
            this.SaveAssocNowButton.Size = new System.Drawing.Size(84, 23);
            this.SaveAssocNowButton.TabIndex = 4;
            this.SaveAssocNowButton.Text = "Сохранить";
            this.SaveAssocNowButton.UseVisualStyleBackColor = true;
            this.SaveAssocNowButton.Click += new System.EventHandler(this.SaveAssocNowButton_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 41);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(84, 23);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.TabStop = false;
            this.buttonLoad.Text = "Загрузить";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonAcceptCompletionProgress);
            this.groupBox2.Controls.Add(this.UnitsComboBox);
            this.groupBox2.Controls.Add(this.CompletionTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonNext);
            this.groupBox2.Controls.Add(this.CurrentViewTaskBox);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(189, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 121);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Назначение %% выполнения";
            this.groupBox2.EnabledChanged += new System.EventHandler(this.groupBox2_EnabledChanged);
            // 
            // StartDataInputButton
            // 
            this.StartDataInputButton.Location = new System.Drawing.Point(12, 94);
            this.StartDataInputButton.Name = "StartDataInputButton";
            this.StartDataInputButton.Size = new System.Drawing.Size(171, 23);
            this.StartDataInputButton.TabIndex = 5;
            this.StartDataInputButton.Text = "Начать!";
            this.StartDataInputButton.UseVisualStyleBackColor = true;
            this.StartDataInputButton.Click += new System.EventHandler(this.StartDataInputButton_Click);
            // 
            // ButtonAcceptCompletionProgress
            // 
            this.ButtonAcceptCompletionProgress.Location = new System.Drawing.Point(231, 60);
            this.ButtonAcceptCompletionProgress.Name = "ButtonAcceptCompletionProgress";
            this.ButtonAcceptCompletionProgress.Size = new System.Drawing.Size(120, 23);
            this.ButtonAcceptCompletionProgress.TabIndex = 6;
            this.ButtonAcceptCompletionProgress.Text = "Назначить";
            this.ButtonAcceptCompletionProgress.UseVisualStyleBackColor = true;
            this.ButtonAcceptCompletionProgress.Click += new System.EventHandler(this.ButtonAcceptCompletionProgress_Click);
            // 
            // UnitsComboBox
            // 
            this.UnitsComboBox.FormattingEnabled = true;
            this.UnitsComboBox.Items.AddRange(new object[] {
            "КГ",
            "кгс",
            "М",
            "мм",
            "шт."});
            this.UnitsComboBox.Location = new System.Drawing.Point(136, 62);
            this.UnitsComboBox.Name = "UnitsComboBox";
            this.UnitsComboBox.Size = new System.Drawing.Size(89, 21);
            this.UnitsComboBox.Sorted = true;
            this.UnitsComboBox.TabIndex = 11;
            // 
            // CompletionTextBox
            // 
            this.CompletionTextBox.Location = new System.Drawing.Point(6, 62);
            this.CompletionTextBox.Name = "CompletionTextBox";
            this.CompletionTextBox.Size = new System.Drawing.Size(124, 20);
            this.CompletionTextBox.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Прогресс выполнения:";
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(231, 89);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(120, 23);
            this.buttonNext.TabIndex = 7;
            this.buttonNext.Text = "Пропустить";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // CurrentViewTaskBox
            // 
            this.CurrentViewTaskBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CurrentViewTaskBox.Location = new System.Drawing.Point(6, 23);
            this.CurrentViewTaskBox.Name = "CurrentViewTaskBox";
            this.CurrentViewTaskBox.ReadOnly = true;
            this.CurrentViewTaskBox.Size = new System.Drawing.Size(345, 20);
            this.CurrentViewTaskBox.TabIndex = 7;
            this.CurrentViewTaskBox.TabStop = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(471, 224);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 223);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "MSProject";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UIform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 258);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StartDataInputButton);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.SaveAssocNowButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.startAssocButton);
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
        private System.Windows.Forms.Button AcceptCurrentSelectionButton;
        private System.Windows.Forms.Button SkipCurrentTaskButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox CurrentAssocTaskBox;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button StartDataInputButton;
        private System.Windows.Forms.TextBox CurrentViewTaskBox;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button SaveAssocNowButton;
        private System.Windows.Forms.ComboBox UnitsComboBox;
        private System.Windows.Forms.TextBox CompletionTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonAcceptCompletionProgress;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button button1;
    }
}