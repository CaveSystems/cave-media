namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for media streams.
    /// </summary>
    public interface IMediaStream
    {
        /// <summary>
        /// Obtains the type of the stream.
        /// </summary>
        MediaType Type { get; }

        /// <summary>
        /// Obtains the ID of the stream.
        /// </summary>
        int ID { get; }
    }
}
