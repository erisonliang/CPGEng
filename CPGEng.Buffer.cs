/*
* Crispycat PixelGraphic Engine
* CPGEng.Buffer.cs; Buffer objects and functions
* (C) 2020 crispycat; https://github.com/crispycat0/CPGEng/LICENSE
* 2020/04/01
*/

namespace CPGEng {
	public class Buffer {
		private byte[] data;
		public readonly uint Length;
		
		/// <summary>Creates a new Buffer.</summary>
		/// <param name="l">Length</param>
		/// <param name="v">Default value</param>
		public Buffer(uint l = 256, byte v = 0) {
			Length = l;
			data = new byte[l];
			if (v > 0) for (int b = 0; b < l; b++) data[b] = v;
		}

		/// <summary>Create a Buffer from a byte[]</summary>
		/// <param name="d">Byte[] data</param>
		public Buffer(byte[] d) {
			data = d;
			Length = (uint)d.Length;
		}

		/// <summary>Create a Buffer from another Buffer</summary>
		/// <param name="b">Buffer buffer</param>
		public Buffer(Buffer b) {
			data = (byte[])b.data.Clone();
			Length = b.Length;
		}

		/// <summary>Get byte</summary>
		/// <param name="n">Index</param>
		/// <returns>byte</returns>
		public byte Get(uint n = 0) {
			if (n >= Length) return 0;
			return data[n];
		}

		/// <summary>Set byte</summary>
		/// <param name="n">Index</param>
		/// <param name="b">Value</param>
		public void Set(uint n, byte b) {
			if (n >= Length) return;
			data[n] = b;
		}

		/// <summary>Set byte</summary>
		/// <param name="n">Index</param>
		/// <param name="b">Value</param>
		public void Set(uint n, uint b) {
			if (n >= Length) return;
			data[n] = (byte)((b < 256) ? b : b % 256);
		}

		/// <summary>Get data</summary>
		/// <returns>byte[]</returns>
		public byte[] Data() {
			return (byte[])data.Clone();
		}
	}
}
