namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for media packets containing audiodata and videoframes.
    /// </summary>
    public interface IMediaPacket
    {
        /// <summary>
        /// Provides the audio data of this packet.
        /// </summary>
        IAudioData[] AudioData { get; }

        /// <summary>
        /// Provides the video frames of this packet.
        /// </summary>
        IVideoFrame[] Frames { get; }
    }
}
