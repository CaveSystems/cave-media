#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable SA1310 // Field names should not contain underscore

using System;

namespace Cave.Media.Structs
{
    /// <summary>
    /// Four Character Code.
    /// </summary>
    public static class FOURCC
    {
        #region static functions

        /// <summary>
        /// Gets the string of the <see cref="FOURCC"/> int.
        /// </summary>
        /// <param name="fcc">The four character code.</param>
        /// <returns>The four character code as string.</returns>
        public static string ToString(uint fcc) => new string(new char[] { (char)((fcc >> 0) & 0xFF), (char)((fcc >> 8) & 0xFF), (char)((fcc >> 16) & 0xFF), (char)((fcc >> 24) & 0xFF) });

        /// <summary>
        /// Gets the string of the <see cref="FOURCC"/> int.
        /// </summary>
        /// <param name="fcc">The four character code.</param>
        /// <returns>The four character code as string.</returns>
        public static string ToString(int fcc) => new string(new char[] { (char)((fcc >> 0) & 0xFF), (char)((fcc >> 8) & 0xFF), (char)((fcc >> 16) & 0xFF), (char)((fcc >> 24) & 0xFF) });

        /// <summary>
        /// Gets the int of the <see cref="FOURCC"/> string.
        /// </summary>
        /// <param name="fcc">The four character code.</param>
        /// <returns>Returns the four character code as 32 bit integer.</returns>
        public static uint ToFCC(string fcc)
        {
            if (fcc == null)
            {
                throw new ArgumentNullException("fcc");
            }

            if (fcc.Length != 4)
            {
                throw new ArgumentException("FCC string is invalid!");
            }

            return (((uint)fcc[3]) << 24) | (((uint)fcc[2]) << 16) | (((uint)fcc[1]) << 8) | (((uint)fcc[0]) << 0);
        }

        /// <summary>
        /// Gets the int of the <see cref="FOURCC"/> characters.
        /// </summary>
        /// <param name="fcc">The four character code.</param>
        /// <returns>Returns the four character code as 32 bit integer.</returns>
        public static uint ToFCC(params char[] fcc)
        {
            if (fcc == null)
            {
                throw new ArgumentNullException("fcc");
            }

            if (fcc.Length != 4)
            {
                throw new ArgumentException("FCC chars are invalid!");
            }

            return (((uint)fcc[3]) << 24) | (((uint)fcc[2]) << 16) | (((uint)fcc[1]) << 8) | (((uint)fcc[0]) << 0);
        }
        #endregion

        /// <summary>
        /// Stream type: VIDEO.
        /// </summary>
        public static readonly uint STREAMTYPE_VIDEO = ToFCC("vids");

        /// <summary>
        /// Stream type: AUDIO.
        /// </summary>
        public static readonly uint STREAMTYPE_AUDIO = ToFCC("auds");

        /// <summary>
        /// Stream type: MIDI.
        /// </summary>
        public static readonly uint STREAMTYPE_MIDI = ToFCC("mids");

        /// <summary>
        /// Stream type: TEXT.
        /// </summary>
        public static readonly uint STREAMTYPE_TEXT = ToFCC("txts");

        /// <summary>
        /// Main AVI Header.
        /// </summary>
        public static readonly uint AVIMAINHEADER = ToFCC("avih");

        /// <summary>
        /// Movilist.
        /// </summary>
        public static readonly uint MOVILIST = ToFCC("movi");

        /// <summary>
        /// OpenDML List.
        /// </summary>
        public static readonly uint OPENDMLLIST = ToFCC("odml");

        /// <summary>
        /// OpenDML Header.
        /// </summary>
        public static readonly uint OPENDMLHEADER = ToFCC("dmlh");

        /// <summary>
        /// OpenDML Index.
        /// </summary>
        public static readonly uint OPENDMLINDEX = ToFCC("indx");

        /// <summary>
        /// Stream list.
        /// </summary>
        public static readonly uint STREAMLIST = ToFCC("strl");

        /// <summary>
        /// Stream header.
        /// </summary>
        public static readonly uint STREAMHEADER = ToFCC("strh");

        /// <summary>
        /// Stream format.
        /// </summary>
        public static readonly uint STREAMFORMAT = ToFCC("strf");

        /// <summary>
        /// AVI OLD Index.
        /// </summary>
        public static readonly uint AVIOLDINDEX = ToFCC("idx1");

        /// <summary>
        /// WAVE data.
        /// </summary>
        public static readonly uint data = ToFCC("data");

        /// <summary>
        /// JUNK (may be ascii text with encoder informations...)
        /// </summary>
        public static readonly uint JUNK = ToFCC("JUNK");

        /// <summary>
        /// INFO (some codec specific informations).
        /// </summary>
        public static readonly uint INFO = ToFCC("INFO");

        /// <summary>
        /// Header list.
        /// </summary>
        public static readonly uint HEADERLIST = ToFCC("hdrl");

        /// <summary>
        /// HiFCC uncompressed bitmap.
        /// </summary>
        public static readonly uint DB = ToFCC("\0\0db");

        /// <summary>
        /// HiFCC compressed bitmap.
        /// </summary>
        public static readonly uint DC = ToFCC("\0\0dc");

        /// <summary>
        /// HiFCC palette change.
        /// </summary>
        public static readonly uint PC = ToFCC("\0\0pc");

        /// <summary>
        /// HiFCC wave.
        /// </summary>
        public static readonly uint WB = ToFCC("\0\0wb");

        /// <summary>
        /// RIFF format header.
        /// </summary>
        public static readonly uint RIFF = ToFCC("RIFF");

        /// <summary>
        /// RIFX format header.
        /// </summary>
        public static readonly uint RIFX = ToFCC("RIFX");

        /// <summary>
        /// LIST header.
        /// </summary>
        public static readonly uint LIST = ToFCC("LIST");

        /// <summary>
        /// AVI Header.
        /// </summary>
        public static readonly uint AVI = ToFCC("AVI ");

        /// <summary>
        /// WAVE Header.
        /// </summary>
        public static readonly uint WAVE = ToFCC("WAVE");

        /// <summary>
        /// MIDI Header.
        /// </summary>
        public static readonly uint RMID = ToFCC("RMID");

        /// <summary>
        /// fmt tag.
        /// </summary>
        public static readonly uint fmt = ToFCC("fmt ");

        /// <summary>
        /// Archival location.
        /// </summary>
        public static readonly uint IARL = ToFCC('I', 'A', 'R', 'L');

        /// <summary>
        /// Artist.
        /// </summary>
        public static readonly uint IART = ToFCC('I', 'A', 'R', 'T');

        /// <summary>
        /// Commissioned.
        /// </summary>
        public static readonly uint ICMS = ToFCC('I', 'C', 'M', 'S');

        /// <summary>
        /// Comments.
        /// </summary>
        public static readonly uint ICMT = ToFCC('I', 'C', 'M', 'T');

        /// <summary>
        /// Copyright.
        /// </summary>
        public static readonly uint ICOP = ToFCC('I', 'C', 'O', 'P');

        /// <summary>
        /// Creation date of subject.
        /// </summary>
        public static readonly uint ICRD = ToFCC('I', 'C', 'R', 'D');

        /// <summary>
        /// Cropped.
        /// </summary>
        public static readonly uint ICRP = ToFCC('I', 'C', 'R', 'P');

        /// <summary>
        /// Dimensions.
        /// </summary>
        public static readonly uint IDIM = ToFCC('I', 'D', 'I', 'M');

        /// <summary>
        /// Dots per inch.
        /// </summary>
        public static readonly uint IDPI = ToFCC('I', 'D', 'P', 'I');

        /// <summary>
        /// Engineer.
        /// </summary>
        public static readonly uint IENG = ToFCC('I', 'E', 'N', 'G');

        /// <summary>
        /// Genre.
        /// </summary>
        public static readonly uint IGNR = ToFCC('I', 'G', 'N', 'R');

        /// <summary>
        /// Keywords.
        /// </summary>
        public static readonly uint IKEY = ToFCC('I', 'K', 'E', 'Y');

        /// <summary>
        /// Lightness settings.
        /// </summary>
        public static readonly uint ILGT = ToFCC('I', 'L', 'G', 'T');

        /// <summary>
        /// Medium.
        /// </summary>
        public static readonly uint IMED = ToFCC('I', 'M', 'E', 'D');

        /// <summary>
        /// Name of subject.
        /// </summary>
        public static readonly uint INAM = ToFCC('I', 'N', 'A', 'M');

        /// <summary>
        /// Palette Settings. No. of colors requested.
        /// </summary>
        public static readonly uint IPLT = ToFCC('I', 'P', 'L', 'T');

        /// <summary>
        /// Product.
        /// </summary>
        public static readonly uint IPRD = ToFCC('I', 'P', 'R', 'D');

        /// <summary>
        /// Subject description.
        /// </summary>
        public static readonly uint ISBJ = ToFCC('I', 'S', 'B', 'J');

        /// <summary>
        /// Software. Name of package used to create file.
        /// </summary>
        public static readonly uint ISFT = ToFCC('I', 'S', 'F', 'T');

        /// <summary>
        /// Sharpness.
        /// </summary>
        public static readonly uint ISHP = ToFCC('I', 'S', 'H', 'P');

        /// <summary>
        /// Source.
        /// </summary>
        public static readonly uint ISRC = ToFCC('I', 'S', 'R', 'C');

        /// <summary>
        /// Source Form. ie slide, paper.
        /// </summary>
        public static readonly uint ISRF = ToFCC('I', 'S', 'R', 'F');

        /// <summary>
        /// Technician who digitized the subject.
        /// </summary>
        public static readonly uint ITCH = ToFCC('I', 'T', 'C', 'H');

        /// <summary>
        /// SMPTE time code of digitization start point expressed as a NULL terminated
        /// text string "HH:MM:SS:FF". If performing MCI capture in AVICAP, this
        /// chunk will be automatically set based on the MCI start time.
        /// </summary>
        public static readonly uint RIFFINFO_ISMP = ToFCC('I', 'S', 'M', 'P');

        /// <summary>
        /// "Digitization Time" Specifies the time and date that the digitization commenced.
        /// The digitization time is contained in an ASCII string which
        /// contains exactly 26 characters and is in the format
        /// "Wed Jan 02 02:03:55 1990\n\0".
        /// The ctime(), asctime(), functions can be used to create strings
        /// in this format. This chunk is automatically added to the capture
        /// file based on the current system time at the moment capture is initiated.
        /// </summary>
        public static readonly uint RIFFINFO_IDIT = ToFCC('I', 'D', 'I', 'T');

        /// <summary>
        /// ASCIIZ representation of the 1-based track number of the content.
        /// </summary>
        public static readonly uint RIFFINFO_ITRK = ToFCC('I', 'T', 'R', 'K');

        /// <summary>
        /// A dump of the table of contents from the CD the content originated from.
        /// </summary>
        public static readonly uint RIFFINFO_ITOC = ToFCC('I', 'T', 'O', 'C');
    }
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
#pragma warning restore SA1310 // Field names should not contain underscore
