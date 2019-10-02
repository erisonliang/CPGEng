/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Lerp.cs; Lerping functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/09/29
*/

using System;

namespace CPGEng {
	public static class Lerper {
		public static double Lerp(double a, double b, double t) {
			return (1 - t) * a + t * b;
		}
	}
}