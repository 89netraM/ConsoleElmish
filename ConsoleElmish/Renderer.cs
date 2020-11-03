using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

		private readonly SemaphoreSlim printingRights = new SemaphoreSlim(1);
		private int waitingToPrint = 0;

		private Renderer(uint? height, uint? width, ConsoleColor? defaultForeground)
		{
			Height = height ?? (uint)SConsole.WindowHeight;
			Width = width ?? (uint)SConsole.WindowWidth;
			DefaultForeground = defaultForeground ?? SConsole.ForegroundColor;
		}

		public void Render(ColoredItem<IRenderable> main)
		{
			if (SConsole.CursorLeft != 0)
			{
				SConsole.WriteLine();
			}

			if (SConsole.BufferHeight < Height)
			{
				SConsole.BufferHeight = (int)Height;
				startRow = 0;
			}
			else if (SConsole.BufferHeight - SConsole.CursorTop < Height)
			{
				SConsole.Write(new String('\n', (int)(Height - 1)));
				startRow = (uint)SConsole.BufferHeight - Height;
			}
			else
			{
				startRow = (uint)SConsole.CursorTop;
			}

			mainBuffer = new Buffer
			{
				{ new Area(0, 0, Height, Width), main }
			};
			mainBuffer.RePrint += Print;

			Print();

		}

		private void Print()
		{
			try
			{
				Interlocked.Increment(ref waitingToPrint);
				printingRights.Wait();
				Interlocked.Decrement(ref waitingToPrint);
				if (waitingToPrint > 0)
				{
					return;
				}

				StringBuilder sb = new StringBuilder((int)Width);
				uint column;
				(ConsoleColor? foreground, ConsoleColor? background) previousColor;
				for (uint r = 0; r < Height; r++)
				{
					sb.Clear();
					column = 0;
					previousColor = default;
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
						else if (current.HasValue && (current.Value.Foreground != previousColor.foreground || current.Value.Background != previousColor.background))
						{
							PrintSB();
							column = c;
							sb.Append(current.Value.Item);
							previousColor = (current.Value.Foreground, current.Value.Background);
						}
						else
						{
							sb.Append(current.HasValue ? current.Value.Item : ' ');
						}
					}
					PrintSB();

					if (waitingToPrint > 0)
					{
						return;
					}

					void PrintSB()
					{
						if (sb.Length > 0)
						{
							if (SConsole.CursorTop != (int)(startRow + r) || SConsole.CursorLeft != (int)column)
							{
								SConsole.SetCursorPosition((int)column, (int)(startRow + r));
							}
							if (!previousColor.foreground.HasValue || !previousColor.background.HasValue)
							{
								SConsole.ResetColor();
							}
							if (previousColor.foreground.HasValue)
							{
								SConsole.ForegroundColor = previousColor.foreground.Value;
							}
							if (previousColor.background.HasValue)
							{
								SConsole.BackgroundColor = previousColor.background.Value;
							}
							SConsole.Write(sb.ToString());
							sb.Clear();
							previousColor = default;
						}
					}
				}
			}
			finally
			{
				SConsole.ResetColor();
				printingRights.Release();
			}
		}

		public void Stop()
		{
			mainBuffer.RePrint -= Print;

			SConsole.SetCursorPosition((int)(Width - 1), (int)(startRow + Height - 1));
			SConsole.WriteLine();
		}
	}
}