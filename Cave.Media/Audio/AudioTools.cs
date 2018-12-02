namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides generic audio tools
    /// </summary>
    public static class AudioTools
    {
        static unsafe byte[] m_Interpolate8Bit(double factor, byte[] buffer)
        {
            int l_SampleCount = buffer.Length;
            int newBufferSize = (int)(l_SampleCount * factor);
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* l_SourceBytePtr = &buffer[0])
            {
                fixed (byte* l_TargetBytePtr = &newBuffer[0])
                {
                    byte* l_SourcePtr = l_SourceBytePtr;
                    byte* l_TargetPtr = l_TargetBytePtr;
                    double current = 0;

                    for (int i = 0; i < l_SampleCount; i++)
                    {
                        int l_NextStep = (int)current + 1;
                        while (current < l_NextStep)
                        {
                            l_TargetPtr[(int)current] = l_SourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }

        static unsafe byte[] m_Interpolate16Bit(double factor, byte[] buffer)
        {
            int l_SampleCount = buffer.Length / 2;
            int newBufferSize = (int)(l_SampleCount * factor) * 2;
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* l_SourceBytePtr = &buffer[0])
            {
                fixed (byte* l_TargetBytePtr = &newBuffer[0])
                {
                    ushort* l_SourcePtr = (ushort*)l_SourceBytePtr;
                    ushort* l_TargetPtr = (ushort*)l_TargetBytePtr;
                    double current = 0;

                    for (int i = 0; i < l_SampleCount; i++)
                    {
                        int l_NextStep = (int)current + 1;
                        while (current < l_NextStep)
                        {
                            l_TargetPtr[(int)current] = l_SourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }

        static unsafe byte[] m_Interpolate32Bit(double factor, byte[] buffer)
        {
            int l_SampleCount = buffer.Length / 4;
            int newBufferSize = (int)(l_SampleCount * factor) * 4;
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* l_SourceBytePtr = &buffer[0])
            {
                fixed (byte* l_TargetBytePtr = &newBuffer[0])
                {
                    uint* l_SourcePtr = (uint*)l_SourceBytePtr;
                    uint* l_TargetPtr = (uint*)l_TargetBytePtr;
                    double current = 0;

                    for (int i = 0; i < l_SampleCount; i++)
                    {
                        int l_NextStep = (int)current + 1;
                        while (current < l_NextStep)
                        {
                            l_TargetPtr[(int)current] = l_SourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }
    }
}
