using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OnZed.Utils
{
	internal class TaskManager
	{
		internal class DelayedTask
		{
			internal Action Callback;

			internal long ExecTime;

			internal DelayedTask(Action callback, long execTime)
			{
				Callback = callback;
				ExecTime = execTime;
			}
		}

		private static List<Action> SimpleTasks;

		private static List<DelayedTask> DelayedTasks;

		internal static void Initialize()
		{
			SimpleTasks = new List<Action>();
			DelayedTasks = new List<DelayedTask>();
		}

		internal static void Run(Action task, long time = 0L)
		{
			if (time == 0L)
			{
				lock (SimpleTasks)
				{
					SimpleTasks.Add(task);
				}
				return;
			}
			DelayedTask item = new DelayedTask(task, time * (Stopwatch.Frequency / 1000) + Stopwatch.GetTimestamp());
			lock (DelayedTasks)
			{
				DelayedTasks.Add(item);
			}
		}

		private static void PulseSimple()
		{
			List<Action> simpleTasks;
			lock (SimpleTasks)
			{
				if (!SimpleTasks.Any())
				{
					return;
				}
				simpleTasks = SimpleTasks;
				SimpleTasks = new List<Action>();
			}
			simpleTasks.ForEach(delegate (Action task)
			{
				task();
			});
		}

		private static void PulseDelayed()
		{
			long timestamp = Stopwatch.GetTimestamp();
			lock (DelayedTasks)
			{
				if (DelayedTasks.Any())
				{
					int num = 0;
					while (num < DelayedTasks.Count)
					{
						DelayedTask delayedTask = DelayedTasks[num];
						if (timestamp >= delayedTask.ExecTime)
						{
							delayedTask.Callback();
							DelayedTasks.RemoveAt(num);
						}
						else
						{
							num++;
						}
					}
				}
			}
		}

		internal static void Pulse()
		{
			PulseSimple();
			PulseDelayed();
		}
	}
}
