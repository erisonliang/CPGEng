/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Bitmap.cs; Bitmap tools
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/09/29
*/

using System;
using System.Windows.Media.Imaging;

namespace CPGEng {
	public class BitmapData {
		public readonly int Width, Height, Channels, Stride;
		public Buffer buffer;

		public BitmapData(int w, int h, Buffer b, int c = 4, int s = 0) {
			Width = w;
			Height = h;
			buffer = b;
			Channels = c;
			Stride = (s == 0) ? w * c + (w % 8) : s;
		}

		public BitmapSource ToBitmapSource() {
			return BitmapSource.Create(Width, Height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null, buffer.Data(), Stride);
		}
	}

	public static class Bitmap {
		public static BitmapData Import(string path, int w = 256, int h = 256, int c = 4) {
			BitmapImage img = new BitmapImage();
			img.BeginInit();
			img.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
			img.DecodePixelWidth = w;
			img.DecodePixelHeight = h;
			img.EndInit();

			byte[] data = new byte[w * h * c];
			img.CopyPixels(data, w * c, 0);

			return new BitmapData(w, h, new Buffer(data), c, w * c);
		}

		public static BitmapData FromView(View v) {
			return new BitmapData((int)v.Width, (int)v.Height, v.buffer, (int)v.Channels, (int)v.Stride);
		}

		public static BitmapData Resize(BitmapData b, int w, int h) {
			return new BitmapData(0, 0, null);
		}
	}
}
