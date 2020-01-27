/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Interpolation.cs; Interpolation functions
 * (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2020/01/27
*/

namespace CPGEng {
	public static class Interpolation {
		/// <summary>Linear interpolation function</summary>
		/// <param name="c0">C0</param>
		/// <param name="c1">C1</param>
		/// <param name="t">Time</param>
		/// <returns>double</returns>
		public static double Lerp(double c0, double c1, double t) {
			return c0 + (c1 - c0) * t;
		}

		/// <summary>Bilinear interpolation function</summary>
		/// <param name="c00">C0,0</param>
		/// <param name="c01">C0,1</param>
		/// <param name="c10">C1,0</param>
		/// <param name="c11">C1,1</param>
		/// <param name="tx">X Time</param>
		/// <param name="ty">Y Time</param>
		/// <returns>double</returns>
		public static double Blerp(double c00, double c10, double c01, double c11, double tx, double ty) {
			return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
		}
	}
}