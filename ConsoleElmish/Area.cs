using System.Collections;
using System.Collections.Generic;

namespace ConsoleElmish
{
	public readonly struct Area : IEnumerable<(uint row, uint column)>
	{
		public uint Row { get; }
		public uint Column { get; }
		public uint Height { get; }
		public uint Width { get; }

		public uint Size => Height * Width;

		public Area(uint row, uint column, uint height, uint width)
		{
			Row = row;
			Column = column;
			Height = height;
			Width = width;
		}

		public bool IsOverlapping(Area other)
		{
			return !((this.Row + this.Height <= other.Row || other.Row + other.Height <= this.Row) ||
				(this.Column + this.Width <= other.Column || other.Column + other.Width <= this.Column));
		}

		public IEnumerator<(uint, uint)> GetEnumerator()
		{
			for (uint r = 0; r < Height; r++)
			{
				for (uint c = 0; c < Width; c++)
				{
					yield return (Row + r, Column + c);
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}