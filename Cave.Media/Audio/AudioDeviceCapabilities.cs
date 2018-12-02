namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a generic wrapper for audio device capabilities
    /// </summary>
    public class AudioDeviceCapabilities : IAudioDeviceCapabilities
    {
        readonly IAudioConfiguration[] m_OutputConfigurations;
        readonly IAudioConfiguration[] m_InputConfigurations;
        readonly AudioDeviceType m_Type;

        /// <summary>
        /// Creates a new <see cref="AudioDeviceCapabilities"/> object
        /// </summary>
        /// <param name="devType"></param>
        /// <param name="configurations"></param>
        public AudioDeviceCapabilities(AudioDeviceType devType, params IAudioConfiguration[] configurations)
        {
            m_Type = devType;
            if (configurations == null) configurations = new IAudioConfiguration[0];
            if ((m_Type & AudioDeviceType.Input) != 0)
            {
                m_InputConfigurations = configurations;
            }
            if ((m_Type & AudioDeviceType.Output) != 0)
            {
                m_OutputConfigurations = configurations;
            }
        }

        /// <summary>
        /// Creates a new <see cref="AudioDeviceCapabilities"/> object
        /// </summary>
        /// <param name="devType">The device type</param>
        /// <param name="outputConfigurations">The available output configurations</param>
        /// <param name="inputConfigurations">The available input configurations</param>
        public AudioDeviceCapabilities(AudioDeviceType devType, IAudioConfiguration[] outputConfigurations, IAudioConfiguration[] inputConfigurations)
        {
            m_Type = devType;
            if (outputConfigurations == null) outputConfigurations = new IAudioConfiguration[0];
            if (inputConfigurations == null) inputConfigurations = new IAudioConfiguration[0];
            m_OutputConfigurations = outputConfigurations;
            m_InputConfigurations = inputConfigurations;
        }

        /// <summary>
        /// Determines the device type
        /// </summary>
        public AudioDeviceType Type { get { return m_Type; } }

        /// <summary>
        /// Determines if the device is an input device
        /// </summary>
        public bool IsInput { get { return (Type & AudioDeviceType.Input) != AudioDeviceType.Invalid; } }

        /// <summary>
        /// Determines if the device is an output device
        /// </summary>
        public bool IsOutput { get { return (Type & AudioDeviceType.Output) != AudioDeviceType.Invalid; } }

        /// <summary>
        /// Obtains the supported input configurations
        /// </summary>
        public IAudioConfiguration[] InputConfigurations { get { return (IAudioConfiguration[])m_InputConfigurations.Clone(); } }

        /// <summary>
        /// Obtains the supported output configurations
        /// </summary>
        public IAudioConfiguration[] OutputConfigurations { get { return (IAudioConfiguration[])m_OutputConfigurations.Clone(); } }
    }
}
