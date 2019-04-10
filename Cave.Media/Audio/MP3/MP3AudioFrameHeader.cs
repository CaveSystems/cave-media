using System;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides a mp3 audio frame header implementation.
    /// </summary>
    public class MP3AudioFrameHeader
    {
        static readonly ushort[] Version1Layer1 = new ushort[] { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448 };
        static readonly ushort[] Version1Layer2 = new ushort[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384 };
        static readonly ushort[] Version1Layer3 = new ushort[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320 };
        static readonly ushort[] Version2Layer1 = new ushort[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256 };
        static readonly ushort[] Version2Layer2 = new ushort[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160 };

        /// <summary>
        /// Obtains the MPEG Version.
        /// </summary>
        public readonly MP3AudioFrameVersion Version;

        /// <summary>
        /// Obtains the MPEG Layer.
        /// </summary>
        public readonly MP3AudioFrameLayer Layer;

        /// <summary>
        /// BitRate in kbps.
        /// </summary>
        public readonly ushort BitRate;

        /// <summary>
        /// BitRate index.
        /// </summary>
        public readonly ushort BitRateIndex;

        /// <summary>
        /// Obtains the SamplingRate.
        /// </summary>
        public readonly ushort SamplingRate;

        /// <summary>
        /// Obtains the SamplingRate index.
        /// </summary>
        public readonly ushort SamplingRateIndex;

        /// <summary>
        /// Obtains the sample count of this frame.
        /// </summary>
        public readonly uint SampleCount;

        /// <summary>
        /// Obtains whether the frame contains padding or not.
        /// </summary>
        public readonly bool Padding;

        /// <summary>
        /// Obtains whether the private bit is set or not.
        /// </summary>
        public readonly bool Private;

        /// <summary>
        /// Obtains the channel configuration.
        /// </summary>
        public readonly MP3AudioFrameChannels Channels;

        /// <summary>Gets the channel count.</summary>
        /// <value>The channel count.</value>
        public int ChannelCount => Channels == MP3AudioFrameChannels.Mono ? 1 : 2;

        /// <summary>
        /// Obtains the mode extension.
        /// </summary>
        public readonly byte ModeExtension;

        /// <summary>
        /// Obtains whether the copyright flag is set or not.
        /// </summary>
        public readonly bool Copyright;

        /// <summary>
        /// Obtains whether the original flag is set or not.
        /// </summary>
        public readonly bool Original;

        /// <summary>
        /// Obtains the emphasis value.
        /// </summary>
        public readonly byte Emphasis;

        /// <summary>
        /// 16-Bit CRC present after header.
        /// </summary>
        public readonly bool Protection;

        /// <summary>
        /// Obtains whether the header is valid or not.
        /// </summary>
        public readonly MP3AudioFrameHeadervalidation Validation;

        /// <summary>The length in bytes including header.</summary>
        public readonly int Length;

        /// <summary>The slot size.</summary>
        public readonly int SlotSize;

        /// <summary>The 32 bit data of the header.</summary>
        public readonly uint Data;

        static uint GetUint(byte[] value)
        {
            uint u = 0;
            for (int i = 0; i < 4; i++) { u = (u << 8) | value[i]; }
            return u;
        }

        /// <summary>Creates a new header from the specified 4 byte header uint.</summary>
        /// <param name="value">The value.</param>
        public MP3AudioFrameHeader(uint value)
        {
            Data = value;
            Emphasis = (byte)(value & 3);
            value >>= 2;
            Original = (value & 1) == 1;
            value >>= 1;
            Copyright = (value & 1) == 1;
            value >>= 1;
            ModeExtension = (byte)(value & 3);
            value >>= 2;
            Channels = (MP3AudioFrameChannels)(value & 3);
            value >>= 2;
            Private = (value & 1) == 1;
            value >>= 1;
            Padding = (value & 1) == 1;
            value >>= 1;
            SamplingRateIndex = (ushort)(value & 3);
            value >>= 2;
            BitRateIndex = (ushort)(value & 0xF);
            value >>= 4;
            if ((BitRateIndex == 0) || (BitRateIndex > 14))
            {
                Validation = MP3AudioFrameHeadervalidation.InvalidBitRate;
                return;
            }
            Protection = (value & 1) == 0;
            value >>= 1;
            Layer = (MP3AudioFrameLayer)(value & 3);
            value >>= 2;
            Version = (MP3AudioFrameVersion)(value & 3);
            value >>= 2;
            if (value != 0x7FF)
            {
                Validation = MP3AudioFrameHeadervalidation.InvalidHeader;
                return;
            }

            int sizeMultiplier;
            switch (Version)
            {
                #region MPEG Version 1
                case MP3AudioFrameVersion.Version1:
                    switch (Layer)
                    {
                        case MP3AudioFrameLayer.Layer1: BitRate = Version1Layer1[BitRateIndex]; SampleCount = 384; sizeMultiplier = 12000; break;
                        case MP3AudioFrameLayer.Layer2: BitRate = Version1Layer2[BitRateIndex]; SampleCount = 1152; sizeMultiplier = 144000; break;
                        case MP3AudioFrameLayer.Layer3: BitRate = Version1Layer3[BitRateIndex]; SampleCount = 1152; sizeMultiplier = 144000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidLayer; return;
                    }
                    switch (SamplingRateIndex)
                    {
                        case 0: SamplingRate = 44100; break;
                        case 1: SamplingRate = 48000; break;
                        case 2: SamplingRate = 32000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidSampleRate; return;
                    }
                    break;
                #endregion

                #region MPEG Version 2
                case MP3AudioFrameVersion.Version2:
                    switch (Layer)
                    {
                        case MP3AudioFrameLayer.Layer1: BitRate = Version2Layer1[BitRateIndex]; SampleCount = 384; sizeMultiplier = 12000; break;
                        case MP3AudioFrameLayer.Layer2: BitRate = Version2Layer2[BitRateIndex]; SampleCount = 1152; sizeMultiplier = 144000; break;
                        case MP3AudioFrameLayer.Layer3: BitRate = Version2Layer2[BitRateIndex]; SampleCount = 576; sizeMultiplier = 72000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidLayer; return;
                    }
                    switch (SamplingRateIndex)
                    {
                        case 0: SamplingRate = 22050; break;
                        case 1: SamplingRate = 24000; break;
                        case 2: SamplingRate = 16000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidSampleRate; return;
                    }
                    break;
                #endregion

                #region MPEG Version 3
                case MP3AudioFrameVersion.Version25:
                    switch (Layer)
                    {
                        case MP3AudioFrameLayer.Layer1: BitRate = Version2Layer1[BitRateIndex]; SampleCount = 384; sizeMultiplier = 12000; break;
                        case MP3AudioFrameLayer.Layer2: BitRate = Version2Layer2[BitRateIndex]; SampleCount = 1152; sizeMultiplier = 144000; break;
                        case MP3AudioFrameLayer.Layer3: BitRate = Version2Layer2[BitRateIndex]; SampleCount = 576; sizeMultiplier = 72000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidLayer; return;
                    }
                    switch (SamplingRateIndex)
                    {
                        case 0: SamplingRate = 11025; break;
                        case 1: SamplingRate = 12000; break;
                        case 2: SamplingRate = 8000; break;
                        default: Validation = MP3AudioFrameHeadervalidation.InvalidSampleRate; return;
                    }
                    break;
                #endregion

                default: Validation = MP3AudioFrameHeadervalidation.InvalidVersion; return;
            }
            SlotSize = (Layer == MP3AudioFrameLayer.Layer1) ? 4 : 1;
            Length = ((sizeMultiplier * BitRate / SamplingRate) + (Padding ? 1 : 0)) * SlotSize;
        }

        /// <summary>
        /// Creates a new header from the specified 4 bytes data.
        /// </summary>
        /// <param name="value"></param>
        public MP3AudioFrameHeader(byte[] value)
            : this(GetUint(value)) { }

        /// <summary>Gets the slots.</summary>
        /// <value>The slots.</value>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int Slots
        {
            get
            {
                switch (Layer)
                {
                    default: throw new NotSupportedException(string.Format("Layer {0} not supported!", Layer));
                    case MP3AudioFrameLayer.Layer1:
                    case MP3AudioFrameLayer.Layer2:
                        return 0;

                    case MP3AudioFrameLayer.Layer3:
                        int channelBytes = 0;
                        int l_FrameSize = Length;
                        switch (Version)
                        {
                            default: throw new NotSupportedException(string.Format("Version {0} not supported!", Version));
                            case MP3AudioFrameVersion.Version1:
                                channelBytes = Channels == MP3AudioFrameChannels.Mono ? 17 : 32;
                                break;

                            case MP3AudioFrameVersion.Version2:
                            case MP3AudioFrameVersion.Version25:
                                channelBytes = Channels == MP3AudioFrameChannels.Mono ? 9 : 17;
                                break;
                        }
                        if (!Protection)
                        {
                            return l_FrameSize - channelBytes - 4;
                        }

                        return l_FrameSize - channelBytes - 4 - 2;
                }
            }
        }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "MP3FrameHeader " + Version + " " + Layer + " " + BitRate + " kBit " + SamplingRate + " Hz " + Channels + " " + Length + " bytes (pad " + (Padding ? 1 : 0) + ", crc " + (Protection ? 2 : 0) + ")";
        }

    }
}
