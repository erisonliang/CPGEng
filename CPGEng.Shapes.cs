/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Shapes.cs; Library of drawable shapes
 * (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2020/01/27
*/

using System;
using System.Collections.Generic;
using System.Windows;

namespace CPGEng {
	public static class Shapes {
		/// <summary>A line from Pixel a to b.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] Line(Pixel a, Pixel b) {
			List<Pixel> pixels = new List<Pixel>();

			if (a.X == b.X || a.Y == b.Y) {
				if (a.X > b.X) { int _ = a.X; a.X = b.X; b.X = _; }
				if (a.Y > b.Y) { int _ = a.Y; a.Y = b.Y; b.Y = _; }

				if (a.X == b.X) for (int i = a.Y; i <= b.Y; i++) pixels.Add(new Pixel(a.X, i));
				else for (int i = a.X; i <= b.X; i++) pixels.Add(new Pixel(i, a.Y));

				return pixels.ToArray();
			}

			int w = b.X - a.X, h = b.Y - a.Y, dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
			if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
			if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

			int l = Math.Abs(w), s = Math.Abs(h);
			if (l <= s) {
				l = Math.Abs(h);
				s = Math.Abs(w);
				if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
				dx2 = 0;
			}

			int n = l >> 1, x = a.X, y = a.Y;
			for (int i = 0; i <= l; i++) {
				pixels.Add(new Pixel(x, y));
				n += s;
				if (n >= l) {
					n -= l;
					x += dx1;
					y += dy1;
				} else {
					x += dx2;
					y += dy2;
				}
			}

			return pixels.ToArray();
		}

		/// <summary>A rectangle from Pixel a to b.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] Rectangle(Pixel a, Pixel b) {
			List<Pixel> pixels = new List<Pixel>();

			if (a.X > b.X) { int _ = a.X; a.X = b.X; b.X = _; }
			if (a.Y > b.Y) { int _ = a.Y; a.Y = b.Y; b.Y = _; }

			for (int i = a.X; i <= b.X; i++) pixels.Add(new Pixel(i, a.Y));
			for (int i = a.X; i <= b.X; i++) pixels.Add(new Pixel(i, b.Y));
			for (int i = a.Y; i <= b.Y; i++) pixels.Add(new Pixel(a.X, i));
			for (int i = a.Y; i <= b.Y; i++) pixels.Add(new Pixel(b.X, i));

			return pixels.ToArray();
		}

		/// <summary>A filled rectangle from Pixel a to b.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] FilledRectangle(Pixel a, Pixel b) {
			List<Pixel> pixels = new List<Pixel>();

			if (a.X > b.X) { int _ = a.X; a.X = b.X; b.X = _; }
			if (a.Y > b.Y) { int _ = a.Y; a.Y = b.Y; b.Y = _; }

			for (int x = a.X; x <= b.X; x++) {
				for (int y = a.Y; y <= b.Y; y++) {
					pixels.Add(new Pixel(x, y));
				}
			}

			return pixels.ToArray();
		}

		/// <summary>A triangle from Pixels a, b and c.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <param name="c">Pixel c</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] Triangle(Pixel a, Pixel b, Pixel c) {
			List<Pixel> pixels = new List<Pixel>();

			pixels.AddRange(Line(new Pixel(a.X, a.Y), new Pixel(b.X, b.Y)));
			pixels.AddRange(Line(new Pixel(b.X, b.Y), new Pixel(c.X, c.Y)));
			pixels.AddRange(Line(new Pixel(c.X, c.Y), new Pixel(a.X, a.Y)));

			return pixels.ToArray();
		}

		/// <summary>A filled triangle from Pixels a, b and c.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <param name="c">Pixel c</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] FilledTriangle(Pixel a, Pixel b, Pixel c) {
			List<Pixel> pixels = new List<Pixel>();

			int MaxX = Math.Max(a.X, Math.Max(b.X, c.X));
			int MinX = Math.Min(a.X, Math.Min(b.X, c.X));
			int MaxY = Math.Max(a.Y, Math.Max(b.Y, c.Y));
			int MinY = Math.Min(a.Y, Math.Min(b.Y, c.Y));

			Vector vs1 = new Vector(b.X - a.X, b.Y - a.Y);
			Vector vs2 = new Vector(c.X - a.X, c.Y - a.Y);

			for (int x = MinX; x <= MaxX; x++) {
				for (int y = MinY; y <= MaxY; y++) {
					Vector q = new Vector(x - a.X, y - a.Y);

					double s = Vector.CrossProduct(q, vs2) / Vector.CrossProduct(vs1, vs2);
					double t = Vector.CrossProduct(vs1, q) / Vector.CrossProduct(vs1, vs2);

					if ((s >= 0) && (t >= 0) && (s + t <= 1)) pixels.Add(new Pixel(x, y));
				}
			}

			return pixels.ToArray();
		}

		/// <summary>An ellipse from Pixel a to b.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] Ellipse(Pixel a, Pixel b) {
			List<Pixel> pixels = new List<Pixel>();

			int ox = Math.Abs(b.X + a.X) / 2;
			int rx = Math.Abs(b.X - a.X) / 2;
			int oy = Math.Abs(b.Y + a.Y) / 2;
			int ry = Math.Abs(b.Y - a.Y) / 2;

			int txx = 2 * rx * rx, tyy = 2 * ry * ry;
			int x = rx, y = 0, xc = ry * ry * (1 - 2 * rx), yc = rx * rx, ee = 0, sx = tyy * rx, sy = 0;

			void pp(int X, int Y) {
				pixels.Add(new Pixel(ox + X, oy + Y));
				pixels.Add(new Pixel(ox - X, oy + Y));
				pixels.Add(new Pixel(ox - X, oy - Y));
				pixels.Add(new Pixel(ox + X, oy - Y));
			}

			while (sx >= sy) {
				pp(x, y);
				y++;
				sy += txx;
				ee += yc;
				yc += txx;
				if (2 * ee + xc > 0) {
					x--;
					sx -= tyy;
					ee += xc;
					xc += tyy;
				}
			}

			x = 0;
			y = ry;
			xc = ry * ry;
			yc = rx * rx * (1 - 2 * ry);
			ee = 0;
			sx = 0;
			sy = txx * ry;

			while (sx <= sy) {
				pp(x, y);
				x++;
				sx += tyy;
				ee += xc;
				xc += tyy;
				if (2 * ee + yc > 0) {
					y--;
					sy -= txx;
					ee += yc;
					yc += txx;
				}
			}

			return pixels.ToArray();
		}

		/// <summary>A filled ellipse from Pixel a to b.</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] FilledEllipse(Pixel a, Pixel b) {
			List<Pixel> pixels = new List<Pixel>();

			int ox = Math.Abs(b.X + a.X) / 2;
			int rx = Math.Abs(b.X - a.X) / 2;
			int oy = Math.Abs(b.Y + a.Y) / 2;
			int ry = Math.Abs(b.Y - a.Y) / 2;

			int tx = rx * rx, ty = ry * ry;
			int txy = tx * ty, x0 = rx, dx = 0;

			for (int x = -rx; x <= rx; x++) pixels.Add(new Pixel(ox + x, oy));

			for (int y = 1; y <= ry; y++) {
				int x1 = x0 - (dx - 1);
				for (; x1 > 0; x1--) if (x1 * x1 * ty + y * y * tx <= txy) break;
				dx = x0 - x1;
				x0 = x1;

				for (int x = -x0; x <= x0; x++) {
					pixels.Add(new Pixel(ox + x, oy - y));
					pixels.Add(new Pixel(ox + x, oy + y));
				}
			}

			return pixels.ToArray();
		}

		/// <summary>A polygon from Pixels a, b, c...</summary>
		/// <param name="p">Pixels</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] Polygon(params Pixel[] p) {
			List<Pixel> pixels = new List<Pixel>();

			for (int i = 0; i < p.Length; i++) {
				if (i == p.Length - 1) {
					pixels.AddRange(Line(p[i], p[0]));
				} else {
					pixels.AddRange(Line(p[i], p[i + 1]));
				}
			}
			return pixels.ToArray();
		}

		/// <summary>A filled polygon from Pixels a, b, c...</summary>
		/// <param name="p">Pixels</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] FilledPolygon(params Pixel[] p) {
			List<Pixel> pixels = new List<Pixel>();
			int nx = int.MaxValue, cx, xx = 0, ny = int.MaxValue, cy, xy = 0;

			Pixel[] q = new Pixel[p.Length + 1];

			for (int i = 0; i < q.Length; i++) {
				q[i] = (i < q.Length - 1) ? p[i] : p[0];
				if (q[i].X < nx) nx = q[i].X;
				if (q[i].X > xx) xx = q[i].X;
				if (q[i].X < ny) ny = q[i].Y;
				if (q[i].X > xy) xy = q[i].Y;
			}

			cx = (nx + xx) / 2;
			cy = (ny + xy) / 2;

			Pixel c = new Pixel(cx, cy);

			for (int i = 0; i < p.Length; i++) pixels.AddRange(FilledTriangle(c, q[i], q[i + 1]));

			return pixels.ToArray();
		}

		private static Pixel[] FancyBezierCurve(Pixel a, Pixel b, Pixel c, double p) {
			List<Pixel> pixels = new List<Pixel>();

			for (double t = 0; t <= 1; t += 1 / p) {
				double x = (1 - t) * (1 - t) * a.X + 2 * (1 - t) * t * b.X + t * t * c.X;
				double y = (1 - t) * (1 - t) * a.Y + 2 * (1 - t) * t * b.Y + t * t * c.Y;

				pixels.Add(new Pixel((int)Math.Round(x), (int)Math.Round(y)));
			}

			return pixels.ToArray();
		}

		private static Pixel[] FancyBezierCurve(Pixel a, Pixel b, Pixel c, Pixel d, double p) {
			List<Pixel> pixels = new List<Pixel>();

			for (double t = 0; t <= 1; t += 1 / p) {
				double x = (1 - t) * (1 - t) * (1 - t) * a.X + 3 * (1 - t) * (1 - t) * t * b.X + 3 * (1 - t) * t * t * c.X + t * t * t * d.X;
				double y = (1 - t) * (1 - t) * (1 - t) * a.Y + 3 * (1 - t) * (1 - t) * t * b.Y + 3 * (1 - t) * t * t * c.Y + t * t * t * d.Y;

				pixels.Add(new Pixel((int)Math.Round(x), (int)Math.Round(y)));
			}

			return pixels.ToArray();
		}

		/// <summary>A Bezier curve from Pixels a, b, and c (3 control points).</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <param name="c">Pixel c</param>
		/// <param name="d">DrawingMode, defaults to Fast</param>
		/// <param name="p">Precision</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] BezierCurve(Pixel a, Pixel b, Pixel c, DrawingMode d = 0, double p = 0) {
			if (p == 0) p = (d == DrawingMode.Fancy) ? 800 : 19;
			List<Pixel> pixels = new List<Pixel>();

			if (d == DrawingMode.Fancy) {
				pixels.AddRange(FancyBezierCurve(a, b, c, p));
			} else {
				Pixel[] q = FancyBezierCurve(a, b, c, p);
				for (int i = 0; i < q.Length - 1; i++) pixels.AddRange(Line(q[i], q[i + 1]));
			}

			return pixels.ToArray();
		}

		/// <summary>A Bezier curve from Pixels a, b, c and d (4 control points).</summary>
		/// <param name="a">Pixel a</param>
		/// <param name="b">Pixel b</param>
		/// <param name="c">Pixel c</param>
		/// <param name="d">Pixel d</param>
		/// <param name="e">DrawingMode, defaults to Fast</param>
		/// <param name="p">Precision</param>
		/// <returns>Pixel[]</returns>
		public static Pixel[] BezierCurve(Pixel a, Pixel b, Pixel c, Pixel d, DrawingMode e = 0, double p = 0) {
			if (p == 0) p = (e == DrawingMode.Fancy) ? 800 : 22;
			List<Pixel> pixels = new List<Pixel>();

			if (e == DrawingMode.Fancy) {
				pixels.AddRange(FancyBezierCurve(a, b, c, d, p));
			} else {
				Pixel[] q = FancyBezierCurve(a, b, c, d, p);
				for (int i = 0; i < q.Length - 1; i++) pixels.AddRange(Line(q[i], q[i + 1]));
			}

			return pixels.ToArray();
		}
	}
}
