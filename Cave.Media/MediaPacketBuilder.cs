using System.Collections.Generic;

namespace Cave.Media
{
    /// <summary>
    /// Allows to create MediaPacket objects
    /// </summary>
    public class MediaPacketBuilder
    {
        /// <summary>
        /// Video Frames of the packet
        /// </summary>
        readonly List<IVideoFrame> m_Frames = new List<IVideoFrame>();

        /// <summary>
        /// Audio Data of the packet
        /// </summary>
        readonly List<IAudioData> m_AudioData = new List<IAudioData>();

        /// <summary>Adds a frame.</summary>
        /// <param name="frame">The frame.</param>
        public void AddFrame(IVideoFrame frame)
        {
            m_Frames.Add(frame);
        }

        /// <summary>Adds an audio data.</summary>
        /// <param name="data">The audio data.</param>
        public void AddAudioData(IAudioData data)
        {
            m_AudioData.Add(data);
        }

        /// <summary>
        /// Obtains a MediaPacket
        /// </summary>
        /// <returns>Returns a MediaPacket</returns>
        public IMediaPacket ToPacket()
        {
            return new MediaPacket(m_AudioData.ToArray(), m_Frames.ToArray());
        }
    }
}
