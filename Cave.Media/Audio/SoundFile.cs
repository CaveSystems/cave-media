using Cave.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides simple sound (.snd) file reading / writing.
    /// </summary>
    public class SoundFile
    {
        /// <summary>Reads a sound file from the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="NotSupportedException">
        /// Unsupported sample format!
        /// or
        /// Unsupported AudioSampleFormat!.
        /// </exception>
        public static SoundFile Read(Stream stream)
        {
            var reader = new DataReader(stream, endian: EndianType.BigEndian);
            int header = reader.ReadInt32();
            if (header != 0x2e736e64)
            {
                throw new InvalidDataException();
            }

            int dataOffset = reader.ReadInt32();
            int dataSize = reader.ReadInt32();
            int format = reader.ReadInt32();
            AudioSampleFormat sampleFormat;
            switch (format)
            {
                case 2: sampleFormat = AudioSampleFormat.Int8; break;
                case 3: sampleFormat = AudioSampleFormat.Int16; break;
                case 4: sampleFormat = AudioSampleFormat.Int24; break;
                case 5: sampleFormat = AudioSampleFormat.Int32; break;
                case 6: sampleFormat = AudioSampleFormat.Float; break;
                case 7: sampleFormat = AudioSampleFormat.Double; break;
                default: throw new NotSupportedException("Unsupported sample format!");
            }
            int sampleRate = reader.ReadInt32();
            int channels = reader.ReadInt32();
            var config = new AudioConfiguration(sampleRate, sampleFormat, channels);
            string comment = reader.ReadZeroTerminatedString(64 * 1024);
            while ((reader.BaseStream.Position % 8) != 0)
            {
                reader.ReadByte();
            }

            int max = Math.Min(dataSize, (int)(reader.BaseStream.Length - reader.BaseStream.Position));
            byte[] data = reader.ReadBytes(max);

            if (BitConverter.IsLittleEndian)
            {
                // invert data
                int bytes;
                switch (config.Format)
                {
                    case AudioSampleFormat.Int8: bytes = 1; break;
                    case AudioSampleFormat.Int16: bytes = 2; break;
                    case AudioSampleFormat.Int24: bytes = 3; break;
                    case AudioSampleFormat.Int32: bytes = 4; break;
                    case AudioSampleFormat.Float: bytes = 4; break;
                    case AudioSampleFormat.Double: bytes = 8; break;
                    default: throw new NotSupportedException("Unsupported AudioSampleFormat!");
                }
                if (bytes > 1)
                {
                    data = Endian.Swap(data, bytes);
                }
            }

            var result = new SoundFile(config, data);
            result.Comment = comment;
            return result;
        }

        /// <summary>Reads a sound file from the specified file.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static SoundFile Read(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                return Read(fs);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="SoundFile"/> class.</summary>
        /// <param name="config">The configuration.</param>
        /// <param name="data">The data.</param>
        public SoundFile(IAudioConfiguration config, byte[] data)
        {
            Config = config;
            Data = data;
        }

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public IAudioConfiguration Config { get; }

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        public byte[] Data { get; }

        /// <summary>Gets or sets the comment.</summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>Plays the specified device.</summary>
        /// <param name="device">The device.</param>
        /// <param name="volume">The volume.</param>
        public void Play(IAudioDevice device, float volume = 1)
        {
            using (var audioOut = device.CreateAudioOut(Config))
            {
                audioOut.Volume = volume;
                Play(audioOut);
            }
        }

        /// <summary>Plays the specified audio out.</summary>
        /// <param name="audioOut">The audio out.</param>
        public void Play(AudioOut audioOut)
        {
            audioOut.Write(Data);
            audioOut.Start();
            while (audioOut.BytesBuffered > 0)
            {
                Thread.Sleep(1);
            }

            audioOut.Close();
        }

        /// <summary>Saves the sound file to the specified file name.</summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName)
        {
            using (FileStream fs = File.OpenWrite(fileName))
            {
                Save(fs);
                fs.Close();
            }
        }

        /// <summary>Saves the sound file to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="NotSupportedException">
        /// Unsupported AudioSampleFormat!
        /// or
        /// Unsupported AudioSampleFormat!.
        /// </exception>
        public void Save(Stream stream)
        {
            var comment = new List<byte>();
            if (Comment != null)
            {
                comment.AddRange(Encoding.UTF8.GetBytes(Comment));
                while ((comment.Count % 8) != 0)
                {
                    comment.Add(0);
                }
            }

            var writer = new DataWriter(stream, endian: EndianType.BigEndian);
            writer.Write(0x2e736e64);
            writer.Write(comment.Count + (6 * 4));
            writer.Write(Data.Length);
            switch (Config.Format)
            {
                case AudioSampleFormat.Int8: writer.Write(2); break;
                case AudioSampleFormat.Int16: writer.Write(3); break;
                case AudioSampleFormat.Int24: writer.Write(4); break;
                case AudioSampleFormat.Int32: writer.Write(5); break;
                case AudioSampleFormat.Float: writer.Write(6); break;
                case AudioSampleFormat.Double: writer.Write(7); break;
                default: throw new NotSupportedException("Unsupported AudioSampleFormat!");
            }
            writer.Write(Config.SamplingRate);
            writer.Write(Config.Channels);
            if (comment.Count > 0)
            {
                writer.Write(comment.ToArray());
            }

            var data = Data;
            if (BitConverter.IsLittleEndian)
            {
                // invert data
                int bytes;
                switch (Config.Format)
                {
                    case AudioSampleFormat.Int8: bytes = 1; break;
                    case AudioSampleFormat.Int16: bytes = 2; break;
                    case AudioSampleFormat.Int24: bytes = 3; break;
                    case AudioSampleFormat.Int32: bytes = 4; break;
                    case AudioSampleFormat.Float: bytes = 4; break;
                    case AudioSampleFormat.Double: bytes = 8; break;
                    default: throw new NotSupportedException("Unsupported AudioSampleFormat!");
                }
                if (bytes > 1)
                {
                    data = Endian.Swap(data, bytes);
                }
            }
            writer.Write(data);
        }
    }
}
