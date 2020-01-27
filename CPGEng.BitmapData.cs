/*
 * Crispycat PixelGraphic Engine
 * CPGEng.BitmapData.cs; BitmapData objects and functions
 * (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2020/01/27
*/

using System.Windows.Media.Imaging;

namespace CPGEng {
	public class BitmapData {
		public readonly int Width, Height, Channels = 4, Stride;
		public Buffer buffer;

		/// <summary>Creates a new BitmapData.</summary>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		/// <param name="b">Data buffer</param>
		/// <param name="s">Stride, not necessary to use</param>
		/// <seealso cref="View"/>
		public BitmapData(int w, int h, Buffer b, int s = 0) {
			Width = w;
			Height = h;
			buffer = b;
			Stride = (s == 0) ? w * Channels + (w % Channels) : s;
		}

		public uint PixelLocationInBuffer(Pixel p) {
			return (uint)(p.X * Channels + p.Y * Stride);
		}

		/// <summary>Get the value of a Pixel in the image.</summary>
		/// <param name="p">Pixel location</param>
		/// <returns>ColorInt</returns>
		public ColorInt Get(Pixel p) {
			return new ColorInt(
				buffer.Get(PixelLocationInBuffer(p)),
				buffer.Get(PixelLocationInBuffer(p) + 1),
				buffer.Get(PixelLocationInBuffer(p) + 2)
			);
		}

		/// <summary>Draws a color at the location specified.</summary>
		/// <param name="p">Pixel location</param>
		/// <param name="c">ColorInt color</param>
		public void Draw(Pixel p, ColorInt c) {
			buffer.Set(PixelLocationInBuffer(p), (byte)c.Blue);
			buffer.Set(PixelLocationInBuffer(p) + 1, (byte)c.Green);
			buffer.Set(PixelLocationInBuffer(p) + 2, (byte)c.Red);
			buffer.Set(PixelLocationInBuffer(p) + 3, 255);
		}

		/// <summary>Returns a BitmapSource created from the BitmapData.</summary>
		/// <returns>BitmapSource</returns>
		public BitmapSource ToBitmapSource() {
			return BitmapSource.Create(Width, Height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, buffer.Data(), Stride);
		}
	}
}
