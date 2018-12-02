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
using System.IO;

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

        /// <summary>Obtains the description of the decoder</summary>
        public override string Description
        {
            get
            {
                return 
                    "Full managed Mpeg Layer 3 Audio Decoder based on the source of libmpg123 and JLayer\n"+
                    "This decoder uses float calculations for the whole decoding process.\n"+
                    "On arm devices or machines lacking decent vfp support this might be slow!";
            }
        }

        /// <summary>Obtains the features list</summary>
        public override string Features
        {
            get
            {
                return "Managed Mpeg Audio decoder for Layer III";
            }
        }

        /// <summary>Obtains the mime types the decoder is able to handle</summary>
        public override string[] MimeTypes
        {
            get
            {
                return new string[] { "audio/mpeg", "audio/mp3", "audio/mpeg3", "audio/x-mpeg-3", };
            }
        }

        /// <summary>Obtains the decoder name</summary>
        public override string Name { get { return "MP3AudioDecoder"; } }

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName { get { return Name; } }

        /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public override bool IsAvailable { get { return true; } }

        IFrameSource m_Source;
        MP3AudioStereoBuffer m_OutputBuffer;
        MP3AudioLayerIIIDecoder m_FrameDecoder;
        int m_SamplingRate;
        int m_OutputChannels;
        MP3AudioEqualizer m_Equalizer = new MP3AudioEqualizer();
        bool m_Resetted = false;

        /// <summary>Synthesis filter for the left channel.</summary>
        MP3AudioSynthesisFilter m_Filter1;

        /// <summary>Sythesis filter for the right channel.</summary>
        MP3AudioSynthesisFilter m_Filter2;

        /// <summary>Reads the next audio frame and silently skips garbage and invalid frames.</summary>
        /// <returns></returns>
        MP3AudioFrame ReadNextAudioFrame()
        {
            MP3AudioFrame l_MP3Frame = null;
            while (l_MP3Frame == null)
            {
                AudioFrame frame = m_Source.GetNextFrame();
                //eof ?
                if (frame == null) return null;
                OnDecoding(frame);
                l_MP3Frame = frame as MP3AudioFrame;
            }
            return l_MP3Frame;
        }

        /// <summary>
        /// Reads a frame from the MP3 stream.  
        /// Returns whether the operation was successful. 
        /// </summary>
        bool DecodeFrame(MP3AudioFrame frame)
        {
            if (frame == null) return false;
            try
            {
                m_OutputBuffer.Clear();
                m_FrameDecoder.DecodeFrame(frame);
                m_Resetted = false;
                return true;
            }
            catch (Exception ex)
            {
                if (!m_Resetted)
                {
                    m_FrameDecoder.Reset();
                    m_Resetted = true;
                }
                Trace.WriteLine("Source " + SourceName + ": Error while decoding mp3 frame.\n" + ex);
                return false;
            }
        }

        /// <summary>Begins the decoding process.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="NotSupportedException">Only Layer 3 Audio is supported!</exception>
        /// <exception cref="Exception">Decoding already started!</exception>
        public override void BeginDecode(IFrameSource source)
        {
            if (m_FrameDecoder != null) Close();
            SourceName = source.Name;
            m_Source = source;
            //get first audio frame
            MP3AudioFrame l_MP3Frame = ReadNextAudioFrame();
            if (l_MP3Frame.Header.Layer != MP3AudioFrameLayer.Layer3) throw new NotSupportedException("Source " + SourceName + ": Only Layer 3 Audio is supported!");
            //prepare decoder
            m_OutputChannels = l_MP3Frame.Header.ChannelCount;
            float[] isEqualizerFactors = m_Equalizer.GetFactors();
            m_Filter1 = new MP3AudioSynthesisFilter(0, 32000.0f, isEqualizerFactors);
            if (m_OutputChannels == 2) m_Filter2 = new MP3AudioSynthesisFilter(1, 32000.0f, isEqualizerFactors);
            m_SamplingRate = l_MP3Frame.Header.SamplingRate;
            m_OutputBuffer = new MP3AudioStereoBuffer(m_SamplingRate);
            m_FrameDecoder = new MP3AudioLayerIIIDecoder(l_MP3Frame.Header, m_Filter1, m_Filter2, m_OutputBuffer, (int)MP3AudioOutputMode.Both);
            
            DecodeFrame(l_MP3Frame);
        }

        /// <summary>Closes this instance and the underlying stream</summary>
        public override void Close()
        {
            m_FrameDecoder = null;
            m_Source.Close();
        }

        /// <summary>Decodes audio data</summary>
        /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available</returns>
        public override IAudioData Decode()
        {
            if (m_OutputBuffer.SampleCount == 0)
            {
                while (true)
                {
                    AudioFrame l_Frame = ReadNextAudioFrame();
                    if (l_Frame == null) return null;
                    if (DecodeFrame(l_Frame as MP3AudioFrame) && (m_OutputBuffer.SampleCount > 0)) break;
                }
            }

            IAudioData data = m_OutputBuffer.GetAudioData();
            m_OutputBuffer.Clear();
            return data;
        }
    }
}