using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleElmish
{
	public class Buffer : IEnumerable<((uint row, uint column) position, ColoredItem<char> item)>, IDisposable
	{
		public event Action<Area> RePrint;

		private readonly IDictionary<(uint row, uint column), ColoredItem<char>> buffer = new Dictionary<(uint, uint), ColoredItem<char>>();

		public ColoredItem<char>? this[(uint row, uint column) position]
		{
			get => buffer.TryGetValue(position, out var curr) ? (ColoredItem<char>?)curr : null;
			set
			{
				if (value.HasValue)
				{
					buffer[position] = value.Value;
				}
				else
				{
					buffer.Remove(position);
				}
			}
		}

		private readonly IList<(IRenderable item, Buffer buffer)> disposables = new List<(IRenderable, Buffer)>();

		public void Add(Area area, ColoredItem<char> item)
		{
			foreach (var position in area)
			{
				this[position] = item;
			}
		}
		public void Add(Area area, ColoredItem<string> item)
		{
			int i = 0;
			foreach (var position in area)
			{
				this[position] = item.WithItem(item.Item[i]);
				i = (i + 1) % item.Item.Length;
			}
		}
		public void Add(Area area, ColoredItem<IRenderable> item)
		{
			Buffer innerBuffer = item.Item.Render(area.Height, area.Width);
			CopyFromBuffer(innerBuffer, area, new Area(0, 0, area.Height, area.Width), item.Foreground, item.Background);

			disposables.Add((item.Item, innerBuffer));

			innerBuffer.RePrint += InnerBuffer_RePrint;
			item.Item.ReRender += Item_ReRender;

			void InnerBuffer_RePrint(Area changedArea)
			{
				CopyFromBuffer(innerBuffer, area, changedArea, item.Foreground, item.Background);
				RePrint?.Invoke(new Area(area.Row + changedArea.Row, area.Column + changedArea.Column, changedArea.Height, changedArea.Width));
			}
			void Item_ReRender()
			{
				item.Item.ReRender -= Item_ReRender;
				innerBuffer.RePrint -= InnerBuffer_RePrint;

				disposables.Remove((item.Item, innerBuffer));
				innerBuffer.Dispose();

				Add(area, item);
				RePrint?.Invoke(area);
			}
		}

		private void CopyFromBuffer(Buffer innerBuffer, Area area, Area subArea, ConsoleColor? foregorund, ConsoleColor? background)
		{
			foreach (var (row, column) in subArea.Offsets)
			{
				this[(area.Row + subArea.Row + row, area.Column + subArea.Column + column)] =
					innerBuffer[(subArea.Row + row, subArea.Column + column)]?
						.WithDefaultColors(foregorund, background);
			}
		}

		public IEnumerator<((uint row, uint column) position, ColoredItem<char> item)> GetEnumerator()
		{
			return buffer.Select(kvp => (kvp.Key, kvp.Value)).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			foreach (var (item, innerBuffer) in disposables)
			{
				item.Dispose();
				innerBuffer.Dispose();
			}
		}
	}
}