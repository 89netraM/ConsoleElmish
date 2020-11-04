using Buffer = ConsoleElmish.Buffer;
using ConsoleElmish;
using ConsoleElmish.Common;

namespace ConsoleApp
{
	class DualComponent : Component<EmptyState>
	{
		public DualComponent() : base() { }

		public override Buffer Render(uint height, uint width)
		{
			return new Buffer
			{
				{ new Area(0, 0, 22, 22), new BorderComponent(new TextComponent("Hello World!", true)) },
				{ new Area(0, 21, 4, 12), new BorderComponent(new ClockComponent()) },
				{ new Area(0, 21, 1, 1), '╦' },
				{ new Area(3, 21, 1, 1), '╠' }
			};
		}
	}
}