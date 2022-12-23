#region Notes
/*
    This is part of the MP3Decoder implementation.
    Based upon jlayer 1.0.1 lgpl 2008 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/
#endregion
using System;
using System.Diagnostics;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Implements the IAudioDecoder interface for MP3 files.
    /// </summary>
    /// <seealso cref="IAudioDecoder" />
    public class MP3AudioDecoder : AudioDecoder
    {
        /// <summary>Gets the name of the source currently beeing decoded. This is used for error messages.</summary>
        public override string SourceName { get; set; }

        /// <summary>Gets the description of the decoder.</summary>
        public override string Description => "Full managed Mpeg Layer 3 Audio Decoder based on the source of libmpg123 and JLayer\n" +
                    "This decoder uses float calculations for the whole decoding process.\n" +
                    "On arm devices or machines lacking decent vfp support this might be slow!";

        /// <summary>Gets the features list.</summary>
        public override string Features => "Managed Mpeg Audio decoder for Layer III";

        /// <summary>Gets the mime types the decoder is able to handle.</summary>
        public override string[] MimeTypes => new string[] { "audio/mpeg", "audio/mp3", "audio/mpeg3", "audio/x-mpeg-3", };

        /// <summary>Gets the decoder name.</summary>
        public override string Name => "MP3AudioDecoder";

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName => Name;

        /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public override bool IsAvailable => true;

        IFrameSource source;
        MP3AudioStereoBuffer outputBuffer;
        MP3AudioLayerIIIDecoder frameDecoder;
        int samplingRate;
        int outputChannels;
        MP3AudioEqualizer equalizer = new MP3AudioEqualizer();
        bool resetted = false;

        /// <summary>Synthesis filter for the left channel.</summary>
        MP3AudioSynthesisFilter filter1;

        /// <summary>Sythesis filter for the right channel.</summary>
        MP3AudioSynthesisFilter filter2;

        /// <summary>Reads the next audio frame and silently skips garbage and invalid frames.</summary>
        /// <returns></returns>
        MP3AudioFrame ReadNextAudioFrame()
        {
            MP3AudioFrame mp3Frame = null;
            while (mp3Frame == null)
            {
                var frame = source.GetNextFrame();

                // eof ?
                if (frame == null)
                {
                    return null;
                }

                OnDecoding(frame);
                mp3Frame = frame as MP3AudioFrame;
            }
            return mp3Frame;
        }

        /// <summary>
        /// Reads a frame from the MP3 stream.
        /// Returns whether the operation was successful.
        /// </summary>
        bool DecodeFrame(MP3AudioFrame frame)
        {
            if (frame == null)
            {
                return false;
            }

            try
            {
                outputBuffer.Clear();
                frameDecoder.DecodeFrame(frame);
                resetted = false;
                return true;
            }
            catch (Exception ex)
            {
                if (!resetted)
                {
                    frameDecoder.Reset();
                    resetted = true;
                }
                Trace.WriteLine("Source " + SourceName + ": Error while decoding mp3 frame.\n" + ex);
                return false;
            }
        }

        /// <summary>Begins the decoding process.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="NotSupportedException">Only Layer 3 Audio is supported!.</exception>
        /// <exception cref="Exception">Decoding already started!.</exception>
        public override void BeginDecode(IFrameSource source)
        {
            if (frameDecoder != null)
            {
                Close();
            }

            SourceName = source.Name;
            this.source = source;

            // get first audio frame
            var mp3Frame = ReadNextAudioFrame();
            if (mp3Frame.Header.Layer != MP3AudioFrameLayer.Layer3)
            {
                throw new NotSupportedException("Source " + SourceName + ": Only Layer 3 Audio is supported!");
            }

            // prepare decoder
            outputChannels = mp3Frame.Header.ChannelCount;
            var isEqualizerFactors = equalizer.GetFactors();
            filter1 = new MP3AudioSynthesisFilter(0, 32000.0f, isEqualizerFactors);
            if (outputChannels == 2)
            {
                filter2 = new MP3AudioSynthesisFilter(1, 32000.0f, isEqualizerFactors);
            }

            samplingRate = mp3Frame.Header.SamplingRate;
            outputBuffer = new MP3AudioStereoBuffer(samplingRate);
            frameDecoder = new MP3AudioLayerIIIDecoder(mp3Frame.Header, filter1, filter2, outputBuffer, (int)MP3AudioOutputMode.Both);

            DecodeFrame(mp3Frame);
        }

        /// <summary>Closes this instance and the underlying stream.</summary>
        public override void Close()
        {
            frameDecoder = null;
            source.Close();
        }

        /// <summary>Decodes audio data.</summary>
        /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available.</returns>
        public override IAudioData Decode()
        {
            if (outputBuffer.SampleCount == 0)
            {
                while (true)
                {
                    AudioFrame frame = ReadNextAudioFrame();
                    if (frame == null)
                    {
                        return null;
                    }

                    if (DecodeFrame(frame as MP3AudioFrame) && (outputBuffer.SampleCount > 0))
                    {
                        break;
                    }
                }
            }

            var data = outputBuffer.GetAudioData();
            outputBuffer.Clear();
            return data;
        }
    }
}
