/*
* Crispycat PixelGraphic Engine
* CPGEng.Effects.DefaultPalette.cs; Default Color Palettes
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

namespace CPGEng.Effects {
	public static class DefaultPalette {
		public static Palette Gray1Bit = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(255, 255, 255)
		});

		public static Palette Gray2Bit = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(86, 86, 86),
			new ColorInt(172, 172, 172),
			new ColorInt(255, 255, 255)
		});

		public static Palette Gray2Bit2 = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(63, 63, 63),
			new ColorInt(127, 127, 127),
			new ColorInt(255, 255, 255)
		});

		public static Palette RGB3Bit = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(0, 0, 255),
			new ColorInt(0, 255, 0),
			new ColorInt(0, 255, 255),
			new ColorInt(255, 0, 0),
			new ColorInt(255, 0, 255),
			new ColorInt(255, 255, 0),
			new ColorInt(255, 255, 255)
		});

		public static Palette RGB4Bit = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(63, 63, 63),
			new ColorInt(127, 127, 127),
			new ColorInt(255, 255, 255),
			new ColorInt(0, 0, 255),
			new ColorInt(0, 255, 0),
			new ColorInt(0, 255, 255),
			new ColorInt(255, 0, 0),
			new ColorInt(255, 0, 255),
			new ColorInt(255, 255, 0),
			new ColorInt(0, 0, 127),
			new ColorInt(0, 127, 0),
			new ColorInt(0, 127, 127),
			new ColorInt(127, 0, 0),
			new ColorInt(127, 0, 127),
			new ColorInt(127, 127, 0),
		});

		public static Palette RGB4Bit2 = new Palette(new System.Collections.Generic.List<ColorInt>() {
			new ColorInt(0),
			new ColorInt(86, 86, 86),
			new ColorInt(172, 172, 172),
			new ColorInt(255, 255, 255),
			new ColorInt(0, 0, 255),
			new ColorInt(0, 255, 0),
			new ColorInt(0, 255, 255),
			new ColorInt(255, 0, 0),
			new ColorInt(255, 0, 255),
			new ColorInt(255, 255, 0),
			new ColorInt(0, 0, 127),
			new ColorInt(0, 127, 0),
			new ColorInt(0, 127, 191),
			new ColorInt(127, 0, 0),
			new ColorInt(127, 0, 191),
			new ColorInt(127, 127, 0),
		});
	}
}
