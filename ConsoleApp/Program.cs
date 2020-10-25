using ConsoleElmish;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
	public static class Program
	{
		public static Task Main()
		{
			Console.CursorVisible = false;
			Console.CancelKeyPress += (s, e) =>
			{
				Console.CursorVisible = true;
				Environment.Exit(0);
			};

			Renderer renderer = new Renderer(4, 40);
			renderer.Render(new ClockComponent());

			return Task.Delay(-1);
		}
	}
}