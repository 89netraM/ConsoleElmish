using ConsoleElmish;
using Buffer = ConsoleElmish.Buffer;
using System;

namespace ConsoleApp
{
	public class BorderComponent : Component<EmptyState>
	{
		public IRenderable Child { get; }

		public BorderComponent(IRenderable child)
		{
			Child = child ?? throw new ArgumentNullException(nameof(child));
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
				{ new Area(1, 1, height - 2, width - 2), new ColoredItem<IRenderable>(Child) }
			};
		}
	}
}