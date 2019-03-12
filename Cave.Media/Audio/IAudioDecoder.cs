using System;
using System.IO;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides an interface for audio decoder implementations.
    /// </summary>
    public interface IAudioDecoder
    {
        /// <summary>Occurs when [decoding a frame].</summary>
        event EventHandler<AudioFrameEventArgs> Decoding;

        /// <summary>
        /// Obtains the decoder name.
        /// </summary>
        string Name { get; }

        /// <summary>Gets the name of the source currently beeing decoded. This is used for error messages.</summary>
        string SourceName { get; set; }

        /// <summary>
        /// Obtains the description of the decoder.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Obtains the features list.
        /// </summary>
        string Features { get; }

        /// <summary>
        /// Obtains the mime types the decoder is able to handle.
        /// </summary>
        string[] MimeTypes { get; }

        /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        bool IsAvailable { get; }

        /// <param name="fileName">Name of the file.</param>
        void BeginDecode(string fileName);

        /// <summary>Starts the decoding process.</summary>
        /// <param name="sourceStream">The source Stream providing the encoded data.</param>
        void BeginDecode(Stream sourceStream);

        /// <summary>Starts the decoding process.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="InvalidOperationException">Source: Decoding already started!.</exception>
        void BeginDecode(IFrameSource source);

        /// <summary>
        /// Decodes audio data.
        /// </summary>
        /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available.</returns>
        IAudioData Decode();

        /// <summary>
        /// Closes the underlying stream and calls Dispose.
        /// </summary>
        void Close();
    }
}
