using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleElmish
{
	internal class RenderableTree : IEnumerable<(Area area, IRenderable renderable)>
	{
		public IRenderable Renderable { get; }
		public Area Area { get; }
		private readonly IList<RenderableTree> children;

		public RenderableTree(IRenderable renderable, Area area)
		{
			Renderable = renderable ?? throw new ArgumentNullException(nameof(renderable));
			Area = area;
			children = new List<RenderableTree>();
		}

		public RenderableTree Add(IRenderable child, Area area)
		{
			RenderableTree tree = new RenderableTree(child, area);
			children.Add(tree);
			return tree;
		}

		public RenderableTree Remove(IRenderable child)
		{
			RenderableTree tree = children.First(t => t.Renderable == child);
			children.Remove(tree);
			return tree;
		}

		public IEnumerator<(Area, IRenderable)> GetEnumerator()
		{
			yield return (Area, Renderable);

			foreach (RenderableTree child in children)
			{
				foreach ((Area, IRenderable) item in child)
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}