using Cave;
using Cave.Media.Audio.MP3;
using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Provides an event
    /// </summary>
    
    public abstract class ID3v2Event
    {
        internal ID3v2Event(long value, bool isTimeStamp)
        {
            m_IsTimeStamp = isTimeStamp;
            m_Value = value;
        }

        bool m_IsTimeStamp;
        long m_Value;
        long m_TicksPerFrame;

        /// <summary>
        /// Obtains whether this event has a timestamp (true) or frame number (false)
        /// </summary>
        public bool IsTimeStamp { get { return m_IsTimeStamp; } }

        /// <summary>
        /// Provides setting of the mp3 frame header used to convert between time per frame and frame number
        /// </summary>
        /// <param name="header">MP3AudioFrameHeader used to calculate the time each MP3AudioFrame contains</param>
        public void SetFrameLength(MP3AudioFrameHeader header)
        {
            if (header == null) throw new ArgumentNullException("Header");
            m_TicksPerFrame = (header.SampleCount * TimeSpan.TicksPerSecond) / header.SamplingRate;
        }

        /// <summary>
        /// Provides setting of the time per frame value used to convert between time per frame and frame number
        /// </summary>
        /// <param name="timePerFrame">Time each MP3AudioFrame contains</param>
        public void SetFrameLength(TimeSpan timePerFrame)
        {
            if (timePerFrame <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timePerFrame));
            m_TicksPerFrame = timePerFrame.Ticks;
        }

        /// <summary>
        /// Obtains the time stamp this event occures (if IsTimeStamp == false SetFrameLength() has to be called prior usage!)
        /// </summary>
        public TimeSpan TimeStamp
        {
            get
            {
                if (m_IsTimeStamp) return new TimeSpan(m_Value * TimeSpan.TicksPerMillisecond);
                if (m_TicksPerFrame == 0) throw new InvalidOperationException(string.Format("This Event uses a FrameNumber instead of TimeStamp. Please use SetFrameLength() to calculate the TimeSpan!"));
                return new TimeSpan(m_TicksPerFrame * FrameNumber);
            }
        }

        /// <summary>
        /// Obtains the frame number this event occures (if IsTimeStamp == true SetFrameLength() has to be called prior usage!)
        /// </summary>
        public int FrameNumber
        {
            get
            {
                if (!m_IsTimeStamp) return (int)m_Value;
                if (m_TicksPerFrame == 0) throw new InvalidOperationException(string.Format("This Event uses a TimeStamp instead of FrameCount. Please use SetFrameLength() to calculate the TimeSpan!"));
                return (int)(TimeStamp.Ticks / m_TicksPerFrame);
            }
        }
    }
}
