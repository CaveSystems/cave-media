#region Notes

/*
    This is part of the MP3Decoder implementation.
    Based upon mpg123, jlayer 1.0.1 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/

#endregion

using System;

namespace Cave.Media.Audio.MP3;

/// <summary>A class for the synthesis filter bank. Taken from JavaLayer.</summary>
public class MP3AudioSynthesisFilter
{
    float[] v1;
    float[] v2;
    float[] actual_v = [];

    /// <summary>The current write position (0-15).</summary>
    int m_Position;

    /// <summary>The 32 subband samples.</summary>
    float[] m_Samples;

    /// <summary>Computes PCM Samples.</summary>
    float[] m_PCMSamples;

    /// <summary>The channel number this filter uses.</summary>
    public readonly int ChannelNumber;

    /// <summary>The equalizer factors this filter uses.</summary>
    public float[] EqualizerFactors;

    /// <summary>Initializes a new instance of the <see cref="MP3AudioSynthesisFilter"/> class.</summary>
    /// <param name="channelNumber">The channel number this filter uses.</param>
    /// <param name="scaleFactor">
    /// The scalefactor scales the calculated float pcm samples to short values (raw pcm samples are in [-1.0, 1.0], if no violations occur).
    /// </param>
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
        for (var p = 0; p < 512; p++)
        {
            v1[p] = v2[p] = 0.0f;
        }

        // initialize samples[]:
        for (var p2 = 0; p2 < 32; p2++)
        {
            m_Samples[p2] = 0.0f;
        }

        actual_v = v1;
        m_Position = 15;
    }

    /// <summary>Adds a sample.</summary>
    /// <param name="sample">The sample.</param>
    /// <param name="subbandnumber">The subbandnumber.</param>
    public void AddSample(float sample, int subbandnumber) => m_Samples[subbandnumber] = EqualizerFactors[subbandnumber] * sample;

    /// <summary>Adds the samples for all 32 bands.</summary>
    /// <param name="samples">The samples.</param>
    public void AddSamples(float[] samples)
    {
#if DEBUG
        if (samples.Length != 32) throw new ArgumentException("Samples[32] expected!");
#endif
        for (var i = 31; i >= 0; i--)
        {
            m_Samples[i] = samples[i] * EqualizerFactors[i];
        }
    }

    /// <summary>Compute new values via a fast cosine transform.</summary>
    void compute_new_v()
    {
        float new_v0, new_v1, new_v2, new_v3, new_v4, new_v5, new_v6, new_v7, new_v8, new_v9;
        float new_v10, new_v11, new_v12, new_v13, new_v14, new_v15, new_v16, new_v17, new_v18, new_v19;
        float new_v20, new_v21, new_v22, new_v23, new_v24, new_v25, new_v26, new_v27, new_v28, new_v29;
        float new_v30, new_v31;

        new_v0 = new_v1 = new_v2 = new_v3 = new_v4 = new_v5 = new_v6 = new_v7 = new_v8 = new_v9 = new_v10 = new_v11 = new_v12 = new_v13 = new_v14 = new_v15 = new_v16 = new_v17 = new_v18 = new_v19 = new_v20 = new_v21 = new_v22 = new_v23 = new_v24 = new_v25 = new_v26 = new_v27 = new_v28 = new_v29 = new_v30 = new_v31 = 0.0f;

        var s = m_Samples;

        var s0 = s[0];
        var s1 = s[1];
        var s2 = s[2];
        var s3 = s[3];
        var s4 = s[4];
        var s5 = s[5];
        var s6 = s[6];
        var s7 = s[7];
        var s8 = s[8];
        var s9 = s[9];
        var s10 = s[10];
        var s11 = s[11];
        var s12 = s[12];
        var s13 = s[13];
        var s14 = s[14];
        var s15 = s[15];
        var s16 = s[16];
        var s17 = s[17];
        var s18 = s[18];
        var s19 = s[19];
        var s20 = s[20];
        var s21 = s[21];
        var s22 = s[22];
        var s23 = s[23];
        var s24 = s[24];
        var s25 = s[25];
        var s26 = s[26];
        var s27 = s[27];
        var s28 = s[28];
        var s29 = s[29];
        var s30 = s[30];
        var s31 = s[31];

        var p0 = s0 + s31;
        var p1 = s1 + s30;
        var p2 = s2 + s29;
        var p3 = s3 + s28;
        var p4 = s4 + s27;
        var p5 = s5 + s26;
        var p6 = s6 + s25;
        var p7 = s7 + s24;
        var p8 = s8 + s23;
        var p9 = s9 + s22;
        var p10 = s10 + s21;
        var p11 = s11 + s20;
        var p12 = s12 + s19;
        var p13 = s13 + s18;
        var p14 = s14 + s17;
        var p15 = s15 + s16;

        var pp0 = p0 + p15;
        var pp1 = p1 + p14;
        var pp2 = p2 + p13;
        var pp3 = p3 + p12;
        var pp4 = p4 + p11;
        var pp5 = p5 + p10;
        var pp6 = p6 + p9;
        var pp7 = p7 + p8;
        var pp8 = (p0 - p15) * cos1a32;
        var pp9 = (p1 - p14) * cos3a32;
        var pp10 = (p2 - p13) * cos5a32;
        var pp11 = (p3 - p12) * cos7a32;
        var pp12 = (p4 - p11) * cos9a32;
        var pp13 = (p5 - p10) * cos11a32;
        var pp14 = (p6 - p9) * cos13a32;
        var pp15 = (p7 - p8) * cos15a32;

        p0 = pp0 + pp7;
        p1 = pp1 + pp6;
        p2 = pp2 + pp5;
        p3 = pp3 + pp4;
        p4 = (pp0 - pp7) * cos1a16;
        p5 = (pp1 - pp6) * cos3a16;
        p6 = (pp2 - pp5) * cos5a16;
        p7 = (pp3 - pp4) * cos7a16;
        p8 = pp8 + pp15;
        p9 = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = (pp8 - pp15) * cos1a16;
        p13 = (pp9 - pp14) * cos3a16;
        p14 = (pp10 - pp13) * cos5a16;
        p15 = (pp11 - pp12) * cos7a16;

        pp0 = p0 + p3;
        pp1 = p1 + p2;
        pp2 = (p0 - p3) * cos1a8;
        pp3 = (p1 - p2) * cos3a8;
        pp4 = p4 + p7;
        pp5 = p5 + p6;
        pp6 = (p4 - p7) * cos1a8;
        pp7 = (p5 - p6) * cos3a8;
        pp8 = p8 + p11;
        pp9 = p9 + p10;
        pp10 = (p8 - p11) * cos1a8;
        pp11 = (p9 - p10) * cos3a8;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = (p12 - p15) * cos1a8;
        pp15 = (p13 - p14) * cos3a8;

        p0 = pp0 + pp1;
        p1 = (pp0 - pp1) * cos1a4;
        p2 = pp2 + pp3;
        p3 = (pp2 - pp3) * cos1a4;
        p4 = pp4 + pp5;
        p5 = (pp4 - pp5) * cos1a4;
        p6 = pp6 + pp7;
        p7 = (pp6 - pp7) * cos1a4;
        p8 = pp8 + pp9;
        p9 = (pp8 - pp9) * cos1a4;
        p10 = pp10 + pp11;
        p11 = (pp10 - pp11) * cos1a4;
        p12 = pp12 + pp13;
        p13 = (pp12 - pp13) * cos1a4;
        p14 = pp14 + pp15;
        p15 = (pp14 - pp15) * cos1a4;

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

        p0 = (s0 - s31) * cos1a64;
        p1 = (s1 - s30) * cos3a64;
        p2 = (s2 - s29) * cos5a64;
        p3 = (s3 - s28) * cos7a64;
        p4 = (s4 - s27) * cos9a64;
        p5 = (s5 - s26) * cos11a64;
        p6 = (s6 - s25) * cos13a64;
        p7 = (s7 - s24) * cos15a64;
        p8 = (s8 - s23) * cos17a64;
        p9 = (s9 - s22) * cos19a64;
        p10 = (s10 - s21) * cos21a64;
        p11 = (s11 - s20) * cos23a64;
        p12 = (s12 - s19) * cos25a64;
        p13 = (s13 - s18) * cos27a64;
        p14 = (s14 - s17) * cos29a64;
        p15 = (s15 - s16) * cos31a64;

        pp0 = p0 + p15;
        pp1 = p1 + p14;
        pp2 = p2 + p13;
        pp3 = p3 + p12;
        pp4 = p4 + p11;
        pp5 = p5 + p10;
        pp6 = p6 + p9;
        pp7 = p7 + p8;
        pp8 = (p0 - p15) * cos1a32;
        pp9 = (p1 - p14) * cos3a32;
        pp10 = (p2 - p13) * cos5a32;
        pp11 = (p3 - p12) * cos7a32;
        pp12 = (p4 - p11) * cos9a32;
        pp13 = (p5 - p10) * cos11a32;
        pp14 = (p6 - p9) * cos13a32;
        pp15 = (p7 - p8) * cos15a32;

        p0 = pp0 + pp7;
        p1 = pp1 + pp6;
        p2 = pp2 + pp5;
        p3 = pp3 + pp4;
        p4 = (pp0 - pp7) * cos1a16;
        p5 = (pp1 - pp6) * cos3a16;
        p6 = (pp2 - pp5) * cos5a16;
        p7 = (pp3 - pp4) * cos7a16;
        p8 = pp8 + pp15;
        p9 = pp9 + pp14;
        p10 = pp10 + pp13;
        p11 = pp11 + pp12;
        p12 = (pp8 - pp15) * cos1a16;
        p13 = (pp9 - pp14) * cos3a16;
        p14 = (pp10 - pp13) * cos5a16;
        p15 = (pp11 - pp12) * cos7a16;

        pp0 = p0 + p3;
        pp1 = p1 + p2;
        pp2 = (p0 - p3) * cos1a8;
        pp3 = (p1 - p2) * cos3a8;
        pp4 = p4 + p7;
        pp5 = p5 + p6;
        pp6 = (p4 - p7) * cos1a8;
        pp7 = (p5 - p6) * cos3a8;
        pp8 = p8 + p11;
        pp9 = p9 + p10;
        pp10 = (p8 - p11) * cos1a8;
        pp11 = (p9 - p10) * cos3a8;
        pp12 = p12 + p15;
        pp13 = p13 + p14;
        pp14 = (p12 - p15) * cos1a8;
        pp15 = (p13 - p14) * cos3a8;

        p0 = pp0 + pp1;
        p1 = (pp0 - pp1) * cos1a4;
        p2 = pp2 + pp3;
        p3 = (pp2 - pp3) * cos1a4;
        p4 = pp4 + pp5;
        p5 = (pp4 - pp5) * cos1a4;
        p6 = pp6 + pp7;
        p7 = (pp6 - pp7) * cos1a4;
        p8 = pp8 + pp9;
        p9 = (pp8 - pp9) * cos1a4;
        p10 = pp10 + pp11;
        p11 = (pp10 - pp11) * cos1a4;
        p12 = pp12 + pp13;
        p13 = (pp12 - pp13) * cos1a4;
        p14 = pp14 + pp15;
        p15 = (pp14 - pp15) * cos1a4;

        // manually doing something that a compiler should handle sucks coding like this is hard to read
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

        // insert V[0-15] (== new_v[0-15]) into actual v: float[] x2 = actual_v + actual_write_pos;
        var dest = actual_v;

        var pos = m_Position;

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
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            float pcm_sample;
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            pcm_sample = (vp[0 + dvp] * dp[0]) + (vp[15 + dvp] * dp[1]) + (vp[14 + dvp] * dp[2]) + (vp[13 + dvp] * dp[3]) + (vp[12 + dvp] * dp[4]) + (vp[11 + dvp] * dp[5]) + (vp[10 + dvp] * dp[6]) + (vp[9 + dvp] * dp[7]) + (vp[8 + dvp] * dp[8]) + (vp[7 + dvp] * dp[9]) + (vp[6 + dvp] * dp[10]) + (vp[5 + dvp] * dp[11]) + (vp[4 + dvp] * dp[12]) + (vp[3 + dvp] * dp[13]) + (vp[2 + dvp] * dp[14]) + (vp[1 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples1()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[1 + dvp] * dp[0]) + (vp[0 + dvp] * dp[1]) + (vp[15 + dvp] * dp[2]) + (vp[14 + dvp] * dp[3]) + (vp[13 + dvp] * dp[4]) + (vp[12 + dvp] * dp[5]) + (vp[11 + dvp] * dp[6]) + (vp[10 + dvp] * dp[7]) + (vp[9 + dvp] * dp[8]) + (vp[8 + dvp] * dp[9]) + (vp[7 + dvp] * dp[10]) + (vp[6 + dvp] * dp[11]) + (vp[5 + dvp] * dp[12]) + (vp[4 + dvp] * dp[13]) + (vp[3 + dvp] * dp[14]) + (vp[2 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples2()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[2 + dvp] * dp[0]) + (vp[1 + dvp] * dp[1]) + (vp[0 + dvp] * dp[2]) + (vp[15 + dvp] * dp[3]) + (vp[14 + dvp] * dp[4]) + (vp[13 + dvp] * dp[5]) + (vp[12 + dvp] * dp[6]) + (vp[11 + dvp] * dp[7]) + (vp[10 + dvp] * dp[8]) + (vp[9 + dvp] * dp[9]) + (vp[8 + dvp] * dp[10]) + (vp[7 + dvp] * dp[11]) + (vp[6 + dvp] * dp[12]) + (vp[5 + dvp] * dp[13]) + (vp[4 + dvp] * dp[14]) + (vp[3 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples3()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[3 + dvp] * dp[0]) + (vp[2 + dvp] * dp[1]) + (vp[1 + dvp] * dp[2]) + (vp[0 + dvp] * dp[3]) + (vp[15 + dvp] * dp[4]) + (vp[14 + dvp] * dp[5]) + (vp[13 + dvp] * dp[6]) + (vp[12 + dvp] * dp[7]) + (vp[11 + dvp] * dp[8]) + (vp[10 + dvp] * dp[9]) + (vp[9 + dvp] * dp[10]) + (vp[8 + dvp] * dp[11]) + (vp[7 + dvp] * dp[12]) + (vp[6 + dvp] * dp[13]) + (vp[5 + dvp] * dp[14]) + (vp[4 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples4()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[4 + dvp] * dp[0]) + (vp[3 + dvp] * dp[1]) + (vp[2 + dvp] * dp[2]) + (vp[1 + dvp] * dp[3]) + (vp[0 + dvp] * dp[4]) + (vp[15 + dvp] * dp[5]) + (vp[14 + dvp] * dp[6]) + (vp[13 + dvp] * dp[7]) + (vp[12 + dvp] * dp[8]) + (vp[11 + dvp] * dp[9]) + (vp[10 + dvp] * dp[10]) + (vp[9 + dvp] * dp[11]) + (vp[8 + dvp] * dp[12]) + (vp[7 + dvp] * dp[13]) + (vp[6 + dvp] * dp[14]) + (vp[5 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples5()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[5 + dvp] * dp[0]) + (vp[4 + dvp] * dp[1]) + (vp[3 + dvp] * dp[2]) + (vp[2 + dvp] * dp[3]) + (vp[1 + dvp] * dp[4]) + (vp[0 + dvp] * dp[5]) + (vp[15 + dvp] * dp[6]) + (vp[14 + dvp] * dp[7]) + (vp[13 + dvp] * dp[8]) + (vp[12 + dvp] * dp[9]) + (vp[11 + dvp] * dp[10]) + (vp[10 + dvp] * dp[11]) + (vp[9 + dvp] * dp[12]) + (vp[8 + dvp] * dp[13]) + (vp[7 + dvp] * dp[14]) + (vp[6 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples6()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[6 + dvp] * dp[0]) + (vp[5 + dvp] * dp[1]) + (vp[4 + dvp] * dp[2]) + (vp[3 + dvp] * dp[3]) + (vp[2 + dvp] * dp[4]) + (vp[1 + dvp] * dp[5]) + (vp[0 + dvp] * dp[6]) + (vp[15 + dvp] * dp[7]) + (vp[14 + dvp] * dp[8]) + (vp[13 + dvp] * dp[9]) + (vp[12 + dvp] * dp[10]) + (vp[11 + dvp] * dp[11]) + (vp[10 + dvp] * dp[12]) + (vp[9 + dvp] * dp[13]) + (vp[8 + dvp] * dp[14]) + (vp[7 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples7()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[7 + dvp] * dp[0]) + (vp[6 + dvp] * dp[1]) + (vp[5 + dvp] * dp[2]) + (vp[4 + dvp] * dp[3]) + (vp[3 + dvp] * dp[4]) + (vp[2 + dvp] * dp[5]) + (vp[1 + dvp] * dp[6]) + (vp[0 + dvp] * dp[7]) + (vp[15 + dvp] * dp[8]) + (vp[14 + dvp] * dp[9]) + (vp[13 + dvp] * dp[10]) + (vp[12 + dvp] * dp[11]) + (vp[11 + dvp] * dp[12]) + (vp[10 + dvp] * dp[13]) + (vp[9 + dvp] * dp[14]) + (vp[8 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples8()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[8 + dvp] * dp[0]) + (vp[7 + dvp] * dp[1]) + (vp[6 + dvp] * dp[2]) + (vp[5 + dvp] * dp[3]) + (vp[4 + dvp] * dp[4]) + (vp[3 + dvp] * dp[5]) + (vp[2 + dvp] * dp[6]) + (vp[1 + dvp] * dp[7]) + (vp[0 + dvp] * dp[8]) + (vp[15 + dvp] * dp[9]) + (vp[14 + dvp] * dp[10]) + (vp[13 + dvp] * dp[11]) + (vp[12 + dvp] * dp[12]) + (vp[11 + dvp] * dp[13]) + (vp[10 + dvp] * dp[14]) + (vp[9 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples9()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[9 + dvp] * dp[0]) + (vp[8 + dvp] * dp[1]) + (vp[7 + dvp] * dp[2]) + (vp[6 + dvp] * dp[3]) + (vp[5 + dvp] * dp[4]) + (vp[4 + dvp] * dp[5]) + (vp[3 + dvp] * dp[6]) + (vp[2 + dvp] * dp[7]) + (vp[1 + dvp] * dp[8]) + (vp[0 + dvp] * dp[9]) + (vp[15 + dvp] * dp[10]) + (vp[14 + dvp] * dp[11]) + (vp[13 + dvp] * dp[12]) + (vp[12 + dvp] * dp[13]) + (vp[11 + dvp] * dp[14]) + (vp[10 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples10()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[10 + dvp] * dp[0]) + (vp[9 + dvp] * dp[1]) + (vp[8 + dvp] * dp[2]) + (vp[7 + dvp] * dp[3]) + (vp[6 + dvp] * dp[4]) + (vp[5 + dvp] * dp[5]) + (vp[4 + dvp] * dp[6]) + (vp[3 + dvp] * dp[7]) + (vp[2 + dvp] * dp[8]) + (vp[1 + dvp] * dp[9]) + (vp[0 + dvp] * dp[10]) + (vp[15 + dvp] * dp[11]) + (vp[14 + dvp] * dp[12]) + (vp[13 + dvp] * dp[13]) + (vp[12 + dvp] * dp[14]) + (vp[11 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples11()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[11 + dvp] * dp[0]) + (vp[10 + dvp] * dp[1]) + (vp[9 + dvp] * dp[2]) + (vp[8 + dvp] * dp[3]) + (vp[7 + dvp] * dp[4]) + (vp[6 + dvp] * dp[5]) + (vp[5 + dvp] * dp[6]) + (vp[4 + dvp] * dp[7]) + (vp[3 + dvp] * dp[8]) + (vp[2 + dvp] * dp[9]) + (vp[1 + dvp] * dp[10]) + (vp[0 + dvp] * dp[11]) + (vp[15 + dvp] * dp[12]) + (vp[14 + dvp] * dp[13]) + (vp[13 + dvp] * dp[14]) + (vp[12 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples12()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[12 + dvp] * dp[0]) + (vp[11 + dvp] * dp[1]) + (vp[10 + dvp] * dp[2]) + (vp[9 + dvp] * dp[3]) + (vp[8 + dvp] * dp[4]) + (vp[7 + dvp] * dp[5]) + (vp[6 + dvp] * dp[6]) + (vp[5 + dvp] * dp[7]) + (vp[4 + dvp] * dp[8]) + (vp[3 + dvp] * dp[9]) + (vp[2 + dvp] * dp[10]) + (vp[1 + dvp] * dp[11]) + (vp[0 + dvp] * dp[12]) + (vp[15 + dvp] * dp[13]) + (vp[14 + dvp] * dp[14]) + (vp[13 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples13()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[13 + dvp] * dp[0]) + (vp[12 + dvp] * dp[1]) + (vp[11 + dvp] * dp[2]) + (vp[10 + dvp] * dp[3]) + (vp[9 + dvp] * dp[4]) + (vp[8 + dvp] * dp[5]) + (vp[7 + dvp] * dp[6]) + (vp[6 + dvp] * dp[7]) + (vp[5 + dvp] * dp[8]) + (vp[4 + dvp] * dp[9]) + (vp[3 + dvp] * dp[10]) + (vp[2 + dvp] * dp[11]) + (vp[1 + dvp] * dp[12]) + (vp[0 + dvp] * dp[13]) + (vp[15 + dvp] * dp[14]) + (vp[14 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples14()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            float pcm_sample;

            pcm_sample = (vp[14 + dvp] * dp[0]) + (vp[13 + dvp] * dp[1]) + (vp[12 + dvp] * dp[2]) + (vp[11 + dvp] * dp[3]) + (vp[10 + dvp] * dp[4]) + (vp[9 + dvp] * dp[5]) + (vp[8 + dvp] * dp[6]) + (vp[7 + dvp] * dp[7]) + (vp[6 + dvp] * dp[8]) + (vp[5 + dvp] * dp[9]) + (vp[4 + dvp] * dp[10]) + (vp[3 + dvp] * dp[11]) + (vp[2 + dvp] * dp[12]) + (vp[1 + dvp] * dp[13]) + (vp[0 + dvp] * dp[14]) + (vp[15 + dvp] * dp[15]);

            tmpOut[i] = pcm_sample;

            dvp += 16;
        }

        // for
    }

    void compute_pcm_samples15()
    {
        var vp = actual_v;

        // int inc = v_inc;
        var tmpOut = m_PCMSamples;
        var dvp = 0;

        // fat chance of having this loop unroll
        for (var i = 0; i < 32; i++)
        {
            float pcm_sample;
            var dp = MP3AudioConstants.SynthesisFilterValues[i];
            pcm_sample = (vp[15 + dvp] * dp[0]) + (vp[14 + dvp] * dp[1]) + (vp[13 + dvp] * dp[2]) + (vp[12 + dvp] * dp[3]) + (vp[11 + dvp] * dp[4]) + (vp[10 + dvp] * dp[5]) + (vp[9 + dvp] * dp[6]) + (vp[8 + dvp] * dp[7]) + (vp[7 + dvp] * dp[8]) + (vp[6 + dvp] * dp[9]) + (vp[5 + dvp] * dp[10]) + (vp[4 + dvp] * dp[11]) + (vp[3 + dvp] * dp[12]) + (vp[2 + dvp] * dp[13]) + (vp[1 + dvp] * dp[14]) + (vp[0 + dvp] * dp[15]);

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

    /// <summary>Calculate 32 PCM samples and output them.</summary>
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

    static readonly float cos1a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 64.0)));
    static readonly float cos3a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 64.0)));
    static readonly float cos5a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 64.0)));
    static readonly float cos7a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 64.0)));
    static readonly float cos9a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 9.0 / 64.0)));
    static readonly float cos11a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 11.0 / 64.0)));
    static readonly float cos13a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 13.0 / 64.0)));
    static readonly float cos15a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 15.0 / 64.0)));
    static readonly float cos17a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 17.0 / 64.0)));
    static readonly float cos19a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 19.0 / 64.0)));
    static readonly float cos21a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 21.0 / 64.0)));
    static readonly float cos23a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 23.0 / 64.0)));
    static readonly float cos25a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 25.0 / 64.0)));
    static readonly float cos27a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 27.0 / 64.0)));
    static readonly float cos29a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 29.0 / 64.0)));
    static readonly float cos31a64 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 31.0 / 64.0)));
    static readonly float cos1a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 32.0)));
    static readonly float cos3a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 32.0)));
    static readonly float cos5a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 32.0)));
    static readonly float cos7a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 32.0)));
    static readonly float cos9a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 9.0 / 32.0)));
    static readonly float cos11a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 11.0 / 32.0)));
    static readonly float cos13a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 13.0 / 32.0)));
    static readonly float cos15a32 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 15.0 / 32.0)));
    static readonly float cos1a16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 16.0)));
    static readonly float cos3a16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 16.0)));
    static readonly float cos5a16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 5.0 / 16.0)));
    static readonly float cos7a16 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 7.0 / 16.0)));
    static readonly float cos1a8 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 8.0)));
    static readonly float cos3a8 = (float)(1.0 / (2.0 * Math.Cos(Math.PI * 3.0 / 8.0)));
    static readonly float cos1a4 = (float)(1.0 / (2.0 * Math.Cos(Math.PI / 4.0)));
}
