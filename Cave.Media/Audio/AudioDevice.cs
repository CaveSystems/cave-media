using System;

namespace Cave.Media.Audio;

/// <summary>provides a generic wrapper for audio devices.</summary>
public abstract class AudioDevice : IAudioDevice
{
    #region Private Fields

    IAudioConfiguration? configuration;

    #endregion Private Fields

    #region Protected Constructors

    /// <summary>Creates a new AudioDevice instance with the specified name and capabilities.</summary>
    /// <param name="api">The API.</param>
    /// <param name="name">The Name of the device.</param>
    /// <param name="capabilities">The capabilities of the device.</param>
    /// <exception cref="ArgumentNullException">API or Name.</exception>
    protected AudioDevice(IAudioAPI api, string name, IAudioDeviceCapabilities capabilities)
    {
        API = api ?? throw new ArgumentNullException("API");
        Name = name ?? throw new ArgumentNullException("Name");
        Capabilities = capabilities;
    }

    #endregion Protected Constructors

    #region Public Properties

    /// <summary>Gets the used audio API.</summary>
    /// <value>The audio API.</value>
    public IAudioAPI API { get; }

    /// <summary>Gets the devices capabilities.</summary>
    public IAudioDeviceCapabilities Capabilities { get; private set; }

    /// <summary>Retrieves the device name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets whether the device supports playback or not.</summary>
    public abstract bool SupportsPlayback { get; }

    /// <summary>Gets whether the device supports recording or not.</summary>
    public abstract bool SupportsRecording { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a new audio queue (sound target/source).</summary>
    /// <param name="configuration">The desired AudioConfiguration.</param>
    /// <returns>Returns an IAudioQueue or IAudioQueue3D.</returns>
    public abstract AudioOut CreateAudioOut(IAudioConfiguration configuration);

    /// <summary>Disposes all unmanged resources.</summary>
    /// <returns></returns>
    public abstract void Dispose();

    /// <summary>Checks against another instance for equality.</summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        var other = obj as IAudioDevice;
        return other == null ? false : Equals(other.Name, Name);
    }

    /// <summary>Gets the hashcode based on the name and configuration.</summary>
    /// <returns></returns>
    public override int GetHashCode() => Name.GetHashCode() ^ (configuration?.GetHashCode() ?? -1);

    /// <summary>Gets the name of the device.</summary>
    /// <returns></returns>
    public override string ToString() => Name;

    #endregion Public Methods
}
