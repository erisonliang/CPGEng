/*
* Crispycat PixelGraphic Engine
* CPGEng.Effects.Palette.cs; Color Palette
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

using System.Collections.Generic;

namespace CPGEng.Effects {
	public class Palette {
		public List<ColorInt> Colors = new List<ColorInt>();

		public Palette() {}

		public Palette(List<ColorInt> colors) {
			Colors.AddRange(colors);
		}

		private int ColorDifference(ColorInt x, ColorInt y) {
			int r = y.Red - x.Red;
			int g = y.Green - x.Green;
			int b = y.Blue - x.Blue;
			return r * r + g * g + b * b;
		}

		/// <summary>Gets the closest color in the palette to the one specified.</summary>
		/// <param name="c">ColorInt</param>
		/// <returns>ColorInt</returns>
		public ColorInt ClosestColor(ColorInt c) {
			int min = 2147483647;
			ColorInt closest = new ColorInt(0);

			Colors.ForEach((ColorInt cc) => {
				int d = ColorDifference(c, cc);
				if (d < min) {
					closest = cc;
					min = d;
				}
			});
			
			return closest;
		}
	}
}
