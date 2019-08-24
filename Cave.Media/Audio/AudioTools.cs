namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides generic audio tools.
    /// </summary>
    public static class AudioTools
    {
        static unsafe byte[] Interpolate8Bit(double factor, byte[] buffer)
        {
            int sampleCount = buffer.Length;
            int newBufferSize = (int)(sampleCount * factor);
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    byte* sourcePtr = sourceBytePtr;
                    byte* targetPtr = targetBytePtr;
                    double current = 0;

                    for (int i = 0; i < sampleCount; i++)
                    {
                        int nextStep = (int)current + 1;
                        while (current < nextStep)
                        {
                            targetPtr[(int)current] = sourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }

        static unsafe byte[] Interpolate16Bit(double factor, byte[] buffer)
        {
            int sampleCount = buffer.Length / 2;
            int newBufferSize = (int)(sampleCount * factor) * 2;
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    ushort* sourcePtr = (ushort*)sourceBytePtr;
                    ushort* targetPtr = (ushort*)targetBytePtr;
                    double current = 0;

                    for (int i = 0; i < sampleCount; i++)
                    {
                        int nextStep = (int)current + 1;
                        while (current < nextStep)
                        {
                            targetPtr[(int)current] = sourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }

        static unsafe byte[] m_Interpolate32Bit(double factor, byte[] buffer)
        {
            int sampleCount = buffer.Length / 4;
            int newBufferSize = (int)(sampleCount * factor) * 4;
            byte[] newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    uint* sourcePtr = (uint*)sourceBytePtr;
                    uint* targetPtr = (uint*)targetBytePtr;
                    double current = 0;

                    for (int i = 0; i < sampleCount; i++)
                    {
                        int nextStep = (int)current + 1;
                        while (current < nextStep)
                        {
                            targetPtr[(int)current] = sourcePtr[i];
                            current += factor;
                        }
                    }
                }
            }
            return newBuffer;
        }
    }
}
