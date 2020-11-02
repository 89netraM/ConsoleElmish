using System;

namespace ConsoleElmish
{
	public readonly struct ColoredItem<T> : IEquatable<ColoredItem<T>>
	{
		public readonly ConsoleColor? Background;
		public readonly ConsoleColor? Foreground;

		public readonly T Item;

		public ColoredItem(T item, ConsoleColor? foreground = null, ConsoleColor? background = null)
		{
			Item = item ?? throw new ArgumentNullException(nameof(item));
			Foreground = foreground;
			Background = background;
		}

		public ColoredItem<U> WithItem<U>(U newItem) where U : IEquatable<U>
			=> new ColoredItem<U>(newItem, Foreground, Background);

		public ColoredItem<T> WithDefaultColors(ConsoleColor? foreground, ConsoleColor? background)
			=> new ColoredItem<T>(Item, Foreground ?? foreground, Background ?? background);

		public override bool Equals(object obj)
		{
			return obj is ColoredItem<T> item && Equals(item);
		}

		public bool Equals(ColoredItem<T> other)
		{
			return Background == other.Background &&
				   Foreground == other.Foreground &&
				   Item.Equals(other.Item);
		}

		public override int GetHashCode()
		{
			int hashCode = -1167065357;
			hashCode = hashCode * -1521134295 + Background.GetHashCode();
			hashCode = hashCode * -1521134295 + Foreground.GetHashCode();
			hashCode = hashCode * -1521134295 + Item.GetHashCode();
			return hashCode;
		}

		public static implicit operator ColoredItem<T>(T item) => new ColoredItem<T>(item);

		public static bool operator ==(ColoredItem<T> left, ColoredItem<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ColoredItem<T> left, ColoredItem<T> right)
		{
			return !(left == right);
		}
	}

	public static class ColoredItemHelpers
	{
		public static ColoredItem<IRenderable> WithColors(this IRenderable item, ConsoleColor? foreground = null, ConsoleColor? background = null) =>
				new ColoredItem<IRenderable>(item, foreground, background);
		public static ColoredItem<string> WithColors(this string item, ConsoleColor? foreground = null, ConsoleColor? background = null) =>
				new ColoredItem<string>(item, foreground, background);
		public static ColoredItem<char> WithColors(this char item, ConsoleColor? foreground = null, ConsoleColor? background = null) =>
				new ColoredItem<char>(item, foreground, background);
	}
}