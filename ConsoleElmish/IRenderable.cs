using System;

namespace ConsoleElmish
{
	public interface IRenderable : IEquatable<IRenderable>, IDisposable
	{
		internal event Action ReRender;

		public Buffer Render(uint height, uint width);
	}
}