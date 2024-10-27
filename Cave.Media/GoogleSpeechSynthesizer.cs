using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Cave.Media.Audio;
using Cave.Media.Audio.MP3;
using Cave.Net;

namespace Cave.Media;

/// <summary>Provides a <see cref="ISpeechSynthesizer"/> implementation for Google Translate to Speech.</summary>
/// <remarks>
/// Do not use this to frequent because you will get blocked by google if you do. We cache all downloaded audio files to reduce calls to the google api.
/// </remarks>
/// <seealso cref="ISpeechSynthesizer"/>
public class GoogleSpeechSynthesizer : ISpeechSynthesizer
{
    CultureInfo cultureInfo;
    IAudioDevice device;

    /// <summary>Initializes a new instance of the <see cref="GoogleSpeechSynthesizer"/> class.</summary>
    /// <param name="device">The device.</param>
    public GoogleSpeechSynthesizer(IAudioDevice device)
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        FilePath = Path.Combine(folder, "GoogleSpeechSynthesizer");
        Directory.CreateDirectory(FilePath);
        cultureInfo = CultureInfo.CurrentCulture;
        this.device = device;
        Trace.WriteLine(string.Format("Using <green>GoogleSpeechSynthesizer <default>Cache: <cyan>{0}<default> Device: {1}", FilePath, device));
    }

    /// <summary>Gets the name of the log source.</summary>
    /// <value>The name of the log source.</value>
    public string LogSourceName => "GoogleSpeechSynthesizer";

    /// <summary>Gets the file path where audio files are stored.</summary>
    /// <value>The file path where audio files are stored.</value>
    public string FilePath { get; }

    /// <summary>Gets or sets the volume.</summary>
    /// <value>The volume.</value>
    public float Volume { get; set; } = 1;

    /// <summary>Selects a voice with a specific gender, age, and locale.</summary>
    /// <param name="gender">The gender.</param>
    /// <param name="age">The age.</param>
    /// <param name="cultureInfo">The culture information.</param>
    /// <exception cref="ArgumentNullException">cultureInfo.</exception>
    public void SelectVoiceByHints(VoiceGender gender, VoiceAge age, CultureInfo cultureInfo) => this.cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));

    /// <summary>Speaks the specified text.</summary>
    /// <param name="text">The text.</param>
    public void Speak(string text)
    {
        try
        {
            Trace.WriteLine(string.Format("Speak: {0}", text));
            var uri = "http://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=" + cultureInfo.Name + "&q=" + Uri.EscapeDataString(text);
            var hash = Base32.Safe.Encode(uri.GetHashCode() * (long)text.Length);

            lock (this)
            {
                if (TryPlay(Path.Combine(FilePath, hash + ".snd")))
                {
                    return;
                }

                Trace.WriteLine(string.Format("Request new google translation for {0}", text));
                byte[] data;
                {
                    var fileName = Path.Combine(FilePath, hash + ".mp3");
                    if (File.Exists(fileName))
                    {
                        data = File.ReadAllBytes(fileName);
                    }
                    else
                    {
                        data = HttpConnection.Get(uri);
                        File.WriteAllBytes(fileName, data);
                    }
                }
                DecodeAndPlay(data, Path.Combine(FilePath, hash + ".snd"), text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Could not speak desired text.", ex);
        }
    }

    bool TryPlay(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return false;
        }

        SoundFile soundFile;
        try
        {
            soundFile = SoundFile.Read(fileName);
        }
        catch (InvalidDataException)
        {
            File.Delete(fileName);
            return false;
        }
        catch { return false; }
        try { soundFile.Play(device, Volume); } catch { }
        return true;
    }

    void DecodeAndPlay(byte[] data, string fileName, string text)
    {
        SoundFile soundFile;
        using (Stream s = new MemoryStream(data))
        {
            soundFile = Decode(s);
            soundFile.Comment = text;
        }
        using (Stream fs = File.OpenWrite(fileName))
        {
            soundFile.Save(fs);
            fs.Close();
        }
#if DEBUG
        // check read after write
        SoundFile.Read(fileName);
#endif
        try { soundFile.Play(device, Volume); } catch { }
    }

    SoundFile Decode(Stream s)
    {
        IAudioDecoder decoder = new Mpg123();
        if (!decoder.IsAvailable)
        {
            decoder = new MP3AudioDecoder();
        }

        if (!decoder.IsAvailable)
        {
            throw new Exception("No mp3 decoder available!");
        }

        try
        {
            decoder.BeginDecode(s);
            using var data = new MemoryStream();
            var packet = decoder.Decode() ?? throw new InvalidOperationException("No data received.");
            IAudioConfiguration config = packet;
            while (packet != null)
            {
                data.Write(packet.Data, 0, packet.Length);
                packet = decoder.Decode();
            }
            return new SoundFile(config, data.ToArray());
        }
        finally
        {
            decoder.Close();
        }
    }
}
