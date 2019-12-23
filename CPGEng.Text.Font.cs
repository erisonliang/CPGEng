/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Text.Font.cs; Font functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2019/12/22
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
		/// <param name="chr">Character</param>
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
		/// <param name="text">Text string</param>
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

	public class FontCollection {
		public Font[] Fonts = new Font[1];
		public string Path, Name;

		/// <summary>Creates a new FontCollection.</summary>
		/// <param name="path">Path to a folder containing a valid bitmap font</param>
		public FontCollection(string path) {
			Path = path;
			if (Directory.Exists(path)) {
				if (File.Exists($"{path}\\_font.inf")) {
					string file = File.ReadAllText($"{path}\\_font.inf").Replace("\r", "").Replace("\n", "");
					string[] info = file.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string font in info) {
						string[] fontinfo = font.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
						if (Name == null) Name = fontinfo[0];

						int size, cols, rows, spacing, offset;
						int.TryParse(fontinfo[1], out size);
						int.TryParse(fontinfo[3], out cols);
						int.TryParse(fontinfo[4], out rows);
						int.TryParse(fontinfo[5], out spacing);
						int.TryParse(fontinfo[6], out offset);

						if (size >= Fonts.Length) Array.Resize(ref Fonts, size + 1);

						Fonts[size] = new Font($"{path}\\" + fontinfo[2], cols, rows, spacing, offset);
					}
				}
			}
		}

		/// <summary>Gets a character from the FontCollection.</summary>
		/// <param name="chr">Character</param>
		/// <param name="size">Size</param>
		/// <returns>Pixel[]</returns>
		public Pixel[] GetCharacter(int chr, int size) {
			if (Fonts.Length > size && Fonts[size] != null) return Fonts[size].GetCharacter(chr);
			return new Pixel[0];
		}

		/// <summary>Gets a string of text from the FontCollection.</summary>
		/// <param name="text">Text string</param>
		/// <param name="size">Size</param>
		/// <returns>Pixel[]</returns>
		public Pixel[] GetString(string text, int size) {
			if (Fonts.Length > size && Fonts[size] != null) return Fonts[size].GetString(text);
			return new Pixel[0];
		}
	}
}