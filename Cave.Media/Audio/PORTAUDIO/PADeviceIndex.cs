namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio device index.</summary>
internal enum PADeviceIndex : int
{
    /// <summary>The no device</summary>
    NoDevice = -1,

    /// <summary>Use Host Api Specific Device Specification</summary>
    UseHostApiSpecificDeviceSpecification = -2,
}
