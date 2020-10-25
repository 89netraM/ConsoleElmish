using System;

namespace ConsoleElmish
{
	public interface IRenderable
	{
		internal event Action<IRenderable> ReRender;

		public void Render(IConsole console, uint height, uint width);
	}
}