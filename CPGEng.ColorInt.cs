/*
 * Crispycat PixelGraphic Engine
 * CPGEng.ColorInt; ColorInt objects and functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/09/29
*/

using System;

namespace CPGEng {
	public class ColorInt {
		public readonly int Value, Red, Green, Blue;

		public ColorInt(int v) {
			Value = v;
			Red = v & 255;
			Green = v >> 8 & 255;
			Blue = v >> 16 & 255;
		}

		public ColorInt(int b, int g, int r, ColorFormat c = 0) {
			if (c == ColorFormat.RGB) {
				int _ = b;
				b = r;
				r = _;
			}

			Red = r;
			Green = g;
			Blue = b;
			Value = (b << 16) | (g << 8) | r;
		}

		public ColorInt ToGrayscale() {
			int x = (int)Math.Round((Red + Green + Blue) / 3d);
			return new ColorInt((x << 16) | (x << 8) | x);
		}
	}
}