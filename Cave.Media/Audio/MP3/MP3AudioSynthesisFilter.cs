#region Notes
/*
    This is part of the MP3Decoder implementation.
    Based upon mpg123, jlayer 1.0.1 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/
#endregion

using System;

namespace Cave.Media.Audio.MP3
{
    /// <summary> A class for the synthesis filter bank. Taken from JavaLayer.</summary>
    public class MP3AudioSynthesisFilter
    {
        float[] v1;
        float[] v2;
        float[] actual_v; 

        /// <summary>The current write position (0-15)</summary>
        int m_Position;

        /// <summary>The 32 subband samples</summary>
        float[] m_Samples;

        /// <summary> Computes PCM Samples.</summary>
        float[] m_PCMSamples;

        /// <summary>The channel number this filter uses</summary>
        public readonly int ChannelNumber;

        /// <summary>The equalizer factors this filter uses</summary>
        public float[] EqualizerFactors;

        /// <summary>Initializes a new instance of the <see cref="MP3AudioSynthesisFilter"/> class.</summary>
        /// <param name="channelNumber">The channel number this filter uses.</param>
        /// <param name="scaleFactor">The scalefactor scales the calculated float pcm samples to short values
        /// (raw pcm samples are in [-1.0, 1.0], if no violations occur).</param>
        /// <param name="equalizerFactors">The equalizer factors.</param>
        public MP3AudioSynthesisFilter(int channelNumber, float scaleFactor, float[] equalizerFactors)
        {
            m_PCMSamples = new float[32];
            v1 = new float[512];
            v2 = new float[512];
            m_Samples = new float[32];
            ChannelNumber = channelNumber;
            EqualizerFactors = equalizerFactors;
            Reset();
        }

        /// <summary>Reset the synthesis filter.</summary>
        void Reset()
        {
            // initialize v1[] and v2[]:
            for (int p = 0; p < 512; p++) v1[p] = v2[p] = 0.0f;
            // initialize samples[]:
            for (int p2 = 0; p2 < 32; p2++) m_Samples[p2] = 0.0f;
            actual_v = v1;
            m_Position = 15;
        }


        /// <summary>Adds a sample</summary>
        /// <param name="sample">The sample.</param>
        /// <param name="subbandnumber">The subbandnumber.</param>
        public void AddSample(float sample, int subbandnumber)
        {
            m_Samples[subbandnumber] = EqualizerFactors[subbandnumber] * sample;
        }

        /// <summary>Adds the samples for all 32 bands.</summary>
        /// <param name="samples">The samples.</param>
        public void AddSamples(float[] samples)
        {
#if DEBUG
            if (samples.Length != 32) throw new ArgumentException("Samples[32] expected!");
#endif
            for (int i = 31; i >= 0; i--)
            {
                m_Samples[i] = samples[i] * EqualizerFactors[i];
            }
        }

        /// <summary> Compute new values via a fast cosine transform.
        /// </summary>
        void compute_new_v()
        {
            float new_v0, new_v1, new_v2, new_v3, new_v4, new_v5, new_v6, new_v7, new_v8, new_v9;
            float new_v10, new_v11, new_v12, new_v13, new_v14, new_v15, new_v16, new_v17, new_v18, new_v19;
            float new_v20, new_v21, new_v22, new_v23, new_v24, new_v25, new_v26, new_v27, new_v28, new_v29;
            float new_v30, new_v31;

            new_v0 = new_v1 = new_v2 = new_v3 = new_v4 = new_v5 = new_v6 = new_v7 = new_v8 = new_v9 = new_v10 = new_v11 = new_v12 = new_v13 = new_v14 = new_v15 = new_v16 = new_v17 = new_v18 = new_v19 = new_v20 = new_v21 = new_v22 = new_v23 = new_v24 = new_v25 = new_v26 = new_v27 = new_v28 = new_v29 = new_v30 = new_v31 = 0.0f;

            float[] s = m_Samples;

            float s0 = s[0];
            float s1 = s[1];
            float s2 = s[2];
            float s3 = s[3];
            float s4 = s[4];
            float s5 = s[5];
            float s6 = s[6];
            float s7 = s[7];
            float s8 = s[8];
            float s9 = s[9];
            float s10 = s[10];
            float s11 = s[11];
            float s12 = s[12];
            float s13 = s[13];
            float s14 = s[14];
            float s15 = s[15];
            float s16 = s[16];
            float s17 = s[17];
            float s18 = s[18];
            float s19 = s[19];
            float s20 = s[20];
            float s21 = s[21];
            float s22 = s[22];
            float s23 = s[23];
            float s24 = s[24];
            float s25 = s[25];
            float s26 = s[26];
            float s27 = s[27];
            float s28 = s[28];
            float s29 = s[29];
            float s30 = s[30];
            float s31 = s[31];

            float p0 = s0 + s31;
            float p1 = s1 + s30;
            float p2 = s2 + s29;
            float p3 = s3 + s28;
            float p4 = s4 + s27;
            float p5 = s5 + s26;
            float p6 = s6 + s25;
            float p7 = s7 + s24;
            float p8 = s8 + s23;
            float p9 = s9 + s22;
            float p10 = s10 + s21;
            float p11 = s11 + s20;
            float p12 = s12 + s19;
            float p13 = s13 + s18;
            float p14 = s14 + s17;
            float p15 = s15 + s16;

            float pp0 = p0 + p15;
            float pp1 = p1 + p14;
            float pp2 = p2 + p13;
            float pp3 = p3 + p12;
            float pp4 = p4 + p11;
            float pp5 = p5 + p10;
            float pp6 = p6 + p9;
            float pp7 = p7 + p8;
            float pp8 = (p0 - p15) * cos1_32;
            float pp9 = (p1 - p14) * cos3_32;
            float pp10 = (p2 - p13) * cos5_32;
            float pp11 = (p3 - p12) * cos7_32;
            float pp12 = (p4 - p11) * cos9_32;
            float pp13 = (p5 - p10) * cos11_32;
            float pp14 = (p6 - p9) * cos13_32;
            float pp15 = (p7 - p8) * cos15_32;

            p0 = pp0 + pp7;
            p1 = pp1 + pp6;
            p2 = pp2 + pp5;
            p3 = pp3 + pp4;
            p4 = (pp0 - pp7) * cos1_16;
            p5 = (pp1 - pp6) * cos3_16;
            p6 = (pp2 - pp5) * cos5_16;
            p7 = (pp3 - pp4) * cos7_16;
            p8 = pp8 + pp15;
            p9 = pp9 + pp14;
            p10 = pp10 + pp13;
            p11 = pp11 + pp12;
            p12 = (pp8 - pp15) * cos1_16;
            p13 = (pp9 - pp14) * cos3_16;
            p14 = (pp10 - pp13) * cos5_16;
            p15 = (pp11 - pp12) * cos7_16;


            pp0 = p0 + p3;
            pp1 = p1 + p2;
            pp2 = (p0 - p3) * cos1_8;
            pp3 = (p1 - p2) * cos3_8;
            pp4 = p4 + p7;
            pp5 = p5 + p6;
            pp6 = (p4 - p7) * cos1_8;
            pp7 = (p5 - p6) * cos3_8;
            pp8 = p8 + p11;
            pp9 = p9 + p10;
            pp10 = (p8 - p11) * cos1_8;
            pp11 = (p9 - p10) * cos3_8;
            pp12 = p12 + p15;
            pp13 = p13 + p14;
            pp14 = (p12 - p15) * cos1_8;
            pp15 = (p13 - p14) * cos3_8;

            p0 = pp0 + pp1;
            p1 = (pp0 - pp1) * cos1_4;
            p2 = pp2 + pp3;
            p3 = (pp2 - pp3) * cos1_4;
            p4 = pp4 + pp5;
            p5 = (pp4 - pp5) * cos1_4;
            p6 = pp6 + pp7;
            p7 = (pp6 - pp7) * cos1_4;
            p8 = pp8 + pp9;
            p9 = (pp8 - pp9) * cos1_4;
            p10 = pp10 + pp11;
            p11 = (pp10 - pp11) * cos1_4;
            p12 = pp12 + pp13;
            p13 = (pp12 - pp13) * cos1_4;
            p14 = pp14 + pp15;
            p15 = (pp14 - pp15) * cos1_4;

            // this is pretty insane coding
            float tmp1;
            new_v19 = -(new_v4 = (new_v12 = p7) + p5) - p6;
            new_v27 = -p6 - p7 - p4;
            new_v6 = (new_v10 = (new_v14 = p15) + p11) + p13;
            new_v17 = -(new_v2 = p15 + p13 + p9) - p14;
            new_v21 = (tmp1 = -p14 - p15 - p10 - p11) - p13;
            new_v29 = -p14 - p15 - p12 - p8;
            new_v25 = tmp1 - p12;
            new_v31 = -p0;
            new_v0 = p1;
            new_v23 = -(new_v8 = p3) - p2;

            p0 = (s0 - s31) * cos1_64;
            p1 = (s1 - s30) * cos3_64;
            p2 = (s2 - s29) * cos5_64;
            p3 = (s3 - s28) * cos7_64;
            p4 = (s4 - s27) * cos9_64;
            p5 = (s5 - s26) * cos11_64;
            p6 = (s6 - s25) * cos13_64;
            p7 = (s7 - s24) * cos15_64;
            p8 = (s8 - s23) * cos17_64;
            p9 = (s9 - s22) * cos19_64;
            p10 = (s10 - s21) * cos21_64;
            p11 = (s11 - s20) * cos23_64;
            p12 = (s12 - s19) * cos25_64;
            p13 = (s13 - s18) * cos27_64;
            p14 = (s14 - s17) * cos29_64;
            p15 = (s15 - s16) * cos31_64;


            pp0 = p0 + p15;
            pp1 = p1 + p14;
            pp2 = p2 + p13;
            pp3 = p3 + p12;
            pp4 = p4 + p11;
            pp5 = p5 + p10;
            pp6 = p6 + p9;
            pp7 = p7 + p8;
            pp8 = (p0 - p15) * cos1_32;
            pp9 = (p1 - p14) * cos3_32;
            pp10 = (p2 - p13) * cos5_32;
            pp11 = (p3 - p12) * cos7_32;
            pp12 = (p4 - p11) * cos9_32;
            pp13 = (p5 - p10) * cos11_32;
            pp14 = (p6 - p9) * cos13_32;
            pp15 = (p7 - p8) * cos15_32;


            p0 = pp0 + pp7;
            p1 = pp1 + pp6;
            p2 = pp2 + pp5;
            p3 = pp3 + pp4;
            p4 = (pp0 - pp7) * cos1_16;
            p5 = (pp1 - pp6) * cos3_16;
            p6 = (pp2 - pp5) * cos5_16;
            p7 = (pp3 - pp4) * cos7_16;
            p8 = pp8 + pp15;
            p9 = pp9 + pp14;
            p10 = pp10 + pp13;
            p11 = pp11 + pp12;
            p12 = (pp8 - pp15) * cos1_16;
            p13 = (pp9 - pp14) * cos3_16;
            p14 = (pp10 - pp13) * cos5_16;
            p15 = (pp11 - pp12) * cos7_16;


            pp0 = p0 + p3;
            pp1 = p1 + p2;
            pp2 = (p0 - p3) * cos1_8;
            pp3 = (p1 - p2) * cos3_8;
            pp4 = p4 + p7;
            pp5 = p5 + p6;
            pp6 = (p4 - p7) * cos1_8;
            pp7 = (p5 - p6) * cos3_8;
            pp8 = p8 + p11;
            pp9 = p9 + p10;
            pp10 = (p8 - p11) * cos1_8;
            pp11 = (p9 - p10) * cos3_8;
            pp12 = p12 + p15;
            pp13 = p13 + p14;
            pp14 = (p12 - p15) * cos1_8;
            pp15 = (p13 - p14) * cos3_8;


            p0 = pp0 + pp1;
            p1 = (pp0 - pp1) * cos1_4;
            p2 = pp2 + pp3;
            p3 = (pp2 - pp3) * cos1_4;
            p4 = pp4 + pp5;
            p5 = (pp4 - pp5) * cos1_4;
            p6 = pp6 + pp7;
            p7 = (pp6 - pp7) * cos1_4;
            p8 = pp8 + pp9;
            p9 = (pp8 - pp9) * cos1_4;
            p10 = pp10 + pp11;
            p11 = (pp10 - pp11) * cos1_4;
            p12 = pp12 + pp13;
            p13 = (pp12 - pp13) * cos1_4;
            p14 = pp14 + pp15;
            p15 = (pp14 - pp15) * cos1_4;


            // manually doing something that a compiler should handle sucks
            // coding like this is hard to read
            float tmp2;
            new_v5 = (new_v11 = (new_v13 = (new_v15 = p15) + p7) + p11) + p5 + p13;
            new_v7 = (new_v9 = p15 + p11 + p3) + p13;
            new_v16 = -(new_v1 = (tmp1 = p13 + p15 + p9) + p1) - p14;
            new_v18 = -(new_v3 = tmp1 + p5 + p7) - p6 - p14;

            new_v22 = (tmp1 = -p10 - p11 - p14 - p15) - p13 - p2 - p3;
            new_v20 = tmp1 - p13 - p5 - p6 - p7;
            new_v24 = tmp1 - p12 - p2 - p3;
            new_v26 = tmp1 - p12 - (tmp2 = p4 + p6 + p7);
            new_v30 = (tmp1 = -p8 - p12 - p14 - p15) - p0;
            new_v28 = tmp1 - tmp2;

            // insert V[0-15] (== new_v[0-15]) into actual v:	
            // float[] x2 = actual_v + actual_write_pos;
            float[] dest = actual_v;

            int pos = m_Position;

            dest[0 + pos] = new_v0;
            dest[16 + pos] = new_v1;
            dest[32 + pos] = new_v2;
            dest[48 + pos] = new_v3;
            dest[64 + pos] = new_v4;
            dest[80 + pos] = new_v5;
            dest[96 + pos] = new_v6;
            dest[112 + pos] = new_v7;
            dest[128 + pos] = new_v8;
            dest[144 + pos] = new_v9;
            dest[160 + pos] = new_v10;
            dest[176 + pos] = new_v11;
            dest[192 + pos] = new_v12;
            dest[208 + pos] = new_v13;
            dest[224 + pos] = new_v14;
            dest[240 + pos] = new_v15;

            // V[16] is always 0.0:
            dest[256 + pos] = 0.0f;

            // insert V[17-31] (== -new_v[15-1]) into actual v:
            dest[272 + pos] = -new_v15;
            dest[288 + pos] = -new_v14;
            dest[304 + pos] = -new_v13;
            dest[320 + pos] = -new_v12;
            dest[336 + pos] = -new_v11;
            dest[352 + pos] = -new_v10;
            dest[368 + pos] = -new_v9;
            dest[384 + pos] = -new_v8;
            dest[400 + pos] = -new_v7;
            dest[416 + pos] = -new_v6;
            dest[432 + pos] = -new_v5;
            dest[448 + pos] = -new_v4;
            dest[464 + pos] = -new_v3;
            dest[480 + pos] = -new_v2;
            dest[496 + pos] = -new_v1;

            // insert V[32] (== -new_v[0]) into other v:
            dest = (actual_v == v1) ? v2 : v1;

            dest[0 + pos] = -new_v0;
            // insert V[33-48] (== new_v[16-31]) into other v:
            dest[16 + pos] = new_v16;
            dest[32 + pos] = new_v17;
            dest[48 + pos] = new_v18;
            dest[64 + pos] = new_v19;
            dest[80 + pos] = new_v20;
            dest[96 + pos] = new_v21;
            dest[112 + pos] = new_v22;
            dest[128 + pos] = new_v23;
            dest[144 + pos] = new_v24;
            dest[160 + pos] = new_v25;
            dest[176 + pos] = new_v26;
            dest[192 + pos] = new_v27;
            dest[208 + pos] = new_v28;
            dest[224 + pos] = new_v29;
            dest[240 + pos] = new_v30;
            dest[256 + pos] = new_v31;

            // insert V[49-63] (== new_v[30-16]) into other v:
            dest[272 + pos] = new_v30;
            dest[288 + pos] = new_v29;
            dest[304 + pos] = new_v28;
            dest[320 + pos] = new_v27;
            dest[336 + pos] = new_v26;
            dest[352 + pos] = new_v25;
            dest[368 + pos] = new_v24;
            dest[384 + pos] = new_v23;
            dest[400 + pos] = new_v22;
            dest[416 + pos] = new_v21;
            dest[432 + pos] = new_v20;
            dest[448 + pos] = new_v19;
            dest[464 + pos] = new_v18;
            dest[480 + pos] = new_v17;
            dest[496 + pos] = new_v16;
        }

        void compute_pcm_samples0()
        {
            float[] vp = actual_v;
            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float pcm_sample;
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                pcm_sample = ((vp[0 + dvp] * dp[0]) + (vp[15 + dvp] * dp[1]) + (vp[14 + dvp] * dp[2]) + (vp[13 + dvp] * dp[3]) + (vp[12 + dvp] * dp[4]) + (vp[11 + dvp] * dp[5]) + (vp[10 + dvp] * dp[6]) + (vp[9 + dvp] * dp[7]) + (vp[8 + dvp] * dp[8]) + (vp[7 + dvp] * dp[9]) + (vp[6 + dvp] * dp[10]) + (vp[5 + dvp] * dp[11]) + (vp[4 + dvp] * dp[12]) + (vp[3 + dvp] * dp[13]) + (vp[2 + dvp] * dp[14]) + (vp[1 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples1()
        {
            float[] vp = actual_v;
            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[1 + dvp] * dp[0]) + (vp[0 + dvp] * dp[1]) + (vp[15 + dvp] * dp[2]) + (vp[14 + dvp] * dp[3]) + (vp[13 + dvp] * dp[4]) + (vp[12 + dvp] * dp[5]) + (vp[11 + dvp] * dp[6]) + (vp[10 + dvp] * dp[7]) + (vp[9 + dvp] * dp[8]) + (vp[8 + dvp] * dp[9]) + (vp[7 + dvp] * dp[10]) + (vp[6 + dvp] * dp[11]) + (vp[5 + dvp] * dp[12]) + (vp[4 + dvp] * dp[13]) + (vp[3 + dvp] * dp[14]) + (vp[2 + dvp] * dp[15])) ;

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples2()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[2 + dvp] * dp[0]) + (vp[1 + dvp] * dp[1]) + (vp[0 + dvp] * dp[2]) + (vp[15 + dvp] * dp[3]) + (vp[14 + dvp] * dp[4]) + (vp[13 + dvp] * dp[5]) + (vp[12 + dvp] * dp[6]) + (vp[11 + dvp] * dp[7]) + (vp[10 + dvp] * dp[8]) + (vp[9 + dvp] * dp[9]) + (vp[8 + dvp] * dp[10]) + (vp[7 + dvp] * dp[11]) + (vp[6 + dvp] * dp[12]) + (vp[5 + dvp] * dp[13]) + (vp[4 + dvp] * dp[14]) + (vp[3 + dvp] * dp[15])) ;

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples3()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[3 + dvp] * dp[0]) + (vp[2 + dvp] * dp[1]) + (vp[1 + dvp] * dp[2]) + (vp[0 + dvp] * dp[3]) + (vp[15 + dvp] * dp[4]) + (vp[14 + dvp] * dp[5]) + (vp[13 + dvp] * dp[6]) + (vp[12 + dvp] * dp[7]) + (vp[11 + dvp] * dp[8]) + (vp[10 + dvp] * dp[9]) + (vp[9 + dvp] * dp[10]) + (vp[8 + dvp] * dp[11]) + (vp[7 + dvp] * dp[12]) + (vp[6 + dvp] * dp[13]) + (vp[5 + dvp] * dp[14]) + (vp[4 + dvp] * dp[15])) ;

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples4()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[4 + dvp] * dp[0]) + (vp[3 + dvp] * dp[1]) + (vp[2 + dvp] * dp[2]) + (vp[1 + dvp] * dp[3]) + (vp[0 + dvp] * dp[4]) + (vp[15 + dvp] * dp[5]) + (vp[14 + dvp] * dp[6]) + (vp[13 + dvp] * dp[7]) + (vp[12 + dvp] * dp[8]) + (vp[11 + dvp] * dp[9]) + (vp[10 + dvp] * dp[10]) + (vp[9 + dvp] * dp[11]) + (vp[8 + dvp] * dp[12]) + (vp[7 + dvp] * dp[13]) + (vp[6 + dvp] * dp[14]) + (vp[5 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples5()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[5 + dvp] * dp[0]) + (vp[4 + dvp] * dp[1]) + (vp[3 + dvp] * dp[2]) + (vp[2 + dvp] * dp[3]) + (vp[1 + dvp] * dp[4]) + (vp[0 + dvp] * dp[5]) + (vp[15 + dvp] * dp[6]) + (vp[14 + dvp] * dp[7]) + (vp[13 + dvp] * dp[8]) + (vp[12 + dvp] * dp[9]) + (vp[11 + dvp] * dp[10]) + (vp[10 + dvp] * dp[11]) + (vp[9 + dvp] * dp[12]) + (vp[8 + dvp] * dp[13]) + (vp[7 + dvp] * dp[14]) + (vp[6 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples6()
        {
            float[] vp = actual_v;
            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[6 + dvp] * dp[0]) + (vp[5 + dvp] * dp[1]) + (vp[4 + dvp] * dp[2]) + (vp[3 + dvp] * dp[3]) + (vp[2 + dvp] * dp[4]) + (vp[1 + dvp] * dp[5]) + (vp[0 + dvp] * dp[6]) + (vp[15 + dvp] * dp[7]) + (vp[14 + dvp] * dp[8]) + (vp[13 + dvp] * dp[9]) + (vp[12 + dvp] * dp[10]) + (vp[11 + dvp] * dp[11]) + (vp[10 + dvp] * dp[12]) + (vp[9 + dvp] * dp[13]) + (vp[8 + dvp] * dp[14]) + (vp[7 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples7()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[7 + dvp] * dp[0]) + (vp[6 + dvp] * dp[1]) + (vp[5 + dvp] * dp[2]) + (vp[4 + dvp] * dp[3]) + (vp[3 + dvp] * dp[4]) + (vp[2 + dvp] * dp[5]) + (vp[1 + dvp] * dp[6]) + (vp[0 + dvp] * dp[7]) + (vp[15 + dvp] * dp[8]) + (vp[14 + dvp] * dp[9]) + (vp[13 + dvp] * dp[10]) + (vp[12 + dvp] * dp[11]) + (vp[11 + dvp] * dp[12]) + (vp[10 + dvp] * dp[13]) + (vp[9 + dvp] * dp[14]) + (vp[8 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples8()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[8 + dvp] * dp[0]) + (vp[7 + dvp] * dp[1]) + (vp[6 + dvp] * dp[2]) + (vp[5 + dvp] * dp[3]) + (vp[4 + dvp] * dp[4]) + (vp[3 + dvp] * dp[5]) + (vp[2 + dvp] * dp[6]) + (vp[1 + dvp] * dp[7]) + (vp[0 + dvp] * dp[8]) + (vp[15 + dvp] * dp[9]) + (vp[14 + dvp] * dp[10]) + (vp[13 + dvp] * dp[11]) + (vp[12 + dvp] * dp[12]) + (vp[11 + dvp] * dp[13]) + (vp[10 + dvp] * dp[14]) + (vp[9 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples9()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[9 + dvp] * dp[0]) + (vp[8 + dvp] * dp[1]) + (vp[7 + dvp] * dp[2]) + (vp[6 + dvp] * dp[3]) + (vp[5 + dvp] * dp[4]) + (vp[4 + dvp] * dp[5]) + (vp[3 + dvp] * dp[6]) + (vp[2 + dvp] * dp[7]) + (vp[1 + dvp] * dp[8]) + (vp[0 + dvp] * dp[9]) + (vp[15 + dvp] * dp[10]) + (vp[14 + dvp] * dp[11]) + (vp[13 + dvp] * dp[12]) + (vp[12 + dvp] * dp[13]) + (vp[11 + dvp] * dp[14]) + (vp[10 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples10()
        {
            float[] vp = actual_v;
            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[10 + dvp] * dp[0]) + (vp[9 + dvp] * dp[1]) + (vp[8 + dvp] * dp[2]) + (vp[7 + dvp] * dp[3]) + (vp[6 + dvp] * dp[4]) + (vp[5 + dvp] * dp[5]) + (vp[4 + dvp] * dp[6]) + (vp[3 + dvp] * dp[7]) + (vp[2 + dvp] * dp[8]) + (vp[1 + dvp] * dp[9]) + (vp[0 + dvp] * dp[10]) + (vp[15 + dvp] * dp[11]) + (vp[14 + dvp] * dp[12]) + (vp[13 + dvp] * dp[13]) + (vp[12 + dvp] * dp[14]) + (vp[11 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples11()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[11 + dvp] * dp[0]) + (vp[10 + dvp] * dp[1]) + (vp[9 + dvp] * dp[2]) + (vp[8 + dvp] * dp[3]) + (vp[7 + dvp] * dp[4]) + (vp[6 + dvp] * dp[5]) + (vp[5 + dvp] * dp[6]) + (vp[4 + dvp] * dp[7]) + (vp[3 + dvp] * dp[8]) + (vp[2 + dvp] * dp[9]) + (vp[1 + dvp] * dp[10]) + (vp[0 + dvp] * dp[11]) + (vp[15 + dvp] * dp[12]) + (vp[14 + dvp] * dp[13]) + (vp[13 + dvp] * dp[14]) + (vp[12 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples12()
        {
            float[] vp = actual_v;
            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[12 + dvp] * dp[0]) + (vp[11 + dvp] * dp[1]) + (vp[10 + dvp] * dp[2]) + (vp[9 + dvp] * dp[3]) + (vp[8 + dvp] * dp[4]) + (vp[7 + dvp] * dp[5]) + (vp[6 + dvp] * dp[6]) + (vp[5 + dvp] * dp[7]) + (vp[4 + dvp] * dp[8]) + (vp[3 + dvp] * dp[9]) + (vp[2 + dvp] * dp[10]) + (vp[1 + dvp] * dp[11]) + (vp[0 + dvp] * dp[12]) + (vp[15 + dvp] * dp[13]) + (vp[14 + dvp] * dp[14]) + (vp[13 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples13()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[13 + dvp] * dp[0]) + (vp[12 + dvp] * dp[1]) + (vp[11 + dvp] * dp[2]) + (vp[10 + dvp] * dp[3]) + (vp[9 + dvp] * dp[4]) + (vp[8 + dvp] * dp[5]) + (vp[7 + dvp] * dp[6]) + (vp[6 + dvp] * dp[7]) + (vp[5 + dvp] * dp[8]) + (vp[4 + dvp] * dp[9]) + (vp[3 + dvp] * dp[10]) + (vp[2 + dvp] * dp[11]) + (vp[1 + dvp] * dp[12]) + (vp[0 + dvp] * dp[13]) + (vp[15 + dvp] * dp[14]) + (vp[14 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples14()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                float pcm_sample;

                pcm_sample = ((vp[14 + dvp] * dp[0]) + (vp[13 + dvp] * dp[1]) + (vp[12 + dvp] * dp[2]) + (vp[11 + dvp] * dp[3]) + (vp[10 + dvp] * dp[4]) + (vp[9 + dvp] * dp[5]) + (vp[8 + dvp] * dp[6]) + (vp[7 + dvp] * dp[7]) + (vp[6 + dvp] * dp[8]) + (vp[5 + dvp] * dp[9]) + (vp[4 + dvp] * dp[10]) + (vp[3 + dvp] * dp[11]) + (vp[2 + dvp] * dp[12]) + (vp[1 + dvp] * dp[13]) + (vp[0 + dvp] * dp[14]) + (vp[15 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;

                dvp += 16;
            }
            // for
        }
        void compute_pcm_samples15()
        {
            float[] vp = actual_v;

            //int inc = v_inc;
            float[] tmpOut = m_PCMSamples;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++)
            {
                float pcm_sample;
                float[] dp = MP3AudioConstants.SynthesisFilterValues[i];
                pcm_sample = ((vp[15 + dvp] * dp[0]) + (vp[14 + dvp] * dp[1]) + (vp[13 + dvp] * dp[2]) + (vp[12 + dvp] * dp[3]) + (vp[11 + dvp] * dp[4]) + (vp[10 + dvp] * dp[5]) + (vp[9 + dvp] * dp[6]) + (vp[8 + dvp] * dp[7]) + (vp[7 + dvp] * dp[8]) + (vp[6 + dvp] * dp[9]) + (vp[5 + dvp] * dp[10]) + (vp[4 + dvp] * dp[11]) + (vp[3 + dvp] * dp[12]) + (vp[2 + dvp] * dp[13]) + (vp[1 + dvp] * dp[14]) + (vp[0 + dvp] * dp[15]));

                tmpOut[i] = pcm_sample;
                dvp += 16;
            }
            // for
        }

        void compute_pcm_samples()
        {

            switch (m_Position)
            {

                case 0:
                    compute_pcm_samples0();
                    break;

                case 1:
                    compute_pcm_samples1();
                    break;

                case 2:
                    compute_pcm_samples2();
                    break;

                case 3:
                    compute_pcm_samples3();
                    break;

                case 4:
                    compute_pcm_samples4();
                    break;

                case 5:
                    compute_pcm_samples5();
                    break;

                case 6:
                    compute_pcm_samples6();
                    break;

                case 7:
                    compute_pcm_samples7();
                    break;

                case 8:
                    compute_pcm_samples8();
                    break;

                case 9:
                    compute_pcm_samples9();
                    break;

                case 10:
                    compute_pcm_samples10();
                    break;

                case 11:
                    compute_pcm_samples11();
                    break;

                case 12:
                    compute_pcm_samples12();
                    break;

                case 13:
                    compute_pcm_samples13();
                    break;

                case 14:
                    compute_pcm_samples14();
                    break;

                case 15:
                    compute_pcm_samples15();
                    break;
            }
        }

        /// <summary>Calculate 32 PCM samples and output them</summary>
        /// <remarks>This is called by the decoders on each frame decode.</remarks>
        /// <param name="buffer">The buffer to write the samples to.</param>
        public void CalculateSamples(MP3AudioStereoBuffer buffer)
        {
            compute_new_v();
            compute_pcm_samples();
            buffer.AddSamples(ChannelNumber, m_PCMSamples);
            m_Position = (m_Position + 1) & 0xf;
            actual_v = (actual_v == v1) ? v2 : v1;
        }

        static readonly float cos1_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 64.0)));
        static readonly float cos3_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 64.0)));
        static readonly float cos5_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 64.0)));
        static readonly float cos7_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 64.0)));
        static readonly float cos9_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 9.0 / 64.0)));
        static readonly float cos11_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 11.0 / 64.0)));
        static readonly float cos13_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 13.0 / 64.0)));
        static readonly float cos15_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 15.0 / 64.0)));
        static readonly float cos17_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 17.0 / 64.0)));
        static readonly float cos19_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 19.0 / 64.0)));
        static readonly float cos21_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 21.0 / 64.0)));
        static readonly float cos23_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 23.0 / 64.0)));
        static readonly float cos25_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 25.0 / 64.0)));
        static readonly float cos27_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 27.0 / 64.0)));
        static readonly float cos29_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 29.0 / 64.0)));
        static readonly float cos31_64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 31.0 / 64.0)));
        static readonly float cos1_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 32.0)));
        static readonly float cos3_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 32.0)));
        static readonly float cos5_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 32.0)));
        static readonly float cos7_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 32.0)));
        static readonly float cos9_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 9.0 / 32.0)));
        static readonly float cos11_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 11.0 / 32.0)));
        static readonly float cos13_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 13.0 / 32.0)));
        static readonly float cos15_32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 15.0 / 32.0)));
        static readonly float cos1_16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 16.0)));
        static readonly float cos3_16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 16.0)));
        static readonly float cos5_16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 16.0)));
        static readonly float cos7_16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 16.0)));
        static readonly float cos1_8 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 8.0)));
        static readonly float cos3_8 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 8.0)));
        static readonly float cos1_4 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 4.0)));
    }
}