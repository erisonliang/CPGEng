/*
 * Crispycat PixelGraphic Engine
 * CPGEng.View.cs; View objects and functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/10/02
*/

using System.Windows.Media.Imaging;

namespace CPGEng {
	public class View {
		public readonly uint Width, Height, Stride, Density, Channels;
		public Buffer buffer;

		public View(uint w, uint h, uint d = 96, uint c = 4) {
			Width = w;
			Height = h;
			Density = d;
			Channels = c;
			Stride = w * c + (w % Channels);
			buffer = new Buffer(Height * Stride);
		}

		public uint PixelLocationInBuffer(Pixel p) {
			return (uint)(p.X * Channels + p.Y * Stride);
		}

		public uint PixelLocationInBuffer(Pixel p, uint c, uint s) {
			return (uint)(p.X * c + p.Y * s);
		}

		public ColorInt Get(Pixel p) {
			return new ColorInt(
				buffer.Get(PixelLocationInBuffer(p)),
				buffer.Get(PixelLocationInBuffer(p) + 1),
				buffer.Get(PixelLocationInBuffer(p) + 2)
			);
		}

		public void Draw(Pixel p, ColorInt c) {
			buffer.Set(PixelLocationInBuffer(p), (byte)c.Blue);
			buffer.Set(PixelLocationInBuffer(p) + 1, (byte)c.Green);
			buffer.Set(PixelLocationInBuffer(p) + 2, (byte)c.Red);
			buffer.Set(PixelLocationInBuffer(p) + 3, 255);
		}

		public void Draw(Pixel[] p, ColorInt c) {
			foreach (Pixel x in p) Draw(x, c);
		}

		public void Draw(Pixel[] p, ColorInt c, Pixel o) {
			foreach (Pixel x in p) Draw(x + o, c);
		}

		public void Draw(Pixel[] p, BitmapData b) {
			foreach (Pixel x in p) Draw(x, new ColorInt(
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 1),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 2)
			));
		}

		public void Draw(Pixel[] p, BitmapData b, Pixel o) {
			foreach (Pixel x in p) Draw(x + o, new ColorInt(
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 1),
				b.buffer.Get(PixelLocationInBuffer(x, (uint)b.Channels, (uint)b.Stride) % b.buffer.Length + 2)
			));
		}

		public void Clear() {
			buffer = new Buffer(Height * Stride);
		}

		public void Clear(byte v) {
			buffer = new Buffer(Height * Stride, v);
		}

		public BitmapSource ToBitmapSource() {
			return BitmapSource.Create((int)Width, (int)Height, Density, Density, System.Windows.Media.PixelFormats.Bgra32, null, buffer.Data(), (int)Stride);
		}
	}
}
