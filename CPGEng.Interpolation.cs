/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Interpolation.cs; Interpolation functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2019/10/07
*/

namespace CPGEng {
	public static class Interpolation {
		public static double Lerp(double s, double e, double t) {
			return s + (e - s) * t;
		}

		public static double Blerp(double c00, double c10, double c01, double c11, double tx, double ty) {
			return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
		}
	}
}