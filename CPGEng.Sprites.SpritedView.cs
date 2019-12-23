/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Sprites.SpritedView.cs; Sprited View objects
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
 * 2019/12/22
*/

using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace CPGEng.Sprites {
	public class SpritedView : View {
		public List<Sprite> Sprites = new List<Sprite>();
		
		public SpritedView(uint w, uint h, uint d = 96) : base(w, h, d) { }

		/// <summary>Returns a BitmapSource created from the View including Sprites.</summary>
		/// <returns>BitmapSource</returns>
		public BitmapSource ToBitmapSourceWithSprites() {
			return Bitmap.FromView(this).ToBitmapSource();
		}
	}
}
