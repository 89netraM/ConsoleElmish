using System;

namespace ConsoleElmish
{
	public abstract class Component<P, S> : IRenderable
	{
		internal event Action<IRenderable> ReRender;
		event Action<IRenderable> IRenderable.ReRender
		{
			add { ReRender += value; }
			remove { ReRender -= value; }
		}

		protected internal P Properties { get; }
		private S state;
		protected internal S State
		{
			get => state;
			set
			{
				state = value;
				ReRender?.Invoke(this);
			}
		}

		protected Component(P properties, S state)
		{
			Properties = properties;
			this.state = state;
		}
		protected Component(P properties) : this(properties, default) { }
		protected Component(S state) : this(default, state) { }
		protected Component() : this(default, default) { }

		public abstract void Render(IConsole console, uint height, uint width);
	}
}