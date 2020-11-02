using ConsoleElmish;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	public static class Program
	{
		public static Task Main()
		{
			Renderer renderer = Renderer.Create((uint)Console.WindowHeight, (uint)Console.WindowWidth);

			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;
			Console.CancelKeyPress += (s, e) =>
			{
				renderer.Stop();
				Console.CursorVisible = true;
				Environment.Exit(0);
			};

			renderer.Render(new ClockComponent());

			return Task.Delay(-1);
		}
	}
}