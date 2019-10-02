/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Blending.cs; Color blending
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/09/29
*/

using System;

namespace CPGEng {
	public static class Blend {
		public static ColorInt BlendColorInt(ColorInt a, ColorInt b, BlendingMode m = 0) {
			switch (m) {
				case BlendingMode.Add:
					return new ColorInt((a.Value + b.Value) & 0xffffff);
				case BlendingMode.Or:
					return new ColorInt(a.Value | b.Value);
				case BlendingMode.And:
					return new ColorInt(a.Value & b.Value);
				case BlendingMode.Xor:
					return new ColorInt(a.Value ^ b.Value);
				case BlendingMode.Lerp:
					return new ColorInt(
						(int)Math.Floor(Lerper.Lerp(a.Blue, b.Blue, 0.5)),
						(int)Math.Floor(Lerper.Lerp(a.Green, b.Green, 0.5)),
						(int)Math.Floor(Lerper.Lerp(a.Red, b.Red, 0.5))
					);
				default:
					return b;
			}
		}
	}
}