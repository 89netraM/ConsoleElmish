using System;
using System.Collections.Generic;
using System.Text;
using SConsole = System.Console;

namespace ConsoleElmish
{
	public class Renderer
	{
		private uint startRow;
		public uint Height { get; }
		public uint Width { get; }

		public ConsoleColor DefaultColor { get; }

		private readonly IDictionary<(uint row, uint column), (char character, ConsoleColor color)> buffer = new Dictionary<(uint, uint), (char, ConsoleColor)>();

		public Renderer(uint? height = null, uint? width = null, ConsoleColor? defaultColor = null)
		{
			Height = height ?? (uint)SConsole.WindowHeight;
			Width = width ?? (uint)SConsole.WindowWidth;
			DefaultColor = defaultColor ?? SConsole.ForegroundColor;
		}

		public void Render(IRenderable main)
		{
			if (SConsole.CursorLeft != 0)
			{
				SConsole.WriteLine();
			}

			startRow = (uint)SConsole.CursorTop;
			Render(main, startRow, 0, Height, Width);
		}
		private void Render(IRenderable main, uint startRow, uint startColumn, uint height, uint width)
		{
			Console console = new Console(height, width, DefaultColor);
			console.Draw(0, 0, height, width, main);

			foreach (var (area, renderable) in console.Renderables)
			{
				renderable.ReRender += Renderable_ReRender(area);
			}

			StringBuilder sb = new StringBuilder((int)width);
			uint column;
			ConsoleColor? previousColor;
			for (uint r = 0; r < height; r++)
			{
				sb.Clear();
				column = startColumn;
				previousColor = null;
				for (uint c = 0; c < width; c++)
				{
					(char, ConsoleColor)? old = buffer.TryGetValue((r, c), out var o) ? ((char, ConsoleColor)?)o : null;
					(char character, ConsoleColor color)? current = console.Buffer.TryGetValue((r, c), out var curr) ? ((char, ConsoleColor)?)curr : null;

					if (current.HasValue)
					{
						buffer[(r, c)] = current.Value;
					}
					else
					{
						buffer.Remove((r, c));
					}

					if (old == current)
					{
						PrintSB();
						column = c + 1;
					}
					else if (current.HasValue && current.Value.color != previousColor)
					{
						PrintSB();
						column = c;
						sb.Append(current.Value.character);
						previousColor = current.Value.color;
					}
					else
					{
						sb.Append(current.HasValue ? current.Value.character : ' ');
					}
				}
				PrintSB();

				void PrintSB()
				{
					if (sb.Length > 0)
					{
						if (SConsole.CursorTop != (int)(startRow + r))
						{
							SConsole.CursorTop = (int)(startRow + r);
						}
						if (SConsole.CursorLeft != (int)column)
						{
							SConsole.CursorLeft = (int)column;
						}
						if (previousColor.HasValue)
						{
							SConsole.ForegroundColor = previousColor.Value;
						}
						SConsole.Write(sb.ToString());
						sb.Clear();
					}
				}
			}
		}

		private Action<IRenderable> Renderable_ReRender(Area area)
		{
			return renderable =>
			{
				Render(renderable, startRow + area.Row, area.Column, area.Height, area.Width);
			};
		}
	}
}