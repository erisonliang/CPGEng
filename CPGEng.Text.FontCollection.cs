using System;
using System.IO;

namespace CPGEng.Text {
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
