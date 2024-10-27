namespace Cave.Media.Audio;

/// <summary>
/// Provides audio device states.
/// </summary>
public enum AudioDeviceState
{
    /// <summary>
    /// Invalid state
    /// </summary>
    Invalid = -1,

    /// <summary>
    /// Device is stopped
    /// </summary>
    Stopped = 0,

    /// <summary>
    /// Device was started and is running
    /// </summary>
    Started = 1,

    /// <summary>
    /// Device was closed and cannot be restarted
    /// </summary>
    Closed = 2,

    /// <summary>Device was disposed</summary>
    Disposed = 3,
}
