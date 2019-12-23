/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Bitmap.cs; Bitmap tools
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2019/12/22
*/

using System;
using System.Windows.Media.Imaging;
using CPGEng.Sprites;

namespace CPGEng {
	public static class Bitmap {
		/// <summary>Returns a BitmapData created from the image at the path specified.</summary>
		/// <param name="path">Path to image</param>
		/// <param name="w">Width of the image</param>
		/// <param name="h">Height of the image</param>
		/// <returns>BitmapSource</returns>
		/// <seealso cref="FromView(View)"/>
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

		/// <summary>Returns a BitmapData created from the View specified.</summary>
		/// <param name="v">View</param>
		/// <returns>BitmapData</returns>
		/// <seealso cref="Import(string, int, int)"/>
		public static BitmapData FromView(View v) {
			return new BitmapData((int)v.Width, (int)v.Height, v.buffer, (int)v.Stride);
		}

		/// <summary>Returns a BitmapData created from the SpritedView specified.</summary>
		/// <param name="v">SpritedView</param>
		/// <returns>BitmapData</returns>
		/// <seealso cref="Import(string, int, int)"/>
		public static BitmapData FromView(SpritedView v) {
			BitmapData bm = new BitmapData((int)v.Width, (int)v.Height, v.buffer, (int)v.Stride);

			foreach (Sprite sprite in v.Sprites.ToArray()) {
				foreach (Pixel x in sprite.TextureMask) bm.Draw(x + sprite.Position, new ColorInt(
					sprite.Texture.buffer.Get(v.PixelLocationInBuffer(x, (uint)sprite.Texture.Channels, (uint)sprite.Texture.Stride) % sprite.Texture.buffer.Length),
					sprite.Texture.buffer.Get(v.PixelLocationInBuffer(x, (uint)sprite.Texture.Channels, (uint)sprite.Texture.Stride) % sprite.Texture.buffer.Length + 1),
					sprite.Texture.buffer.Get(v.PixelLocationInBuffer(x, (uint)sprite.Texture.Channels, (uint)sprite.Texture.Stride) % sprite.Texture.buffer.Length + 2)
				));
			}

			return bm;
		}

		/// <summary>Returns a scaled version of the BitmapData specified.</summary>
		/// <param name="b">BitmapData</param>
		/// <param name="w">New width</param>
		/// <param name="h">New height</param>
		/// <param name="m">ScalingMode, defaults to NearestNeighbor</param>
		/// <returns>BitmapData</returns>
		public static BitmapData Resize(BitmapData b, int w, int h, ScalingMode m = 0) {
			BitmapData nb = new BitmapData(w, h, new Buffer((uint)(w * h * 4)), w * 4);

			if (m == ScalingMode.Bilinear) {
				if (w == 0 || h == 0) return b;

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
