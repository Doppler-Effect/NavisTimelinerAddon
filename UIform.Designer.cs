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
            this.SaveTaskButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDOWN = new System.Windows.Forms.Button();
            this.buttonUP = new System.Windows.Forms.Button();
            this.TasksView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonAcceptCompletionProgress = new System.Windows.Forms.Button();
            this.UnitsComboBox = new System.Windows.Forms.ComboBox();
            this.CompletionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartDataInputButton = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.ManualAssocButton = new System.Windows.Forms.Button();
            this.buttonMSProject = new System.Windows.Forms.Button();
            this.StartDataInputWithoutSelectedButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveTaskButton
            // 
            this.SaveTaskButton.Location = new System.Drawing.Point(12, 104);
            this.SaveTaskButton.Name = "SaveTaskButton";
            this.SaveTaskButton.Size = new System.Drawing.Size(160, 40);
            this.SaveTaskButton.TabIndex = 4;
            this.SaveTaskButton.Text = "Сохранить наборы";
            this.SaveTaskButton.UseVisualStyleBackColor = true;
            this.SaveTaskButton.Click += new System.EventHandler(this.SaveTaskButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 58);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(160, 40);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.TabStop = false;
            this.LoadButton.Text = "Загрузить прикреплённые наборы";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDOWN);
            this.groupBox2.Controls.Add(this.buttonUP);
            this.groupBox2.Controls.Add(this.TasksView);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ButtonAcceptCompletionProgress);
            this.groupBox2.Controls.Add(this.UnitsComboBox);
            this.groupBox2.Controls.Add(this.CompletionTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Enabled = false;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBox2.Location = new System.Drawing.Point(183, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 570);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Назначение %% выполнения";
            // 
            // buttonDOWN
            // 
            this.buttonDOWN.Location = new System.Drawing.Point(281, 540);
            this.buttonDOWN.Name = "buttonDOWN";
            this.buttonDOWN.Size = new System.Drawing.Size(61, 21);
            this.buttonDOWN.TabIndex = 16;
            this.buttonDOWN.Text = "ВНИЗ";
            this.buttonDOWN.UseVisualStyleBackColor = true;
            this.buttonDOWN.Click += new System.EventHandler(this.buttonDOWN_Click);
            // 
            // buttonUP
            // 
            this.buttonUP.Location = new System.Drawing.Point(281, 514);
            this.buttonUP.Name = "buttonUP";
            this.buttonUP.Size = new System.Drawing.Size(61, 20);
            this.buttonUP.TabIndex = 15;
            this.buttonUP.Text = "ВВЕРХ";
            this.buttonUP.UseVisualStyleBackColor = true;
            this.buttonUP.Click += new System.EventHandler(this.buttonUP_Click);
            // 
            // TasksView
            // 
            this.TasksView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TasksView.FullRowSelect = true;
            this.TasksView.HideSelection = false;
            this.TasksView.Location = new System.Drawing.Point(6, 19);
            this.TasksView.Name = "TasksView";
            this.TasksView.Size = new System.Drawing.Size(337, 489);
            this.TasksView.TabIndex = 14;
            this.TasksView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TasksView_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(3, 543);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Единицы измерения:";
            // 
            // ButtonAcceptCompletionProgress
            // 
            this.ButtonAcceptCompletionProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ButtonAcceptCompletionProgress.Location = new System.Drawing.Point(206, 514);
            this.ButtonAcceptCompletionProgress.Name = "ButtonAcceptCompletionProgress";
            this.ButtonAcceptCompletionProgress.Size = new System.Drawing.Size(69, 47);
            this.ButtonAcceptCompletionProgress.TabIndex = 6;
            this.ButtonAcceptCompletionProgress.Text = "Назначить";
            this.ButtonAcceptCompletionProgress.UseVisualStyleBackColor = true;
            this.ButtonAcceptCompletionProgress.Click += new System.EventHandler(this.ButtonAcceptCompletionProgress_Click);
            // 
            // UnitsComboBox
            // 
            this.UnitsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.UnitsComboBox.FormattingEnabled = true;
            this.UnitsComboBox.Items.AddRange(new object[] {
            "%",
            "м.п.",
            "м2",
            "м3",
            "тн.",
            "шт."});
            this.UnitsComboBox.Location = new System.Drawing.Point(133, 540);
            this.UnitsComboBox.Name = "UnitsComboBox";
            this.UnitsComboBox.Size = new System.Drawing.Size(67, 21);
            this.UnitsComboBox.Sorted = true;
            this.UnitsComboBox.TabIndex = 11;
            // 
            // CompletionTextBox
            // 
            this.CompletionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CompletionTextBox.Location = new System.Drawing.Point(133, 514);
            this.CompletionTextBox.Name = "CompletionTextBox";
            this.CompletionTextBox.Size = new System.Drawing.Size(67, 20);
            this.CompletionTextBox.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(3, 517);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Прогресс выполнения:";
            // 
            // StartDataInputButton
            // 
            this.StartDataInputButton.Location = new System.Drawing.Point(12, 150);
            this.StartDataInputButton.Name = "StartDataInputButton";
            this.StartDataInputButton.Size = new System.Drawing.Size(160, 40);
            this.StartDataInputButton.TabIndex = 5;
            this.StartDataInputButton.Text = "Начать";
            this.StartDataInputButton.UseVisualStyleBackColor = true;
            this.StartDataInputButton.Click += new System.EventHandler(this.StartDataInputButton_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(12, 242);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(160, 40);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // ManualAssocButton
            // 
            this.ManualAssocButton.Location = new System.Drawing.Point(12, 12);
            this.ManualAssocButton.Name = "ManualAssocButton";
            this.ManualAssocButton.Size = new System.Drawing.Size(160, 40);
            this.ManualAssocButton.TabIndex = 10;
            this.ManualAssocButton.Text = "Прикрепить наборы вручную";
            this.ManualAssocButton.UseVisualStyleBackColor = true;
            this.ManualAssocButton.Click += new System.EventHandler(this.ManualAssocButton_Click);
            // 
            // buttonMSProject
            // 
            this.buttonMSProject.Location = new System.Drawing.Point(12, 517);
            this.buttonMSProject.Name = "buttonMSProject";
            this.buttonMSProject.Size = new System.Drawing.Size(160, 40);
            this.buttonMSProject.TabIndex = 11;
            this.buttonMSProject.Text = "выгрузка в MSProject";
            this.buttonMSProject.UseVisualStyleBackColor = true;
            this.buttonMSProject.Click += new System.EventHandler(this.buttonMSProject_Click);
            // 
            // StartDataInputWithoutSelectedButton
            // 
            this.StartDataInputWithoutSelectedButton.Location = new System.Drawing.Point(12, 196);
            this.StartDataInputWithoutSelectedButton.Name = "StartDataInputWithoutSelectedButton";
            this.StartDataInputWithoutSelectedButton.Size = new System.Drawing.Size(160, 40);
            this.StartDataInputWithoutSelectedButton.TabIndex = 12;
            this.StartDataInputWithoutSelectedButton.Text = "Начать (без наборов)";
            this.StartDataInputWithoutSelectedButton.UseVisualStyleBackColor = true;
            this.StartDataInputWithoutSelectedButton.Click += new System.EventHandler(this.StartDataInputWithoutSelectedButton_Click);
            // 
            // UIform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 576);
            this.ControlBox = false;
            this.Controls.Add(this.StartDataInputWithoutSelectedButton);
            this.Controls.Add(this.buttonMSProject);
            this.Controls.Add(this.ManualAssocButton);
            this.Controls.Add(this.StartDataInputButton);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.SaveTaskButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.LoadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UIform";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TimelinerPlugin";
            this.TopMost = true;
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button StartDataInputButton;
        private System.Windows.Forms.Button SaveTaskButton;
        private System.Windows.Forms.ComboBox UnitsComboBox;
        private System.Windows.Forms.TextBox CompletionTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonAcceptCompletionProgress;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button ManualAssocButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView TasksView;
        private System.Windows.Forms.Button buttonDOWN;
        private System.Windows.Forms.Button buttonUP;
        private System.Windows.Forms.Button buttonMSProject;
        private System.Windows.Forms.Button StartDataInputWithoutSelectedButton;
    }
}