﻿/* 
 * $Id$
 * Copyright 2008 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By:
 * 
 * This file is part of Eraser.
 * 
 * Eraser is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 * 
 * Eraser is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
 * A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * 
 * A copy of the GNU General Public License can be found at
 * <http://www.gnu.org/licenses/>.
 */

namespace Eraser
{
	partial class UpdateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
			this.updateListDownloader = new System.ComponentModel.BackgroundWorker();
			this.updatesPanel = new System.Windows.Forms.Panel();
			this.updatesLv = new System.Windows.Forms.ListView();
			this.updatesLvNameCol = new System.Windows.Forms.ColumnHeader();
			this.updatesLvVersionCol = new System.Windows.Forms.ColumnHeader();
			this.updatesLvPublisherCol = new System.Windows.Forms.ColumnHeader();
			this.updatesLvFilesizeCol = new System.Windows.Forms.ColumnHeader();
			this.updatesBtn = new System.Windows.Forms.Button();
			this.updatesLbl = new System.Windows.Forms.Label();
			this.progressPanel = new System.Windows.Forms.Panel();
			this.progressProgressLbl = new System.Windows.Forms.Label();
			this.progressPb = new System.Windows.Forms.ProgressBar();
			this.progressLbl = new System.Windows.Forms.Label();
			this.downloader = new System.ComponentModel.BackgroundWorker();
			this.downloadingPnl = new System.Windows.Forms.Panel();
			this.downloadingOverallPb = new System.Windows.Forms.ProgressBar();
			this.downloadingOverallLbl = new System.Windows.Forms.Label();
			this.downloadingItemPb = new System.Windows.Forms.ProgressBar();
			this.downloadingItemLbl = new System.Windows.Forms.Label();
			this.downloadingLv = new System.Windows.Forms.ListView();
			this.downloadingLvColName = new System.Windows.Forms.ColumnHeader();
			this.downloadingLvColAmount = new System.Windows.Forms.ColumnHeader();
			this.downloadingLbl = new System.Windows.Forms.Label();
			this.updatesPanel.SuspendLayout();
			this.progressPanel.SuspendLayout();
			this.downloadingPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// updateListDownloader
			// 
			this.updateListDownloader.WorkerReportsProgress = true;
			this.updateListDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateListDownloader_DoWork);
			this.updateListDownloader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.updateListDownloader_RunWorkerCompleted);
			this.updateListDownloader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.updateListDownloader_ProgressChanged);
			// 
			// updatesPanel
			// 
			this.updatesPanel.Controls.Add(this.updatesLv);
			this.updatesPanel.Controls.Add(this.updatesBtn);
			this.updatesPanel.Controls.Add(this.updatesLbl);
			resources.ApplyResources(this.updatesPanel, "updatesPanel");
			this.updatesPanel.Name = "updatesPanel";
			// 
			// updatesLv
			// 
			resources.ApplyResources(this.updatesLv, "updatesLv");
			this.updatesLv.CheckBoxes = true;
			this.updatesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.updatesLvNameCol,
            this.updatesLvVersionCol,
            this.updatesLvPublisherCol,
            this.updatesLvFilesizeCol});
			this.updatesLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.updatesLv.Name = "updatesLv";
			this.updatesLv.UseCompatibleStateImageBehavior = false;
			this.updatesLv.View = System.Windows.Forms.View.Details;
			// 
			// updatesLvNameCol
			// 
			resources.ApplyResources(this.updatesLvNameCol, "updatesLvNameCol");
			// 
			// updatesLvVersionCol
			// 
			resources.ApplyResources(this.updatesLvVersionCol, "updatesLvVersionCol");
			// 
			// updatesLvPublisherCol
			// 
			resources.ApplyResources(this.updatesLvPublisherCol, "updatesLvPublisherCol");
			// 
			// updatesLvFilesizeCol
			// 
			resources.ApplyResources(this.updatesLvFilesizeCol, "updatesLvFilesizeCol");
			// 
			// updatesBtn
			// 
			resources.ApplyResources(this.updatesBtn, "updatesBtn");
			this.updatesBtn.Name = "updatesBtn";
			this.updatesBtn.UseVisualStyleBackColor = true;
			this.updatesBtn.Click += new System.EventHandler(this.updatesBtn_Click);
			// 
			// updatesLbl
			// 
			resources.ApplyResources(this.updatesLbl, "updatesLbl");
			this.updatesLbl.Name = "updatesLbl";
			// 
			// progressPanel
			// 
			this.progressPanel.Controls.Add(this.progressProgressLbl);
			this.progressPanel.Controls.Add(this.progressPb);
			this.progressPanel.Controls.Add(this.progressLbl);
			resources.ApplyResources(this.progressPanel, "progressPanel");
			this.progressPanel.Name = "progressPanel";
			// 
			// progressProgressLbl
			// 
			resources.ApplyResources(this.progressProgressLbl, "progressProgressLbl");
			this.progressProgressLbl.Name = "progressProgressLbl";
			// 
			// progressPb
			// 
			resources.ApplyResources(this.progressPb, "progressPb");
			this.progressPb.Name = "progressPb";
			this.progressPb.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			// 
			// progressLbl
			// 
			resources.ApplyResources(this.progressLbl, "progressLbl");
			this.progressLbl.ForeColor = System.Drawing.SystemColors.ControlText;
			this.progressLbl.Name = "progressLbl";
			// 
			// downloader
			// 
			this.downloader.WorkerReportsProgress = true;
			this.downloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloader_DoWork);
			this.downloader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.downloader_RunWorkerCompleted);
			this.downloader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.downloader_ProgressChanged);
			// 
			// downloadingPnl
			// 
			this.downloadingPnl.Controls.Add(this.downloadingOverallPb);
			this.downloadingPnl.Controls.Add(this.downloadingOverallLbl);
			this.downloadingPnl.Controls.Add(this.downloadingItemPb);
			this.downloadingPnl.Controls.Add(this.downloadingItemLbl);
			this.downloadingPnl.Controls.Add(this.downloadingLv);
			this.downloadingPnl.Controls.Add(this.downloadingLbl);
			resources.ApplyResources(this.downloadingPnl, "downloadingPnl");
			this.downloadingPnl.Name = "downloadingPnl";
			// 
			// downloadingOverallPb
			// 
			resources.ApplyResources(this.downloadingOverallPb, "downloadingOverallPb");
			this.downloadingOverallPb.Name = "downloadingOverallPb";
			this.downloadingOverallPb.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// downloadingOverallLbl
			// 
			resources.ApplyResources(this.downloadingOverallLbl, "downloadingOverallLbl");
			this.downloadingOverallLbl.Name = "downloadingOverallLbl";
			// 
			// downloadingItemPb
			// 
			resources.ApplyResources(this.downloadingItemPb, "downloadingItemPb");
			this.downloadingItemPb.Name = "downloadingItemPb";
			this.downloadingItemPb.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// downloadingItemLbl
			// 
			resources.ApplyResources(this.downloadingItemLbl, "downloadingItemLbl");
			this.downloadingItemLbl.Name = "downloadingItemLbl";
			// 
			// downloadingLv
			// 
			resources.ApplyResources(this.downloadingLv, "downloadingLv");
			this.downloadingLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.downloadingLvColName,
            this.downloadingLvColAmount});
			this.downloadingLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.downloadingLv.Name = "downloadingLv";
			this.downloadingLv.UseCompatibleStateImageBehavior = false;
			this.downloadingLv.View = System.Windows.Forms.View.Details;
			// 
			// downloadingLvColName
			// 
			resources.ApplyResources(this.downloadingLvColName, "downloadingLvColName");
			// 
			// downloadingLvColAmount
			// 
			resources.ApplyResources(this.downloadingLvColAmount, "downloadingLvColAmount");
			// 
			// downloadingLbl
			// 
			resources.ApplyResources(this.downloadingLbl, "downloadingLbl");
			this.downloadingLbl.Name = "downloadingLbl";
			// 
			// UpdateForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.downloadingPnl);
			this.Controls.Add(this.updatesPanel);
			this.Controls.Add(this.progressPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateForm";
			this.ShowInTaskbar = false;
			this.updatesPanel.ResumeLayout(false);
			this.updatesPanel.PerformLayout();
			this.progressPanel.ResumeLayout(false);
			this.downloadingPnl.ResumeLayout(false);
			this.downloadingPnl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.ComponentModel.BackgroundWorker updateListDownloader;
		private System.Windows.Forms.Panel updatesPanel;
		private System.Windows.Forms.ListView updatesLv;
		private System.Windows.Forms.Button updatesBtn;
		private System.Windows.Forms.Label updatesLbl;
		private System.Windows.Forms.ColumnHeader updatesLvNameCol;
		private System.Windows.Forms.ColumnHeader updatesLvVersionCol;
		private System.Windows.Forms.ColumnHeader updatesLvPublisherCol;
		private System.Windows.Forms.ColumnHeader updatesLvFilesizeCol;
		private System.Windows.Forms.Panel progressPanel;
		private System.Windows.Forms.Label progressProgressLbl;
		private System.Windows.Forms.ProgressBar progressPb;
		private System.Windows.Forms.Label progressLbl;
		private System.ComponentModel.BackgroundWorker downloader;
		private System.Windows.Forms.Panel downloadingPnl;
		private System.Windows.Forms.ProgressBar downloadingOverallPb;
		private System.Windows.Forms.Label downloadingOverallLbl;
		private System.Windows.Forms.ProgressBar downloadingItemPb;
		private System.Windows.Forms.Label downloadingItemLbl;
		private System.Windows.Forms.ListView downloadingLv;
		private System.Windows.Forms.Label downloadingLbl;
		private System.Windows.Forms.ColumnHeader downloadingLvColName;
		private System.Windows.Forms.ColumnHeader downloadingLvColAmount;
	}
}