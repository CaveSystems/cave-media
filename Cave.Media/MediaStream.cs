namespace Cave.Media
{
    /// <summary>
    /// Implements the <see cref="IMediaStream"/> interface.
    /// </summary>
    public class MediaStream : IMediaStream
    {
        int m_ID;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public MediaStream(MediaType type, int id)
        {
            Type = type;
            m_ID = id;
        }

        /// <summary>
        /// Obtains the type of the stream.
        /// </summary>
        public MediaType Type { get; private set; }

        /// <summary>
        /// Obtains the ID of the stream.
        /// </summary>
        public int ID
        {
            get { return m_ID; }
        }
    }
}
