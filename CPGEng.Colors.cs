/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Colors.cs; Colors library and color functions
 * (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2020/01/27
*/

using System;

namespace CPGEng {
	public static class Colors {
		public static ColorInt Maroon = new ColorInt(0, 0, 127);
		public static ColorInt Brown = new ColorInt(0, 63, 127);
		public static ColorInt Olive = new ColorInt(0, 127, 127);
		public static ColorInt DarkGreen = new ColorInt(0, 127, 0);
		public static ColorInt DarkCyan = new ColorInt(127, 127, 0);
		public static ColorInt DarkBlue = new ColorInt(127, 0, 0);
		public static ColorInt Aubergine = new ColorInt(127, 0, 63);
		public static ColorInt Plum = new ColorInt(127, 0, 127);

		public static ColorInt Red = new ColorInt(0, 0, 255);
		public static ColorInt Orange = new ColorInt(0, 127, 255);
		public static ColorInt Yellow = new ColorInt(0, 255, 255);
		public static ColorInt Lime = new ColorInt(0, 255, 127);
		public static ColorInt Green = new ColorInt(0, 255, 0);
		public static ColorInt Cyan = new ColorInt(255, 255, 0);
		public static ColorInt LightBlue = new ColorInt(255, 127, 127);
		public static ColorInt Blue = new ColorInt(255, 0, 0);
		public static ColorInt Purple = new ColorInt(255, 0, 127);
		public static ColorInt Magenta = new ColorInt(255, 0, 255);

		public static ColorInt Pink = new ColorInt(127, 127, 255);
		public static ColorInt LightOrange = new ColorInt(127, 191, 255);
		public static ColorInt Cream = new ColorInt(127, 255, 255);
		public static ColorInt PaleLime = new ColorInt(127, 255, 191);
		public static ColorInt LightGreen = new ColorInt(127, 255, 127);
		public static ColorInt LightCyan = new ColorInt(255, 255, 127);
		public static ColorInt SkyBlue = new ColorInt(255, 191, 191);
		public static ColorInt LighterBlue = new ColorInt(255, 63, 63);
		public static ColorInt LightPurple = new ColorInt(255, 63, 191);
		public static ColorInt LightMagenta = new ColorInt(255, 127, 255);

		public static ColorInt Black = new ColorInt(0, 0, 0);
		public static ColorInt AlmostBlack = new ColorInt(31, 31, 31);
		public static ColorInt DarkGray = new ColorInt(63, 63, 63);
		public static ColorInt DimGray = new ColorInt(95, 95, 95);
		public static ColorInt Gray = new ColorInt(127, 127, 127);
		public static ColorInt BrightGray = new ColorInt(159, 159, 159);
		public static ColorInt LightGray = new ColorInt(192, 192, 192);
		public static ColorInt White = new ColorInt(255, 255, 255);

		/// <summary>Returns a ColorInt from the HSL values specified.</summary>
		/// <param name="h">Hue (0-1)</param>
		/// <param name="s">Saturation (0-1)</param>
		/// <param name="l">Luminosity (0-1)</param>
		/// <returns>ColorInt</returns>
		public static ColorInt FromHSL(double h, double s, double l) {
			double calc(double z, double y, double x) {
				if (z < 0) z++;
				if (z > 1) z--;
				if (z < 1d / 6) return y + (x - y) * 6 * z;
				if (z < 0.5) return x;
				if (z < 2d / 3) return y + ((y - x) * (2d / 3 - z) * 6);
				return y;
			}

			if (s == 0) {
				return new ColorInt((int)Math.Round(l * 255), (int)Math.Round(l * 255), (int)Math.Round(l * 255));
			} else if (l == 0) {
				return new ColorInt(0);
			} else {
				double a, b;

				if (l < 0.5) b = l * (1 + s); else b = l + s - l * s;
				a = 2 * l - b;

				double yr = h + 1d / 3;
				double yg = h;
				double yb = h - 1d / 3;

				yr = calc(yr, a, b);
				yg = calc(yg, a, b);
				yb = calc(yb, a, b);

				return new ColorInt((int)Math.Floor(yb * 255), (int)Math.Floor(yg * 255), (int)Math.Floor(yr * 255));
			}
		}

		/// <summary>Returns HSL values from the ColorInt specified.</summary>
		/// <param name="c">ColorInt</param>
		/// <param name="s">out double Saturation (0-1)</param>
		/// <param name="l">out double Luminosity (0-1)</param>
		/// <returns>double Hue (0-1)</returns>
		public static double GetHSL(ColorInt c, out double s, out double l) {
			double r = c.Red / 255d, g = c.Green / 255d, b = c.Blue / 255d;
			double mn = Math.Min(r, Math.Min(g, b));
			double mx = Math.Max(r, Math.Max(g, b));
			double d = mx - mn;

			l = (mx + mn) / 2;

			if (d != 0d) {
				if (l < 0.5) s = d / (mx + mn); else s = d / (2 - mx - mn);

				if (r == mx) return (g - b) / d / 6;
				else if (g == mx) return (2 + (b - r) / d) / 6;
				else if (b == mx) return (4 + (r - g) / d) / 6;
				else return 0d;
			} else {
				s = 0d;
				return 0d;
			}
		}

		/// <summary>Linear interpolation function for ColorInts</summary>
		/// <param name="a">C0</param>
		/// <param name="b">C1</param>
		/// <param name="t">Time</param>
		/// <param name="p">BlendingColorSpace, defaults to RGB</param>
		/// <returns>ColorInt</returns>
		public static ColorInt Lerp(ColorInt a, ColorInt b, double t = 0.5, BlendingColorSpace p = 0) {
			if (p == BlendingColorSpace.HSL) {
				// To be fixed at a later date
				double sa, sb, la, lb;
				double ha = GetHSL(a, out sa, out la);
				double hb = GetHSL(b, out sb, out lb);

				double h = Interpolation.Lerp(ha, hb, t);
				double s = Interpolation.Lerp(sa, sb, t);
				double l = Interpolation.Lerp(la, lb, t);

				return FromHSL(h, s, l);
			} else {
				return new ColorInt(
					(int)Math.Round(Interpolation.Lerp(a.Blue, b.Blue, t)),
					(int)Math.Round(Interpolation.Lerp(a.Green, b.Green, t)),
					(int)Math.Round(Interpolation.Lerp(a.Red, b.Red, t))
				);
			}
		}

		/// <summary>Converts a ColorInt to grayscale.</summary>
		/// <param name="c">ColorInt c</param>
		/// <returns>ColorInt</returns>
		public static ColorInt ToGrayscale(ColorInt c) {
			int v = (int)Math.Round((double)(c.Red + c.Green + c.Blue) / 3);
			return new ColorInt(v, v, v);
		}

		/// <summary>Converts a ColorInt to grayscale with luminance optimizations.</summary>
		/// <param name="c">ColorInt c</param>
		/// <returns>ColorInt</returns>
		public static ColorInt ToOptimizedGrayscale(ColorInt c) {
			int v = (int)Math.Round(c.Red * 0.299 + c.Green * 0.587 + c.Blue * 0.114);
			return new ColorInt(v, v, v);
		}

		/// <summary>Returns a random ColorInt.</summary>
		/// <returns>ColorInt</returns>
		public static ColorInt Random() {
			return new ColorInt(new Random().Next(16777216));
		}

		/// <summary>Returns a random ColorInt from the specified seed.</summary>
		/// <param name="s">int Seed</param>
		/// <returns>ColorInt</returns>
		public static ColorInt Random(int s) {
			return new ColorInt(new Random(s).Next(16777216));
		}
	}
}
