using System.Diagnostics.CodeAnalysis;

namespace Cave.Media.Audio;

/// <summary>Provides an abstract base class to implement audio apis.</summary>
[SuppressMessage("Design", "CA1036")]
public abstract class AudioAPI : IAudioAPI
{
    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="AudioAPI"/> class.</summary>
    public AudioAPI() { }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the default input device.</summary>
    /// <returns>Returns the default input device.</returns>
    public virtual IAudioDevice? DefaultInputDevice => InputDevices.Length == 0 ? null : InputDevices[0];

    /// <summary>Gets the default output device.</summary>
    /// <returns>Returns the default output device.</returns>
    public virtual IAudioDevice? DefaultOutputDevice => OutputDevices.Length == 0 ? null : OutputDevices[0];

    /// <summary>Gets the available input devices.</summary>
    /// <returns>Returns all input devices.</returns>
    public abstract IAudioDevice[] InputDevices { get; }

    /// <summary>Determines if the API is available.</summary>
    public abstract bool IsAvailable { get; }

    /// <summary>Gets all available output devices.</summary>
    /// <returns>Returns all output devices.</returns>
    public abstract IAudioDevice[] OutputDevices { get; }

    /// <summary>Gets the preference value.</summary>
    /// <value>The preference.</value>
    /// <remarks>Small values represent a higher priority.</remarks>
    public abstract int Preference { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets all available <see cref="AudioAPI"/> instances.</summary>
    /// <returns></returns>
    public static IAudioAPI[] GetAvailableAudioAPIs()
    {
        var apis = AppDom.GetInstances<IAudioAPI>(true);
        apis.Sort();
        return apis.ToArray();
    }

    /// <summary>Gets all available <see cref="AudioAPI"/> instances.</summary>
    /// <returns></returns>
    public static IAudioDecoder[] GetAvailableAudioDecoders()
    {
        var decoder = AppDom.GetInstances<IAudioDecoder>(true);
        decoder.Sort();
        return decoder.ToArray();
    }

    /// <summary>Gets all available <see cref="AudioAPI"/> instances.</summary>
    /// <returns></returns>
    public static IAudioEncoder[] GetAvailableAudioEncoders()
    {
        var encoder = AppDom.GetInstances<IAudioEncoder>(true);
        encoder.Sort();
        return encoder.ToArray();
    }

    /// <summary>Vergleicht das aktuelle Objekt mit einem anderen Objekt desselben Typs.</summary>
    /// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
    /// <returns>
    /// Ein Wert, der die relative Reihenfolge der verglichenen Objekte angibt.Der Rückgabewert hat folgende Bedeutung:Wert Bedeutung Kleiner als 0 (null)
    /// Dieses Objekt ist kleiner als der <paramref name="other"/>-Parameter.Zero Dieses Objekt ist gleich <paramref name="other"/>. Größer als 0 (null) Dieses
    /// Objekt ist größer als <paramref name="other"/>.
    /// </returns>
    public int CompareTo(IAudioAPI? other) => other is null ? 1 : Preference.CompareTo(other.Preference);

    #endregion Public Methods
}
