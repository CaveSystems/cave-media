using System.Globalization;

namespace Cave.Media;

/// <summary>
/// Provides a generic speech synthesizer interface.
/// </summary>
public interface ISpeechSynthesizer
{
    /// <summary>Gets or sets the volume.</summary>
    /// <value>The volume.</value>
    float Volume { get; set; }

    /// <summary>Speaks the specified text.</summary>
    /// <param name="text">The text.</param>
    void Speak(string text);

    /// <summary>Selects a voice with a specific gender, age, and locale.</summary>
    /// <param name="gender">The gender.</param>
    /// <param name="age">The age.</param>
    /// <param name="cultureInfo">The culture information.</param>
    void SelectVoiceByHints(VoiceGender gender, VoiceAge age, CultureInfo cultureInfo);
}