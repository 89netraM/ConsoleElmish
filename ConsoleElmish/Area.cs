namespace ConsoleElmish
{
	public readonly struct Area
	{
		public uint Row { get; }
		public uint Column { get; }
		public uint Height { get; }
		public uint Width { get; }

		public Area(uint row, uint column, uint height, uint width)
		{
			Row = row;
			Column = column;
			Height = height;
			Width = width;
		}
	}
}