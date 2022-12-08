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
    public sealed class ID3v2APICFrame : ID3v2Frame
    {
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
            var encoding = ID3v2Encoding.Select(header, description + mimeType);

            // header, encoding[1], mimeType+0, pitureType[1], description+0, data
            var descriptionBytes = ID3v2Encoding.GetBytes(encoding, description, true);
            var mimeTypeBytes = ID3v2Encoding.GetBytes(encoding, mimeType, true);
            var contentSize = descriptionBytes.Length + mimeTypeBytes.Length + 1 + 1 + imageData.Length;
            var frameHeader = ID3v2FrameHeader.Create(header, "APIC", flags, contentSize);
            using var ms = new MemoryStream();
            var writer = new DataWriter(ms);
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

        ID3v2PictureType pictureType;
        string mimeType;
        string description;
        int imageDataStart;

        #region Parser functions
        void Parse()
        {
            var encoding = (ID3v2EncodingType)Content[0];
            var index = 1 + ID3v2Encoding.Parse(0, Content, 1, out mimeType);
            pictureType = (ID3v2PictureType)Content[index++];
            index += ID3v2Encoding.Parse(encoding, Content, index, out description);
            imageDataStart = index;
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
        /// Gets the mime type of the picture.
        /// </summary>
        public string MimeType
        {
            get
            {
                if (imageDataStart <= 0)
                {
                    Parse();
                }

                return mimeType;
            }
        }

        /// <summary>
        /// Gets the type of the picture.
        /// </summary>
        public ID3v2PictureType PictureType
        {
            get
            {
                if (imageDataStart <= 0)
                {
                    Parse();
                }

                return pictureType;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get
            {
                if (imageDataStart <= 0)
                {
                    Parse();
                }

                return description;
            }
        }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        public byte[] ImageData
        {
            get
            {
                if (imageDataStart <= 0)
                {
                    Parse();
                }

                return new DataFrameReader(Content).Read(imageDataStart, Content.Length - imageDataStart);
            }
        }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] MimeType "Description".</returns>
        public override string ToString() => base.ToString() + " " + MimeType + " \"" + Description + '"';
    }
}
