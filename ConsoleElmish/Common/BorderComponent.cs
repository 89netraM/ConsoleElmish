using System;
using System.Collections.Generic;

namespace ConsoleElmish.Common
{
	public class BorderComponent : Component<EmptyState>
	{
		private static readonly IReadOnlyDictionary<Style, IReadOnlyDictionary<Line, char>> lineStyleMap = new Dictionary<Style, IReadOnlyDictionary<Line, char>>
		{
			{
				Style.Thin,
				new Dictionary<Line, char>
				{
					{ Line.CornerNW, '┌' },
					{ Line.CornerNE, '┐' },
					{ Line.CornerSW, '└' },
					{ Line.CornerSE, '┘' },
					{ Line.Horizontal, '─' },
					{ Line.Vertical, '│'}
				}
			},
			{
				Style.Thick,
				new Dictionary<Line, char>
				{
					{ Line.CornerNW, '┏' },
					{ Line.CornerNE, '┓' },
					{ Line.CornerSW, '┗' },
					{ Line.CornerSE, '┛' },
					{ Line.Horizontal, '━' },
					{ Line.Vertical, '┃'}
				}
			},
			{
				Style.Double,
				new Dictionary<Line, char>
				{
					{ Line.CornerNW, '╔' },
					{ Line.CornerNE, '╗' },
					{ Line.CornerSW, '╚' },
					{ Line.CornerSE, '╝' },
					{ Line.Horizontal, '═' },
					{ Line.Vertical, '║'}
				}
			},
			{
				Style.ASCII,
				new Dictionary<Line, char>
				{
					{ Line.CornerNW, '+' },
					{ Line.CornerNE, '+' },
					{ Line.CornerSW, '+' },
					{ Line.CornerSE, '+' },
					{ Line.Horizontal, '-' },
					{ Line.Vertical, '|'}
				}
			}
		};

		public IRenderable Child { get; }
		public Style BorderStyle { get; }

		public BorderComponent(IRenderable child, Style borderStyle = Style.Double)
		{
			Child = child ?? throw new ArgumentNullException(nameof(child));
			BorderStyle = borderStyle;
		}

		public override Buffer Render(uint height, uint width)
		{
			return new Buffer
			{
				{ new Area(0, 0, 1, 1), lineStyleMap[BorderStyle][Line.CornerNW] },
				{ new Area(0, 1, 1, width - 2), lineStyleMap[BorderStyle][Line.Horizontal] },
				{ new Area(0, width - 1, 1, 1), lineStyleMap[BorderStyle][Line.CornerNE] },
				{ new Area(1, 0, height - 2, 1), lineStyleMap[BorderStyle][Line.Vertical] },
				{ new Area(1, width - 1, height - 2, 1), lineStyleMap[BorderStyle][Line.Vertical] },
				{ new Area(height - 1, 0, 1, 1), lineStyleMap[BorderStyle][Line.CornerSW] },
				{ new Area(height - 1, 1, 1, width - 2), lineStyleMap[BorderStyle][Line.Horizontal] },
				{ new Area(height - 1, width - 1, 1, 1), lineStyleMap[BorderStyle][Line.CornerSE] },
				{ new Area(1, 1, height - 2, width - 2), new ColoredItem<IRenderable>(Child) }
			};
		}

		public enum Style
		{
			Thin,
			Thick,
			Double,
			ASCII
		}

		private enum Line
		{
			CornerNW,
			CornerNE,
			CornerSW,
			CornerSE,
			Horizontal,
			Vertical
		}
	}
}