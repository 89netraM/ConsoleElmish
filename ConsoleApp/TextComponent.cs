using ConsoleElmish;
using Buffer = ConsoleElmish.Buffer;
using System;
using System.Text;

namespace ConsoleApp
{
	public class TextComponent : Component<EmptyState>
	{
		private static string Center(string text, int width)
		{
			return text.PadLeft((width - text.Length) / 2 + text.Length).PadRight(width);
		}

		public string Text { get; }
		public bool IsCentered { get; }

		public TextComponent(string text, bool isCentered = false)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
			IsCentered = isCentered;
		}

		public override Buffer Render(uint height, uint width)
		{
			Buffer buffer = new Buffer();

			string[] words = Text.Split();
			int w = 0;
			StringBuilder sb = new StringBuilder((int)width);

			for (uint r = 0; r < height && w < words.Length; r++)
			{
				sb.Clear();
				sb.Append(words[w++]);
				while (w < words.Length && sb.Length + 1 + words[w].Length < width)
				{
					sb.Append(' ');
					sb.Append(words[w++]);
				}

				if (IsCentered)
				{
					buffer.Add(new Area(r, 0, 1, width), Center(sb.ToString(), (int)width));
				}
				else
				{
					buffer.Add(new Area(r, 0, 1, (uint)sb.Length), sb.ToString());
				}
			}

			if (w != words.Length)
			{
				buffer.Add(new Area(height - 1, width - 3, 1, 3), new ColoredItem<string>("...", ConsoleColor.Red));
			}

			return buffer;
		}
	}
}