namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a audio frame source
    /// </summary>
    public interface IFrameSource
    {
        /// <summary>Gets the next frame.</summary>
        /// <returns></returns>
        AudioFrame GetNextFrame();

        /// <summary>Gets the name of the source.</summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>Closes this instance.</summary>
        void Close();
    }
}
