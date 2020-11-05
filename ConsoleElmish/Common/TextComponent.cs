using System;
using System.Text;

namespace ConsoleElmish.Common
{
	public class TextComponent : Component<EmptyState>
	{
		public string Text { get; }
		public Alignment TextAlignment { get; }

		public TextComponent(string text) : this(text, false) { }
		public TextComponent(string text, bool isCentered) :
			this(text, isCentered ? Alignment.Center : Alignment.Left) { }
		public TextComponent(string text, Alignment textAlignment)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
			TextAlignment = textAlignment;
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

				buffer.Add(new Area(r, LeftPadding((uint)sb.Length, width), 1, (uint)sb.Length), sb.ToString());
			}

			if (w != words.Length)
			{
				buffer.Add(new Area(height - 1, width - 3, 1, 3), new ColoredItem<string>("...", ConsoleColor.Red));
			}

			return buffer;
		}

		private uint LeftPadding(uint textWidth, uint totalWidth) => TextAlignment switch
		{
			Alignment.Left => 0,
			Alignment.Center => (totalWidth - textWidth) / 2,
			Alignment.Right => totalWidth - textWidth,
			_ => throw new ArgumentException("Unssuported alignment enum"),
		};

		public enum Alignment
		{
			Left,
			Center,
			Right
		}
	}
}