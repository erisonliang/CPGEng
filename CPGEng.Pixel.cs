/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Pixel.cs; Pixel struct
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/10/02
*/

using System;

namespace CPGEng {
	public struct Pixel {
		public int X, Y;

		public Pixel(int x = 0, int y = 0) {
			X = x;
			Y = y;
		}

		public Pixel Add(Pixel p) {
			return new Pixel(X + p.X, Y + p.Y);
		}

		public Pixel Sub(Pixel p) {
			return new Pixel(X - p.X, Y - p.Y);
		}

		public Pixel Mul(Pixel p) {
			return new Pixel(X * p.X, Y * p.Y);
		}

		public Pixel Div(Pixel p) {
			return new Pixel(X / p.X, Y / p.Y);
		}

		public Pixel Mod(Pixel p) {
			return new Pixel(X % p.X, Y % p.Y);
		}

		public Pixel Exp(Pixel p) {
			int x = 0, y = 0;
			if (p.X == 0) x = 1;
			else if (p.X == 1) x = X;
			else if (p.X == 2) x = X * X;
			else if (p.X == 3) x = X * X * X;
			else x = (int)Math.Round(Math.Pow(X, p.X));
			if (p.Y == 0) y = 1;
			else if (p.Y == 1) y = Y;
			else if (p.Y == 2) y = Y * Y;
			else if (p.Y == 3) y = Y * Y * Y;
			else y = (int)Math.Round(Math.Pow(Y, p.Y));

			return new Pixel(x, y);
		}

		public static Pixel operator +(Pixel a, Pixel b) => a.Add(b);
		public static Pixel operator -(Pixel a, Pixel b) => a.Sub(b);
		public static Pixel operator *(Pixel a, Pixel b) => a.Mul(b);
		public static Pixel operator /(Pixel a, Pixel b) => a.Div(b);
		public static Pixel operator %(Pixel a, Pixel b) => a.Mod(b);

	}
}
