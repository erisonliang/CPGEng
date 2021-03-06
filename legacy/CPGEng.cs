﻿/*
 * Crispycat PixelGraphic Engine v1.3
 * crispycat
 * 2019/08/25
*/

using System;
using System.Collections.Generic;
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

	public class CPGBitmapData {
		public byte[] Data;
		public int Stride, B, Width, Height;
		public readonly string Path;
		public CPGBitmapData(string path, int sx, int sy) {
			Path = path;
			Width = sx;
			Height = sy;
		}

		public void Load() {
			if (File.Exists(Path)) {
				BitmapImage img = new BitmapImage();
				img.BeginInit();
				img.UriSource = new Uri(Path);
				img.DecodePixelWidth = Width;
				img.DecodePixelHeight = Height;
				img.EndInit();

				B = 4;
				Stride = Width * B;
				Data = new byte[Stride * Height];
				img.CopyPixels(Data, Stride, 0);
			} else {
				B = 4;
				Stride = Width * B;
				Data = new byte[Stride * Height];
			}
		}

		public void Resize(int nw, int nh, bool reload = true) {
			Width = nw;
			Height = nh;
			if (reload) Load();
		}
	}

	public class Sprite {
		public CPGBitmapData Image;
		public int PosX, PosY, SizeX, SizeY;

		public Sprite(CPGBitmapData img, int px = 0, int py = 0) {
			Image = img;
			PosX = px;
			PosY = py;
			SizeX = img.Width;
			SizeY = img.Height;
		}

		public CPGBitmapData Resize(int nw, int nh) {
			Image.Resize(nw, nh);
			return Image;
		}
	}

	public class View {
		// Initialization
		public byte[] Buffer;
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

		public byte ReadBuffer(int loc) {
			if (loc < Buffer.Length && loc > 0) return Buffer[loc];
			return new byte();
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

		// Output functions
		public BitmapSource Output() {
			if (Sprites.Length > 0) {
				byte[] UnspritedBuff = Buffer.Clone() as byte[];
				foreach (Sprite sprite in Sprites) DrawBitmap(sprite.PosX, sprite.PosY, sprite.Image);
				BitmapSource s = BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
				Buffer = UnspritedBuff.Clone() as byte[];
				return s;
			}
			return BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
		}
		public void Update(object obj) {
			if (Sprites.Length > 0) {
				byte[] UnspritedBuff = Buffer.Clone() as byte[];
				foreach (Sprite sprite in Sprites) DrawBitmap(sprite.PosX, sprite.PosY, sprite.Image);
				(obj as Image).Source = BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
				Buffer = UnspritedBuff.Clone() as byte[];
				return;
			}
			
			(obj as Image).Source = BitmapSource.Create(Width, Height, Density, Density, PixelFormats.Bgr24, null, Buffer, Stride);
		}

		// Drawing functions
		public Action<int, int, ColorInt> DrawPoint => SetPixel;

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
			if (x1 > x2) { int _ = x1; x1 = x2; x2 = _; }
			if (y1 > y2) { int _ = y1; y1 = y2; y2 = _; }

			for (int i = x1; i <= x2; i++) SetPixel(i, y1, c);
			for (int i = x1; i <= x2; i++) SetPixel(i, y2, c);
			for (int i = y1; i <= y2; i++) SetPixel(x1, i, c);
			for (int i = y1; i <= y2; i++) SetPixel(x2, i, c);
		}

		public void DrawFilledRectangle(int x1, int y1, int x2, int y2, ColorInt c) {
			if (x1 > x2) { int _ = x1; x1 = x2; x2 = _; }
			if (y1 > y2) { int _ = y1; y1 = y2; y2 = _; }

			for (int x = x1; x <= x2; x++) {
				for (int y = y1; y <= y2; y++) {
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

		public void DrawEllipse(int x1, int y1, int x2, int y2, ColorInt c, double p = 3) {
			int ox = Math.Abs(x2 + x1) / 2;
			int sx = Math.Abs(x2 - x1) / 2;
			int oy = Math.Abs(y2 + y1) / 2;
			int sy = Math.Abs(y2 - y1) / 2;

			for (double i = 0; i < 360; i += 1 / p)
				SetPixel((int)Math.Floor(Math.Cos(i) * sx + ox), (int)Math.Floor(Math.Sin(i) * sy + oy), c);
		}

		public void DrawFilledEllipse(int x1, int y1, int x2, int y2, ColorInt c, double p = 6) {
			int ox = Math.Abs(x2 + x1) / 2;
			int sx = Math.Abs(x2 - x1) / 2;
			int oy = Math.Abs(y2 + y1) / 2;
			int sy = Math.Abs(y2 - y1) / 2;

			for (double i = 0; i < 360; i += 1 / p) {
				for (int j = 0; j <= sx; j++) {
					SetPixel((int)Math.Floor(Math.Cos(Math.PI / 180 * i) * j + ox), (int)Math.Floor(Math.Sin(Math.PI / 180 * i) * j * sy / sx + oy), c);
				}
			}
		}

		public void DrawPoly(ColorInt c, params int[] p) {
			int[,] points = new int[(int)Math.Floor((double)p.Length / 2) + 1, 2];
			int cp = 0, cc = 0;
			for (int i = 0; i < p.Length; i++) {
				points[cp, cc] = p[i];
				cc++;
				if (cc > 1) {
					cc = 0;
					cp++;
				}
			}
			points[points.Length / 2 - 1, 0] = points[0, 0];
			points[points.Length / 2 - 1, 1] = points[0, 1];

			for (int pp = 0; pp < points.Length / 2 - 1; pp++) {
				DrawLine(points[pp, 0], points[pp, 1], points[pp + 1, 0], points[pp + 1, 1], c);
			}
		}

		public void DrawFilledPoly(ColorInt c, params int[] p) {
			int[,] points = new int[(int)Math.Floor((double)p.Length / 2) + 1, 2];
			int cp = 0, cc = 0, mnx = 0, mxx = 0, mny = 0, mxy = 0, cx = 0, cy = 0;
			for (int i = 0; i < p.Length; i++) {
				points[cp, cc] = p[i];
				cc++;
				if (cc > 1) {
					cc = 0;
					cp++;
				}
			}
			points[points.Length / 2 - 1, 0] = points[0, 0];
			points[points.Length / 2 - 1, 1] = points[0, 1];

			for (int mp = 0; mp < points.Length / 2 - 1; mp++) {
				if (points[mp, 0] < mnx) mnx = points[mp, 0];
				if (points[mp, 0] > mxx) mxx = points[mp, 0];
				if (points[mp, 1] < mny) mny = points[mp, 1];
				if (points[mp, 1] > mxy) mxy = points[mp, 1];
			}
			cx = (mxx + mnx) / 2;
			cy = (mxy + mny) / 2;

			for (int pp = 0; pp < points.Length / 2 - 1; pp++) {
				DrawFilledTriangle(points[pp, 0], points[pp, 1], points[pp + 1, 0], points[pp + 1, 1], cx, cy, c);
			}
		}

		public void DrawBitmap(int x, int y, CPGBitmapData data) {
			if (data.Data == null) return;
			for (int i = 0; i < data.Height; i++) {
				for (int j = 0; j < data.Width; j++) {
					WriteBuffer(BufferLocation(j + x, i + y), data.Data[i * data.Stride + j * data.B]);
					WriteBuffer(BufferLocation(j + x, i + y) + 1, data.Data[i * data.Stride + j * data.B + 1]);
					WriteBuffer(BufferLocation(j + x, i + y) + 2, data.Data[i * data.Stride + j * data.B + 2]);
				}
			}
		}

		// Sprite functions
		public void AddSprite(Sprite sprite) {
			Array.Resize(ref Sprites, Sprites.Length + 1);
			Sprites[Sprites.Length - 1] = sprite;
		}

		public void RemoveSprite(Sprite sprite) {
			List<Sprite> _ = new List<Sprite>(Sprites);
			_.Remove(sprite);
			Sprites = _.ToArray();
		}

		public void ClearSprites() {
			Sprites = new Sprite[0];
		}
	}
}
