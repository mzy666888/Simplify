﻿using System.Diagnostics;
using System.Threading;

namespace Simplify.WindowsServices.IntegrationTests
{
	public class TaskProcessor4
	{
		public void Run()
		{
			Trace.WriteLine("--- TaskProcessor4 launched");

			Thread.Sleep(190000);
		}
	}
}