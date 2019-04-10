namespace Cave.Media
{
    /// <summary>
    /// provides an interface for media frames.
    /// </summary>
    public interface IDataFrame
    {
        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        bool Parse(DataFrameReader reader);

        /// <summary>
        /// Obtains an array with the data for this instance.
        /// </summary>
        /// <returns></returns>
        byte[] Data { get; }

        /// <summary>
        /// Length of the frame in bytes, this always matches Data.Length!.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Obtains whether the <see cref="Length"/> of the frame is immutable or not.
        /// </summary>
        bool IsFixedLength { get; }
    }
}
