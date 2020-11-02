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
			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;
			Console.CancelKeyPress += (s, e) =>
			{
				Console.CursorVisible = true;
				Environment.Exit(0);
			};

			Renderer.Create(4, 40)
				.Render(new ClockComponent());

			return Task.Delay(-1);
		}
	}
}