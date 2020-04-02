/*
* Crispycat PixelGraphic Engine
* CPGEng.Effects.Effect.cs; Effects
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

using System;

namespace CPGEng.Effects {
	public static class Effect {
		/// <summary>Returns a ColorInt with the simulated bits per channel.</summary>
		/// <param name="c">ColorInt</param>
		/// <param name="i">int Bits per channel</param>
		/// <returns>ColorInt</returns>
		public static ColorInt SimulateColorDepth(ColorInt c, int i) {
			i = (int)Math.Pow(2, i - 1);
			int r = Math.Min((int)Math.Round((double)c.Red * i) * i, 255);
			int g = Math.Min((int)Math.Round((double)c.Green * i) * i, 255);
			int b = Math.Min((int)Math.Round((double)c.Blue * i) * i, 255);
			return new ColorInt(b, g, r);
		}

		/// <summary>Returns a BitmapData with the simulated bits per channel.</summary>
		/// <param name="b">BitmapData</param>
		/// <param name="i">int Bits per channel</param>
		/// <returns>BitmapData</returns>
		public static BitmapData SimulateColorDepth(BitmapData b, int i) {
			BitmapData nb = new BitmapData(b.Width, b.Height, new Buffer((uint)(b.Width * b.Height * 4)), b.Width * 4);
			for (int x = 0; x < nb.Width; x++)
				for (int y = 0; y < nb.Height; y++)
					nb.Draw(new Pixel(x, y), SimulateColorDepth(b.Get(new Pixel(x, y)), i));
			return nb;
		}

		/// <summary>Returns a View with the simulated bits per channel.</summary>
		/// <param name="v">View</param>
		/// <param name="i">int Bits per channel</param>
		/// <returns>View</returns>
		public static View SimulateColorDepth(View v, int i) {
			View nv = new View(v.Width, v.Height, v.Density);
			for (int x = 0; x < nv.Width; x++)
				for (int y = 0; y < nv.Height; y++)
					nv.Draw(new Pixel(x, y), SimulateColorDepth(v.Get(new Pixel(x, y)), i));
			return nv;
		}

		/// <summary>Returns a BitmapData with the simulated color palette.</summary>
		/// <param name="b">BitmapData</param>
		/// <param name="p">Palette</param>
		/// <returns>View</returns>
		public static BitmapData SimulateColorPalette(BitmapData b, Palette p) {
			BitmapData nb = new BitmapData(b.Width, b.Height, new Buffer((uint)(b.Width * b.Height * 4)), b.Width * 4);
			for (int x = 0; x < nb.Width; x++)
				for (int y = 0; y < nb.Height; y++)
					nb.Draw(new Pixel(x, y), p.ClosestColor(b.Get(new Pixel(x, y))));
			return nb;
		}

		/// <summary>Returns a View with the simulated color palette.</summary>
		/// <param name="v">View</param>
		/// <param name="p">Palette</param>
		/// <returns>View</returns>
		public static View SimulateColorPalette(View v, Palette p) {
			View nv = new View(v.Width, v.Height, v.Density);
			for (int x = 0; x < nv.Width; x++)
				for (int y = 0; y < nv.Height; y++)
					nv.Draw(new Pixel(x, y), p.ClosestColor(v.Get(new Pixel(x, y))));
			return nv;
		}

		/// <summary>Returns a BitmapData with the simulated color palette and dithering.</summary>
		/// <param name="b">BitmapData</param>
		/// <param name="p">Palette</param>
		/// <returns>View</returns>
		public static BitmapData SimulateColorPaletteWithDithering(BitmapData b, Palette p, byte[,] matrix, int size = 8, int gap = 129, int denom = 0) {
			if (denom == 0) denom = size * size + 1;
			BitmapData nb = new BitmapData(b.Width, b.Height, new Buffer((uint)(b.Width * b.Height * 4)), b.Width * 4);
			for (int x = 0; x < nb.Width; x++) {
				for (int y = 0; y < nb.Height; y++) {
					ColorInt c = b.Get(new Pixel(x, y));
					int br = Math.Min(c.Red + matrix[x % size, y % size] * gap / denom, 255);
					int bg = Math.Min(c.Green + matrix[x % size, y % size] * gap / denom, 255);
					int bb = Math.Min(c.Blue + matrix[x % size, y % size] * gap / denom, 255);
					nb.Draw(new Pixel(x, y), p.ClosestColor(new ColorInt(bb, bg, br)));
				}
			}
			return nb;
		}

		/// <summary>Returns a View with the simulated color palette and dithering.</summary>
		/// <param name="v">View</param>
		/// <param name="p">Palette</param>
		/// <returns>View</returns>
		public static View SimulateColorPaletteWithDithering(View v, Palette p, byte[,] matrix, int size = 8, int gap = 17, int denom = 0) {
			if (denom == 0) denom = size * size + 1;
			View nv = new View(v.Width, v.Height, v.Density);
			for (int x = 0; x < nv.Width; x++) {
				for (int y = 0; y < nv.Height; y++) {
					ColorInt c = v.Get(new Pixel(x, y));
					int br = Math.Min(c.Red + matrix[x % size, y % size] * gap / denom, 255);
					int bg = Math.Min(c.Green + matrix[x % size, y % size] * gap / denom, 255);
					int bb = Math.Min(c.Blue + matrix[x % size, y % size] * gap / denom, 255);
					nv.Draw(new Pixel(x, y), p.ClosestColor(new ColorInt(bb, bg, br)));
				}
			}
			return nv;
		}
	}
}