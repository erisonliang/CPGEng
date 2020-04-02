/*
* Crispycat PixelGraphic Engine
* CPGEng.Text.Font.cs; Font class;
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CPGEng.Text {
	public class Font {
		public BitArray Data;
		public string Path;
		public int Width, Height, Spacing, Offset;

		byte ReverseByte(byte v) {
			byte r = v;
			int s = 7;
			for (v >>= 1; v != 0; v >>= 1) {
				r <<= 1;
				r |= (byte)(v & 1);
				s--;
			}
			r <<= s;
			return r;
		}

		/// <summary>Creates a new Font.</summary>
		public Font(string path, int width, int height, int spacing = 1, int offset = 32) {
			Path = path;
			Width = width;
			Height = height;
			Spacing = spacing;
			Offset = offset;

			byte[] data = new byte[Width * Height * 32];

			if (File.Exists(path)) {
				data = File.ReadAllBytes(Path);
			}

			for (int i = 0; i < data.Length; i++) data[i] = ReverseByte(data[i]);

			Data = new BitArray(data);
		}

		int GetPixel(int chr, int row, int col) {
			chr -= Offset;
			int ind = chr * Width * Height + row * Width + col;

			int r = (ind < Data.Length) ? Convert.ToInt32(Data[ind]) : 0;
			return r;
		}

		/// <summary>Gets a character from the Font.</summary>
		/// <param name="chr">char Character</param>
		/// <returns>Pixel[]</returns>
		public Pixel[] GetCharacter(int chr) {
			List<Pixel> pixels = new List<Pixel>();

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++) {
					if (GetPixel(chr, i, j) == 1) pixels.Add(new Pixel(j, i));
				}
			}

			return pixels.ToArray();
		}

		/// <summary>Gets a string of text from the Font.</summary>
		/// <param name="text">string Text</param>
		/// <returns>Pixel[]</returns>
		public Pixel[] GetString(string text) {
			List<Pixel> pixels = new List<Pixel>();
			char[] chars = text.ToCharArray();

			for (int i = 0; i < chars.Length; i++) {
				Pixel[] chr = GetCharacter(chars[i]);
				for (int j = 0; j < chr.Length; j++) {
					pixels.Add(chr[j] + new Pixel((Width + Spacing) * i, 0));
				}
			}

			return pixels.ToArray();
		}
	}
}