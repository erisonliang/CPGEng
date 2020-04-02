/*
* Crispycat PixelGraphic Engine
* CPGEng.Sprites.Sprite.cs; Sprites and functions
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

namespace CPGEng.Sprites {
	public class Sprite {
		public Pixel Position, Size;
		public BitmapData Texture;
		public Pixel[] TextureMask;

		public Sprite(BitmapData tex, Pixel[] texm, Pixel size, Pixel pos = new Pixel()) {
			Position = pos;
			Size = size;
			Texture = tex;
			TextureMask = texm;
		}
	}
}
