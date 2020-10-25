using ConsoleElmish;

namespace ConsoleApp
{
	public class BorderComponent<P, S> : Component<BorderProperties<P, S>, BorderState>
	{
		public BorderComponent(Component<P, S> child) : base(new BorderProperties<P, S>(child)) { }

		public override void Render(IConsole console, uint height, uint width)
		{
			console.Draw(0, 0, '╔');
			console.Draw(0, width - 1, '╗');
			console.Draw(height - 1, 0, '╚');
			console.Draw(height - 1, width - 1, '╝');
			for (uint c = 1; c < width - 1; c++)
			{
				console.Draw(0, c, '═');
				console.Draw(height - 1, c, '═');
			}
			for (uint r = 1; r < height - 1; r++)
			{
				console.Draw(r, 0, '║');
				console.Draw(r, width - 1, '║');
			}

			if (!(Properties.Child is null))
			{
				console.Draw(1, 1, height - 2, width - 2, Properties.Child);
			}
		}

	}
	public readonly struct BorderProperties<P, S>
	{
		public Component<P, S> Child { get; }

		public BorderProperties(Component<P, S> child)
		{
			Child = child;
		}
	}
	public readonly struct BorderState { }
}