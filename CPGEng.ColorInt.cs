/*
 * Crispycat PixelGraphic Engine
 * CPGEng.ColorInt; ColorInt objects and functions
 * (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2020/01/27
*/

using System;

namespace CPGEng {
	public class ColorInt {
		public readonly int Value, Red, Green, Blue;

		/// <summary>Creates a ColorInt from an integer.</summary>
		/// <param name="v">Value</param>
		public ColorInt(int v) {
			Value = v & 16777215;
			Red = v & 255;
			Green = v >> 8 & 255;
			Blue = v >> 16 & 255;
		}

		/// <summary>Creates a ColorInt from BGR or RGB values.</summary>
		/// <param name="b">BGR Blue/RGB Red</param>
		/// <param name="g">Green</param>
		/// <param name="r">BGR Red/RGB Blue</param>
		/// <param name="c">Color format, defaults to BGR</param>
		public ColorInt(int b, int g, int r, ColorFormat c = 0) {
			if (c == ColorFormat.RGB) {
				int _ = b;
				b = r;
				r = _;
			}

			Red = r & 255;
			Green = g & 255;
			Blue = b & 255;
			Value = (Blue << 16) | (Green << 8) | Red;
		}
	}
}