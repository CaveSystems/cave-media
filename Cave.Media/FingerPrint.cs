using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cave.IO;
using Cave.Text;

namespace Cave.Media
{
	/// <summary>
	/// Provides very simple finger printing for images.
	/// </summary>
	public class FingerPrint
    {
		/// <summary>
		/// Creates a 32x32 fingerprint for the specified bitmap.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns>Returns a fingerprint with 6 bits per pixel (32 px = 6144 bit = 768 byte = 1024 base32 chars)</returns>
		public static FingerPrint Create(IBitmap32 bitmap)
		{
			using (var thumb = bitmap.Resize(32, 32, ResizeFlags.KeepAspect))
			{
				var data = thumb.Data;
				using (MemoryStream ms = new MemoryStream())
				{
					//calculate fingerprint and distance matrix
					BitStreamWriter writer = new BitStreamWriter(ms);
					float[] distanceMatrix = new float[16];
					{
						int x = 0, y = 0;
						ARGB last = 0;
						foreach (ARGB pixel in data.Data)
						{
							if (++x > 15)
							{
								x = 0;
								++y;
							}

							int r = pixel.Red >> 6;
							int g = pixel.Green >> 6;
							int b = pixel.Blue >> 6;
							writer.WriteBits(r, 2);
							writer.WriteBits(g, 2);
							writer.WriteBits(b, 2);

							unchecked
							{
								int i = ((y << 1) & 0xC) + (x >> 2);
								var distance = Math.Abs(pixel.GetDistance(last));
								distanceMatrix[i] += distance;
								last = pixel;
							}
						}
					}
					//normalize matrix
					float maxDistance = distanceMatrix.Max();
					for (int i = 0; i < distanceMatrix.Length; i++) distanceMatrix[i] /= maxDistance;
					//calculate blocks
					uint[] blocks = new uint[4];
					int[] index = new int[] { 0, 2, 8, 10 };
					for(int i = 0; i < 4; i++)
					{
						int idx = index[i];
						uint blockValue = (uint)(255 * distanceMatrix[idx]) << 24;
						blockValue |= (uint)(255 * distanceMatrix[idx + 1]) << 16;
						blockValue |= (uint)(255 * distanceMatrix[idx + 4]) << 8;
						blockValue |= (uint)(255 * distanceMatrix[idx + 5]);
						blocks[i] = blockValue;
					}

					/*
					uint b1 = (uint)(uint.MaxValue * (distanceMatrix[0] + distanceMatrix[1]  + distanceMatrix[4] + distanceMatrix[5]) /4);
					uint b2 = (uint)(uint.MaxValue * (distanceMatrix[3] + distanceMatrix[2]  + distanceMatrix[7] + distanceMatrix[6]) / 4);
					uint b3 = (uint)(uint.MaxValue * (distanceMatrix[12] + distanceMatrix[13]  + distanceMatrix[8]  + distanceMatrix[9]) / 4);
					uint b4 = (uint)(uint.MaxValue * (distanceMatrix[15] + distanceMatrix[14]  + distanceMatrix[11] + distanceMatrix[10]) / 4);
					*/
					return new FingerPrint(32, blocks, ms.ToArray());
				}
			}
		}
		
		string base32data;
		byte[] data;
		uint[] blocks;

		/// <summary>
		/// Gets the size in pixels.
		/// </summary>
		public int PixelSize { get; }

		/// <summary>
		/// Gets the 4 bits blocks (1 bits per 4x4 pin)
		/// </summary>
		public uint[] Blocks { get => blocks; }

		/// <summary>
		/// Gets the full fingerprint data.
		/// </summary>
		public byte[] Data { get => data; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FingerPrint"/> class.
		/// </summary>
		/// <param name="pixelSize">Size in pixels.</param>
		/// <param name="blocks">The blocks.</param>
		/// <param name="data">The data.</param>
		public FingerPrint(int pixelSize, uint[] blocks, byte[] data)
		{
			PixelSize = pixelSize;
			this.blocks = blocks;
			this.data = data;
			if (data.Length != (pixelSize * pixelSize * 6 +7)/ 8) throw new ArgumentOutOfRangeException("data.Length", "Data length is out of range!");
		}

		/// <summary>
		/// Gets a name for the image file.
		/// </summary>
		public string FileName
		{
			get
			{
				return blocks.Select(b => b.ToHexString()).Join('-');
			}
		}

		/// <summary>
		/// Gets a identifier for the fingerprint (this is not collision free).
		/// </summary>
		public Guid Guid
		{
			get
			{
				byte[] data = new byte[16];
				for (int i = 0; i < 4; i++) BitConverterBE.Instance.GetBytes(blocks[i]).CopyTo(data, i << 2);
				return new Guid(data);
			}
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			FingerPrint other = obj as FingerPrint;
			if (other == null) return false;
			return ToString().Equals(other.ToString());
		}

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (base32data == null)
			{
				base32data = Base32.Safe.Encode(Data);
			}
			return base32data;
		}
	}
}
