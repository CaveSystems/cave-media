namespace Cave.Media.Audio.OPENAL;

/// <summary>Provides functions on <see cref="ALsource"/></summary>
public static class ALsourceExtension
{
    #region Public Methods

    /// <summary>Gets the state of a source</summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ALenum GetState(this ALsource source)
    {
        OAL.SafeNativeMethods.alGetSourcei(source, ALenum.AL_SOURCE_STATE, out var state);
        OAL.CheckError();
        return (ALenum)state;
    }

    /// <summary>This function pauses a source.</summary>
    /// <param name="source"></param>
    public static void Pause(this ALsource source)
    {
        OAL.SafeNativeMethods.alSourcePause(source);
        OAL.CheckError();
    }

    /// <summary>This function pauses a set of sources.</summary>
    /// <param name="sources"></param>
    public static void Pause(this ALsource[] sources)
    {
        OAL.SafeNativeMethods.alSourcePausev(sources.Length, sources);
        OAL.CheckError();
    }

    /// <summary>Starts playing a source</summary>
    /// <param name="source"></param>
    public static void Play(this ALsource source)
    {
        OAL.SafeNativeMethods.alSourcePlay(source);
        OAL.CheckError();
    }

    /// <summary>Starts playing a list of sources</summary>
    /// <param name="sources"></param>
    public static void Play(this ALsource[] sources)
    {
        OAL.SafeNativeMethods.alSourcePlayv(sources.Length, sources);
        OAL.CheckError();
    }

    /// <summary>Rewinds a source. This function stops the source and sets its state to AL_INITIAL.</summary>
    /// <param name="source"></param>
    public static void Rewind(this ALsource source)
    {
        OAL.SafeNativeMethods.alSourceRewind(source);
        OAL.CheckError();
    }

    /// <summary>Rewinds a number of sources. This function stops all sources and sets their state to AL_INITIAL.</summary>
    /// <param name="sources"></param>
    public static void Rewind(this ALsource[] sources)
    {
        OAL.SafeNativeMethods.alSourceRewindv(sources.Length, sources);
        OAL.CheckError();
    }

    /// <summary>Stops a source and sets the state to AL_STOPPED.</summary>
    /// <param name="source"></param>
    public static void Stop(this ALsource source)
    {
        OAL.SafeNativeMethods.alSourceStop(source);
        OAL.CheckError();
    }

    /// <summary>Stops a number of sources and sets their state to AL_STOPPED.</summary>
    /// <param name="sources"></param>
    public static void Stop(this ALsource[] sources)
    {
        OAL.SafeNativeMethods.alSourceStopv(sources.Length, sources);
        OAL.CheckError();
    }

    #endregion Public Methods
}
