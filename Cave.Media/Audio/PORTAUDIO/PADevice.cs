#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

using System.Collections.Generic;

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// port audio - audio device implementation.
    /// </summary>
    /// <seealso cref="AudioDevice" />
    internal class PADevice : AudioDevice
    {
        #region static implementation

        static string GetName(int devIndex)
        {
            PADeviceInfo l_DeviceInfo = PA.GetDeviceInfo(devIndex);
            PAHostApiInfo l_HostApiInfo = PA.GetHostApiInfo(l_DeviceInfo.HostApi);

            // by default use utf-8, but mme uses ansi
            switch (l_HostApiInfo.Type)
            {
                case PAHostApiTypeId.MME: return "PortAudio " + l_HostApiInfo.NameUtf8 + ": " + l_DeviceInfo.NameAnsi;
            }
            return "PortAudio " + l_HostApiInfo.NameUtf8 + ": " + l_DeviceInfo.NameUtf8;
        }

        static IAudioDeviceCapabilities GetCapabilities(int devNumber)
        {
            var configs = new List<AudioConfiguration>();
            foreach (int sampleRate in new int[] { 11025, 16000, 22050, 32000, 44100, 48000, 64000, 96000 })
            {
                foreach (AudioChannelSetup setup in new AudioChannelSetup[] { AudioChannelSetup.Mono, AudioChannelSetup.Stereo })
                {
                    foreach (AudioSampleFormat format in new AudioSampleFormat[] { AudioSampleFormat.Float, AudioSampleFormat.Int8, AudioSampleFormat.Int16, AudioSampleFormat.Int32 })
                    {
                        configs.Add(new AudioConfiguration(sampleRate, format, setup));
                    }
                }
            }
            return new AudioDeviceCapabilities(AudioDeviceType.Output, configs.ToArray());
        }
        #endregion

        /// <summary>The device index.</summary>
        public readonly int DeviceIndex;

        /// <summary>Initializes a new instance of the <see cref="PADevice" /> class.</summary>
        /// <param name="api">The API.</param>
        /// <param name="devIndex">Index of the device.</param>
        internal PADevice(IAudioAPI api, int devIndex)
            : base(api, GetName(devIndex), GetCapabilities(devIndex))
        {
            DeviceIndex = devIndex;
        }

        /// <summary>Gets whether the device supports playback or not.</summary>
        public override bool SupportsPlayback
        {
            get
            {
                PADeviceInfo l_DeviceInfo = PA.GetDeviceInfo(DeviceIndex);
                return l_DeviceInfo.MaxOutputChannels > 0;
            }
        }

        /// <summary>Gets whether the device supports recording or not.</summary>
        public override bool SupportsRecording
        {
            get
            {
                PADeviceInfo l_DeviceInfo = PA.GetDeviceInfo(DeviceIndex);
                return l_DeviceInfo.MaxInputChannels > 0;
            }
        }

        /// <summary>Gets a new audio queue (sound target/source).</summary>
        /// <param name="configuration">The desired AudioConfiguration.</param>
        /// <returns>Returns an IAudioQueue or IAudioQueue3D.</returns>
        public override AudioOut CreateAudioOut(IAudioConfiguration configuration)
        {
            return new PAOut(this, configuration);
        }

        /// <summary>Disposes all unmanged resources.</summary>
        public override void Dispose() { }
    }
}
