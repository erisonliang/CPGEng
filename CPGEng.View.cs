/*
* Crispycat PixelGraphic Engine
* CPGEng.View.cs; View objects and functions
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

using System.Windows.Media.Imaging;

namespace CPGEng {
	public class View {
		public readonly uint Width, Height, Stride, Density, Channels = 4;
		public Buffer buffer;

		/// <summary>Creates a View.</summary>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		/// <param name="d">Density, defaults to 96ppi (244ppc)</param>
		public View(uint w, uint h, uint d = 96) {
			Width = w;
			Height = h;
			Density = d;
			Stride = w * Channels + (w % Channels);
			buffer = new Buffer(Height * Stride);
		}

		public uint PixelLocationInBuffer(Pixel p) {
			return (uint)(p.X * Channels + p.Y * Stride);
		}

		public uint PixelLocationInBuffer(Pixel p, uint c, uint s) {
			return (uint)(p.X * c + p.Y * s);
		}

		/// <summary>Get the value of a Pixel in the View.</summary>
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

		/// <summary>Draws a color at the locations specified.</summary>
		/// <param name="p">Pixel[] locations</param>
		/// <param name="c">ColorInt color</param>
		public void Draw(Pixel[] p, ColorInt c) {
			foreach (Pixel x in p) Draw(x, c);
		}

		/// <summary>Draws a color at the locations specified.</summary>
		/// <param name="p">Pixel[] locations</param>
		/// <param name="c">ColorInt color</param>
		/// <param name="o">Pixel offset</param>
		public void Draw(Pixel[] p, ColorInt c, Pixel o) {
			foreach (Pixel x in p) Draw(x + o, c);
		}

		/// <summary>Draws a BitmapData at the locations specified.</summary>
		/// <param name="p">Pixel[] locations</param>
		/// <param name="b">BitmapData bitmap</param>
		public void Draw(Pixel[] p, BitmapData b) {
			foreach (Pixel x in p) Draw(x, new ColorInt(
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 1),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 2)
			));
		}


		/// <summary>Draws a BitmapData at the locations specified.</summary>
		/// <param name="p">Pixel[] locations</param>
		/// <param name="b">BitmapData bitmap</param>
		/// <param name="o">Pixel offset</param>
		public void Draw(Pixel[] p, BitmapData b, Pixel o) {
			foreach (Pixel x in p) Draw(x + o, new ColorInt(
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 1),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 2)
			));
		}

		/// <summary>Clears the View.</summary>
		public void Clear() {
			buffer = new Buffer(Height * Stride);
		}


		/// <summary>Clears the View.</summary>
		public void Clear(byte v) {
			buffer = new Buffer(Height * Stride, v);
		}

		/// <summary>Returns a BitmapSource created from the View.</summary>
		/// <returns>BitmapSource</returns>
		public BitmapSource ToBitmapSource() {
			return BitmapSource.Create((int)Width, (int)Height, Density, Density, System.Windows.Media.PixelFormats.Bgra32, null, buffer.Data(), (int)Stride);
		}
	}
}
