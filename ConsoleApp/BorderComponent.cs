using ConsoleElmish;

namespace ConsoleApp
{
	public class BorderComponent<T> : Component<EmptyState>
	{
		public Component<T> Child { get; }

		public BorderComponent(Component<T> child)
		{
			Child = child ?? throw new System.ArgumentNullException(nameof(child));
		}

		public override Buffer Render(uint height, uint width)
		{
			return new Buffer
			{
				{ new Area(0, 0, 1, 1), '╔' },
				{ new Area(0, 1, 1, width - 2), '═' },
				{ new Area(0, width - 1, 1, 1), '╗' },
				{ new Area(1, 0, height - 2, 1), '║' },
				{ new Area(1, width - 1, height - 2, 1), '║' },
				{ new Area(height - 1, 0, 1, 1), '╚' },
				{ new Area(height - 1, 1, 1, width - 2), '═' },
				{ new Area(height - 1, width - 1, 1, 1), '╝' },
				{ new Area(1, 1, height - 2, width - 2), Child }
			};
		}
	}
}