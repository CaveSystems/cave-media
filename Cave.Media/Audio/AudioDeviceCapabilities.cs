namespace Cave.Media.Audio;

/// <summary>Provides a generic wrapper for audio device capabilities.</summary>
public class AudioDeviceCapabilities : IAudioDeviceCapabilities
{
    #region Private Fields

    readonly IAudioConfiguration[] inputConfigurations;
    readonly IAudioConfiguration[] outputConfigurations;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="AudioDeviceCapabilities"/> class.</summary>
    /// <param name="devType">Device type.</param>
    /// <param name="configurations">Configurations available.</param>
    public AudioDeviceCapabilities(AudioDeviceType devType, params IAudioConfiguration[] configurations)
    {
        Type = devType;
        if ((Type & AudioDeviceType.Input) != 0)
        {
            inputConfigurations = configurations;
        }
        if ((Type & AudioDeviceType.Output) != 0)
        {
            outputConfigurations = configurations;
        }
        inputConfigurations ??= [];
        outputConfigurations ??= [];
    }

    /// <summary>Initializes a new instance of the <see cref="AudioDeviceCapabilities"/> class.</summary>
    /// <param name="devType">The device type.</param>
    /// <param name="outputConfigurations">The available output configurations.</param>
    /// <param name="inputConfigurations">The available input configurations.</param>
    public AudioDeviceCapabilities(AudioDeviceType devType, IAudioConfiguration[] outputConfigurations, IAudioConfiguration[] inputConfigurations)
    {
        Type = devType;
        this.outputConfigurations = outputConfigurations ?? [];
        this.inputConfigurations = inputConfigurations ?? [];
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the supported input configurations.</summary>
    public IAudioConfiguration[] InputConfigurations => (IAudioConfiguration[])inputConfigurations.Clone();

    /// <summary>Gets a value indicating whether the device is an input device.</summary>
    public bool IsInput => (Type & AudioDeviceType.Input) != AudioDeviceType.Invalid;

    /// <summary>Gets a value indicating whether the device is an output device.</summary>
    public bool IsOutput => (Type & AudioDeviceType.Output) != AudioDeviceType.Invalid;

    /// <summary>Gets the supported output configurations.</summary>
    public IAudioConfiguration[] OutputConfigurations => (IAudioConfiguration[])outputConfigurations.Clone();

    /// <summary>Gets the device type.</summary>
    public AudioDeviceType Type { get; private set; }

    #endregion Public Properties
}
