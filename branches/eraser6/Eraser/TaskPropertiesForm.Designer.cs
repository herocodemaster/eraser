namespace Eraser
{
	partial class TaskPropertiesForm
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
			this.nameLbl = new System.Windows.Forms.Label();
			this.name = new System.Windows.Forms.TextBox();
			this.eraseLbl = new System.Windows.Forms.Label();
			this.typeLbl = new System.Windows.Forms.Label();
			this.typeOneTime = new System.Windows.Forms.RadioButton();
			this.typeRecurring = new System.Windows.Forms.RadioButton();
			this.data = new System.Windows.Forms.ListView();
			this.dataColData = new System.Windows.Forms.ColumnHeader();
			this.dataColMethod = new System.Windows.Forms.ColumnHeader();
			this.dataAdd = new System.Windows.Forms.Button();
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.container = new System.Windows.Forms.TabControl();
			this.containerTask = new System.Windows.Forms.TabPage();
			this.containerSchedule = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.schedulePattern = new System.Windows.Forms.GroupBox();
			this.scheduleWeeklyFreq = new System.Windows.Forms.NumericUpDown();
			this.scheduleDaily = new System.Windows.Forms.RadioButton();
			this.scheduleDailyPanel = new System.Windows.Forms.Panel();
			this.scheduleDailyByDayFreq = new System.Windows.Forms.NumericUpDown();
			this.scheduleDailyByDay = new System.Windows.Forms.RadioButton();
			this.scheduleDailyByDayLbl = new System.Windows.Forms.Label();
			this.scheduleDailyByWeekday = new System.Windows.Forms.RadioButton();
			this.scheduleWeeklyDays = new System.Windows.Forms.FlowLayoutPanel();
			this.scheduleWeeklyMonday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklyTuesday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklyWednesday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklyThursday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklyFriday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklySaturday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklySunday = new System.Windows.Forms.CheckBox();
			this.scheduleWeeklyFreqLbl = new System.Windows.Forms.Label();
			this.scheduleWeeklyLbl = new System.Windows.Forms.Label();
			this.scheduleWeekly = new System.Windows.Forms.RadioButton();
			this.scheduleMonthly = new System.Windows.Forms.RadioButton();
			this.scheduleMonthlyPanel = new System.Windows.Forms.Panel();
			this.scheduleMonthlyByDayNumber = new System.Windows.Forms.NumericUpDown();
			this.scheduleMonthlyByDayFreq = new System.Windows.Forms.NumericUpDown();
			this.scheduleMonthlyRelativeFreq = new System.Windows.Forms.NumericUpDown();
			this.scheduleMonthlyRelativeFreqLbl = new System.Windows.Forms.Label();
			this.scheduleMonthlyRelativeEveryLbl = new System.Windows.Forms.Label();
			this.scheduleMonthlyByDayMonthLbl = new System.Windows.Forms.Label();
			this.scheduleMonthlyRelativeWeekDay = new System.Windows.Forms.ComboBox();
			this.scheduleMonthlyRelativeWeek = new System.Windows.Forms.ComboBox();
			this.scheduleMonthlyRelative = new System.Windows.Forms.RadioButton();
			this.scheduleMonthlyByDayEveryLbl = new System.Windows.Forms.Label();
			this.scheduleMonthlyByDay = new System.Windows.Forms.RadioButton();
			this.oneTimePanel = new System.Windows.Forms.Panel();
			this.oneTimeLbl = new System.Windows.Forms.Label();
			this.oneTimeBitmap = new System.Windows.Forms.PictureBox();
			this.scheduleTimePanel = new System.Windows.Forms.Panel();
			this.scheduleTime = new System.Windows.Forms.MaskedTextBox();
			this.scheduleTimeLbl = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.container.SuspendLayout();
			this.containerTask.SuspendLayout();
			this.containerSchedule.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.schedulePattern.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleWeeklyFreq)).BeginInit();
			this.scheduleDailyPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDailyByDayFreq)).BeginInit();
			this.scheduleWeeklyDays.SuspendLayout();
			this.scheduleMonthlyPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyByDayNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyByDayFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyRelativeFreq)).BeginInit();
			this.oneTimePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.oneTimeBitmap)).BeginInit();
			this.scheduleTimePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// nameLbl
			// 
			this.nameLbl.AutoSize = true;
			this.nameLbl.Location = new System.Drawing.Point(6, 9);
			this.nameLbl.Name = "nameLbl";
			this.nameLbl.Size = new System.Drawing.Size(122, 15);
			this.nameLbl.TabIndex = 0;
			this.nameLbl.Text = "Task name (optional):";
			// 
			// name
			// 
			this.name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.name.Location = new System.Drawing.Point(142, 6);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(191, 23);
			this.name.TabIndex = 1;
			// 
			// eraseLbl
			// 
			this.eraseLbl.AutoSize = true;
			this.eraseLbl.Location = new System.Drawing.Point(6, 75);
			this.eraseLbl.Name = "eraseLbl";
			this.eraseLbl.Size = new System.Drawing.Size(78, 15);
			this.eraseLbl.TabIndex = 2;
			this.eraseLbl.Text = "Data to erase:";
			// 
			// typeLbl
			// 
			this.typeLbl.AutoSize = true;
			this.typeLbl.Location = new System.Drawing.Point(6, 37);
			this.typeLbl.Name = "typeLbl";
			this.typeLbl.Size = new System.Drawing.Size(63, 15);
			this.typeLbl.TabIndex = 4;
			this.typeLbl.Text = "Task Type:";
			// 
			// typeOneTime
			// 
			this.typeOneTime.AutoSize = true;
			this.typeOneTime.Location = new System.Drawing.Point(142, 35);
			this.typeOneTime.Name = "typeOneTime";
			this.typeOneTime.Size = new System.Drawing.Size(76, 19);
			this.typeOneTime.TabIndex = 5;
			this.typeOneTime.Text = "One-time";
			this.typeOneTime.UseVisualStyleBackColor = true;
			this.typeOneTime.CheckedChanged += new System.EventHandler(this.taskType_CheckedChanged);
			// 
			// typeRecurring
			// 
			this.typeRecurring.AutoSize = true;
			this.typeRecurring.Location = new System.Drawing.Point(142, 53);
			this.typeRecurring.Name = "typeRecurring";
			this.typeRecurring.Size = new System.Drawing.Size(76, 19);
			this.typeRecurring.TabIndex = 6;
			this.typeRecurring.Text = "Recurring";
			this.typeRecurring.UseVisualStyleBackColor = true;
			this.typeRecurring.CheckedChanged += new System.EventHandler(this.taskType_CheckedChanged);
			// 
			// data
			// 
			this.data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dataColData,
            this.dataColMethod});
			this.data.FullRowSelect = true;
			this.data.Location = new System.Drawing.Point(9, 93);
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(324, 254);
			this.data.TabIndex = 7;
			this.data.UseCompatibleStateImageBehavior = false;
			this.data.View = System.Windows.Forms.View.Details;
			// 
			// dataColData
			// 
			this.dataColData.Text = "Data Set";
			this.dataColData.Width = 200;
			// 
			// dataColMethod
			// 
			this.dataColMethod.Text = "Erasure Method";
			this.dataColMethod.Width = 100;
			// 
			// dataAdd
			// 
			this.dataAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dataAdd.Location = new System.Drawing.Point(9, 353);
			this.dataAdd.Name = "dataAdd";
			this.dataAdd.Size = new System.Drawing.Size(75, 23);
			this.dataAdd.TabIndex = 8;
			this.dataAdd.Text = "Add Data";
			this.dataAdd.UseVisualStyleBackColor = true;
			this.dataAdd.Click += new System.EventHandler(this.dataAdd_Click);
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ok.Location = new System.Drawing.Point(206, 435);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(75, 23);
			this.ok.TabIndex = 9;
			this.ok.Text = "OK";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(287, 435);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(75, 23);
			this.cancel.TabIndex = 10;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			// 
			// container
			// 
			this.container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.container.Controls.Add(this.containerTask);
			this.container.Controls.Add(this.containerSchedule);
			this.container.Location = new System.Drawing.Point(15, 12);
			this.container.Name = "container";
			this.container.SelectedIndex = 0;
			this.container.Size = new System.Drawing.Size(347, 414);
			this.container.TabIndex = 11;
			// 
			// containerTask
			// 
			this.containerTask.Controls.Add(this.nameLbl);
			this.containerTask.Controls.Add(this.name);
			this.containerTask.Controls.Add(this.typeLbl);
			this.containerTask.Controls.Add(this.typeOneTime);
			this.containerTask.Controls.Add(this.typeRecurring);
			this.containerTask.Controls.Add(this.eraseLbl);
			this.containerTask.Controls.Add(this.data);
			this.containerTask.Controls.Add(this.dataAdd);
			this.containerTask.Location = new System.Drawing.Point(4, 24);
			this.containerTask.Name = "containerTask";
			this.containerTask.Padding = new System.Windows.Forms.Padding(3);
			this.containerTask.Size = new System.Drawing.Size(339, 386);
			this.containerTask.TabIndex = 0;
			this.containerTask.Text = "Task";
			this.containerTask.UseVisualStyleBackColor = true;
			// 
			// containerSchedule
			// 
			this.containerSchedule.Controls.Add(this.tableLayoutPanel1);
			this.containerSchedule.Location = new System.Drawing.Point(4, 24);
			this.containerSchedule.Name = "containerSchedule";
			this.containerSchedule.Padding = new System.Windows.Forms.Padding(3);
			this.containerSchedule.Size = new System.Drawing.Size(339, 386);
			this.containerSchedule.TabIndex = 1;
			this.containerSchedule.Text = "Schedule";
			this.containerSchedule.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.schedulePattern, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.oneTimePanel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.scheduleTimePanel, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(333, 380);
			this.tableLayoutPanel1.TabIndex = 10;
			// 
			// schedulePattern
			// 
			this.schedulePattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.schedulePattern.Controls.Add(this.scheduleWeeklyFreq);
			this.schedulePattern.Controls.Add(this.scheduleDaily);
			this.schedulePattern.Controls.Add(this.scheduleDailyPanel);
			this.schedulePattern.Controls.Add(this.scheduleWeeklyDays);
			this.schedulePattern.Controls.Add(this.scheduleWeeklyFreqLbl);
			this.schedulePattern.Controls.Add(this.scheduleWeeklyLbl);
			this.schedulePattern.Controls.Add(this.scheduleWeekly);
			this.schedulePattern.Controls.Add(this.scheduleMonthly);
			this.schedulePattern.Controls.Add(this.scheduleMonthlyPanel);
			this.schedulePattern.Location = new System.Drawing.Point(3, 71);
			this.schedulePattern.Name = "schedulePattern";
			this.schedulePattern.Size = new System.Drawing.Size(327, 306);
			this.schedulePattern.TabIndex = 4;
			this.schedulePattern.TabStop = false;
			this.schedulePattern.Text = "Recurrance Pattern";
			// 
			// scheduleWeeklyFreq
			// 
			this.scheduleWeeklyFreq.Location = new System.Drawing.Point(61, 109);
			this.scheduleWeeklyFreq.Maximum = new decimal(new int[] {
            104,
            0,
            0,
            0});
			this.scheduleWeeklyFreq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.scheduleWeeklyFreq.Name = "scheduleWeeklyFreq";
			this.scheduleWeeklyFreq.Size = new System.Drawing.Size(43, 23);
			this.scheduleWeeklyFreq.TabIndex = 4;
			this.scheduleWeeklyFreq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// scheduleDaily
			// 
			this.scheduleDaily.AutoSize = true;
			this.scheduleDaily.Location = new System.Drawing.Point(6, 15);
			this.scheduleDaily.Name = "scheduleDaily";
			this.scheduleDaily.Size = new System.Drawing.Size(51, 19);
			this.scheduleDaily.TabIndex = 0;
			this.scheduleDaily.Text = "Daily";
			this.scheduleDaily.UseVisualStyleBackColor = true;
			this.scheduleDaily.CheckedChanged += new System.EventHandler(this.scheduleSpan_CheckedChanged);
			// 
			// scheduleDailyPanel
			// 
			this.scheduleDailyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.scheduleDailyPanel.Controls.Add(this.scheduleDailyByDayFreq);
			this.scheduleDailyPanel.Controls.Add(this.scheduleDailyByDay);
			this.scheduleDailyPanel.Controls.Add(this.scheduleDailyByDayLbl);
			this.scheduleDailyPanel.Controls.Add(this.scheduleDailyByWeekday);
			this.scheduleDailyPanel.Location = new System.Drawing.Point(26, 36);
			this.scheduleDailyPanel.Name = "scheduleDailyPanel";
			this.scheduleDailyPanel.Size = new System.Drawing.Size(301, 48);
			this.scheduleDailyPanel.TabIndex = 1;
			// 
			// scheduleDailyByDayFreq
			// 
			this.scheduleDailyByDayFreq.Location = new System.Drawing.Point(62, 3);
			this.scheduleDailyByDayFreq.Name = "scheduleDailyByDayFreq";
			this.scheduleDailyByDayFreq.Size = new System.Drawing.Size(43, 23);
			this.scheduleDailyByDayFreq.TabIndex = 4;
			// 
			// scheduleDailyByDay
			// 
			this.scheduleDailyByDay.AutoSize = true;
			this.scheduleDailyByDay.Checked = true;
			this.scheduleDailyByDay.Location = new System.Drawing.Point(3, 3);
			this.scheduleDailyByDay.Name = "scheduleDailyByDay";
			this.scheduleDailyByDay.Size = new System.Drawing.Size(53, 19);
			this.scheduleDailyByDay.TabIndex = 0;
			this.scheduleDailyByDay.TabStop = true;
			this.scheduleDailyByDay.Text = "Every";
			this.scheduleDailyByDay.UseVisualStyleBackColor = true;
			this.scheduleDailyByDay.CheckedChanged += new System.EventHandler(this.scheduleDailySpan_CheckedChanged);
			// 
			// scheduleDailyByDayLbl
			// 
			this.scheduleDailyByDayLbl.AutoSize = true;
			this.scheduleDailyByDayLbl.Location = new System.Drawing.Point(111, 5);
			this.scheduleDailyByDayLbl.Name = "scheduleDailyByDayLbl";
			this.scheduleDailyByDayLbl.Size = new System.Drawing.Size(34, 15);
			this.scheduleDailyByDayLbl.TabIndex = 2;
			this.scheduleDailyByDayLbl.Text = "days,";
			// 
			// scheduleDailyByWeekday
			// 
			this.scheduleDailyByWeekday.AutoSize = true;
			this.scheduleDailyByWeekday.Location = new System.Drawing.Point(3, 25);
			this.scheduleDailyByWeekday.Name = "scheduleDailyByWeekday";
			this.scheduleDailyByWeekday.Size = new System.Drawing.Size(102, 19);
			this.scheduleDailyByWeekday.TabIndex = 3;
			this.scheduleDailyByWeekday.Text = "Every weekday";
			this.scheduleDailyByWeekday.UseVisualStyleBackColor = true;
			this.scheduleDailyByWeekday.CheckedChanged += new System.EventHandler(this.scheduleDailySpan_CheckedChanged);
			// 
			// scheduleWeeklyDays
			// 
			this.scheduleWeeklyDays.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklyMonday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklyTuesday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklyWednesday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklyThursday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklyFriday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklySaturday);
			this.scheduleWeeklyDays.Controls.Add(this.scheduleWeeklySunday);
			this.scheduleWeeklyDays.Location = new System.Drawing.Point(23, 135);
			this.scheduleWeeklyDays.Name = "scheduleWeeklyDays";
			this.scheduleWeeklyDays.Size = new System.Drawing.Size(304, 50);
			this.scheduleWeeklyDays.TabIndex = 6;
			// 
			// scheduleWeeklyMonday
			// 
			this.scheduleWeeklyMonday.AutoSize = true;
			this.scheduleWeeklyMonday.Location = new System.Drawing.Point(3, 3);
			this.scheduleWeeklyMonday.Name = "scheduleWeeklyMonday";
			this.scheduleWeeklyMonday.Size = new System.Drawing.Size(70, 19);
			this.scheduleWeeklyMonday.TabIndex = 12;
			this.scheduleWeeklyMonday.Text = "Monday";
			this.scheduleWeeklyMonday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklyTuesday
			// 
			this.scheduleWeeklyTuesday.AutoSize = true;
			this.scheduleWeeklyTuesday.Location = new System.Drawing.Point(79, 3);
			this.scheduleWeeklyTuesday.Name = "scheduleWeeklyTuesday";
			this.scheduleWeeklyTuesday.Size = new System.Drawing.Size(70, 19);
			this.scheduleWeeklyTuesday.TabIndex = 13;
			this.scheduleWeeklyTuesday.Text = "Tuesday";
			this.scheduleWeeklyTuesday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklyWednesday
			// 
			this.scheduleWeeklyWednesday.AutoSize = true;
			this.scheduleWeeklyWednesday.Location = new System.Drawing.Point(155, 3);
			this.scheduleWeeklyWednesday.Name = "scheduleWeeklyWednesday";
			this.scheduleWeeklyWednesday.Size = new System.Drawing.Size(87, 19);
			this.scheduleWeeklyWednesday.TabIndex = 14;
			this.scheduleWeeklyWednesday.Text = "Wednesday";
			this.scheduleWeeklyWednesday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklyThursday
			// 
			this.scheduleWeeklyThursday.AutoSize = true;
			this.scheduleWeeklyThursday.Location = new System.Drawing.Point(3, 28);
			this.scheduleWeeklyThursday.Name = "scheduleWeeklyThursday";
			this.scheduleWeeklyThursday.Size = new System.Drawing.Size(75, 19);
			this.scheduleWeeklyThursday.TabIndex = 15;
			this.scheduleWeeklyThursday.Text = "Thursday";
			this.scheduleWeeklyThursday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklyFriday
			// 
			this.scheduleWeeklyFriday.AutoSize = true;
			this.scheduleWeeklyFriday.Location = new System.Drawing.Point(84, 28);
			this.scheduleWeeklyFriday.Name = "scheduleWeeklyFriday";
			this.scheduleWeeklyFriday.Size = new System.Drawing.Size(58, 19);
			this.scheduleWeeklyFriday.TabIndex = 16;
			this.scheduleWeeklyFriday.Text = "Friday";
			this.scheduleWeeklyFriday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklySaturday
			// 
			this.scheduleWeeklySaturday.AutoSize = true;
			this.scheduleWeeklySaturday.Location = new System.Drawing.Point(148, 28);
			this.scheduleWeeklySaturday.Name = "scheduleWeeklySaturday";
			this.scheduleWeeklySaturday.Size = new System.Drawing.Size(72, 19);
			this.scheduleWeeklySaturday.TabIndex = 17;
			this.scheduleWeeklySaturday.Text = "Saturday";
			this.scheduleWeeklySaturday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklySunday
			// 
			this.scheduleWeeklySunday.AutoSize = true;
			this.scheduleWeeklySunday.Location = new System.Drawing.Point(226, 28);
			this.scheduleWeeklySunday.Name = "scheduleWeeklySunday";
			this.scheduleWeeklySunday.Size = new System.Drawing.Size(65, 19);
			this.scheduleWeeklySunday.TabIndex = 18;
			this.scheduleWeeklySunday.Text = "Sunday";
			this.scheduleWeeklySunday.UseVisualStyleBackColor = true;
			// 
			// scheduleWeeklyFreqLbl
			// 
			this.scheduleWeeklyFreqLbl.AutoSize = true;
			this.scheduleWeeklyFreqLbl.Location = new System.Drawing.Point(110, 111);
			this.scheduleWeeklyFreqLbl.Name = "scheduleWeeklyFreqLbl";
			this.scheduleWeeklyFreqLbl.Size = new System.Drawing.Size(170, 15);
			this.scheduleWeeklyFreqLbl.TabIndex = 5;
			this.scheduleWeeklyFreqLbl.Text = "week(s), on the following days:";
			// 
			// scheduleWeeklyLbl
			// 
			this.scheduleWeeklyLbl.AutoSize = true;
			this.scheduleWeeklyLbl.Location = new System.Drawing.Point(20, 111);
			this.scheduleWeeklyLbl.Name = "scheduleWeeklyLbl";
			this.scheduleWeeklyLbl.Size = new System.Drawing.Size(35, 15);
			this.scheduleWeeklyLbl.TabIndex = 3;
			this.scheduleWeeklyLbl.Text = "Every";
			// 
			// scheduleWeekly
			// 
			this.scheduleWeekly.AutoSize = true;
			this.scheduleWeekly.Location = new System.Drawing.Point(6, 83);
			this.scheduleWeekly.Name = "scheduleWeekly";
			this.scheduleWeekly.Size = new System.Drawing.Size(63, 19);
			this.scheduleWeekly.TabIndex = 2;
			this.scheduleWeekly.Text = "Weekly";
			this.scheduleWeekly.UseVisualStyleBackColor = true;
			this.scheduleWeekly.CheckedChanged += new System.EventHandler(this.scheduleSpan_CheckedChanged);
			// 
			// scheduleMonthly
			// 
			this.scheduleMonthly.AutoSize = true;
			this.scheduleMonthly.Location = new System.Drawing.Point(6, 188);
			this.scheduleMonthly.Name = "scheduleMonthly";
			this.scheduleMonthly.Size = new System.Drawing.Size(70, 19);
			this.scheduleMonthly.TabIndex = 7;
			this.scheduleMonthly.TabStop = true;
			this.scheduleMonthly.Text = "Monthly";
			this.scheduleMonthly.UseVisualStyleBackColor = true;
			this.scheduleMonthly.CheckedChanged += new System.EventHandler(this.scheduleSpan_CheckedChanged);
			// 
			// scheduleMonthlyPanel
			// 
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyByDayNumber);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyByDayFreq);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelativeFreq);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelativeFreqLbl);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelativeEveryLbl);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyByDayMonthLbl);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelativeWeekDay);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelativeWeek);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyRelative);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyByDayEveryLbl);
			this.scheduleMonthlyPanel.Controls.Add(this.scheduleMonthlyByDay);
			this.scheduleMonthlyPanel.Location = new System.Drawing.Point(23, 213);
			this.scheduleMonthlyPanel.Name = "scheduleMonthlyPanel";
			this.scheduleMonthlyPanel.Size = new System.Drawing.Size(304, 87);
			this.scheduleMonthlyPanel.TabIndex = 8;
			// 
			// scheduleMonthlyByDayNumber
			// 
			this.scheduleMonthlyByDayNumber.Location = new System.Drawing.Point(54, 1);
			this.scheduleMonthlyByDayNumber.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
			this.scheduleMonthlyByDayNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.scheduleMonthlyByDayNumber.Name = "scheduleMonthlyByDayNumber";
			this.scheduleMonthlyByDayNumber.Size = new System.Drawing.Size(43, 23);
			this.scheduleMonthlyByDayNumber.TabIndex = 1;
			this.scheduleMonthlyByDayNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// scheduleMonthlyByDayFreq
			// 
			this.scheduleMonthlyByDayFreq.Location = new System.Drawing.Point(158, 1);
			this.scheduleMonthlyByDayFreq.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
			this.scheduleMonthlyByDayFreq.Name = "scheduleMonthlyByDayFreq";
			this.scheduleMonthlyByDayFreq.Size = new System.Drawing.Size(43, 23);
			this.scheduleMonthlyByDayFreq.TabIndex = 3;
			// 
			// scheduleMonthlyRelativeFreq
			// 
			this.scheduleMonthlyRelativeFreq.Location = new System.Drawing.Point(23, 56);
			this.scheduleMonthlyRelativeFreq.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
			this.scheduleMonthlyRelativeFreq.Name = "scheduleMonthlyRelativeFreq";
			this.scheduleMonthlyRelativeFreq.Size = new System.Drawing.Size(42, 23);
			this.scheduleMonthlyRelativeFreq.TabIndex = 9;
			// 
			// scheduleMonthlyRelativeFreqLbl
			// 
			this.scheduleMonthlyRelativeFreqLbl.AutoSize = true;
			this.scheduleMonthlyRelativeFreqLbl.Location = new System.Drawing.Point(71, 58);
			this.scheduleMonthlyRelativeFreqLbl.Name = "scheduleMonthlyRelativeFreqLbl";
			this.scheduleMonthlyRelativeFreqLbl.Size = new System.Drawing.Size(56, 15);
			this.scheduleMonthlyRelativeFreqLbl.TabIndex = 10;
			this.scheduleMonthlyRelativeFreqLbl.Text = "month(s)";
			// 
			// scheduleMonthlyRelativeEveryLbl
			// 
			this.scheduleMonthlyRelativeEveryLbl.AutoSize = true;
			this.scheduleMonthlyRelativeEveryLbl.Location = new System.Drawing.Point(248, 32);
			this.scheduleMonthlyRelativeEveryLbl.Name = "scheduleMonthlyRelativeEveryLbl";
			this.scheduleMonthlyRelativeEveryLbl.Size = new System.Drawing.Size(41, 15);
			this.scheduleMonthlyRelativeEveryLbl.TabIndex = 8;
			this.scheduleMonthlyRelativeEveryLbl.Text = ", every";
			// 
			// scheduleMonthlyByDayMonthLbl
			// 
			this.scheduleMonthlyByDayMonthLbl.AutoSize = true;
			this.scheduleMonthlyByDayMonthLbl.Location = new System.Drawing.Point(207, 3);
			this.scheduleMonthlyByDayMonthLbl.Name = "scheduleMonthlyByDayMonthLbl";
			this.scheduleMonthlyByDayMonthLbl.Size = new System.Drawing.Size(56, 15);
			this.scheduleMonthlyByDayMonthLbl.TabIndex = 4;
			this.scheduleMonthlyByDayMonthLbl.Text = "month(s)";
			// 
			// scheduleMonthlyRelativeWeekDay
			// 
			this.scheduleMonthlyRelativeWeekDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.scheduleMonthlyRelativeWeekDay.FormattingEnabled = true;
			this.scheduleMonthlyRelativeWeekDay.Items.AddRange(new object[] {
            "day",
            "weekday",
            "weekend day",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
			this.scheduleMonthlyRelativeWeekDay.Location = new System.Drawing.Point(142, 29);
			this.scheduleMonthlyRelativeWeekDay.Name = "scheduleMonthlyRelativeWeekDay";
			this.scheduleMonthlyRelativeWeekDay.Size = new System.Drawing.Size(100, 23);
			this.scheduleMonthlyRelativeWeekDay.TabIndex = 7;
			// 
			// scheduleMonthlyRelativeWeek
			// 
			this.scheduleMonthlyRelativeWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.scheduleMonthlyRelativeWeek.FormattingEnabled = true;
			this.scheduleMonthlyRelativeWeek.Items.AddRange(new object[] {
            "first",
            "second",
            "third",
            "fourth",
            "fifth",
            "last"});
			this.scheduleMonthlyRelativeWeek.Location = new System.Drawing.Point(70, 29);
			this.scheduleMonthlyRelativeWeek.Name = "scheduleMonthlyRelativeWeek";
			this.scheduleMonthlyRelativeWeek.Size = new System.Drawing.Size(72, 23);
			this.scheduleMonthlyRelativeWeek.TabIndex = 6;
			// 
			// scheduleMonthlyRelative
			// 
			this.scheduleMonthlyRelative.AutoSize = true;
			this.scheduleMonthlyRelative.Location = new System.Drawing.Point(3, 30);
			this.scheduleMonthlyRelative.Name = "scheduleMonthlyRelative";
			this.scheduleMonthlyRelative.Size = new System.Drawing.Size(61, 19);
			this.scheduleMonthlyRelative.TabIndex = 5;
			this.scheduleMonthlyRelative.TabStop = true;
			this.scheduleMonthlyRelative.Text = "On the";
			this.scheduleMonthlyRelative.UseVisualStyleBackColor = true;
			this.scheduleMonthlyRelative.CheckedChanged += new System.EventHandler(this.scheduleMonthlySpan_CheckedChanged);
			// 
			// scheduleMonthlyByDayEveryLbl
			// 
			this.scheduleMonthlyByDayEveryLbl.AutoSize = true;
			this.scheduleMonthlyByDayEveryLbl.Location = new System.Drawing.Point(103, 3);
			this.scheduleMonthlyByDayEveryLbl.Name = "scheduleMonthlyByDayEveryLbl";
			this.scheduleMonthlyByDayEveryLbl.Size = new System.Drawing.Size(49, 15);
			this.scheduleMonthlyByDayEveryLbl.TabIndex = 2;
			this.scheduleMonthlyByDayEveryLbl.Text = "of every";
			// 
			// scheduleMonthlyByDay
			// 
			this.scheduleMonthlyByDay.AutoSize = true;
			this.scheduleMonthlyByDay.Checked = true;
			this.scheduleMonthlyByDay.Location = new System.Drawing.Point(3, 1);
			this.scheduleMonthlyByDay.Name = "scheduleMonthlyByDay";
			this.scheduleMonthlyByDay.Size = new System.Drawing.Size(45, 19);
			this.scheduleMonthlyByDay.TabIndex = 0;
			this.scheduleMonthlyByDay.TabStop = true;
			this.scheduleMonthlyByDay.Text = "Day";
			this.scheduleMonthlyByDay.UseVisualStyleBackColor = true;
			this.scheduleMonthlyByDay.CheckedChanged += new System.EventHandler(this.scheduleMonthlySpan_CheckedChanged);
			// 
			// oneTimePanel
			// 
			this.oneTimePanel.Controls.Add(this.oneTimeLbl);
			this.oneTimePanel.Controls.Add(this.oneTimeBitmap);
			this.oneTimePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.oneTimePanel.Location = new System.Drawing.Point(3, 3);
			this.oneTimePanel.Name = "oneTimePanel";
			this.oneTimePanel.Size = new System.Drawing.Size(327, 34);
			this.oneTimePanel.TabIndex = 1;
			// 
			// oneTimeLbl
			// 
			this.oneTimeLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.oneTimeLbl.Location = new System.Drawing.Point(38, 0);
			this.oneTimeLbl.Name = "oneTimeLbl";
			this.oneTimeLbl.Size = new System.Drawing.Size(287, 34);
			this.oneTimeLbl.TabIndex = 1;
			this.oneTimeLbl.Text = "The task being edited is a non-recurring task, none of the settings on this page " +
				"apply.";
			// 
			// oneTimeBitmap
			// 
			this.oneTimeBitmap.Image = global::Eraser.Properties.Resources.Information;
			this.oneTimeBitmap.Location = new System.Drawing.Point(0, 0);
			this.oneTimeBitmap.Name = "oneTimeBitmap";
			this.oneTimeBitmap.Size = new System.Drawing.Size(32, 32);
			this.oneTimeBitmap.TabIndex = 0;
			this.oneTimeBitmap.TabStop = false;
			// 
			// scheduleTimePanel
			// 
			this.scheduleTimePanel.AutoSize = true;
			this.scheduleTimePanel.Controls.Add(this.scheduleTime);
			this.scheduleTimePanel.Controls.Add(this.scheduleTimeLbl);
			this.scheduleTimePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scheduleTimePanel.Location = new System.Drawing.Point(0, 40);
			this.scheduleTimePanel.Margin = new System.Windows.Forms.Padding(0);
			this.scheduleTimePanel.Name = "scheduleTimePanel";
			this.scheduleTimePanel.Size = new System.Drawing.Size(333, 28);
			this.scheduleTimePanel.TabIndex = 2;
			// 
			// scheduleTime
			// 
			this.scheduleTime.Location = new System.Drawing.Point(99, 2);
			this.scheduleTime.Mask = "90:00";
			this.scheduleTime.Name = "scheduleTime";
			this.scheduleTime.Size = new System.Drawing.Size(47, 23);
			this.scheduleTime.TabIndex = 4;
			this.scheduleTime.ValidatingType = typeof(System.DateTime);
			// 
			// scheduleTimeLbl
			// 
			this.scheduleTimeLbl.AutoSize = true;
			this.scheduleTimeLbl.Location = new System.Drawing.Point(3, 5);
			this.scheduleTimeLbl.Name = "scheduleTimeLbl";
			this.scheduleTimeLbl.Size = new System.Drawing.Size(90, 15);
			this.scheduleTimeLbl.TabIndex = 3;
			this.scheduleTimeLbl.Text = "Run this task at:";
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// TaskPropertiesForm
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(374, 470);
			this.Controls.Add(this.container);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TaskPropertiesForm";
			this.ShowInTaskbar = false;
			this.Text = "Task Properties";
			this.container.ResumeLayout(false);
			this.containerTask.ResumeLayout(false);
			this.containerTask.PerformLayout();
			this.containerSchedule.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.schedulePattern.ResumeLayout(false);
			this.schedulePattern.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleWeeklyFreq)).EndInit();
			this.scheduleDailyPanel.ResumeLayout(false);
			this.scheduleDailyPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleDailyByDayFreq)).EndInit();
			this.scheduleWeeklyDays.ResumeLayout(false);
			this.scheduleWeeklyDays.PerformLayout();
			this.scheduleMonthlyPanel.ResumeLayout(false);
			this.scheduleMonthlyPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyByDayNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyByDayFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scheduleMonthlyRelativeFreq)).EndInit();
			this.oneTimePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.oneTimeBitmap)).EndInit();
			this.scheduleTimePanel.ResumeLayout(false);
			this.scheduleTimePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label nameLbl;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.Label eraseLbl;
		private System.Windows.Forms.Label typeLbl;
		private System.Windows.Forms.RadioButton typeOneTime;
		private System.Windows.Forms.RadioButton typeRecurring;
		private System.Windows.Forms.ListView data;
		private System.Windows.Forms.ColumnHeader dataColData;
		private System.Windows.Forms.ColumnHeader dataColMethod;
		private System.Windows.Forms.Button dataAdd;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.TabControl container;
		private System.Windows.Forms.TabPage containerTask;
		private System.Windows.Forms.TabPage containerSchedule;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox schedulePattern;
		private System.Windows.Forms.NumericUpDown scheduleWeeklyFreq;
		private System.Windows.Forms.RadioButton scheduleDaily;
		private System.Windows.Forms.Panel scheduleDailyPanel;
		private System.Windows.Forms.NumericUpDown scheduleDailyByDayFreq;
		private System.Windows.Forms.RadioButton scheduleDailyByDay;
		private System.Windows.Forms.Label scheduleDailyByDayLbl;
		private System.Windows.Forms.RadioButton scheduleDailyByWeekday;
		private System.Windows.Forms.FlowLayoutPanel scheduleWeeklyDays;
		private System.Windows.Forms.CheckBox scheduleWeeklyMonday;
		private System.Windows.Forms.CheckBox scheduleWeeklyTuesday;
		private System.Windows.Forms.CheckBox scheduleWeeklyWednesday;
		private System.Windows.Forms.CheckBox scheduleWeeklyThursday;
		private System.Windows.Forms.CheckBox scheduleWeeklyFriday;
		private System.Windows.Forms.CheckBox scheduleWeeklySaturday;
		private System.Windows.Forms.CheckBox scheduleWeeklySunday;
		private System.Windows.Forms.Label scheduleWeeklyFreqLbl;
		private System.Windows.Forms.Label scheduleWeeklyLbl;
		private System.Windows.Forms.RadioButton scheduleWeekly;
		private System.Windows.Forms.RadioButton scheduleMonthly;
		private System.Windows.Forms.Panel scheduleMonthlyPanel;
		private System.Windows.Forms.NumericUpDown scheduleMonthlyByDayNumber;
		private System.Windows.Forms.NumericUpDown scheduleMonthlyByDayFreq;
		private System.Windows.Forms.NumericUpDown scheduleMonthlyRelativeFreq;
		private System.Windows.Forms.Label scheduleMonthlyRelativeFreqLbl;
		private System.Windows.Forms.Label scheduleMonthlyRelativeEveryLbl;
		private System.Windows.Forms.Label scheduleMonthlyByDayMonthLbl;
		private System.Windows.Forms.ComboBox scheduleMonthlyRelativeWeekDay;
		private System.Windows.Forms.ComboBox scheduleMonthlyRelativeWeek;
		private System.Windows.Forms.RadioButton scheduleMonthlyRelative;
		private System.Windows.Forms.Label scheduleMonthlyByDayEveryLbl;
		private System.Windows.Forms.RadioButton scheduleMonthlyByDay;
		private System.Windows.Forms.Panel oneTimePanel;
		private System.Windows.Forms.Label oneTimeLbl;
		private System.Windows.Forms.PictureBox oneTimeBitmap;
		private System.Windows.Forms.Panel scheduleTimePanel;
		private System.Windows.Forms.MaskedTextBox scheduleTime;
		private System.Windows.Forms.Label scheduleTimeLbl;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}