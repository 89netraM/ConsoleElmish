using System;

namespace ConsoleElmish
{
	public abstract class Component<T> : IRenderable
	{
		internal event Action ReRender;
		event Action IRenderable.ReRender
		{
			add { ReRender += value; }
			remove { ReRender -= value; }
		}

		private T state;
		protected internal T State
		{
			get => state;
			set
			{
				state = value;
				ReRender?.Invoke();
			}
		}

		protected Component(T state)
		{
			this.state = state;
		}
		protected Component() : this(default) { }

		public abstract Buffer Render(uint height, uint width);

		protected void ForceReRender() => ReRender?.Invoke();

		public bool Equals(IRenderable other)
		{
			return other is Component<T> otherComponent && this == otherComponent;
		}

		public virtual void Dispose() { }
	}

	public readonly struct EmptyState { }
}