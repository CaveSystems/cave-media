namespace Cave.Media;

class MediaPacket : IMediaPacket
{
    IVideoFrame[] m_Frames;

    public MediaPacket(IAudioData[] data, IVideoFrame[] frames)
    {
        AudioData = data;
        m_Frames = frames;
    }

    public IAudioData[] AudioData { get; private set; }

    public IVideoFrame[] Frames => m_Frames;
}
