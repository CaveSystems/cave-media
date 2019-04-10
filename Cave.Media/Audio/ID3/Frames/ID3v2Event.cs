using System;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Provides an event.
    /// </summary>
    public abstract class ID3v2Event
    {
        internal ID3v2Event(long value, bool isTimeStamp)
        {
            IsTimeStamp = isTimeStamp;
            this.value = value;
        }

        long value;
        long ticksPerFrame;

        /// <summary>
        /// Gets a value indicating whether this event has a timestamp (true) or frame number (false).
        /// </summary>
        public bool IsTimeStamp { get; private set; }

        /// <summary>
        /// Provides setting of the mp3 frame header used to convert between time per frame and frame number.
        /// </summary>
        /// <param name="header">MP3AudioFrameHeader used to calculate the time each MP3AudioFrame contains.</param>
        public void SetFrameLength(MP3AudioFrameHeader header)
        {
            if (header == null)
            {
                throw new ArgumentNullException("Header");
            }

            ticksPerFrame = header.SampleCount * TimeSpan.TicksPerSecond / header.SamplingRate;
        }

        /// <summary>
        /// Provides setting of the time per frame value used to convert between time per frame and frame number.
        /// </summary>
        /// <param name="timePerFrame">Time each MP3AudioFrame contains.</param>
        public void SetFrameLength(TimeSpan timePerFrame)
        {
            if (timePerFrame <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timePerFrame));
            }

            ticksPerFrame = timePerFrame.Ticks;
        }

        /// <summary>
        /// Obtains the time stamp this event occures (if IsTimeStamp == false SetFrameLength() has to be called prior usage!).
        /// </summary>
        public TimeSpan TimeStamp
        {
            get
            {
                if (IsTimeStamp)
                {
                    return new TimeSpan(value * TimeSpan.TicksPerMillisecond);
                }

                if (ticksPerFrame == 0)
                {
                    throw new InvalidOperationException(string.Format("This Event uses a FrameNumber instead of TimeStamp. Please use SetFrameLength() to calculate the TimeSpan!"));
                }

                return new TimeSpan(ticksPerFrame * FrameNumber);
            }
        }

        /// <summary>
        /// Obtains the frame number this event occures (if IsTimeStamp == true SetFrameLength() has to be called prior usage!).
        /// </summary>
        public int FrameNumber
        {
            get
            {
                if (!IsTimeStamp)
                {
                    return (int)value;
                }

                if (ticksPerFrame == 0)
                {
                    throw new InvalidOperationException(string.Format("This Event uses a TimeStamp instead of FrameCount. Please use SetFrameLength() to calculate the TimeSpan!"));
                }

                return (int)(TimeStamp.Ticks / ticksPerFrame);
            }
        }
    }
}
