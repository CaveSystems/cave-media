namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for media streams.
    /// </summary>
    public interface IMediaStream
    {
        /// <summary>
        /// Gets the type of the stream.
        /// </summary>
        MediaType Type { get; }

        /// <summary>
        /// Gets the ID of the stream.
        /// </summary>
        int ID { get; }
    }
}
