/*
 * Crispycat PixelGraphic Engine
 * CPGEng.Buffer.cs; Buffer objects and functions
 * (C) 2019 crispycat; https://github.com/crispycat0/CPGEng
 * 2019/09/29
*/

namespace CPGEng {
	public class Buffer {
		private byte[] data;
		public readonly uint Length;

		public Buffer(uint l = 256, byte v = 0) {
			Length = l;
			data = new byte[l];
			if (v > 0) for (int b = 0; b < l; b++) data[b] = v;
		}

		public Buffer(byte[] d) {
			data = d;
			Length = (uint)d.Length;
		}

		public byte Get(uint n = 0) {
			if (n >= Length) return 0;
			return data[n];
		}

		public void Set(uint n, byte b) {
			if (n >= Length) return;
			data[n] = b;
		}

		public void Set(uint n, uint b) {
			if (n >= Length) return;
			data[n] = (byte)((b < 256) ? b : b % 256);
		}

		public byte[] Data() {
			return (byte[])data.Clone();
		}
	}
}
