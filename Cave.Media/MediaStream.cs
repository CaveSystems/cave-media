namespace Cave.Media
{
    /// <summary>
    /// Implements the <see cref="IMediaStream"/> interface
    /// </summary>
    public class MediaStream : IMediaStream
    {
        MediaType m_Type;
        int m_ID;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public MediaStream(MediaType type, int id)
        {
            m_Type = type;
            m_ID = id;
        }

        /// <summary>
        /// Obtains the type of the stream
        /// </summary>
        public MediaType Type
        {
            get { return m_Type; }
        }

        /// <summary>
        /// Obtains the ID of the stream
        /// </summary>
        public int ID
        {
            get { return m_ID; }
        }
    }
}
