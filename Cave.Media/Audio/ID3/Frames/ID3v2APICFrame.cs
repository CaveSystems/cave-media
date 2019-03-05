#region License ID3v2
/*
    This file contains the implementation for the following drafts:

    + Informal Standard - Document: id3v2.4 - M. Nilsson - 1st November 2000
    + Informal Standard - Document: id3v2.3 - M. Nilsson - 3rd February 1999
    + Informal Standard - Document: id3v2-00 - M. Nilsson - 26th March 1998

    Additionally the documentation was extracted from the Informal Standard
    Documents and is used to describe the classes, properties and values.

    The ID3v2 logo is copyright (c) 1998 Martin Nilsson. You may freely use the
    ID3v2 logo on pages containing ID3v2 information and in programs supporting
    the ID3v2 standard.

    For redistribution details on the documentation only you can check
    http://www.id3.org/Copyright

    For details on other parts (everything except the documentation) of the
    sourcecode or software please check all other license headers, files
    and copyright informations of the package!
*/
#endregion

using System;
using System.IO;
using Cave.IO;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// This frame contains a picture directly related to the audio file.
    /// Image format is the MIME type and subtype [MIME] for the image.
    /// </summary>
    public sealed class ID3v2APICFrame: ID3v2Frame
    {
#if SKIA && (NETSTANDARD20 || NET45 || NET46 || NET471)
		/// <summary>
		/// Creates a new header.
		/// </summary>
		/// <param name="header">The tag header.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="description">The description.</param>
		/// <param name="type">The type.</param>
		/// <param name="image">The image.</param>
		/// <param name="imageFormat">The image format.</param>
		/// <param name="quality">The quality.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static ID3v2APICFrame Create(ID3v2Header header, ID3v2FrameFlags flags, string description, ID3v2PictureType type, 
            SkiaSharp.SKImage image, SkiaSharp.SKEncodedImageFormat imageFormat = SkiaSharp.SKEncodedImageFormat.Jpeg, int quality = 99)
        {
            var data = image.Encode(imageFormat, quality);
            string mimeType;
            switch (imageFormat)
            {
                case SkiaSharp.SKEncodedImageFormat.Jpeg: mimeType = MimeTypes.FromExtension(".jpg"); break;
                case SkiaSharp.SKEncodedImageFormat.Png: mimeType = MimeTypes.FromExtension(".png"); break;
                default: throw new ArgumentOutOfRangeException(string.Format("ImageFormat {0} not suppoerted!", imageFormat));
            }
            return Create(header, flags, description, type, mimeType, data.ToArray());
        }
#elif NET20 || NET35 || NET40 || !SKIA
#else
#error No code defined for the current framework or NETXX version define missing!
#endif

        /// <summary>Creates the specified header.</summary>
        /// <param name="header">The header.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="description">The description.</param>
        /// <param name="type">The type.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="imageData">The image data.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static ID3v2APICFrame Create(ID3v2Header header, ID3v2FrameFlags flags, string description, ID3v2PictureType type, string mimeType, byte[] imageData)
        {
            ID3v2EncodingType encoding = ID3v2Encoding.Select(header, description + mimeType);
            //header, encoding[1], mimeType+0, pitureType[1], description+0, data
            byte[] descriptionBytes = ID3v2Encoding.GetBytes(encoding, description, true);
            byte[] mimeTypeBytes = ID3v2Encoding.GetBytes(encoding, mimeType, true);
            int contentSize = descriptionBytes.Length + mimeTypeBytes.Length + 1 + 1 + imageData.Length;
            ID3v2FrameHeader frameHeader = ID3v2FrameHeader.Create(header, "APIC", flags, contentSize);
            using (MemoryStream ms = new MemoryStream())
            {
                DataWriter writer = new DataWriter(ms);
                writer.Write(frameHeader.Data);
                writer.Write((byte)encoding);
                writer.Write(mimeTypeBytes);
                writer.Write((byte)type);
                writer.Write(descriptionBytes);
                writer.Write(imageData);
                if (frameHeader.HeaderSize + contentSize != ms.Position)
                {
                    throw new Exception();
                }

                return new ID3v2APICFrame(new ID3v2Frame(header, ms.ToArray()));
            }
        }

        ID3v2PictureType m_PictureType;
        string m_MimeType;
        string m_Description;
        int m_ImageDataStart;

        #region Parser functions
        void Parse()
        {
            ID3v2EncodingType encoding = (ID3v2EncodingType)m_Content[0];
            int index = 1 + ID3v2Encoding.Parse(0, m_Content, 1, out m_MimeType);
            m_PictureType = (ID3v2PictureType)m_Content[index++];
            index += ID3v2Encoding.Parse(encoding, m_Content, index, out m_Description);
            m_ImageDataStart = index;
        }

        #endregion

        internal ID3v2APICFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "APIC")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "APIC"));
            }
        }

        /// <summary>
        /// Obtains the mime type of the picture
        /// </summary>
        public string MimeType
        {
            get
            {
                if (m_ImageDataStart <= 0)
                {
                    Parse();
                }

                return m_MimeType;
            }
        }

        /// <summary>
        /// Obtains the type of the picture
        /// </summary>
        public ID3v2PictureType PictureType
        {
            get
            {
                if (m_ImageDataStart <= 0)
                {
                    Parse();
                }

                return m_PictureType;
            }
        }

        /// <summary>
        /// Obtains the description
        /// </summary>
        public string Description
        {
            get
            {
                if (m_ImageDataStart <= 0)
                {
                    Parse();
                }

                return m_Description;
            }
        }

        /// <summary>
        /// Obtains the image data
        /// </summary>
        public byte[] ImageData
        {
            get
            {
                if (m_ImageDataStart <= 0)
                {
                    Parse();
                }

                return new DataFrameReader(m_Content).Read(m_ImageDataStart, m_Content.Length - m_ImageDataStart);
            }
        }

        /// <summary>
        /// Obtains a string describing this frame
        /// </summary>
        /// <returns>ID[Length] MimeType "Description"</returns>
        public override string ToString()
        {
            return base.ToString() + " " + MimeType + " \"" + Description + '"';
        }
    }
}
