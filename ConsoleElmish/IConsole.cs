using System;

namespace ConsoleElmish
{
	public interface IConsole
	{
		public void Draw(uint row, uint column, char c);
		public void Draw(uint row, uint column, char c, ConsoleColor color);
		public void Draw(uint row, uint column, string s);
		public void Draw(uint row, uint column, string s, ConsoleColor color);
		public void Draw(uint row, uint column, uint height, uint width, IRenderable renderable);
		public void Draw(uint row, uint column, uint height, uint width, IRenderable renderable, ConsoleColor color);
	}
}