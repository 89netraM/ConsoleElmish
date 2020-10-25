using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleElmish
{
	public class Buffer : IEnumerable<((uint row, uint column) position, ColoredItem<char> item)>
	{
		public event Action RePrint;

		private readonly IDictionary<(uint row, uint column), ColoredItem<char>> buffer = new Dictionary<(uint, uint), ColoredItem<char>>();

		public ColoredItem<char>? this[(uint, uint) position] => buffer.TryGetValue(position, out var curr) ? (ColoredItem<char>?)curr : null;

		public void Add(Area area, ColoredItem<char> item)
		{
			foreach (var position in area)
			{
				buffer[position] = item;
			}
		}
		public void Add(Area area, ColoredItem<string> item)
		{
			int i = 0;
			foreach (var position in area)
			{
				buffer[position] = item.WithItem(item.Item[i]);
				i = (i + 1) % item.Item.Length;
			}
		}
		public void Add(Area area, ColoredItem<IRenderable> item)
		{
			Buffer innerBuffer = item.Item.Render(area.Height, area.Width);
			CopyFromBuffer(innerBuffer, area.Row, area.Column, item.Foreground, item.Background);

			innerBuffer.RePrint += InnerBuffer_RePrint;
			item.Item.ReRender += Item_ReRender;

			void InnerBuffer_RePrint()
			{
				CopyFromBuffer(innerBuffer, area.Row, area.Column, item.Foreground, item.Background);
				RePrint?.Invoke();
			}
			void Item_ReRender()
			{
				item.Item.ReRender -= Item_ReRender;
				innerBuffer.RePrint -= InnerBuffer_RePrint;

				Add(area, item);
				RePrint?.Invoke();
			}
		}

		private void CopyFromBuffer(Buffer innerBuffer, uint row, uint column, ConsoleColor? foregorund, ConsoleColor? background)
		{
			foreach (var ((r, c), innerItem) in innerBuffer)
			{
				buffer[(row + r, column + c)] = innerItem.WithDefaultColors(foregorund, background);
			}
		}

		public IEnumerator<((uint, uint), ColoredItem<char>)> GetEnumerator()
		{
			return buffer.Select(kvp => (kvp.Key, kvp.Value)).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}