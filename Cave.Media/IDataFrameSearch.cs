namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for frame start searches done with a <see cref="DataFrameReader"/>
    /// </summary>
    
    public interface IDataFrameSearch
    {
        /// <summary>
        /// This function is called during the search to transmit the current byte and to obtain if a match is found.
        /// If a match is found the return value contains the length of the match.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        bool Check(byte b);
    }
}
