using ConsoleElmish;
using System;
using System.Text;

namespace ConsoleApp
{
	public class TextComponent : Component<TextProperties, TextState>
	{
		public TextComponent(string text) : base(new TextProperties(text)) { }

		public override void Render(IConsole console, uint height, uint width)
		{
			string[] words = Properties.Text.Split();
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
				console.Draw(r, 0, sb.ToString());
			}

			if (w != words.Length)
			{
				console.Draw(height - 1, width - 3, "...", ConsoleColor.Red);
			}
		}
	}

	public readonly struct TextProperties
	{
		public string Text { get; }

		public TextProperties(string text)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
		}
	}
	public readonly struct TextState { }
}