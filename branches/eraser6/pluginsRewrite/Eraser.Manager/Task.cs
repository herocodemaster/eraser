/* 
 * $Id$
 * Copyright 2008-2010 The Eraser Project
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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

using Eraser.Util;
using Eraser.Util.ExtensionMethods;
using Eraser.Plugins;
using Eraser.Plugins.ExtensionPoints;

namespace Eraser.Manager
{
	/// <summary>
	/// Deals with an erase task.
	/// </summary>
	[Serializable]
	public class Task : ITask, ISerializable
	{
		#region Serialization code
		protected Task(SerializationInfo info, StreamingContext context)
		{
			Name = (string)info.GetValue("Name", typeof(string));
			Executor = context.Context as Executor;
			Targets = (ErasureTargetCollection)info.GetValue("Targets", typeof(ErasureTargetCollection));
			Targets.Owner = this;
			Log = (List<LogSink>)info.GetValue("Log", typeof(List<LogSink>));
			Canceled = false;

			Schedule schedule = (Schedule)info.GetValue("Schedule", typeof(Schedule));
			if (schedule.GetType() == Schedule.RunManually.GetType())
				Schedule = Schedule.RunManually;
			else if (schedule.GetType() == Schedule.RunNow.GetType())
				Schedule = Schedule.RunNow;
			else if (schedule.GetType() == Schedule.RunOnRestart.GetType())
				Schedule = Schedule.RunOnRestart;
			else if (schedule is RecurringSchedule)
				Schedule = schedule;
			else
				throw new InvalidDataException(S._("An invalid type was found when loading " +
					"the task schedule"));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", Name);
			info.AddValue("Schedule", Schedule);
			info.AddValue("Targets", Targets);
			info.AddValue("Log", Log);
		}
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public Task()
		{
			Name = string.Empty;
			Targets = new ErasureTargetCollection(this);
			Schedule = Schedule.RunNow;
			Canceled = false;
			Log = new List<LogSink>();
		}

		/// <summary>
		/// The Executor object which is managing this task.
		/// </summary>
		public Executor Executor
		{
			get
			{
				return executor;
			}
			internal set
			{
				if (value == null)
					throw new ArgumentNullException("Task.Executor cannot be null");
				if (executor != null)
					throw new InvalidOperationException("A task can only belong to one " +
						"executor at any one time");

				executor = value;
			}
		}

		/// <summary>
		/// The name for this task. This is just an opaque value for the user to
		/// recognize the task.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The name of the task, used for display in UI elements.
		/// </summary>
		public override string ToString()
		{
			//Simple case, the task name was given by the user.
			if (!string.IsNullOrEmpty(Name))
				return Name;

			string result = string.Empty;
			if (Targets.Count == 0)
				return result;
			else if (Targets.Count < 5)
			{
				//Simpler case, small set of data.
				foreach (IErasureTarget tgt in Targets)
					result += S._("{0}, ", tgt);

				return result.Remove(result.Length - 2);
			}
			else
			{
				//Ok, we've quite a few entries, get the first, the mid and the end.
				result = S._("{0}, ", Targets[0]);
				result += S._("{0}, ", Targets[Targets.Count / 2]);
				result += Targets[Targets.Count - 1];

				return S._("{0} and {1} other targets", result, Targets.Count - 3);
			}
		}

		/// <summary>
		/// The set of data to erase when this task is executed.
		/// </summary>
		public ErasureTargetCollection Targets { get; private set; }

		/// <summary>
		/// <see cref="Targets"/>
		/// </summary>
		ICollection<IErasureTarget> ITask.Targets
		{
			get { return Targets; }
		}

		/// <summary>
		/// The schedule for running the task.
		/// </summary>
		public Schedule Schedule
		{
			get
			{
				return schedule;
			}
			set
			{
				if (value.Owner != null)
					throw new ArgumentException("The schedule provided can only " +
						"belong to one task at a time");

				if (schedule is RecurringSchedule)
					((RecurringSchedule)schedule).Owner = null;
				schedule = value;
				if (schedule is RecurringSchedule)
					((RecurringSchedule)schedule).Owner = this;
				OnTaskEdited();
			}
		}

		/// <summary>
		/// The log entries which this task has accumulated.
		/// </summary>
		public List<LogSink> Log { get; private set; }

		/// <summary>
		/// The progress manager object which manages the progress of this task.
		/// </summary>
		public SteppedProgressManager Progress
		{
			get
			{
				if (!Executing)
					throw new InvalidOperationException("The progress of an erasure can only " +
						"be queried when the task is being executed.");

				return progress;
			}
			private set
			{
				progress = value;
			}
		}

		/// <summary>
		/// Gets the status of the task - whether it is being executed.
		/// </summary>
		public bool Executing { get; private set; }

		/// <summary>
		/// Gets whether this task is currently queued to run. This is true only
		/// if the queue it is in is an explicit request, i.e will run when the
		/// executor is idle.
		/// </summary>
		public bool Queued
		{
			get
			{
				if (Executor == null)
					throw new InvalidOperationException();

				return Executor.IsTaskQueued(this);
			}
		}

		/// <summary>
		/// Gets whether the task has been cancelled from execution.
		/// </summary>
		public bool Canceled
		{
			get;
			private set;
		}

		/// <summary>
		/// Cancels the task from running, or, if the task is queued for running,
		/// removes the task from the queue.
		/// </summary>
		public void Cancel()
		{
			Executor.UnqueueTask(this);
			Canceled = true;
		}

		/// <summary>
		/// Executes the task in the context of the calling thread.
		/// </summary>
		public void Execute()
		{
			OnTaskStarted();
			Executing = true;
			Canceled = false;
			Progress = new SteppedProgressManager();

			try
			{
				//Run the task
				foreach (IErasureTarget target in Targets)
					try
					{
						Progress.Steps.Add(new ErasureTargetProgressManagerStep(
							target, Targets.Count));
						target.Execute();
					}
					catch (FatalException)
					{
						throw;
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (SharingViolationException)
					{
					}
			}
			catch (FatalException e)
			{
				Logger.Log(e.Message, LogLevel.Fatal);
			}
			catch (OperationCanceledException e)
			{
				Logger.Log(e.Message, LogLevel.Fatal);
			}
			catch (SharingViolationException)
			{
			}
			finally
			{
				//If the task is a recurring task, reschedule it since we are done.
				if (Schedule is RecurringSchedule)
				{
					((RecurringSchedule)Schedule).Reschedule(DateTime.Now);
				}

				//If the task is an execute on restart task or run immediately task, it is
				//only run once and can now be restored to a manually run task
				if (Schedule == Schedule.RunOnRestart || Schedule == Schedule.RunNow)
					Schedule = Schedule.RunManually;

				Progress = null;
				Executing = false;
				OnTaskFinished();
			}
		}

		private Executor executor;
		private Schedule schedule;
		private SteppedProgressManager progress;

		#region Events
		/// <summary>
		/// The task has been edited.
		/// </summary>
		public EventHandler TaskEdited { get; set; }

		/// <summary>
		/// The start of the execution of a task.
		/// </summary>
		public EventHandler TaskStarted { get; set; }

		/// <summary>
		/// The completion of the execution of a task.
		/// </summary>
		public EventHandler TaskFinished { get; set; }

		/// <summary>
		/// Broadcasts the task edited event.
		/// </summary>
		internal void OnTaskEdited()
		{
			if (TaskEdited != null)
				TaskEdited(this, EventArgs.Empty);
		}

		/// <summary>
		/// Broadcasts the task execution start event.
		/// </summary>
		private void OnTaskStarted()
		{
			if (TaskStarted != null)
				TaskStarted(this, EventArgs.Empty);
		}

		/// <summary>
		/// Broadcasts the task execution completion event.
		/// </summary>
		private void OnTaskFinished()
		{
			if (TaskFinished != null)
				TaskFinished(this, EventArgs.Empty);
		}
		#endregion
	}

	/// <summary>
	/// Returns the progress of an erasure target, since that comprises the
	/// steps of the Task Progress.
	/// </summary>
	public class ErasureTargetProgressManagerStep : SteppedProgressManagerStepBase
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="target">The erasure target represented by this object.</param>
		/// <param name="steps">The number of targets in the task.</param>
		public ErasureTargetProgressManagerStep(IErasureTarget target, int targets)
			: base(1.0f / targets)
		{
			Target = target;
		}

		public override ProgressManagerBase Progress
		{
			get
			{
				return Target.Progress;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// The erasure target represented by this step.
		/// </summary>
		public IErasureTarget Target
		{
			get;
			private set;
		}
	}

	/// <summary>
	/// A base event class for all event arguments involving a task.
	/// </summary>
	public class TaskEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="task">The task being referred to by this event.</param>
		public TaskEventArgs(Task task)
		{
			Task = task;
		}

		/// <summary>
		/// The executing task.
		/// </summary>
		public Task Task { get; private set; }
	}
}