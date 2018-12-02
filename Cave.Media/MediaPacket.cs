namespace Cave.Media
{
    class MediaPacket : IMediaPacket
    {
        IAudioData[] m_AudioData;
        IVideoFrame[] m_Frames;

        public MediaPacket(IAudioData[] data, IVideoFrame[] frames)
        {
            m_AudioData = data;
            m_Frames = frames;
        }

        public IAudioData[] AudioData
        {
            get { return m_AudioData; }
        }

        public IVideoFrame[] Frames
        {
            get { return m_Frames; }
        }
    }
}
