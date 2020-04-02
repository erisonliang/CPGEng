/*
* Crispycat PixelGraphic Engine
* CPGEng.Effects.DitherMatrix.cs; Dither matrices
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

namespace CPGEng.Effects {
	public static class DitherMatrix {
		public static int Size2x2 = 2;
		public static int Size4x4 = 4;
		public static int Size8x8 = 8;
		public static int Size16x16 = 16;

		/*
		 * Some of the matrices here are taken from a paper by C. A. Bouman:
		 * https://engineering.purdue.edu/~bouman/ece637/notes/pdf/Halftoning.pdf
		*/

		public static byte[,] DispersedDitherMatrix2x2 = {
			{1, 2},
			{3, 0}
		};

		public static byte[,] DispersedDitherMatrix4x4 = {
			{ 5,  9,  6, 10},
			{13,  1, 14,  2},
			{ 7, 11,  4,  8},
			{15,  3, 12,  0}
		};

		public static byte[,] DispersedDitherMatrix8x8 = {
			{21, 37, 25, 41, 22, 38, 26, 42},
			{53,  5, 57,  9, 54,  6, 58, 10},
			{29, 45, 17, 33, 30, 46, 18, 34},
			{61, 13, 49,  1, 62, 14, 50,  2},
			{23, 39, 27, 43, 20, 36, 24, 40},
			{55,  7, 59, 11, 52,  4, 56,  8},
			{31, 47, 19, 35, 28, 44, 16, 32},
			{63, 15, 51,  3, 60, 12, 48,  0}
		};

		public static byte[,] ClusteredDitherMatrix2x2 = {
			{3, 0},
			{2, 1}
		};

		public static byte[,] ClusteredDitherMatrix4x4 = {
			{15, 10, 11, 12},
			{ 9,  3,  0,  4},
			{ 8,  2,  1,  5},
			{14,  7,  6, 13}
		};

		public static byte[,] ClusteredDitherMatrix8x8 = {
			{62, 57, 48, 36, 37, 49, 58, 63},
			{56, 47, 35, 21, 22, 38, 50, 59},
			{46, 34, 20, 10, 11, 23, 39, 51},
			{33, 19,  9,  3,  0,  4, 12, 24},
			{32, 18,  8,  2,  1,  5, 13, 25},
			{45, 31, 17,  7,  6, 14, 26, 40},
			{55, 44, 30, 16, 15, 27, 41, 52},
			{61, 54, 43, 29, 28, 42, 53, 60}
		};
	}
}