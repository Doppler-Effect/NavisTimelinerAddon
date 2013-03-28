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
            this.groupBoxElem = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CompletionTextBox = new System.Windows.Forms.TextBox();
            this.UnitsComboBox = new System.Windows.Forms.ComboBox();
            this.maxCompletionTextBox = new System.Windows.Forms.TextBox();
            this.ButtonAcceptCompletionProgress = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonElem = new System.Windows.Forms.RadioButton();
            this.buttonDOWN = new System.Windows.Forms.Button();
            this.buttonUP = new System.Windows.Forms.Button();
            this.TasksView = new System.Windows.Forms.TreeView();
            this.StartDataInputButton = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.ManualAssocButton = new System.Windows.Forms.Button();
            this.buttonMSProject = new System.Windows.Forms.Button();
            this.StartDataInputWithoutSelectedButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBoxElem.SuspendLayout();
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
            this.groupBox2.Controls.Add(this.groupBoxElem);
            this.groupBox2.Controls.Add(this.radioButtonAll);
            this.groupBox2.Controls.Add(this.radioButtonElem);
            this.groupBox2.Controls.Add(this.buttonDOWN);
            this.groupBox2.Controls.Add(this.buttonUP);
            this.groupBox2.Controls.Add(this.TasksView);
            this.groupBox2.Enabled = false;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBox2.Location = new System.Drawing.Point(183, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 628);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Назначение %% выполнения";
            // 
            // groupBoxElem
            // 
            this.groupBoxElem.Controls.Add(this.label2);
            this.groupBoxElem.Controls.Add(this.CompletionTextBox);
            this.groupBoxElem.Controls.Add(this.UnitsComboBox);
            this.groupBoxElem.Controls.Add(this.maxCompletionTextBox);
            this.groupBoxElem.Controls.Add(this.ButtonAcceptCompletionProgress);
            this.groupBoxElem.Controls.Add(this.label1);
            this.groupBoxElem.Location = new System.Drawing.Point(6, 548);
            this.groupBoxElem.Name = "groupBoxElem";
            this.groupBoxElem.Size = new System.Drawing.Size(358, 68);
            this.groupBoxElem.TabIndex = 21;
            this.groupBoxElem.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Прогресс выполнения:";
            // 
            // CompletionTextBox
            // 
            this.CompletionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CompletionTextBox.Location = new System.Drawing.Point(133, 13);
            this.CompletionTextBox.Name = "CompletionTextBox";
            this.CompletionTextBox.Size = new System.Drawing.Size(81, 20);
            this.CompletionTextBox.TabIndex = 10;
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
            this.UnitsComboBox.Location = new System.Drawing.Point(220, 13);
            this.UnitsComboBox.Name = "UnitsComboBox";
            this.UnitsComboBox.Size = new System.Drawing.Size(52, 21);
            this.UnitsComboBox.Sorted = true;
            this.UnitsComboBox.TabIndex = 11;
            // 
            // maxCompletionTextBox
            // 
            this.maxCompletionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.maxCompletionTextBox.Location = new System.Drawing.Point(169, 40);
            this.maxCompletionTextBox.Name = "maxCompletionTextBox";
            this.maxCompletionTextBox.Size = new System.Drawing.Size(103, 20);
            this.maxCompletionTextBox.TabIndex = 18;
            // 
            // ButtonAcceptCompletionProgress
            // 
            this.ButtonAcceptCompletionProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ButtonAcceptCompletionProgress.Location = new System.Drawing.Point(278, 13);
            this.ButtonAcceptCompletionProgress.Name = "ButtonAcceptCompletionProgress";
            this.ButtonAcceptCompletionProgress.Size = new System.Drawing.Size(74, 47);
            this.ButtonAcceptCompletionProgress.TabIndex = 6;
            this.ButtonAcceptCompletionProgress.Text = "Назначить";
            this.ButtonAcceptCompletionProgress.UseVisualStyleBackColor = true;
            this.ButtonAcceptCompletionProgress.Click += new System.EventHandler(this.ButtonAcceptCompletionProgress_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Максимальный объём работ:";
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(6, 525);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(178, 17);
            this.radioButtonAll.TabIndex = 20;
            this.radioButtonAll.Text = "Назначение для всего набора";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.radioButtonAll_CheckedChanged);
            // 
            // radioButtonElem
            // 
            this.radioButtonElem.AutoSize = true;
            this.radioButtonElem.Location = new System.Drawing.Point(190, 525);
            this.radioButtonElem.Name = "radioButtonElem";
            this.radioButtonElem.Size = new System.Drawing.Size(162, 17);
            this.radioButtonElem.TabIndex = 19;
            this.radioButtonElem.Text = "Поэлементное назначение";
            this.radioButtonElem.UseVisualStyleBackColor = true;
            // 
            // buttonDOWN
            // 
            this.buttonDOWN.Location = new System.Drawing.Point(370, 587);
            this.buttonDOWN.Name = "buttonDOWN";
            this.buttonDOWN.Size = new System.Drawing.Size(61, 29);
            this.buttonDOWN.TabIndex = 16;
            this.buttonDOWN.Text = "ВНИЗ";
            this.buttonDOWN.UseVisualStyleBackColor = true;
            this.buttonDOWN.Click += new System.EventHandler(this.buttonDOWN_Click);
            // 
            // buttonUP
            // 
            this.buttonUP.Location = new System.Drawing.Point(370, 548);
            this.buttonUP.Name = "buttonUP";
            this.buttonUP.Size = new System.Drawing.Size(61, 29);
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
            this.TasksView.ShowNodeToolTips = true;
            this.TasksView.Size = new System.Drawing.Size(425, 500);
            this.TasksView.TabIndex = 14;
            this.TasksView.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TasksView_NodeMouseHover);
            this.TasksView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TasksView_AfterSelect);
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
            this.buttonMSProject.Location = new System.Drawing.Point(12, 288);
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
            this.ClientSize = new System.Drawing.Size(628, 632);
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
            this.groupBoxElem.ResumeLayout(false);
            this.groupBoxElem.PerformLayout();
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
        private System.Windows.Forms.TreeView TasksView;
        private System.Windows.Forms.Button buttonDOWN;
        private System.Windows.Forms.Button buttonUP;
        private System.Windows.Forms.Button buttonMSProject;
        private System.Windows.Forms.Button StartDataInputWithoutSelectedButton;
        private System.Windows.Forms.TextBox maxCompletionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonElem;
        private System.Windows.Forms.GroupBox groupBoxElem;
    }
}