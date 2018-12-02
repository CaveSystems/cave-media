namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides device types
    /// </summary>
    public enum AudioDeviceType : byte
    {
        /// <summary>
        /// Invalid device
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Input device (also called capture device)
        /// </summary>
        Input = 1,

        /// <summary>
        /// Output device (also called playback device)
        /// </summary>
        Output = 2,
    }
}
