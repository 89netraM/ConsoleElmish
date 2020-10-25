using System;
using System.Collections.Generic;
using System.Text;
using SConsole = System.Console;

namespace ConsoleElmish
{
	public class Renderer
	{
		public static Renderer Instance { get; private set; } = null;
		public static Renderer Create(uint? height = null, uint? width = null, ConsoleColor? defaultForeground = null)
		{
			if (!(Instance is null))
			{
				throw new InvalidOperationException($"An {nameof(Instance)} already exists");
			}

			Instance = new Renderer(height, width, defaultForeground);
			return Instance;
		}

		public uint Height { get; }
		public uint Width { get; }

		public ConsoleColor DefaultForeground { get; }

		private uint startRow;
		private readonly IDictionary<(uint row, uint column), ColoredItem<char>> characterBuffer = new Dictionary<(uint, uint), ColoredItem<char>>();
		private Buffer mainBuffer;

		private Renderer(uint? height, uint? width, ConsoleColor? defaultForeground)
		{
			Height = height ?? (uint)SConsole.WindowHeight;
			Width = width ?? (uint)SConsole.WindowWidth;
			DefaultForeground = defaultForeground ?? SConsole.ForegroundColor;
		}

		public void Render(IRenderable main)
		{
			if (SConsole.CursorLeft != 0)
			{
				SConsole.WriteLine();
			}
			startRow = (uint)SConsole.CursorTop;

			mainBuffer = new Buffer
			{
				{ new Area(0, 0, Height, Width), new ColoredItem<IRenderable>(main) }
			};
			mainBuffer.RePrint += Print;
			Print();
		}

		private void Print()
		{
			StringBuilder sb = new StringBuilder((int)Width);
			uint column;
			ConsoleColor? previousColor;
			for (uint r = 0; r < Height; r++)
			{
				sb.Clear();
				column = 0;
				previousColor = null;
				for (uint c = 0; c < Width; c++)
				{
					ColoredItem<char>? old = characterBuffer.TryGetValue((r, c), out var o) ? (ColoredItem<char>?)o : null;
					ColoredItem<char>? current = mainBuffer[(r, c)];

					if (current.HasValue)
					{
						characterBuffer[(r, c)] = current.Value;
					}
					else
					{
						characterBuffer.Remove((r, c));
					}

					if (old == current)
					{
						PrintSB();
						column = c + 1;
					}
					else if (current.HasValue && current.Value.Foreground != previousColor)
					{
						PrintSB();
						column = c;
						sb.Append(current.Value.Item);
						previousColor = current.Value.Foreground;
					}
					else
					{
						sb.Append(current.HasValue ? current.Value.Item : ' ');
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
	}
}