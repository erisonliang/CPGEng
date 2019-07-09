/*
 * Crispycat PixelGraphic Engine v1.0
 * crispycat
 * 2019/07/09
*/

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CPGEng {
	public class ColorInt {
		public int Blue, Green, Red, Value;

		public ColorInt(int v) {
			Value = v;
			Blue = (Value >> 16) & 255;
			Green = (Value >> 8) & 255;
			Red = (Value >> 0) & 255;
		}

		public ColorInt(int b, int g, int r) {
			Blue = b;
			Green = g;
			Red = r;
			Value = (b << 16) | (g << 8) | (r << 0);
		}
	}

	public class Sprite {
		public string ImagePath;
		public int PosX = 0, PosY = 0;
		public int SizeX, SizeY;

		public Sprite(string img, int sx, int sy) {
			ImagePath = img;
			SizeX = sx;
			SizeY = sy;
		}
	}

	public class View {
		// Initialization
		private byte[] Buffer;
		public int Width, Height, Stride, Density;
		private int ColorBytes = 3;
		private Sprite[] Sprites = new Sprite[0];

		public View(int width, int height, int density) {
			Width = width;
			Height = height;
			Density = density;
			Stride = Width * ColorBytes + (Width % 8);
			Buffer = new byte[Stride * Height];
		}

		// Buffer manipulation functions
		public int BufferLocation(int x, int y) {
			return x * ColorBytes + y * Stride;
		}

		public void WriteBuffer(int loc, byte val) {
			if (loc < Buffer.Length && loc > 0) Buffer[loc] = val;
		}

		public ColorInt GetPixel(int x, int y) {
			if (x < 0 || y < 0 || x >= Width || y >= Height) return new ColorInt(0);
			return new ColorInt((int)Buffer[BufferLocation(x, y)], (int)Buffer[BufferLocation(x, y) + 1], (int)Buffer[BufferLocation(x, y) + 2]);
		}

		public void SetPixel(int x, int y, ColorInt c) {
			if (x < 0 || y < 0 || x >= Width || y >= Height) return;
			Buffer[BufferLocation(x, y)] = (byte)c.Blue;
			Buffer[BufferLocation(x, y) + 1] = (byte)c.Green;
			Buffer[BufferLocation(x, y) + 2] = (byte)c.Red;
		}

		public void Clear() {
			for (int i = 0; i < Height * Stride; i++) {
				Buffer[i] = 0;
			}
		}

		public void Clear(ColorInt c) {
			for (int i = 0; i < Height * Stride; i += 3) {
				Buffer[i] = (byte)c.Blue;
				Buffer[i + 1] = (byte)c.Green;
				Buffer[i + 2] = (byte)c.Red;
			}
		}

		// Update function
		public void Update(object obj) {
			// Sprites? Do this:
			if (Sprites.Length > 0) {
				byte[] UnspritedBuff = Buffer.Clone() as byte[];
				foreach (Sprite sprite in Sprites) {
					DrawBitmap(sprite.PosX, sprite.PosY, sprite.PosX + sprite.SizeX, sprite.PosY + sprite.SizeY, sprite.ImagePath);
				}
				(obj as Image).Source = BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
				ClearSprites();
				Buffer = UnspritedBuff.Clone() as byte[];
				return;
			}
			
			// No sprites? Just do this:
			(obj as Image).Source = BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
		}

		// Drawing functions
		public void DrawPoint(int x, int y, ColorInt c) { SetPixel(x, y, c); }

		public void DrawLine(int x1, int y1, int x2, int y2, ColorInt c) {
			int w = x2 - x1;
			int h = y2 - y1;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
			if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
			if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
			int longest = Math.Abs(w);
			int shortest = Math.Abs(h);
			if (!(longest > shortest)) {
				longest = Math.Abs(h);
				shortest = Math.Abs(w);
				if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
				dx2 = 0;
			}
			int numerator = longest >> 1;
			for (int i = 0; i <= longest; i++) {
				SetPixel(x1, y1, c);
				numerator += shortest;
				if (!(numerator < longest)) {
					numerator -= longest;
					x1 += dx1;
					y1 += dy1;
				} else {
					x1 += dx2;
					y1 += dy2;
				}
			}
		}

		public void DrawRectangle(int x1, int y1, int x2, int y2, ColorInt c) {
			DrawLine(x1, y1, x2, y1, c);
			DrawLine(x2, y1, x2, y2, c);
			DrawLine(x1, y2, x2, y2, c);
			DrawLine(x1, y1, x1, y2, c);
		}

		public void DrawFilledRectangle(int x1, int y1, int x2, int y2, ColorInt c) {
			for (int x = x1; x < x2; x++) {
				for (int y = y1; y < y2; y++) {
					SetPixel(x, y, c);
				}
			}
		}

		public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, ColorInt c) {
			DrawLine(x1, y1, x2, y2, c);
			DrawLine(x2, y2, x3, y3, c);
			DrawLine(x3, y3, x1, y1, c);
		}
		public void DrawFilledTriangle(int x1, int y1, int x2, int y2, int x3, int y3, ColorInt c) {
			int MaxX = Math.Max(x1, Math.Max(x2, x3));
			int MinX = Math.Min(x1, Math.Min(x2, x3));
			int MaxY = Math.Max(y1, Math.Max(y2, y3));
			int MinY = Math.Min(y1, Math.Min(y2, y3));

			Vector vs1 = new Vector(x2 - x1, y2 - y1);
			Vector vs2 = new Vector(x3 - x1, y3 - y1);

			for (int x = MinX; x <= MaxX; x++) {
				for (int y = MinY; y <= MaxY; y++) {
					Vector q = new Vector(x - x1, y - y1);

					double s = Vector.CrossProduct(q, vs2) / Vector.CrossProduct(vs1, vs2);
					double t = Vector.CrossProduct(vs1, q) / Vector.CrossProduct(vs1, vs2);

					if ((s >= 0) && (t >= 0) && (s + t <= 1)) SetPixel(x, y, c);
				}
			}
		}

		private int[] GetEllipsePoints(int rw, int rh, int ox, int oy, double t) {
			int[] loc = new int[2];
			loc[0] = (int)(ox + rw * Math.Cos(t * Math.PI / 180));
			loc[1] = (int)(oy + rh * Math.Sin(t * Math.PI / 180));
			return loc;
		}

		public void DrawEllipse(int x1, int y1, int x2, int y2, ColorInt c) {
			int ox = (x2 + x1) / 2;
			int sx = (x2 - x1) / 2;
			int oy = (y2 + y1) / 2;
			int sy = (y2 - y1) / 2;

			for (double i = 0; i < 360; i += 0.05) {
				int[] loc = GetEllipsePoints(sx, sy, ox, oy, i);
				SetPixel(loc[0], loc[1], c);
			}
		}

		public void DrawEllipse(int x1, int y1, int x2, int y2, double step, ColorInt c) {
			int ox = (x2 + x1) / 2;
			int sx = (x2 - x1) / 2;
			int oy = (y2 + y1) / 2;
			int sy = (y2 - y1) / 2;

			step = (step > 0) ? 1 / step : 0.05;

			for (double i = 0; i < 360; i += step) {
				int[] loc = GetEllipsePoints(sx, sy, ox, oy, i);
				SetPixel(loc[0], loc[1], c);
			}
		}

		public void DrawFilledEllipse(int x1, int y1, int x2, int y2, ColorInt c) {
			int ox = (x2 + x1) / 2;
			int sx = (x2 - x1) / 2;
			int oy = (y2 + y1) / 2;
			int sy = (y2 - y1) / 2;

			for (double i = 0; i < 360; i += 0.05) {
				int[] loc = GetEllipsePoints(sx, sy, ox, oy, i);
				DrawLine(ox, oy, loc[0], loc[1], c);
			}
		}

		public void DrawBitmap(int x1, int y1, int x2, int y2, string imgp) {
			if (!File.Exists(imgp)) return;

			BitmapImage img = new BitmapImage();
			img.BeginInit();
			img.UriSource = new Uri(Environment.CurrentDirectory + "\\" + imgp);
			img.DecodePixelWidth = x2 - x1;
			img.DecodePixelHeight = y2 - y1;
			img.EndInit();

			int imgcolorbytes = 4;
			int stride = (x2 - x1) * imgcolorbytes;
			byte[] pixels = new byte[stride * (y2 - y1)];

			img.CopyPixels(pixels, stride, 0);

			for (int i = 0; i < y2 - y1; i++) {
				for (int j = 0; j < x2 - x1; j++) {
					WriteBuffer(BufferLocation(j + x1, i + y1), pixels[i * stride + j * imgcolorbytes]);
					WriteBuffer(BufferLocation(j + x1, i + y1) + 1, pixels[i * stride + j * imgcolorbytes + 1]);
					WriteBuffer(BufferLocation(j + x1, i + y1) + 2, pixels[i * stride + j * imgcolorbytes + 2]);
				}
			}
		}

		// Sprite functions
		public void AddSprite(Sprite sprite) {
			Array.Resize(ref Sprites, Sprites.Length + 1);
			Sprites[Sprites.Length - 1] = sprite;
		}

		public void ClearSprites() {
			Sprites = new Sprite[0];
		}
	}
}
