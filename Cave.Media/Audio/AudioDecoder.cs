#region Notes
/*
    This is part of the MP3Decoder implementation.
    Based upon jlayer 1.0.1 lgpl 2008 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/
#endregion
using System;
using System.IO;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a abstract base class for audio decoder implementations.
    /// </summary>
    /// <seealso cref="IAudioDecoder" />
    public abstract class AudioDecoder : IAudioDecoder
    {
        /// <summary>Called when [decoding a frame].</summary>
        /// <param name="frame">The frame.</param>
        protected virtual void OnDecoding(AudioFrame frame)
        {
            Decoding?.Invoke(this, new AudioFrameEventArgs(frame));
        }

        /// <summary>Gets the description of the decoder.</summary>
        public abstract string Description { get; }

        /// <summary>Gets the features list.</summary>
        public abstract string Features { get; }

        /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsAvailable { get; }

        /// <summary>Gets the mime types the decoder is able to handle.</summary>
        public abstract string[] MimeTypes { get; }

        /// <summary>Gets the decoder name.</summary>
        public abstract string Name { get; }

        /// <summary>Gets the name of the source currently beeing decoded. This is used for error messages.</summary>
        public abstract string SourceName { get; set; }

        /// <summary>Starts the decoding process.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="InvalidOperationException">Source: Decoding already started!.</exception>
        public abstract void BeginDecode(IFrameSource source);

        /// <summary>Closes the underlying stream and calls Dispose.</summary>
        public abstract void Close();

        /// <summary>Decodes audio data.</summary>
        /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available.</returns>
        public abstract IAudioData Decode();

        /// <summary>Occurs when [decoding a frame].</summary>
        public event EventHandler<AudioFrameEventArgs> Decoding;

        /// <summary>Starts the decoding process.</summary>
        /// <param name="fileName">Name of the file.</param>
        public void BeginDecode(string fileName)
        {
            BeginDecode(new MP3Reader(fileName));
        }

        /// <summary>Starts the decoding process.</summary>
        /// <param name="sourceStream">The source Stream providing the encoded data.</param>
        /// <exception cref="Exception">Source  + SourceName + : Decoding already started!.</exception>
        public void BeginDecode(Stream sourceStream)
        {
            BeginDecode(new MP3Reader(sourceStream));
        }
    }
}
