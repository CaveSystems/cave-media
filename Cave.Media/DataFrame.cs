using Cave.Collections.Generic;

namespace Cave.Media
{
    /// <summary>
    /// Provides an abstract implementation of the <see cref="IDataFrame"/> interface
    /// </summary>
    public abstract class DataFrame : IDataFrame
    {
        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame
        /// </summary>
        /// <param name="reader">FrameReader to read from</param>
        public abstract bool Parse(DataFrameReader reader);

        /// <summary>
        /// Obtains an array with the data for this instance
        /// </summary>
        /// <returns></returns>
        public abstract byte[] Data { get; }

        /// <summary>
        /// Length of the frame in bytes, this always matches Data.Length!
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Obtains whether the <see cref="Length"/> of the frame is immutable or not
        /// </summary>
        public abstract bool IsFixedLength { get; }

        /// <summary>
        /// Obtains whether the frame contains valid data or not
        /// </summary>
        public abstract bool IsValid { get; }
    }
}
