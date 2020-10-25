using System;
using System.Collections.Generic;

namespace ConsoleElmish
{
	internal class Console : IConsole
	{
		public IDictionary<(uint row, uint column), (char c, ConsoleColor color)> Buffer { get; } = new Dictionary<(uint, uint), (char, ConsoleColor)>();
		public RenderableTree Renderables { get; private set; } = null;
		private Stack<RenderableTree> trees { get; }

		private Stack<Area> areas { get; }

		private Stack<ConsoleColor> colors { get; }

		public Console(uint height, uint width, ConsoleColor defaultColor)
		{
			trees = new Stack<RenderableTree>();

			areas = new Stack<Area>();
			areas.Push(new Area(0, 0, height, width));

			colors = new Stack<ConsoleColor>();
			colors.Push(defaultColor);
		}

		public void Draw(uint row, uint column, char c) => Draw(row, column, c, colors.Peek());
		public void Draw(uint row, uint column, char c, ConsoleColor color)
		{
			Area area = areas.Peek();
			if (!(row < area.Height))
			{
				throw new ArgumentException("Out of bounds", nameof(row));
			}
			if (!(column < area.Width))
			{
				throw new ArgumentException("Out of bounds", nameof(column));
			}

			Buffer[(area.Row + row, area.Column + column)] = (c, color);
		}

		public void Draw(uint row, uint column, string s) => Draw(row, column, s, colors.Peek());
		public void Draw(uint row, uint column, string s, ConsoleColor color)
		{
			Area area = areas.Peek();
			if (!(row < area.Height))
			{
				throw new ArgumentException("Out of bounds", nameof(row));
			}
			if (!(column + s.Length <= area.Width))
			{
				throw new ArgumentException("Out of bounds", nameof(column));
			}

			for (int i = 0; i < s.Length; i++)
			{
				Buffer[(area.Row + row, area.Column + column + (uint)i)] = (s[i], color);
			}
		}

		public void Draw(uint row, uint column, uint height, uint width, IRenderable renderable) => Draw(row, column, height, width, renderable, colors.Peek());
		public void Draw(uint row, uint column, uint height, uint width, IRenderable renderable, ConsoleColor color)
		{
			Area area = areas.Peek();
			if (!(row + height <= area.Height))
			{
				throw new ArgumentException("Out of bounds", nameof(row));
			}
			if (!(column + width <= area.Width))
			{
				throw new ArgumentException("Out of bounds", nameof(column));
			}

			Area innerArea = new Area(area.Row + row, area.Column + column, height, width);
			RenderableTree bottom;
			if (trees.Count == 0)
			{
				bottom = Renderables = new RenderableTree(renderable, innerArea);
			}
			else
			{
				bottom = trees.Peek().Add(renderable, innerArea);
			}

			trees.Push(bottom);
			areas.Push(innerArea);
			colors.Push(color);
			renderable.Render(this, height, width);
			colors.Pop();
			areas.Pop();
			trees.Pop();
		}
	}
}