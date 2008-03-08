using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Eraser.Manager;

namespace Eraser
{
	public partial class SchedulerPanel : Eraser.BasePanel
	{
		public SchedulerPanel()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Adds a task to the list of scheduled tasks
		/// </summary>
		/// <param name="task">The task object.</param>
		public void AddTask(Task task)
		{
			ListViewItem item = scheduler.Items.Add(GenerateTaskName(task));
			item.SubItems.Add(string.Empty); //Time of next run
			item.SubItems.Add("Not running");

			if (task.Schedule == null)
				item.Group = scheduler.Groups["single"];
			else
				item.Group = scheduler.Groups["recurring"];
		}

		/// <summary>
		/// Determines the task name to display, deciding on whether a task name is
		/// provided by the user.
		/// </summary>
		/// <param name="task">The task object for which a name is to be generated</param>
		/// <returns>A task name, may not be unique.</returns>
		private string GenerateTaskName(Task task)
		{
			//Simple case, the task name was given by the user.
			if (task.Name.Length != 0)
				return task.Name;

			string result = string.Empty;
			if (task.Entries.Count < 3)
				//Simpler case, small set of data.
				foreach (Task.EraseTarget tgt in task.Entries)
					result += tgt.UIText + ", ";
			else
				//Ok, we've quite a few entries, get the first, the mid and the end.
				for (int i = 0; i < task.Entries.Count; i += task.Entries.Count / 3)
					result += task.Entries[i].UIText + ", ";
			return result.Substring(0, result.Length - 2);
		}
	}
}

