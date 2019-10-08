/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Bitmap.cs; Bitmap tools
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2019/10/07
*/

using System;
using System.Windows.Media.Imaging;

namespace CPGEng {
	public class BitmapData {
		public readonly int Width, Height, Channels = 4, Stride;
		public Buffer buffer;

		public BitmapData(int w, int h, Buffer b, int s = 0) {
			Width = w;
			Height = h;
			buffer = b;
			Stride = (s == 0) ? w * Channels + (w % Channels) : s;
		}

		public uint PixelLocationInBuffer(Pixel p) {
			return (uint)(p.X * Channels + p.Y * Stride);
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

		public BitmapSource ToBitmapSource() {
			return BitmapSource.Create(Width, Height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, buffer.Data(), Stride);
		}
	}

	public static class Bitmap {
		public static BitmapData Import(string path, int w = 256, int h = 256) {
			BitmapImage img = new BitmapImage();
			img.BeginInit();
			img.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
			img.DecodePixelWidth = w;
			img.DecodePixelHeight = h;
			img.EndInit();

			byte[] data = new byte[w * h * 4];
			img.CopyPixels(data, w * 4, 0);

			return new BitmapData(w, h, new Buffer(data), w * 4);
		}

		public static BitmapData FromView(View v) {
			return new BitmapData((int)v.Width, (int)v.Height, v.buffer, (int)v.Stride);
		}

		public static BitmapData Resize(BitmapData b, int w, int h, ScalingMode m = 0) {
			BitmapData nb = new BitmapData(w, h, new Buffer((uint)(w * h * 4)), w * 4);

			if (m == ScalingMode.Bilinear) {
				if (w == 0 || h == 0) return b;
				double rx = w / b.Width;
				double ry = h / b.Height;

				for (int x = 0; x < w; x++) {
					for (int y = 0; y < h; y++) {
						double gx = (double)x / w * (b.Width - 1);
						double gy = (double)y / h * (b.Height - 1);
						int ix = (int)gx;
						int iy = (int)gy;
						double dx = gx - ix;
						double dy = gy - iy;

						ColorInt c00 = b.Get(new Pixel(ix, iy));
						ColorInt c10 = b.Get(new Pixel(ix + 1, iy));
						ColorInt c01 = b.Get(new Pixel(ix, iy + 1));
						ColorInt c11 = b.Get(new Pixel(ix + 1, iy + 1));

						int ob = (int)Interpolation.Blerp(c00.Blue, c10.Blue, c01.Blue, c11.Blue, dx, dy);
						int og = (int)Interpolation.Blerp(c00.Green, c10.Green, c01.Green, c11.Green, dx, dy);
						int or = (int)Interpolation.Blerp(c00.Red, c10.Red, c01.Red, c11.Red, dx, dy);

						nb.Draw(new Pixel(x, y), new ColorInt(ob, og, or));
					}
				}
			} else {
				double rx = (double)b.Width / w;
				double ry = (double)b.Height / h;
				double px, py;

				for (int i = 0; i < h; i++) {
					for (int j = 0; j < w; j++) {
						px = Math.Floor(j * rx);
						py = Math.Floor(i * ry);

						nb.Draw(new Pixel(j, i), b.Get(new Pixel((int)px, (int)py)));
					}
				}
			}

			return nb;
		}
	}
}
