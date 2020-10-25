using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleElmish
{
	public static class Input
	{
		public static event Action<ConsoleKeyInfo> KeyDown;

		private static readonly object key = new object();

		private static bool isRunning = false;
		private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		public static void Start()
		{
			lock (key)
			{
				if (!isRunning)
				{
					Task.Run(Loop, cancellationTokenSource.Token);
					isRunning = true;
				}
			}
		}

		private static void Loop()
		{
			while (true)
			{
				ConsoleKeyInfo info = Console.ReadKey(true);
				KeyDown?.Invoke(info);
			}
		}

		public static void Stop()
		{
			lock (key)
			{
				if (isRunning)
				{
					cancellationTokenSource.Cancel(true);
					isRunning = false;
				}
			}
		}
	}
}