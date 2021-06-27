namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides generic audio tools.
    /// </summary>
    public static class AudioTools
    {
        static unsafe byte[] Interpolate8Bit(double factor, byte[] buffer)
        {
            var sampleCount = buffer.Length;
            var newBufferSize = (int)(sampleCount * factor);
            var newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    var sourcePtr = sourceBytePtr;
                    var targetPtr = targetBytePtr;
                    double current = 0;

                    for (var i = 0; i < sampleCount; i++)
                    {
                        var nextStep = (int)current + 1;
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
            var sampleCount = buffer.Length / 2;
            var newBufferSize = (int)(sampleCount * factor) * 2;
            var newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    var sourcePtr = (ushort*)sourceBytePtr;
                    var targetPtr = (ushort*)targetBytePtr;
                    double current = 0;

                    for (var i = 0; i < sampleCount; i++)
                    {
                        var nextStep = (int)current + 1;
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
            var sampleCount = buffer.Length / 4;
            var newBufferSize = (int)(sampleCount * factor) * 4;
            var newBuffer = new byte[newBufferSize];

            fixed (byte* sourceBytePtr = &buffer[0])
            {
                fixed (byte* targetBytePtr = &newBuffer[0])
                {
                    var sourcePtr = (uint*)sourceBytePtr;
                    var targetPtr = (uint*)targetBytePtr;
                    double current = 0;

                    for (var i = 0; i < sampleCount; i++)
                    {
                        var nextStep = (int)current + 1;
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
