namespace ProgramPlannerClient
{
    partial class Client
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.addProgramButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtProgramPath = new System.Windows.Forms.TextBox();
            this.getProgramListButton = new System.Windows.Forms.Label();
            this.delProgramButton = new System.Windows.Forms.Label();
            this.timerCheckService = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.управлениеСлужбойToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.включитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отключитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыПодключенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.repeatModeBox = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFileButton = new System.Windows.Forms.Button();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.waitingProgramList = new System.Windows.Forms.DataGridView();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waitingProgramList)).BeginInit();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Программы ожидающие запуск";
            // 
            // addProgramButton
            // 
            this.addProgramButton.Location = new System.Drawing.Point(12, 125);
            this.addProgramButton.Name = "addProgramButton";
            this.addProgramButton.Size = new System.Drawing.Size(196, 26);
            this.addProgramButton.TabIndex = 3;
            this.addProgramButton.Text = "Добавить программу";
            this.addProgramButton.UseVisualStyleBackColor = true;
            this.addProgramButton.Click += new System.EventHandler(this.addFolderButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.closeButton.Location = new System.Drawing.Point(9, 198);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(51, 13);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Закрыть";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatus,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 220);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(649, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatus
            // 
            this.slStatus.Name = "slStatus";
            this.slStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // txtProgramPath
            // 
            this.txtProgramPath.Location = new System.Drawing.Point(12, 47);
            this.txtProgramPath.Name = "txtProgramPath";
            this.txtProgramPath.Size = new System.Drawing.Size(142, 20);
            this.txtProgramPath.TabIndex = 8;
            // 
            // getProgramListButton
            // 
            this.getProgramListButton.AutoSize = true;
            this.getProgramListButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.getProgramListButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.getProgramListButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.getProgramListButton.Location = new System.Drawing.Point(243, 198);
            this.getProgramListButton.Name = "getProgramListButton";
            this.getProgramListButton.Size = new System.Drawing.Size(147, 13);
            this.getProgramListButton.TabIndex = 10;
            this.getProgramListButton.Text = "Получить список программ";
            this.getProgramListButton.Click += new System.EventHandler(this.getProgramListButton_Click);
            // 
            // delProgramButton
            // 
            this.delProgramButton.AutoSize = true;
            this.delProgramButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.delProgramButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.delProgramButton.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.delProgramButton.Location = new System.Drawing.Point(527, 198);
            this.delProgramButton.Name = "delProgramButton";
            this.delProgramButton.Size = new System.Drawing.Size(109, 13);
            this.delProgramButton.TabIndex = 11;
            this.delProgramButton.Text = "Удалить программу";
            this.delProgramButton.Click += new System.EventHandler(this.delProgramButton_Click);
            // 
            // timerCheckService
            // 
            this.timerCheckService.Enabled = true;
            this.timerCheckService.Interval = 30000;
            this.timerCheckService.Tick += new System.EventHandler(this.timerCheckService_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.управлениеСлужбойToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // управлениеСлужбойToolStripMenuItem
            // 
            this.управлениеСлужбойToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.включитьToolStripMenuItem,
            this.отключитьToolStripMenuItem,
            this.параметрыПодключенияToolStripMenuItem});
            this.управлениеСлужбойToolStripMenuItem.Name = "управлениеСлужбойToolStripMenuItem";
            this.управлениеСлужбойToolStripMenuItem.Size = new System.Drawing.Size(137, 20);
            this.управлениеСлужбойToolStripMenuItem.Text = "Управление службой";
            // 
            // включитьToolStripMenuItem
            // 
            this.включитьToolStripMenuItem.Name = "включитьToolStripMenuItem";
            this.включитьToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.включитьToolStripMenuItem.Text = "Включить";
            this.включитьToolStripMenuItem.Click += new System.EventHandler(this.включитьToolStripMenuItem_Click);
            // 
            // отключитьToolStripMenuItem
            // 
            this.отключитьToolStripMenuItem.Name = "отключитьToolStripMenuItem";
            this.отключитьToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.отключитьToolStripMenuItem.Text = "Отключить";
            this.отключитьToolStripMenuItem.Click += new System.EventHandler(this.отключитьToolStripMenuItem_Click);
            // 
            // параметрыПодключенияToolStripMenuItem
            // 
            this.параметрыПодключенияToolStripMenuItem.Name = "параметрыПодключенияToolStripMenuItem";
            this.параметрыПодключенияToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.параметрыПодключенияToolStripMenuItem.Text = "Параметры подключения";
            this.параметрыПодключенияToolStripMenuItem.Click += new System.EventHandler(this.параметрыПодключенияToolStripMenuItem_Click);
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "";
            this.datePicker.Location = new System.Drawing.Point(12, 73);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(118, 20);
            this.datePicker.TabIndex = 13;
            // 
            // repeatModeBox
            // 
            this.repeatModeBox.DisplayMember = "0";
            this.repeatModeBox.FormattingEnabled = true;
            this.repeatModeBox.Items.AddRange(new object[] {
            "Один раз",
            "Каждую минуту",
            "Каждый час",
            "Каждый день"});
            this.repeatModeBox.Location = new System.Drawing.Point(12, 99);
            this.repeatModeBox.Name = "repeatModeBox";
            this.repeatModeBox.Size = new System.Drawing.Size(196, 21);
            this.repeatModeBox.TabIndex = 15;
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(160, 44);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(48, 23);
            this.openFileButton.TabIndex = 16;
            this.openFileButton.Text = "Файл";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // timePicker
            // 
            this.timePicker.CustomFormat = "HH:mm";
            this.timePicker.Location = new System.Drawing.Point(136, 73);
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(72, 20);
            this.timePicker.TabIndex = 17;
            // 
            // waitingProgramList
            // 
            this.waitingProgramList.AllowUserToAddRows = false;
            this.waitingProgramList.AllowUserToDeleteRows = false;
            this.waitingProgramList.AllowUserToResizeColumns = false;
            this.waitingProgramList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.waitingProgramList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.waitingProgramList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.path,
            this.startDate,
            this.repeat});
            this.waitingProgramList.Location = new System.Drawing.Point(241, 47);
            this.waitingProgramList.MultiSelect = false;
            this.waitingProgramList.Name = "waitingProgramList";
            this.waitingProgramList.ReadOnly = true;
            this.waitingProgramList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.waitingProgramList.ShowRowErrors = false;
            this.waitingProgramList.Size = new System.Drawing.Size(398, 148);
            this.waitingProgramList.TabIndex = 18;
            // 
            // path
            // 
            this.path.HeaderText = "Программа";
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.path.Width = 105;
            // 
            // startDate
            // 
            this.startDate.HeaderText = "Дата и время запуска";
            this.startDate.Name = "startDate";
            this.startDate.ReadOnly = true;
            this.startDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.startDate.Width = 145;
            // 
            // repeat
            // 
            this.repeat.HeaderText = "Периодичность";
            this.repeat.Name = "repeat";
            this.repeat.ReadOnly = true;
            this.repeat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.repeat.Width = 87;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Добавить программу";
            // 
            // ProgramPlannerClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(649, 242);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.waitingProgramList);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.repeatModeBox);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.delProgramButton);
            this.Controls.Add(this.getProgramListButton);
            this.Controls.Add(this.txtProgramPath);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addProgramButton);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(665, 280);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(665, 280);
            this.Name = "ProgramPlannerClient";
            this.Text = "Клиент управления сервером";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waitingProgramList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addProgramButton;
        private System.Windows.Forms.Label closeButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.TextBox txtProgramPath;
        private System.Windows.Forms.Label getProgramListButton;
        private System.Windows.Forms.Label delProgramButton;
        private System.Windows.Forms.Timer timerCheckService;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem управлениеСлужбойToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem включитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отключитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem параметрыПодключенияToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.ComboBox repeatModeBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.DataGridView waitingProgramList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn repeat;
    }
}

