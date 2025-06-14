/*
    This is part of the MP3Decoder implementation.
    Based upon mpg123, jlayer 1.0.1 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/

using System;

namespace Cave.Media.Audio.MP3;

/// <summary>Mpeg Audio Layer III frame decoder.</summary>
public sealed class MP3AudioLayerIIIDecoder
{
    #region Private Classes

    class ChanInfo
    {
        #region Public Fields

        public GRInfo[] GR = new GRInfo[]
        {
            new GRInfo(),
            new GRInfo(),
        };

        public int[] SCFSI = new int[4];

        #endregion Public Fields
    }

    class GRInfo
    {
        #region Public Fields

        public int BigValues = 0;
        public int BlockType = 0;
        public int Count1TableSelect = 0;
        public int GlobalGain = 0;
        public int MixedBlockFlag = 0;
        public int Part23Length = 0;
        public int Preflag = 0;
        public int Region0Count = 0;
        public int Region1Count = 0;
        public int ScaleFactorCompress = 0;
        public int ScaleFactorScale = 0;
        public int[] SubBlockGain;
        public int[] TableSelect;
        public int WindowSwitchingFlag = 0;

        #endregion Public Fields

        #region Public Constructors

        public GRInfo()
        {
            TableSelect = new int[3];
            SubBlockGain = new int[3];
        }

        #endregion Public Constructors
    }

    class SBI
    {
        #region Public Fields

        public int[] L;
        public int[] S;

        #endregion Public Fields

        #region Public Constructors

        public SBI()
        {
            L = new int[23];
            S = new int[14];
        }

        public SBI(int[] thel, int[] thes)
        {
            L = thel;
            S = thes;
        }

        #endregion Public Constructors
    }

    class ScaleFactors
    {
        #region Public Fields

        public int[] L; /* [cb] */
        public int[][] S;

        #endregion Public Fields

        /* [window][cb] */

        #region Public Constructors

        public ScaleFactors()
        {
            L = new int[23];
            S = new int[3][];
            for (var i = 0; i < 3; i++)
            {
                S[i] = new int[13];
            }
        }

        #endregion Public Constructors
    }

    class SideInfo
    {
        #region Public Fields

        public ChanInfo[] CH = new ChanInfo[]
        {
            new ChanInfo(),
            new ChanInfo(),
        };

        public int MainDataBegin;
        public int PrivateBits;

        #endregion Public Fields
    }

    #endregion Private Classes

    #region Private Fields

    const int SBLIMIT = 32;
    const int SSLIMIT = 18;
    static readonly float[] ca = [-0.5144957554270f, -0.4717319685650f, -0.3133774542040f, -0.1819131996110f, -0.0945741925262f, -0.0409655828852f, -0.0141985685725f, -0.00369997467375f];
    static readonly float[] cs = [0.857492925712f, 0.881741997318f, 0.949628649103f, 0.983314592492f, 0.995517816065f, 0.999160558175f, 0.999899195243f, 0.999993155067f];
    static readonly float[][] io = { new float[] { 1.0000000000e+00f, 8.4089641526e-01f, 7.0710678119e-01f, 5.9460355751e-01f, 5.0000000001e-01f, 4.2044820763e-01f, 3.5355339060e-01f, 2.9730177876e-01f, 2.5000000001e-01f, 2.1022410382e-01f, 1.7677669530e-01f, 1.4865088938e-01f, 1.2500000000e-01f, 1.0511205191e-01f, 8.8388347652e-02f, 7.4325444691e-02f, 6.2500000003e-02f, 5.2556025956e-02f, 4.4194173826e-02f, 3.7162722346e-02f, 3.1250000002e-02f, 2.6278012978e-02f, 2.2097086913e-02f, 1.8581361173e-02f, 1.5625000001e-02f, 1.3139006489e-02f, 1.1048543457e-02f, 9.2906805866e-03f, 7.8125000006e-03f, 6.5695032447e-03f, 5.5242717285e-03f, 4.6453402934e-03f }, new float[] { 1.0000000000e+00f, 7.0710678119e-01f, 5.0000000000e-01f, 3.5355339060e-01f, 2.5000000000e-01f, 1.7677669530e-01f, 1.2500000000e-01f, 8.8388347650e-02f, 6.2500000001e-02f, 4.4194173825e-02f, 3.1250000001e-02f, 2.2097086913e-02f, 1.5625000000e-02f, 1.1048543456e-02f, 7.8125000002e-03f, 5.5242717282e-03f, 3.9062500001e-03f, 2.7621358641e-03f, 1.9531250001e-03f, 1.3810679321e-03f, 9.7656250004e-04f, 6.9053396603e-04f, 4.8828125002e-04f, 3.4526698302e-04f, 2.4414062501e-04f, 1.7263349151e-04f, 1.2207031251e-04f, 8.6316745755e-05f, 6.1035156254e-05f, 4.3158372878e-05f, 3.0517578127e-05f, 2.1579186439e-05f } };
    static readonly int[][][] nrOfSfbBlock = { new int[][] { new int[] { 6, 5, 5, 5 }, new int[] { 9, 9, 9, 9 }, new int[] { 6, 9, 9, 9 } }, new int[][] { new int[] { 6, 5, 7, 3 }, new int[] { 9, 9, 12, 6 }, new int[] { 6, 9, 12, 6 } }, new int[][] { new int[] { 11, 10, 0, 0 }, new int[] { 18, 18, 0, 0 }, new int[] { 15, 18, 0, 0 } }, new int[][] { new int[] { 7, 7, 7, 0 }, new int[] { 12, 12, 12, 0 }, new int[] { 6, 15, 12, 0 } }, new int[][] { new int[] { 6, 6, 6, 3 }, new int[] { 12, 9, 9, 6 }, new int[] { 6, 12, 9, 6 } }, new int[][] { new int[] { 8, 8, 5, 0 }, new int[] { 15, 12, 9, 0 }, new int[] { 6, 18, 9, 0 } } };
    static readonly int[] pretab = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 3, 3, 3, 2, 0 };
    static readonly int[][] slen = { new int[] { 0, 0, 0, 0, 3, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4 }, new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 1, 2, 3, 1, 2, 3, 2, 3 } };
    static readonly float[] t43 = CreateT43();
    static readonly float[] tan12 = new float[] { 0.0f, 0.26794919f, 0.57735027f, 1.0f, 1.73205081f, 3.73205081f, 9.9999999e10f, -3.73205081f, -1.73205081f, -1.0f, -0.57735027f, -0.26794919f, 0.0f, 0.26794919f, 0.57735027f, 1.0f };
    static readonly float[] twoToNegativeHalfPow = new float[] { 1.0000000000e+00f, 7.0710678119e-01f, 5.0000000000e-01f, 3.5355339059e-01f, 2.5000000000e-01f, 1.7677669530e-01f, 1.2500000000e-01f, 8.8388347648e-02f, 6.2500000000e-02f, 4.4194173824e-02f, 3.1250000000e-02f, 2.2097086912e-02f, 1.5625000000e-02f, 1.1048543456e-02f, 7.8125000000e-03f, 5.5242717280e-03f, 3.9062500000e-03f, 2.7621358640e-03f, 1.9531250000e-03f, 1.3810679320e-03f, 9.7656250000e-04f, 6.9053396600e-04f, 4.8828125000e-04f, 3.4526698300e-04f, 2.4414062500e-04f, 1.7263349150e-04f, 1.2207031250e-04f, 8.6316745750e-05f, 6.1035156250e-05f, 4.3158372875e-05f, 3.0517578125e-05f, 2.1579186438e-05f, 1.5258789062e-05f, 1.0789593219e-05f, 7.6293945312e-06f, 5.3947966094e-06f, 3.8146972656e-06f, 2.6973983047e-06f, 1.9073486328e-06f, 1.3486991523e-06f, 9.5367431641e-07f, 6.7434957617e-07f, 4.7683715820e-07f, 3.3717478809e-07f, 2.3841857910e-07f, 1.6858739404e-07f, 1.1920928955e-07f, 8.4293697022e-08f, 5.9604644775e-08f, 4.2146848511e-08f, 2.9802322388e-08f, 2.1073424255e-08f, 1.4901161194e-08f, 1.0536712128e-08f, 7.4505805969e-09f, 5.2683560639e-09f, 3.7252902985e-09f, 2.6341780319e-09f, 1.8626451492e-09f, 1.3170890160e-09f, 9.3132257462e-10f, 6.5854450798e-10f, 4.6566128731e-10f, 3.2927225399e-10f };

    static readonly float[][] win =
    {
        new float[] { -1.6141214951e-02f, -5.3603178919e-02f, -1.0070713296e-01f, -1.6280817573e-01f, -4.9999999679e-01f, -3.8388735032e-01f, -6.2061144372e-01f, -1.1659756083e+00f, -3.8720752656e+00f, -4.2256286556e+00f, -1.5195289984e+00f, -9.7416483388e-01f, -7.3744074053e-01f, -1.2071067773e+00f, -5.1636156596e-01f, -4.5426052317e-01f, -4.0715656898e-01f, -3.6969460527e-01f, -3.3876269197e-01f, -3.1242222492e-01f, -2.8939587111e-01f, -2.6880081906e-01f, -5.0000000266e-01f, -2.3251417468e-01f, -2.1596714708e-01f, -2.0004979098e-01f, -1.8449493497e-01f, -1.6905846094e-01f, -1.5350360518e-01f, -1.3758624925e-01f, -1.2103922149e-01f, -2.0710679058e-01f, -8.4752577594e-02f, -6.4157525656e-02f, -4.1131172614e-02f, -1.4790705759e-02f },
        new float[] { -1.6141214951e-02f, -5.3603178919e-02f, -1.0070713296e-01f, -1.6280817573e-01f, -4.9999999679e-01f, -3.8388735032e-01f, -6.2061144372e-01f, -1.1659756083e+00f, -3.8720752656e+00f, -4.2256286556e+00f, -1.5195289984e+00f, -9.7416483388e-01f, -7.3744074053e-01f, -1.2071067773e+00f, -5.1636156596e-01f, -4.5426052317e-01f, -4.0715656898e-01f, -3.6969460527e-01f, -3.3908542600e-01f, -3.1511810350e-01f, -2.9642226150e-01f, -2.8184548650e-01f, -5.4119610000e-01f, -2.6213228100e-01f, -2.5387916537e-01f, -2.3296291359e-01f, -1.9852728987e-01f, -1.5233534808e-01f, -9.6496400054e-02f, -3.3423828516e-02f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f },
        new float[] { -4.8300800645e-02f, -1.5715656932e-01f, -2.8325045177e-01f, -4.2953747763e-01f, -1.2071067795e+00f, -8.2426483178e-01f, -1.1451749106e+00f, -1.7695290101e+00f, -4.5470225061e+00f, -3.4890531002e+00f, -7.3296292804e-01f, -1.5076514758e-01f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f },
        new float[] { 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, -1.5076513660e-01f, -7.3296291107e-01f, -3.4890530566e+00f, -4.5470224727e+00f, -1.7695290031e+00f, -1.1451749092e+00f, -8.3137738100e-01f, -1.3065629650e+00f, -5.4142014250e-01f, -4.6528974900e-01f, -4.1066990750e-01f, -3.7004680800e-01f, -3.3876269197e-01f, -3.1242222492e-01f, -2.8939587111e-01f, -2.6880081906e-01f, -5.0000000266e-01f, -2.3251417468e-01f, -2.1596714708e-01f, -2.0004979098e-01f, -1.8449493497e-01f, -1.6905846094e-01f, -1.5350360518e-01f, -1.3758624925e-01f, -1.2103922149e-01f, -2.0710679058e-01f, -8.4752577594e-02f, -6.4157525656e-02f, -4.1131172614e-02f, -1.4790705759e-02f },
    };

    static int[][] reorderTable = [];
    MP3BitReserve bitReserve;
    int channels;
    int counter = 0;
    MP3AudioSynthesisFilter? filter1;
    MP3AudioSynthesisFilter? filter2;
    int firstChannel;
    int frameStart;
    int[] is1d; 
    int[] isPos = new int[576];
    float[] isRatio = new float[576]; 
    float[][] k; 
    int lastChannel; 
    float[][][] lr; 
    int maxGr;
    int[] newSlen = new int[4];
    int[] nonzero;
    float[] out1d;
    MP3AudioStereoBuffer outputBuffer;
    MP3AudioOutputMode outputMode;
    int part2Start;
    float[][] prevblck;
    float[] rawout = new float[36];
    float[][][] ro;

    /// <summary>The samples for channel 1 - preparation buffer for the synthesis filter.</summary>
    float[] sampleBuffer1 = new float[32];

    /// <summary>The samples for channel 2 - preparation buffer for the synthesis filter.</summary>
    float[] sampleBuffer2 = new float[32];

    int[] scaleFactorBuffer;
    ScaleFactors[] scaleFactors;
    SBI[] sfBandIndex;
    int sfreq;
    SideInfo sideInfo;
    float[] tsOutCopy = new float[18];
    int[] v = new int[] { 0 };
    int[] w = new int[] { 0 };
    // hufman
    int[] x = new int[] { 0 };
    int[] y = new int[] { 0 };

    #endregion Private Fields

    #region Private Methods

    static float[] CreateT43()
    {
        var t43 = new float[8192];
        var d43 = 4.0 / 3.0;

        for (var i = 0; i < 8192; i++)
        {
            t43[i] = (float)Math.Pow(i, d43);
        }
        return t43;
    }

    void Antialias(int ch, int gr)
    {
        int sb18, ss, sb18lim;
        var gr_info = sideInfo.CH[ch].GR[gr];

        // 31 alias-reduction operations between each pair of sub-bands with 8 butterflies between each pair
        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2) && !(gr_info.MixedBlockFlag != 0))
        {
            return;
        }

        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.MixedBlockFlag != 0) && (gr_info.BlockType == 2))
        {
            sb18lim = 18;
        }
        else
        {
            sb18lim = 558;
        }

        for (sb18 = 0; sb18 < sb18lim; sb18 += 18)
        {
            for (ss = 0; ss < 8; ss++)
            {
                var src_idx1 = sb18 + 17 - ss;
                var src_idx2 = sb18 + 18 + ss;
                var bu = out1d[src_idx1];
                var bd = out1d[src_idx2];
                out1d[src_idx1] = (bu * cs[ss]) - (bd * ca[ss]);
                out1d[src_idx2] = (bd * cs[ss]) + (bu * ca[ss]);
            }
        }
    }

    void DequantizeSample(float[][] xr, int ch, int gr)
    {
        var gr_info = sideInfo.CH[ch].GR[gr];
        var cb = 0;
        int next_cb_boundary;
        var cb_begin = 0;
        var cb_width = 0;
        int index = 0, t_index, j;
        float g_gain;
        var xr_1d = xr;

        // choose correct scalefactor band per block type, initalize boundary
        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
        {
            if (gr_info.MixedBlockFlag != 0)
            {
                next_cb_boundary = sfBandIndex[sfreq].L[1];
            }

            // LONG blocks: 0,1,3
            else
            {
                cb_width = sfBandIndex[sfreq].S[1];
                next_cb_boundary = (cb_width << 2) - cb_width;
                cb_begin = 0;
            }
        }
        else
        {
            next_cb_boundary = sfBandIndex[sfreq].L[1]; // LONG blocks: 0,1,3
        }

        // Compute overall (global) scaling.
        g_gain = (float)Math.Pow(2.0, 0.25 * (gr_info.GlobalGain - 210.0));

        for (j = 0; j < nonzero[ch]; j++)
        {
            // Modif E.B 02/22/99
            var reste = j % SSLIMIT;
            var quotien = (j - reste) / SSLIMIT;
            if (is1d[j] == 0)
            {
                xr_1d[quotien][reste] = 0.0f;
            }
            else
            {
                var abv = is1d[j];
                if (is1d[j] > 0)
                {
                    xr_1d[quotien][reste] = g_gain * t43[abv];
                }
                else
                {
                    xr_1d[quotien][reste] = -g_gain * t43[-abv];
                }
            }
        }

        // apply formula per block type
        for (j = 0; j < nonzero[ch]; j++)
        {
            // Modif E.B 02/22/99
            var reste = j % SSLIMIT;
            var quotien = (j - reste) / SSLIMIT;

            if (index == next_cb_boundary)
            {
                /* Adjust critical band boundary */
                if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
                {
                    if (gr_info.MixedBlockFlag != 0)
                    {
                        if (index == sfBandIndex[sfreq].L[8])
                        {
                            next_cb_boundary = sfBandIndex[sfreq].S[4];
                            next_cb_boundary = (next_cb_boundary << 2) - next_cb_boundary;
                            cb = 3;
                            cb_width = sfBandIndex[sfreq].S[4] - sfBandIndex[sfreq].S[3];

                            cb_begin = sfBandIndex[sfreq].S[3];
                            cb_begin = (cb_begin << 2) - cb_begin;
                        }
                        else if (index < sfBandIndex[sfreq].L[8])
                        {
                            next_cb_boundary = sfBandIndex[sfreq].L[(++cb) + 1];
                        }
                        else
                        {
                            next_cb_boundary = sfBandIndex[sfreq].S[(++cb) + 1];
                            next_cb_boundary = (next_cb_boundary << 2) - next_cb_boundary;

                            cb_begin = sfBandIndex[sfreq].S[cb];
                            cb_width = sfBandIndex[sfreq].S[cb + 1] - cb_begin;
                            cb_begin = (cb_begin << 2) - cb_begin;
                        }
                    }
                    else
                    {
                        next_cb_boundary = sfBandIndex[sfreq].S[(++cb) + 1];
                        next_cb_boundary = (next_cb_boundary << 2) - next_cb_boundary;

                        cb_begin = sfBandIndex[sfreq].S[cb];
                        cb_width = sfBandIndex[sfreq].S[cb + 1] - cb_begin;
                        cb_begin = (cb_begin << 2) - cb_begin;
                    }
                }
                else
                {
                    // long blocks
                    next_cb_boundary = sfBandIndex[sfreq].L[(++cb) + 1];
                }
            }

            // Do long/short dependent scaling operations
            if ((gr_info.WindowSwitchingFlag != 0) && (((gr_info.BlockType == 2) && (gr_info.MixedBlockFlag == 0)) || ((gr_info.BlockType == 2) && (gr_info.MixedBlockFlag != 0) && (j >= 36))))
            {
                t_index = (index - cb_begin) / cb_width;
                /*            xr[sb][ss] *= pow(2.0, ((-2.0 * gr_info.subblock_gain[t_index])
                -(0.5 * (1.0 + gr_info.scalefac_scale)
                * scalefac[ch].s[t_index][cb]))); */
                var idx = scaleFactors[ch].S[t_index][cb] << gr_info.ScaleFactorScale;
                idx += gr_info.SubBlockGain[t_index] << 2;

                xr_1d[quotien][reste] *= twoToNegativeHalfPow[idx];
            }
            else
            {
                // LONG block types 0,1,3 & 1st 2 subbands of switched blocks
                /*                xr[sb][ss] *= pow(2.0, -0.5 * (1.0+gr_info.scalefac_scale)
                * (scalefac[ch].l[cb]
                + gr_info.preflag * pretab[cb])); */
                var idx = scaleFactors[ch].L[cb];

                if (gr_info.Preflag != 0)
                {
                    idx += pretab[cb];
                }

                idx <<= gr_info.ScaleFactorScale;
                xr_1d[quotien][reste] *= twoToNegativeHalfPow[idx];
            }
            index++;
        }

        for (j = nonzero[ch]; j < 576; j++)
        {
            // Modif E.B 02/22/99
            var reste = j % SSLIMIT;
            var quotien = (j - reste) / SSLIMIT;
            if (reste < 0)
            {
                reste = 0;
            }

            if (quotien < 0)
            {
                quotien = 0;
            }

            xr_1d[quotien][reste] = 0.0f;
        }

        return;
    }

    void DoDownmix()
    {
        for (var sb = 0; sb < SSLIMIT; sb++)
        {
            for (var ss = 0; ss < SSLIMIT; ss += 3)
            {
                lr[0][sb][ss] = (lr[0][sb][ss] + lr[1][sb][ss]) * 0.5f;
                lr[0][sb][ss + 1] = (lr[0][sb][ss + 1] + lr[1][sb][ss + 1]) * 0.5f;
                lr[0][sb][ss + 2] = (lr[0][sb][ss + 2] + lr[1][sb][ss + 2]) * 0.5f;
            }
        }
    }

    void GetLsfScaleData(MP3AudioFrameHeader header, int ch, int gr)
    {
        int scalefac_comp, int_scalefac_comp;
        int mode_ext = header.ModeExtension;

        int m;
        int blocktypenumber;
        var blocknumber = 0;

        var gr_info = sideInfo.CH[ch].GR[gr];

        scalefac_comp = gr_info.ScaleFactorCompress;

        if (gr_info.BlockType == 2)
        {
            if (gr_info.MixedBlockFlag == 0)
            {
                blocktypenumber = 1;
            }
            else if (gr_info.MixedBlockFlag == 1)
            {
                blocktypenumber = 2;
            }
            else
            {
                blocktypenumber = 0;
            }
        }
        else
        {
            blocktypenumber = 0;
        }

        if (!(((mode_ext == 1) || (mode_ext == 3)) && (ch == 1)))
        {
            if (scalefac_comp < 400)
            {
                newSlen[0] = (scalefac_comp >> 4) / 5;
                newSlen[1] = (scalefac_comp >> 4) % 5;
                newSlen[2] = (scalefac_comp & 0xF) >> 2;
                newSlen[3] = scalefac_comp & 3;
                sideInfo.CH[ch].GR[gr].Preflag = 0;
                blocknumber = 0;
            }
            else if (scalefac_comp < 500)
            {
                newSlen[0] = ((scalefac_comp - 400) >> 2) / 5;
                newSlen[1] = ((scalefac_comp - 400) >> 2) % 5;
                newSlen[2] = (scalefac_comp - 400) & 3;
                newSlen[3] = 0;
                sideInfo.CH[ch].GR[gr].Preflag = 0;
                blocknumber = 1;
            }
            else if (scalefac_comp < 512)
            {
                newSlen[0] = (scalefac_comp - 500) / 3;
                newSlen[1] = (scalefac_comp - 500) % 3;
                newSlen[2] = 0;
                newSlen[3] = 0;
                sideInfo.CH[ch].GR[gr].Preflag = 1;
                blocknumber = 2;
            }
        }

        if (((mode_ext == 1) || (mode_ext == 3)) && (ch == 1))
        {
            int_scalefac_comp = scalefac_comp >> 1;

            if (int_scalefac_comp < 180)
            {
                newSlen[0] = int_scalefac_comp / 36;
                newSlen[1] = (int_scalefac_comp % 36) / 6;
                newSlen[2] = (int_scalefac_comp % 36) % 6;
                newSlen[3] = 0;
                sideInfo.CH[ch].GR[gr].Preflag = 0;
                blocknumber = 3;
            }
            else if (int_scalefac_comp < 244)
            {
                newSlen[0] = ((int_scalefac_comp - 180) & 0x3F) >> 4;
                newSlen[1] = ((int_scalefac_comp - 180) & 0xF) >> 2;
                newSlen[2] = (int_scalefac_comp - 180) & 3;
                newSlen[3] = 0;
                sideInfo.CH[ch].GR[gr].Preflag = 0;
                blocknumber = 4;
            }
            else if (int_scalefac_comp < 255)
            {
                newSlen[0] = (int_scalefac_comp - 244) / 3;
                newSlen[1] = (int_scalefac_comp - 244) % 3;
                newSlen[2] = 0;
                newSlen[3] = 0;
                sideInfo.CH[ch].GR[gr].Preflag = 0;
                blocknumber = 5;
            }
        }

        Array.Clear(scaleFactorBuffer, 0, scaleFactorBuffer.Length);

        // why 45, not 54? -> bug for (int x = 0; x < 45; x++) m_ScaleFactorBuffer[x] = 0;
        m = 0;
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < nrOfSfbBlock[blocknumber][blocktypenumber][i]; j++)
            {
                scaleFactorBuffer[m] = (newSlen[i] == 0) ? 0 : bitReserve.ReadBits(newSlen[i]);
                m++;
            }

            // for (unint32 j ...
        }

        // for (uint32 i ...
    }

    void GetLsfScaleFactors(MP3AudioFrameHeader header, int ch, int gr)
    {
        var m = 0;
        int sfb, window;
        var gr_info = sideInfo.CH[ch].GR[gr];

        GetLsfScaleData(header, ch, gr);

        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
        {
            if (gr_info.MixedBlockFlag != 0)
            {
                // MIXED
                for (sfb = 0; sfb < 8; sfb++)
                {
                    scaleFactors[ch].L[sfb] = scaleFactorBuffer[m];
                    m++;
                }
                for (sfb = 3; sfb < 12; sfb++)
                {
                    for (window = 0; window < 3; window++)
                    {
                        scaleFactors[ch].S[window][sfb] = scaleFactorBuffer[m];
                        m++;
                    }
                }
                for (window = 0; window < 3; window++)
                {
                    scaleFactors[ch].S[window][12] = 0;
                }
            }
            else
            {
                // SHORT
                for (sfb = 0; sfb < 12; sfb++)
                {
                    for (window = 0; window < 3; window++)
                    {
                        scaleFactors[ch].S[window][sfb] = scaleFactorBuffer[m];
                        m++;
                    }
                }

                for (window = 0; window < 3; window++)
                {
                    scaleFactors[ch].S[window][12] = 0;
                }
            }
        }
        else
        {
            // LONG types 0,1,3
            for (sfb = 0; sfb < 21; sfb++)
            {
                scaleFactors[ch].L[sfb] = scaleFactorBuffer[m];
                m++;
            }
            scaleFactors[ch].L[21] = 0; // Jeff
            scaleFactors[ch].L[22] = 0;
        }
    }

    void GetScaleFactors(int ch, int gr)
    {
        int sfb, window;
        var gr_info = sideInfo.CH[ch].GR[gr];
        var scale_comp = gr_info.ScaleFactorCompress;
        var length0 = slen[0][scale_comp];
        var length1 = slen[1][scale_comp];

        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
        {
            if (gr_info.MixedBlockFlag != 0)
            {
                // MIXED
                for (sfb = 0; sfb < 8; sfb++)
                {
                    scaleFactors[ch].L[sfb] = bitReserve.ReadBits(slen[0][gr_info.ScaleFactorCompress]);
                }

                for (sfb = 3; sfb < 6; sfb++)
                {
                    for (window = 0; window < 3; window++)
                    {
                        scaleFactors[ch].S[window][sfb] = bitReserve.ReadBits(slen[0][gr_info.ScaleFactorCompress]);
                    }
                }

                for (sfb = 6; sfb < 12; sfb++)
                {
                    for (window = 0; window < 3; window++)
                    {
                        scaleFactors[ch].S[window][sfb] = bitReserve.ReadBits(slen[1][gr_info.ScaleFactorCompress]);
                    }
                }

                for (sfb = 12, window = 0; window < 3; window++)
                {
                    scaleFactors[ch].S[window][sfb] = 0;
                }
            }
            else
            {
                // SHORT
                scaleFactors[ch].S[0][0] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][0] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][0] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][1] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][1] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][1] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][2] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][2] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][2] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][3] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][3] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][3] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][4] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][4] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][4] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][5] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[1][5] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[2][5] = bitReserve.ReadBits(length0);
                scaleFactors[ch].S[0][6] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][6] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][6] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][7] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][7] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][7] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][8] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][8] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][8] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][9] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][9] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][9] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][10] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][10] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][10] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][11] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[1][11] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[2][11] = bitReserve.ReadBits(length1);
                scaleFactors[ch].S[0][12] = 0;
                scaleFactors[ch].S[1][12] = 0;
                scaleFactors[ch].S[2][12] = 0;
            }

            // SHORT
        }
        else
        {
            // LONG types 0,1,3
            if ((sideInfo.CH[ch].SCFSI[0] == 0) || (gr == 0))
            {
                scaleFactors[ch].L[0] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[1] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[2] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[3] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[4] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[5] = bitReserve.ReadBits(length0);
            }
            if ((sideInfo.CH[ch].SCFSI[1] == 0) || (gr == 0))
            {
                scaleFactors[ch].L[6] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[7] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[8] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[9] = bitReserve.ReadBits(length0);
                scaleFactors[ch].L[10] = bitReserve.ReadBits(length0);
            }
            if ((sideInfo.CH[ch].SCFSI[2] == 0) || (gr == 0))
            {
                scaleFactors[ch].L[11] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[12] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[13] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[14] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[15] = bitReserve.ReadBits(length1);
            }
            if ((sideInfo.CH[ch].SCFSI[3] == 0) || (gr == 0))
            {
                scaleFactors[ch].L[16] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[17] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[18] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[19] = bitReserve.ReadBits(length1);
                scaleFactors[ch].L[20] = bitReserve.ReadBits(length1);
            }

            scaleFactors[ch].L[21] = 0;
            scaleFactors[ch].L[22] = 0;
        }
    }

    /// <summary>
    /// Reads the side info from the stream, assuming the entire. frame has been read already. Mono : 136 bits (= 17 bytes) Stereo : 256 bits (= 32 bytes).
    /// </summary>
    bool GetSideInfoVer1(MP3AudioFrame frame)
    {
        int ch, gr;
        sideInfo.MainDataBegin = frame.Bits.ReadBits(9);
        if (channels == 1)
        {
            sideInfo.PrivateBits = frame.Bits.ReadBits(5);
        }
        else
        {
            sideInfo.PrivateBits = frame.Bits.ReadBits(3);
        }

        for (ch = 0; ch < channels; ch++)
        {
            sideInfo.CH[ch].SCFSI[0] = frame.Bits.ReadBits(1);
            sideInfo.CH[ch].SCFSI[1] = frame.Bits.ReadBits(1);
            sideInfo.CH[ch].SCFSI[2] = frame.Bits.ReadBits(1);
            sideInfo.CH[ch].SCFSI[3] = frame.Bits.ReadBits(1);
        }

        for (gr = 0; gr < 2; gr++)
        {
            for (ch = 0; ch < channels; ch++)
            {
                sideInfo.CH[ch].GR[gr].Part23Length = frame.Bits.ReadBits(12);
                sideInfo.CH[ch].GR[gr].BigValues = frame.Bits.ReadBits(9);
                sideInfo.CH[ch].GR[gr].GlobalGain = frame.Bits.ReadBits(8);
                sideInfo.CH[ch].GR[gr].ScaleFactorCompress = frame.Bits.ReadBits(4);
                sideInfo.CH[ch].GR[gr].WindowSwitchingFlag = frame.Bits.ReadBits(1);
                if (sideInfo.CH[ch].GR[gr].WindowSwitchingFlag != 0)
                {
                    sideInfo.CH[ch].GR[gr].BlockType = frame.Bits.ReadBits(2);
                    sideInfo.CH[ch].GR[gr].MixedBlockFlag = frame.Bits.ReadBits(1);

                    sideInfo.CH[ch].GR[gr].TableSelect[0] = frame.Bits.ReadBits(5);
                    sideInfo.CH[ch].GR[gr].TableSelect[1] = frame.Bits.ReadBits(5);

                    sideInfo.CH[ch].GR[gr].SubBlockGain[0] = frame.Bits.ReadBits(3);
                    sideInfo.CH[ch].GR[gr].SubBlockGain[1] = frame.Bits.ReadBits(3);
                    sideInfo.CH[ch].GR[gr].SubBlockGain[2] = frame.Bits.ReadBits(3);

                    // Set region_count parameters since they are implicit in this case.
                    if (sideInfo.CH[ch].GR[gr].BlockType == 0)
                    {
                        // Side info bad: block_type == 0 in split block
                        return false;
                    }
                    else if (sideInfo.CH[ch].GR[gr].BlockType == 2 && sideInfo.CH[ch].GR[gr].MixedBlockFlag == 0)
                    {
                        sideInfo.CH[ch].GR[gr].Region0Count = 8;
                    }
                    else
                    {
                        sideInfo.CH[ch].GR[gr].Region0Count = 7;
                    }
                    sideInfo.CH[ch].GR[gr].Region1Count = 20 - sideInfo.CH[ch].GR[gr].Region0Count;
                }
                else
                {
                    sideInfo.CH[ch].GR[gr].TableSelect[0] = frame.Bits.ReadBits(5);
                    sideInfo.CH[ch].GR[gr].TableSelect[1] = frame.Bits.ReadBits(5);
                    sideInfo.CH[ch].GR[gr].TableSelect[2] = frame.Bits.ReadBits(5);
                    sideInfo.CH[ch].GR[gr].Region0Count = frame.Bits.ReadBits(4);
                    sideInfo.CH[ch].GR[gr].Region1Count = frame.Bits.ReadBits(3);
                    sideInfo.CH[ch].GR[gr].BlockType = 0;
                }
                sideInfo.CH[ch].GR[gr].Preflag = frame.Bits.ReadBits(1);
                sideInfo.CH[ch].GR[gr].ScaleFactorScale = frame.Bits.ReadBits(1);
                sideInfo.CH[ch].GR[gr].Count1TableSelect = frame.Bits.ReadBits(1);
            }
        }
        return true;
    }

    /// <summary>
    /// Reads the side info from the stream, assuming the entire. frame has been read already. Mono : 136 bits (= 17 bytes) Stereo : 256 bits (= 32 bytes).
    /// </summary>
    bool GetSideInfoVer2(MP3AudioFrame frame)
    {
        int ch;

        // MPEG-2 LSF, SZD: MPEG-2.5 LSF
        sideInfo.MainDataBegin = frame.Bits.ReadBits(8);
        if (channels == 1)
        {
            sideInfo.PrivateBits = frame.Bits.ReadBits(1);
        }
        else
        {
            sideInfo.PrivateBits = frame.Bits.ReadBits(2);
        }

        for (ch = 0; ch < channels; ch++)
        {
            sideInfo.CH[ch].GR[0].Part23Length = frame.Bits.ReadBits(12);
            sideInfo.CH[ch].GR[0].BigValues = frame.Bits.ReadBits(9);
            sideInfo.CH[ch].GR[0].GlobalGain = frame.Bits.ReadBits(8);
            sideInfo.CH[ch].GR[0].ScaleFactorCompress = frame.Bits.ReadBits(9);
            sideInfo.CH[ch].GR[0].WindowSwitchingFlag = frame.Bits.ReadBits(1);

            if (sideInfo.CH[ch].GR[0].WindowSwitchingFlag != 0)
            {
                sideInfo.CH[ch].GR[0].BlockType = frame.Bits.ReadBits(2);
                sideInfo.CH[ch].GR[0].MixedBlockFlag = frame.Bits.ReadBits(1);
                sideInfo.CH[ch].GR[0].TableSelect[0] = frame.Bits.ReadBits(5);
                sideInfo.CH[ch].GR[0].TableSelect[1] = frame.Bits.ReadBits(5);

                sideInfo.CH[ch].GR[0].SubBlockGain[0] = frame.Bits.ReadBits(3);
                sideInfo.CH[ch].GR[0].SubBlockGain[1] = frame.Bits.ReadBits(3);
                sideInfo.CH[ch].GR[0].SubBlockGain[2] = frame.Bits.ReadBits(3);

                // Set region_count parameters since they are implicit in this case.
                if (sideInfo.CH[ch].GR[0].BlockType == 0)
                {
                    // Side info bad: block_type == 0 in split block
                    return false;
                }
                else if (sideInfo.CH[ch].GR[0].BlockType == 2 && sideInfo.CH[ch].GR[0].MixedBlockFlag == 0)
                {
                    sideInfo.CH[ch].GR[0].Region0Count = 8;
                }
                else
                {
                    sideInfo.CH[ch].GR[0].Region0Count = 7;
                    sideInfo.CH[ch].GR[0].Region1Count = 20 - sideInfo.CH[ch].GR[0].Region0Count;
                }
            }
            else
            {
                sideInfo.CH[ch].GR[0].TableSelect[0] = frame.Bits.ReadBits(5);
                sideInfo.CH[ch].GR[0].TableSelect[1] = frame.Bits.ReadBits(5);
                sideInfo.CH[ch].GR[0].TableSelect[2] = frame.Bits.ReadBits(5);
                sideInfo.CH[ch].GR[0].Region0Count = frame.Bits.ReadBits(4);
                sideInfo.CH[ch].GR[0].Region1Count = frame.Bits.ReadBits(3);
                sideInfo.CH[ch].GR[0].BlockType = 0;
            }

            sideInfo.CH[ch].GR[0].ScaleFactorScale = frame.Bits.ReadBits(1);
            sideInfo.CH[ch].GR[0].Count1TableSelect = frame.Bits.ReadBits(1);
        }
        return true;
    }

    void HuffmanDecode(int ch, int gr)
    {
        x[0] = 0;
        y[0] = 0;
        v[0] = 0;
        w[0] = 0;

        var part2_3_end = part2Start + sideInfo.CH[ch].GR[gr].Part23Length;
        int num_bits;
        int region1Start;
        int region2Start;
        int index;

        int buf, buf1;

        MP3AudioHuffman h;

        // Find region boundary for short block case
        if ((sideInfo.CH[ch].GR[gr].WindowSwitchingFlag != 0) && (sideInfo.CH[ch].GR[gr].BlockType == 2))
        {
            // Region2.
            // MS: Extrahandling for 8KHZ
            region1Start = (sfreq == 8) ? 72 : 36; // sfb[9/3]*3=36 or in case 8KHZ = 72
            region2Start = 576; // No Region2 for short block case
        }
        else
        {
            // Find region boundary for long block case
            buf = sideInfo.CH[ch].GR[gr].Region0Count + 1;
            buf1 = buf + sideInfo.CH[ch].GR[gr].Region1Count + 1;

            if (buf1 > sfBandIndex[sfreq].L.Length - 1)
            {
                buf1 = sfBandIndex[sfreq].L.Length - 1;
            }

            region1Start = sfBandIndex[sfreq].L[buf];
            region2Start = sfBandIndex[sfreq].L[buf1]; /* MI */
        }

        index = 0;

        // Read bigvalues area
        for (var i = 0; i < (sideInfo.CH[ch].GR[gr].BigValues << 1); i += 2)
        {
            if (i < region1Start)
            {
                h = MP3AudioHuffman.Tables[sideInfo.CH[ch].GR[gr].TableSelect[0]];
            }
            else if (i < region2Start)
            {
                h = MP3AudioHuffman.Tables[sideInfo.CH[ch].GR[gr].TableSelect[1]];
            }
            else
            {
                h = MP3AudioHuffman.Tables[sideInfo.CH[ch].GR[gr].TableSelect[2]];
            }

            h.Decode(x, y, v, w, bitReserve);

            is1d[index++] = x[0];
            is1d[index++] = y[0];

            // m_CheckSumHuffman = m_CheckSumHuffman + x[0] + y[0];
        }

        // Read count1 area
        h = MP3AudioHuffman.Tables[sideInfo.CH[ch].GR[gr].Count1TableSelect + 32];
        num_bits = bitReserve.ReadPosition;

        while ((num_bits < part2_3_end) && (index < 576))
        {
            h.Decode(x, y, v, w, bitReserve);

            is1d[index++] = v[0];
            is1d[index++] = w[0];
            is1d[index++] = x[0];
            is1d[index++] = y[0];

            // m_CheckSumHuffman = m_CheckSumHuffman + v[0] + w[0] + x[0] + y[0];
            num_bits = bitReserve.ReadPosition;
        }

        if (num_bits > part2_3_end)
        {
            bitReserve.Rewind(num_bits - part2_3_end);
            index -= 4;
        }

        num_bits = bitReserve.ReadPosition;

        // Dismiss stuffing bits
        if (num_bits < part2_3_end)
        {
            bitReserve.Skip(part2_3_end - num_bits);
        }

        // Zero out rest
        if (index < 576)
        {
            nonzero[ch] = index;
        }
        else
        {
            nonzero[ch] = 576;
        }

        if (index < 0)
        {
            index = 0;
        }

        Array.Clear(is1d, 0, 576);
    }

    void Hybrid(int ch, int gr)
    {
        int bt;
        int sb18;
        var gr_info = sideInfo.CH[ch].GR[gr];
        float[] tsOut;

        float[][] prvblk;

        for (sb18 = 0; sb18 < 576; sb18 += 18)
        {
            bt = ((gr_info.WindowSwitchingFlag != 0) && (gr_info.MixedBlockFlag != 0) && (sb18 < 36)) ? 0 : gr_info.BlockType;

            tsOut = out1d;

            // Modif E.B 02/22/99
            for (var cc = 0; cc < 18; cc++)
            {
                tsOutCopy[cc] = tsOut[cc + sb18];
            }

            InvMdct(tsOutCopy, rawout, bt);

            for (var cc = 0; cc < 18; cc++)
            {
                tsOut[cc + sb18] = tsOutCopy[cc];
            }

            // Fin Modif

            // overlap addition
            prvblk = prevblck;

            tsOut[0 + sb18] = rawout[0] + prvblk[ch][sb18 + 0];
            prvblk[ch][sb18 + 0] = rawout[18];
            tsOut[1 + sb18] = rawout[1] + prvblk[ch][sb18 + 1];
            prvblk[ch][sb18 + 1] = rawout[19];
            tsOut[2 + sb18] = rawout[2] + prvblk[ch][sb18 + 2];
            prvblk[ch][sb18 + 2] = rawout[20];
            tsOut[3 + sb18] = rawout[3] + prvblk[ch][sb18 + 3];
            prvblk[ch][sb18 + 3] = rawout[21];
            tsOut[4 + sb18] = rawout[4] + prvblk[ch][sb18 + 4];
            prvblk[ch][sb18 + 4] = rawout[22];
            tsOut[5 + sb18] = rawout[5] + prvblk[ch][sb18 + 5];
            prvblk[ch][sb18 + 5] = rawout[23];
            tsOut[6 + sb18] = rawout[6] + prvblk[ch][sb18 + 6];
            prvblk[ch][sb18 + 6] = rawout[24];
            tsOut[7 + sb18] = rawout[7] + prvblk[ch][sb18 + 7];
            prvblk[ch][sb18 + 7] = rawout[25];
            tsOut[8 + sb18] = rawout[8] + prvblk[ch][sb18 + 8];
            prvblk[ch][sb18 + 8] = rawout[26];
            tsOut[9 + sb18] = rawout[9] + prvblk[ch][sb18 + 9];
            prvblk[ch][sb18 + 9] = rawout[27];
            tsOut[10 + sb18] = rawout[10] + prvblk[ch][sb18 + 10];
            prvblk[ch][sb18 + 10] = rawout[28];
            tsOut[11 + sb18] = rawout[11] + prvblk[ch][sb18 + 11];
            prvblk[ch][sb18 + 11] = rawout[29];
            tsOut[12 + sb18] = rawout[12] + prvblk[ch][sb18 + 12];
            prvblk[ch][sb18 + 12] = rawout[30];
            tsOut[13 + sb18] = rawout[13] + prvblk[ch][sb18 + 13];
            prvblk[ch][sb18 + 13] = rawout[31];
            tsOut[14 + sb18] = rawout[14] + prvblk[ch][sb18 + 14];
            prvblk[ch][sb18 + 14] = rawout[32];
            tsOut[15 + sb18] = rawout[15] + prvblk[ch][sb18 + 15];
            prvblk[ch][sb18 + 15] = rawout[33];
            tsOut[16 + sb18] = rawout[16] + prvblk[ch][sb18 + 16];
            prvblk[ch][sb18 + 16] = rawout[34];
            tsOut[17 + sb18] = rawout[17] + prvblk[ch][sb18 + 17];
            prvblk[ch][sb18 + 17] = rawout[35];
        }
    }

    void InvMdct(float[] in_Renamed, float[] out_Renamed, int block_type)
    {
        float[] win_bt;
        int i;

        float tmpf_0, tmpf_1, tmpf_2, tmpf_3, tmpf_4, tmpf_5, tmpf_6, tmpf_7, tmpf_8, tmpf_9;
        float tmpf_10, tmpf_11, tmpf_12, tmpf_13, tmpf_14, tmpf_15, tmpf_16, tmpf_17;

        tmpf_0 = tmpf_1 = tmpf_2 = tmpf_3 = tmpf_4 = tmpf_5 = tmpf_6 = tmpf_7 = tmpf_8 = tmpf_9 = tmpf_10 = tmpf_11 = tmpf_12 = tmpf_13 = tmpf_14 = tmpf_15 = tmpf_16 = tmpf_17 = 0.0f;

        if (block_type == 2)
        {
            /*
            *
            *        Under MicrosoftVM 2922, This causes a GPF, or
            *        At best, an ArrayIndexOutOfBoundsExceptin.
            for(int p=0;p<36;p+=9)
            {
            out[p]   = out[p+1] = out[p+2] = out[p+3] =
            out[p+4] = out[p+5] = out[p+6] = out[p+7] =
            out[p+8] = 0.0f;
            }
            */
            Array.Clear(out_Renamed, 0, out_Renamed.Length);
            /*
            out_Renamed[0] = 0.0f;
            out_Renamed[1] = 0.0f;
            out_Renamed[2] = 0.0f;
            out_Renamed[3] = 0.0f;
            out_Renamed[4] = 0.0f;
            out_Renamed[5] = 0.0f;
            out_Renamed[6] = 0.0f;
            out_Renamed[7] = 0.0f;
            out_Renamed[8] = 0.0f;
            out_Renamed[9] = 0.0f;
            out_Renamed[10] = 0.0f;
            out_Renamed[11] = 0.0f;
            out_Renamed[12] = 0.0f;
            out_Renamed[13] = 0.0f;
            out_Renamed[14] = 0.0f;
            out_Renamed[15] = 0.0f;
            out_Renamed[16] = 0.0f;
            out_Renamed[17] = 0.0f;
            out_Renamed[18] = 0.0f;
            out_Renamed[19] = 0.0f;
            out_Renamed[20] = 0.0f;
            out_Renamed[21] = 0.0f;
            out_Renamed[22] = 0.0f;
            out_Renamed[23] = 0.0f;
            out_Renamed[24] = 0.0f;
            out_Renamed[25] = 0.0f;
            out_Renamed[26] = 0.0f;
            out_Renamed[27] = 0.0f;
            out_Renamed[28] = 0.0f;
            out_Renamed[29] = 0.0f;
            out_Renamed[30] = 0.0f;
            out_Renamed[31] = 0.0f;
            out_Renamed[32] = 0.0f;
            out_Renamed[33] = 0.0f;
            out_Renamed[34] = 0.0f;
            out_Renamed[35] = 0.0f;
            */

            var six_i = 0;

            for (i = 0; i < 3; i++)
            {
                // 12 point IMDCT Begin 12 point IDCT Input aliasing for 12 pt IDCT
                in_Renamed[15 + i] += in_Renamed[12 + i];
                in_Renamed[12 + i] += in_Renamed[9 + i];
                in_Renamed[9 + i] += in_Renamed[6 + i];
                in_Renamed[6 + i] += in_Renamed[3 + i];
                in_Renamed[3 + i] += in_Renamed[0 + i];

                // Input aliasing on odd indices (for 6 point IDCT)
                in_Renamed[15 + i] += in_Renamed[9 + i];
                in_Renamed[9 + i] += in_Renamed[3 + i];

                // 3 point IDCT on even indices
                float pp1, pp2, sum;
                pp2 = in_Renamed[12 + i] * 0.500000000f;
                pp1 = in_Renamed[6 + i] * 0.866025403f;
                sum = in_Renamed[0 + i] + pp2;
                tmpf_1 = in_Renamed[0 + i] - in_Renamed[12 + i];
                tmpf_0 = sum + pp1;
                tmpf_2 = sum - pp1;

                // End 3 point IDCT on even indices 3 point IDCT on odd indices (for 6 point IDCT)
                pp2 = in_Renamed[15 + i] * 0.500000000f;
                pp1 = in_Renamed[9 + i] * 0.866025403f;
                sum = in_Renamed[3 + i] + pp2;
                tmpf_4 = in_Renamed[3 + i] - in_Renamed[15 + i];
                tmpf_5 = sum + pp1;
                tmpf_3 = sum - pp1;

                // End 3 point IDCT on odd indices Twiddle factors on odd indices (for 6 point IDCT)
                tmpf_3 *= 1.931851653f;
                tmpf_4 *= 0.707106781f;
                tmpf_5 *= 0.517638090f;

                // Output butterflies on 2 3 point IDCT's (for 6 point IDCT)
                var save = tmpf_0;
                tmpf_0 += tmpf_5;
                tmpf_5 = save - tmpf_5;
                save = tmpf_1;
                tmpf_1 += tmpf_4;
                tmpf_4 = save - tmpf_4;
                save = tmpf_2;
                tmpf_2 += tmpf_3;
                tmpf_3 = save - tmpf_3;

                // End 6 point IDCT Twiddle factors on indices (for 12 point IDCT)
                tmpf_0 *= 0.504314480f;
                tmpf_1 *= 0.541196100f;
                tmpf_2 *= 0.630236207f;
                tmpf_3 *= 0.821339815f;
                tmpf_4 *= 1.306562965f;
                tmpf_5 *= 3.830648788f;

                // End 12 point IDCT

                // Shift to 12 point modified IDCT, multiply by window type 2
                tmpf_8 = -tmpf_0 * 0.793353340f;
                tmpf_9 = -tmpf_0 * 0.608761429f;
                tmpf_7 = -tmpf_1 * 0.923879532f;
                tmpf_10 = -tmpf_1 * 0.382683432f;
                tmpf_6 = -tmpf_2 * 0.991444861f;
                tmpf_11 = -tmpf_2 * 0.130526192f;

                tmpf_0 = tmpf_3;
                tmpf_1 = tmpf_4 * 0.382683432f;
                tmpf_2 = tmpf_5 * 0.608761429f;

                tmpf_3 = -tmpf_5 * 0.793353340f;
                tmpf_4 = -tmpf_4 * 0.923879532f;
                tmpf_5 = -tmpf_0 * 0.991444861f;

                tmpf_0 *= 0.130526192f;

                out_Renamed[six_i + 6] += tmpf_0;
                out_Renamed[six_i + 7] += tmpf_1;
                out_Renamed[six_i + 8] += tmpf_2;
                out_Renamed[six_i + 9] += tmpf_3;
                out_Renamed[six_i + 10] += tmpf_4;
                out_Renamed[six_i + 11] += tmpf_5;
                out_Renamed[six_i + 12] += tmpf_6;
                out_Renamed[six_i + 13] += tmpf_7;
                out_Renamed[six_i + 14] += tmpf_8;
                out_Renamed[six_i + 15] += tmpf_9;
                out_Renamed[six_i + 16] += tmpf_10;
                out_Renamed[six_i + 17] += tmpf_11;

                six_i += 6;
            }
        }
        else
        {
            // 36 point IDCT input aliasing for 36 point IDCT
            in_Renamed[17] += in_Renamed[16];
            in_Renamed[16] += in_Renamed[15];
            in_Renamed[15] += in_Renamed[14];
            in_Renamed[14] += in_Renamed[13];
            in_Renamed[13] += in_Renamed[12];
            in_Renamed[12] += in_Renamed[11];
            in_Renamed[11] += in_Renamed[10];
            in_Renamed[10] += in_Renamed[9];
            in_Renamed[9] += in_Renamed[8];
            in_Renamed[8] += in_Renamed[7];
            in_Renamed[7] += in_Renamed[6];
            in_Renamed[6] += in_Renamed[5];
            in_Renamed[5] += in_Renamed[4];
            in_Renamed[4] += in_Renamed[3];
            in_Renamed[3] += in_Renamed[2];
            in_Renamed[2] += in_Renamed[1];
            in_Renamed[1] += in_Renamed[0];

            // 18 point IDCT for odd indices input aliasing for 18 point IDCT
            in_Renamed[17] += in_Renamed[15];
            in_Renamed[15] += in_Renamed[13];
            in_Renamed[13] += in_Renamed[11];
            in_Renamed[11] += in_Renamed[9];
            in_Renamed[9] += in_Renamed[7];
            in_Renamed[7] += in_Renamed[5];
            in_Renamed[5] += in_Renamed[3];
            in_Renamed[3] += in_Renamed[1];

            float tmp0, tmp1, tmp2, tmp3, tmp4, tmp0_, tmp1_, tmp2_, tmp3_;
            float tmp0o, tmp1o, tmp2o, tmp3o, tmp4o, tmp0_o, tmp1_o, tmp2_o, tmp3_o;

            // Fast 9 Point Inverse Discrete Cosine Transform
            //
            // By Francois-Raymond Boyer mailto:boyerf@iro.umontreal.ca http://www.iro.umontreal.ca/~boyerf
            //
            // The code has been optimized for Intel processors (takes a lot of time to convert float to and from iternal FPU representation)
            //
            // It is a simple "factorization" of the IDCT matrix.

            // 9 point IDCT on even indices

            // 5 points on odd indices (not realy an IDCT)
            var i00 = in_Renamed[0] + in_Renamed[0];
            var iip12 = i00 + in_Renamed[12];

            tmp0 = iip12 + (in_Renamed[4] * 1.8793852415718f) + (in_Renamed[8] * 1.532088886238f) + (in_Renamed[16] * 0.34729635533386f);
            tmp1 = i00 + in_Renamed[4] - in_Renamed[8] - in_Renamed[12] - in_Renamed[12] - in_Renamed[16];
            tmp2 = iip12 - (in_Renamed[4] * 0.34729635533386f) - (in_Renamed[8] * 1.8793852415718f) + (in_Renamed[16] * 1.532088886238f);
            tmp3 = iip12 - (in_Renamed[4] * 1.532088886238f) + (in_Renamed[8] * 0.34729635533386f) - (in_Renamed[16] * 1.8793852415718f);
            tmp4 = in_Renamed[0] - in_Renamed[4] + in_Renamed[8] - in_Renamed[12] + in_Renamed[16];

            // 4 points on even indices
            var i66_ = in_Renamed[6] * 1.732050808f; // Sqrt[3]

            tmp0_ = (in_Renamed[2] * 1.9696155060244f) + i66_ + (in_Renamed[10] * 1.2855752193731f) + (in_Renamed[14] * 0.68404028665134f);
            tmp1_ = (in_Renamed[2] - in_Renamed[10] - in_Renamed[14]) * 1.732050808f;
            tmp2_ = (in_Renamed[2] * 1.2855752193731f) - i66_ - (in_Renamed[10] * 0.68404028665134f) + (in_Renamed[14] * 1.9696155060244f);
            tmp3_ = (in_Renamed[2] * 0.68404028665134f) - i66_ + (in_Renamed[10] * 1.9696155060244f) - (in_Renamed[14] * 1.2855752193731f);

            // 9 point IDCT on odd indices 5 points on odd indices (not realy an IDCT)
            var i0 = in_Renamed[0 + 1] + in_Renamed[0 + 1];
            var i0p12 = i0 + in_Renamed[12 + 1];

            tmp0o = i0p12 + (in_Renamed[4 + 1] * 1.8793852415718f) + (in_Renamed[8 + 1] * 1.532088886238f) + (in_Renamed[16 + 1] * 0.34729635533386f);
            tmp1o = i0 + in_Renamed[4 + 1] - in_Renamed[8 + 1] - in_Renamed[12 + 1] - in_Renamed[12 + 1] - in_Renamed[16 + 1];
            tmp2o = i0p12 - (in_Renamed[4 + 1] * 0.34729635533386f) - (in_Renamed[8 + 1] * 1.8793852415718f) + (in_Renamed[16 + 1] * 1.532088886238f);
            tmp3o = i0p12 - (in_Renamed[4 + 1] * 1.532088886238f) + (in_Renamed[8 + 1] * 0.34729635533386f) - (in_Renamed[16 + 1] * 1.8793852415718f);
            tmp4o = (in_Renamed[0 + 1] - in_Renamed[4 + 1] + in_Renamed[8 + 1] - in_Renamed[12 + 1] + in_Renamed[16 + 1]) * 0.707106781f; // Twiddled

            // 4 points on even indices
            var i6_ = in_Renamed[6 + 1] * 1.732050808f; // Sqrt[3]

            tmp0_o = (in_Renamed[2 + 1] * 1.9696155060244f) + i6_ + (in_Renamed[10 + 1] * 1.2855752193731f) + (in_Renamed[14 + 1] * 0.68404028665134f);
            tmp1_o = (in_Renamed[2 + 1] - in_Renamed[10 + 1] - in_Renamed[14 + 1]) * 1.732050808f;
            tmp2_o = (in_Renamed[2 + 1] * 1.2855752193731f) - i6_ - (in_Renamed[10 + 1] * 0.68404028665134f) + (in_Renamed[14 + 1] * 1.9696155060244f);
            tmp3_o = (in_Renamed[2 + 1] * 0.68404028665134f) - i6_ + (in_Renamed[10 + 1] * 1.9696155060244f) - (in_Renamed[14 + 1] * 1.2855752193731f);

            // Twiddle factors on odd indices and Butterflies on 9 point IDCT's and twiddle factors for 36 point IDCT
            float e, o;
            e = tmp0 + tmp0_;
            o = (tmp0o + tmp0_o) * 0.501909918f;
            tmpf_0 = e + o;
            tmpf_17 = e - o;
            e = tmp1 + tmp1_;
            o = (tmp1o + tmp1_o) * 0.517638090f;
            tmpf_1 = e + o;
            tmpf_16 = e - o;
            e = tmp2 + tmp2_;
            o = (tmp2o + tmp2_o) * 0.551688959f;
            tmpf_2 = e + o;
            tmpf_15 = e - o;
            e = tmp3 + tmp3_;
            o = (tmp3o + tmp3_o) * 0.610387294f;
            tmpf_3 = e + o;
            tmpf_14 = e - o;
            tmpf_4 = tmp4 + tmp4o;
            tmpf_13 = tmp4 - tmp4o;
            e = tmp3 - tmp3_;
            o = (tmp3o - tmp3_o) * 0.871723397f;
            tmpf_5 = e + o;
            tmpf_12 = e - o;
            e = tmp2 - tmp2_;
            o = (tmp2o - tmp2_o) * 1.183100792f;
            tmpf_6 = e + o;
            tmpf_11 = e - o;
            e = tmp1 - tmp1_;
            o = (tmp1o - tmp1_o) * 1.931851653f;
            tmpf_7 = e + o;
            tmpf_10 = e - o;
            e = tmp0 - tmp0_;
            o = (tmp0o - tmp0_o) * 5.736856623f;
            tmpf_8 = e + o;
            tmpf_9 = e - o;

            // end 36 point IDCT */ shift to modified IDCT
            win_bt = win[block_type];

            out_Renamed[0] = -tmpf_9 * win_bt[0];
            out_Renamed[1] = -tmpf_10 * win_bt[1];
            out_Renamed[2] = -tmpf_11 * win_bt[2];
            out_Renamed[3] = -tmpf_12 * win_bt[3];
            out_Renamed[4] = -tmpf_13 * win_bt[4];
            out_Renamed[5] = -tmpf_14 * win_bt[5];
            out_Renamed[6] = -tmpf_15 * win_bt[6];
            out_Renamed[7] = -tmpf_16 * win_bt[7];
            out_Renamed[8] = -tmpf_17 * win_bt[8];
            out_Renamed[9] = tmpf_17 * win_bt[9];
            out_Renamed[10] = tmpf_16 * win_bt[10];
            out_Renamed[11] = tmpf_15 * win_bt[11];
            out_Renamed[12] = tmpf_14 * win_bt[12];
            out_Renamed[13] = tmpf_13 * win_bt[13];
            out_Renamed[14] = tmpf_12 * win_bt[14];
            out_Renamed[15] = tmpf_11 * win_bt[15];
            out_Renamed[16] = tmpf_10 * win_bt[16];
            out_Renamed[17] = tmpf_9 * win_bt[17];
            out_Renamed[18] = tmpf_8 * win_bt[18];
            out_Renamed[19] = tmpf_7 * win_bt[19];
            out_Renamed[20] = tmpf_6 * win_bt[20];
            out_Renamed[21] = tmpf_5 * win_bt[21];
            out_Renamed[22] = tmpf_4 * win_bt[22];
            out_Renamed[23] = tmpf_3 * win_bt[23];
            out_Renamed[24] = tmpf_2 * win_bt[24];
            out_Renamed[25] = tmpf_1 * win_bt[25];
            out_Renamed[26] = tmpf_0 * win_bt[26];
            out_Renamed[27] = tmpf_0 * win_bt[27];
            out_Renamed[28] = tmpf_1 * win_bt[28];
            out_Renamed[29] = tmpf_2 * win_bt[29];
            out_Renamed[30] = tmpf_3 * win_bt[30];
            out_Renamed[31] = tmpf_4 * win_bt[31];
            out_Renamed[32] = tmpf_5 * win_bt[32];
            out_Renamed[33] = tmpf_6 * win_bt[33];
            out_Renamed[34] = tmpf_7 * win_bt[34];
            out_Renamed[35] = tmpf_8 * win_bt[35];
        }
    }

    void IStereoKValues(int is_pos, int io_type, int i)
    {
        if (is_pos == 0)
        {
            k[0][i] = 1.0f;
            k[1][i] = 1.0f;
        }
        else if ((is_pos & 1) != 0)
        {
            k[0][i] = io[io_type][(is_pos + 1) >> 1];
            k[1][i] = 1.0f;
        }
        else
        {
            k[0][i] = 1.0f;
            k[1][i] = io[io_type][is_pos >> 1];
        }
    }

    void Reorder(float[][] xr, int ch, int gr)
    {
        var gr_info = sideInfo.CH[ch].GR[gr];
        int freq, freq3;
        int index;
        int sfb, sfb_start, sfb_lines;
        int src_line, des_line;
        var xr_1d = xr;

        if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
        {
            for (index = 0; index < 576; index++)
            {
                out1d[index] = 0.0f;
            }

            if (gr_info.MixedBlockFlag != 0)
            {
                // NO REORDER FOR LOW 2 SUBBANDS
                for (index = 0; index < 36; index++)
                {
                    // Modif E.B 02/22/99
                    var reste = index % SSLIMIT;
                    var quotien = (index - reste) / SSLIMIT;
                    out1d[index] = xr_1d[quotien][reste];
                }

                // REORDERING FOR REST SWITCHED SHORT
                for (sfb = 3, sfb_start = sfBandIndex[sfreq].S[3], sfb_lines = sfBandIndex[sfreq].S[4] - sfb_start; sfb < 13; sfb++, sfb_start = sfBandIndex[sfreq].S[sfb], sfb_lines = sfBandIndex[sfreq].S[sfb + 1] - sfb_start)
                {
                    var sfb_start3 = (sfb_start << 2) - sfb_start;

                    for (freq = 0, freq3 = 0; freq < sfb_lines; freq++, freq3 += 3)
                    {
                        src_line = sfb_start3 + freq;
                        des_line = sfb_start3 + freq3;

                        // Modif E.B 02/22/99
                        var reste = src_line % SSLIMIT;
                        var quotien = (src_line - reste) / SSLIMIT;

                        out1d[des_line] = xr_1d[quotien][reste];
                        src_line += sfb_lines;
                        des_line++;

                        reste = src_line % SSLIMIT;
                        quotien = (src_line - reste) / SSLIMIT;

                        out1d[des_line] = xr_1d[quotien][reste];
                        src_line += sfb_lines;
                        des_line++;

                        reste = src_line % SSLIMIT;
                        quotien = (src_line - reste) / SSLIMIT;

                        out1d[des_line] = xr_1d[quotien][reste];
                    }
                }
            }
            else
            {
                // pure short
                for (index = 0; index < 576; index++)
                {
                    var j = reorderTable[sfreq][index];
                    var reste = j % SSLIMIT;
                    var quotien = (j - reste) / SSLIMIT;
                    out1d[index] = xr_1d[quotien][reste];
                }
            }
        }
        else
        {
            // long blocks
            for (index = 0; index < 576; index++)
            {
                // Modif E.B 02/22/99
                var reste = index % SSLIMIT;
                var quotien = (index - reste) / SSLIMIT;
                out1d[index] = xr_1d[quotien][reste];
            }
        }
    }

    void Stereo(MP3AudioFrameHeader header, int gr)
    {
        int sb, ss;

        if (channels == 1)
        {
            // mono , bypass xr[0][][] to lr[0][][]
            for (sb = 0; sb < SBLIMIT; sb++)
            {
                for (ss = 0; ss < SSLIMIT; ss += 3)
                {
                    lr[0][sb][ss] = ro[0][sb][ss];
                    lr[0][sb][ss + 1] = ro[0][sb][ss + 1];
                    lr[0][sb][ss + 2] = ro[0][sb][ss + 2];
                }
            }
        }
        else
        {
            var gr_info = sideInfo.CH[0].GR[gr];
            int mode_ext = header.ModeExtension;

            int sfb;
            int i;
            int lines, temp, temp2;

            var ms_stereo = false;
            var i_stereo = false;

            if (header.Channels == MP3AudioFrameChannels.JointStereo)
            {
                ms_stereo = (header.ModeExtension & 0x2) != 0;
                i_stereo = (header.ModeExtension & 0x1) != 0;
            }
            var lsf = (header.Version == MP3AudioFrameVersion.Version2) || (header.Version == MP3AudioFrameVersion.Version25);
            var io_type = gr_info.ScaleFactorCompress & 1;

            // initialization
            for (i = 0; i < 576; i++)
            {
                isPos[i] = 7;

                isRatio[i] = 0.0f;
            }

            if (i_stereo)
            {
                if ((gr_info.WindowSwitchingFlag != 0) && (gr_info.BlockType == 2))
                {
                    if (gr_info.MixedBlockFlag != 0)
                    {
                        var max_sfb = 0;

                        for (var j = 0; j < 3; j++)
                        {
                            int sfbcnt;
                            sfbcnt = 2;
                            for (sfb = 12; sfb >= 3; sfb--)
                            {
                                i = sfBandIndex[sfreq].S[sfb];
                                lines = sfBandIndex[sfreq].S[sfb + 1] - i;
                                i = (i << 2) - i + ((j + 1) * lines) - 1;

                                while (lines > 0)
                                {
                                    if (ro[1][i / 18][i % 18] != 0.0f)
                                    {
                                        // MDM: in java, array access is very slow. Is quicker to compute div and mod values. if (ro[1][ss_div[i]][ss_mod[i]] !=
                                        // 0.0f) {
                                        sfbcnt = sfb;
                                        sfb = -10;
                                        lines = -10;
                                    }

                                    lines--;
                                    i--;
                                } // while (lines > 0)
                            }

                            // for (sfb=12 ...
                            sfb = sfbcnt + 1;

                            if (sfb > max_sfb)
                            {
                                max_sfb = sfb;
                            }

                            while (sfb < 12)
                            {
                                temp = sfBandIndex[sfreq].S[sfb];
                                sb = sfBandIndex[sfreq].S[sfb + 1] - temp;
                                i = (temp << 2) - temp + (j * sb);

                                for (; sb > 0; sb--)
                                {
                                    isPos[i] = scaleFactors[1].S[j][sfb];
                                    if (isPos[i] != 7)
                                    {
                                        if (lsf)
                                        {
                                            IStereoKValues(isPos[i], io_type, i);
                                        }
                                        else
                                        {
                                            isRatio[i] = tan12[isPos[i]];
                                        }
                                    }

                                    i++;
                                }

                                // for (; sb>0...
                                sfb++;
                            } // while (sfb < 12)
                            sfb = sfBandIndex[sfreq].S[10];
                            sb = sfBandIndex[sfreq].S[11] - sfb;
                            sfb = (sfb << 2) - sfb + (j * sb);
                            temp = sfBandIndex[sfreq].S[11];
                            sb = sfBandIndex[sfreq].S[12] - temp;
                            i = (temp << 2) - temp + (j * sb);

                            for (; sb > 0; sb--)
                            {
                                isPos[i] = isPos[sfb];

                                if (lsf)
                                {
                                    k[0][i] = k[0][sfb];
                                    k[1][i] = k[1][sfb];
                                }
                                else
                                {
                                    isRatio[i] = isRatio[sfb];
                                }
                                i++;
                            }

                            // for (; sb > 0 ...
                        }
                        if (max_sfb <= 3)
                        {
                            i = 2;
                            ss = 17;
                            sb = -1;
                            while (i >= 0)
                            {
                                if (ro[1][i][ss] != 0.0f)
                                {
                                    sb = (i << 4) + (i << 1) + ss;
                                    i = -1;
                                }
                                else
                                {
                                    ss--;
                                    if (ss < 0)
                                    {
                                        i--;
                                        ss = 17;
                                    }
                                }

                                // if (ro ...
                            } // while (i>=0)
                            i = 0;
                            while (sfBandIndex[sfreq].L[i] <= sb)
                            {
                                i++;
                            }

                            sfb = i;
                            i = sfBandIndex[sfreq].L[i];
                            for (; sfb < 8; sfb++)
                            {
                                sb = sfBandIndex[sfreq].L[sfb + 1] - sfBandIndex[sfreq].L[sfb];
                                for (; sb > 0; sb--)
                                {
                                    isPos[i] = scaleFactors[1].L[sfb];
                                    if (isPos[i] != 7)
                                    {
                                        if (lsf)
                                        {
                                            IStereoKValues(isPos[i], io_type, i);
                                        }
                                        else
                                        {
                                            isRatio[i] = tan12[isPos[i]];
                                        }
                                    }

                                    i++;
                                }

                                // for (; sb>0 ...
                            }

                            // for (; sfb<8 ...
                        }

                        // for (j=0 ...
                    }
                    else
                    {
                        // if (gr_info.mixed_block_flag)
                        for (var j = 0; j < 3; j++)
                        {
                            int sfbcnt;
                            sfbcnt = -1;
                            for (sfb = 12; sfb >= 0; sfb--)
                            {
                                temp = sfBandIndex[sfreq].S[sfb];
                                lines = sfBandIndex[sfreq].S[sfb + 1] - temp;
                                i = (temp << 2) - temp + ((j + 1) * lines) - 1;

                                while (lines > 0)
                                {
                                    if (ro[1][i / 18][i % 18] != 0.0f)
                                    {
                                        // MDM: in java, array access is very slow. Is quicker to compute div and mod values. if (ro[1][ss_div[i]][ss_mod[i]] !=
                                        // 0.0f) {
                                        sfbcnt = sfb;
                                        sfb = -10;
                                        lines = -10;
                                    }
                                    lines--;
                                    i--;
                                } // while (lines > 0) */
                            }

                            // for (sfb=12 ...
                            sfb = sfbcnt + 1;
                            while (sfb < 12)
                            {
                                temp = sfBandIndex[sfreq].S[sfb];
                                sb = sfBandIndex[sfreq].S[sfb + 1] - temp;
                                i = (temp << 2) - temp + (j * sb);
                                for (; sb > 0; sb--)
                                {
                                    isPos[i] = scaleFactors[1].S[j][sfb];
                                    if (isPos[i] != 7)
                                    {
                                        if (lsf)
                                        {
                                            IStereoKValues(isPos[i], io_type, i);
                                        }
                                        else
                                        {
                                            isRatio[i] = tan12[isPos[i]];
                                        }
                                    }

                                    i++;
                                }

                                // for (; sb>0 ...
                                sfb++;
                            } // while (sfb<12)

                            temp = sfBandIndex[sfreq].S[10];
                            temp2 = sfBandIndex[sfreq].S[11];
                            sb = temp2 - temp;
                            sfb = (temp << 2) - temp + (j * sb);
                            sb = sfBandIndex[sfreq].S[12] - temp2;
                            i = (temp2 << 2) - temp2 + (j * sb);

                            for (; sb > 0; sb--)
                            {
                                isPos[i] = isPos[sfb];

                                if (lsf)
                                {
                                    k[0][i] = k[0][sfb];
                                    k[1][i] = k[1][sfb];
                                }
                                else
                                {
                                    isRatio[i] = isRatio[sfb];
                                }
                                i++;
                            }

                            // for (; sb>0 ...
                        }

                        // for (sfb=12
                    }

                    // for (j=0 ...
                }
                else
                {
                    // if (gr_info.window_switching_flag ...
                    i = 31;
                    ss = 17;
                    sb = 0;
                    while (i >= 0)
                    {
                        if (ro[1][i][ss] != 0.0f)
                        {
                            sb = (i << 4) + (i << 1) + ss;
                            i = -1;
                        }
                        else
                        {
                            ss--;
                            if (ss < 0)
                            {
                                i--;
                                ss = 17;
                            }
                        }
                    }
                    i = 0;
                    while (sfBandIndex[sfreq].L[i] <= sb)
                    {
                        i++;
                    }

                    sfb = i;
                    i = sfBandIndex[sfreq].L[i];
                    for (; sfb < 21; sfb++)
                    {
                        sb = sfBandIndex[sfreq].L[sfb + 1] - sfBandIndex[sfreq].L[sfb];
                        for (; sb > 0; sb--)
                        {
                            isPos[i] = scaleFactors[1].L[sfb];
                            if (isPos[i] != 7)
                            {
                                if (lsf)
                                {
                                    IStereoKValues(isPos[i], io_type, i);
                                }
                                else
                                {
                                    isRatio[i] = tan12[isPos[i]];
                                }
                            }

                            i++;
                        }
                    }
                    sfb = sfBandIndex[sfreq].L[20];
                    for (sb = 576 - sfBandIndex[sfreq].L[21]; (sb > 0) && (i < 576); sb--)
                    {
                        isPos[i] = isPos[sfb]; // error here : i >=576

                        if (lsf)
                        {
                            k[0][i] = k[0][sfb];
                            k[1][i] = k[1][sfb];
                        }
                        else
                        {
                            isRatio[i] = isRatio[sfb];
                        }
                        i++;
                    }

                    // if (gr_info.mixed_block_flag)
                }

                // if (gr_info.window_switching_flag ...
            }

            // if (i_stereo)
            i = 0;
            for (sb = 0; sb < SBLIMIT; sb++)
            {
                for (ss = 0; ss < SSLIMIT; ss++)
                {
                    if (isPos[i] == 7)
                    {
                        if (ms_stereo)
                        {
                            lr[0][sb][ss] = (ro[0][sb][ss] + ro[1][sb][ss]) * 0.707106781f;
                            lr[1][sb][ss] = (ro[0][sb][ss] - ro[1][sb][ss]) * 0.707106781f;
                        }
                        else
                        {
                            lr[0][sb][ss] = ro[0][sb][ss];
                            lr[1][sb][ss] = ro[1][sb][ss];
                        }
                    }
                    else if (i_stereo)
                    {
                        if (lsf)
                        {
                            lr[0][sb][ss] = ro[0][sb][ss] * k[0][i];
                            lr[1][sb][ss] = ro[0][sb][ss] * k[1][i];
                        }
                        else
                        {
                            lr[1][sb][ss] = ro[0][sb][ss] / (1 + isRatio[i]);
                            lr[0][sb][ss] = lr[1][sb][ss] * isRatio[i];
                        }
                    }
                    /*                else {
                    System.out.println("Error in stereo processing\n");
                    } */
                    i++;
                }
            }
        }

        // channels == 2
    }

    #endregion Private Methods

    #region Internal Methods

    internal static int[] Reorder(int[] scalefac_band)
    {
        // SZD: converted from LAME
        var j = 0;
        var ix = new int[576];
        for (var sfb = 0; sfb < 13; sfb++)
        {
            var start = scalefac_band[sfb];
            var end = scalefac_band[sfb + 1];
            for (var window = 0; window < 3; window++)
            {
                for (var i = start; i < end; i++)
                {
                    ix[(3 * i) + window] = j++;
                }
            }
        }
        return ix;
    }

    #endregion Internal Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="MP3AudioLayerIIIDecoder"/> class.</summary>
    /// <param name="header">The first frame header.</param>
    /// <param name="filter1">The output filter for the first channel.</param>
    /// <param name="filter2">The output filter for the second channel.</param>
    /// <param name="buffer">The output (sample) buffer.</param>
    /// <param name="mode">The audio channel decoder mode.</param>
    /// <exception cref="NotImplementedException">MP3AudioFrameVersion {0} is not implemented.</exception>
    public MP3AudioLayerIIIDecoder(MP3AudioFrameHeader header, MP3AudioSynthesisFilter filter1, MP3AudioSynthesisFilter? filter2, MP3AudioStereoBuffer buffer, MP3AudioOutputMode mode)
    {
        if (header == null)
        {
            throw new ArgumentNullException("Header");
        }

        this.filter1 = filter1 ?? throw new ArgumentNullException("Filter1");
        this.filter2 = filter2;
        outputBuffer = buffer ?? throw new ArgumentNullException("Buffer");
        outputMode = mode;

        is1d = new int[(SBLIMIT * SSLIMIT) + 4];
        ro = new float[2][][];
        for (var i = 0; i < 2; i++)
        {
            ro[i] = new float[SBLIMIT][];
            for (var i2 = 0; i2 < SBLIMIT; i2++)
            {
                ro[i][i2] = new float[SSLIMIT];
            }
        }
        lr = new float[2][][];
        for (var i3 = 0; i3 < 2; i3++)
        {
            lr[i3] = new float[SBLIMIT][];
            for (var i4 = 0; i4 < SBLIMIT; i4++)
            {
                lr[i3][i4] = new float[SSLIMIT];
            }
        }
        out1d = new float[SBLIMIT * SSLIMIT];
        prevblck = new float[2][];
        for (var i5 = 0; i5 < 2; i5++)
        {
            prevblck[i5] = new float[SBLIMIT * SSLIMIT];
        }
        k = new float[2][];
        for (var i6 = 0; i6 < 2; i6++)
        {
            k[i6] = new float[SBLIMIT * SSLIMIT];
        }
        nonzero = new int[2];

        scaleFactors = new ScaleFactors[2];
        scaleFactors[0] = new ScaleFactors();
        scaleFactors[1] = new ScaleFactors();

        // L3TABLE INIT
        sfBandIndex = new SBI[9]; // SZD: MPEG2.5 +3 indices
        var l0 = new int[] { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        var s0 = new int[] { 0, 4, 8, 12, 18, 24, 32, 42, 56, 74, 100, 132, 174, 192 };
        var l1 = new int[] { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 114, 136, 162, 194, 232, 278, 330, 394, 464, 540, 576 };
        var s1 = new int[] { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 136, 180, 192 };
        var l2 = new int[] { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        var s2 = new int[] { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };

        var l3 = new int[] { 0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 52, 62, 74, 90, 110, 134, 162, 196, 238, 288, 342, 418, 576 };
        var s3 = new int[] { 0, 4, 8, 12, 16, 22, 30, 40, 52, 66, 84, 106, 136, 192 };
        var l4 = new int[] { 0, 4, 8, 12, 16, 20, 24, 30, 36, 42, 50, 60, 72, 88, 106, 128, 156, 190, 230, 276, 330, 384, 576 };
        var s4 = new int[] { 0, 4, 8, 12, 16, 22, 28, 38, 50, 64, 80, 100, 126, 192 };
        var l5 = new int[] { 0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 54, 66, 82, 102, 126, 156, 194, 240, 296, 364, 448, 550, 576 };
        var s5 = new int[] { 0, 4, 8, 12, 16, 22, 30, 42, 58, 78, 104, 138, 180, 192 };

        // SZD: MPEG2.5
        var l6 = new int[] { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        var s6 = new int[] { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };
        var l7 = new int[] { 0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576 };
        var s7 = new int[] { 0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192 };
        var l8 = new int[] { 0, 12, 24, 36, 48, 60, 72, 88, 108, 132, 160, 192, 232, 280, 336, 400, 476, 566, 568, 570, 572, 574, 576 };
        var s8 = new int[] { 0, 8, 16, 24, 36, 52, 72, 96, 124, 160, 162, 164, 166, 192 };

        sfBandIndex[0] = new SBI(l0, s0);
        sfBandIndex[1] = new SBI(l1, s1);
        sfBandIndex[2] = new SBI(l2, s2);

        sfBandIndex[3] = new SBI(l3, s3);
        sfBandIndex[4] = new SBI(l4, s4);
        sfBandIndex[5] = new SBI(l5, s5);

        // SZD: MPEG2.5
        sfBandIndex[6] = new SBI(l6, s6);
        sfBandIndex[7] = new SBI(l7, s7);
        sfBandIndex[8] = new SBI(l8, s8);

        // END OF L3TABLE INIT
        if (reorderTable.Length == 0)
        {
            // SZD: generate LUT
            reorderTable = new int[9][];
            for (var i = 0; i < 9; i++)
            {
                reorderTable[i] = Reorder(sfBandIndex[i].S);
            }
        }

        // scalefac_buffer
        scaleFactorBuffer = new int[54];

        // END OF scalefac_buffer
        frameStart = 0;
        channels = header.ChannelCount;

        switch (header.Version)
        {
            case MP3AudioFrameVersion.Version1:
                sfreq = header.SamplingRateIndex + 3;
                maxGr = 2;
                break;

            case MP3AudioFrameVersion.Version2:
                sfreq = header.SamplingRateIndex;
                maxGr = 1;
                break;

            case MP3AudioFrameVersion.Version25:
                sfreq = header.SamplingRateIndex + 6;
                maxGr = 1;
                break;

            default: throw new NotImplementedException(string.Format("MP3AudioFrameVersion {0} is not implemented!", header.Version));
        }

        if (channels == 2)
        {
            switch (outputMode)
            {
                case MP3AudioOutputMode.Left:
                case MP3AudioOutputMode.DownMix:
                    firstChannel = lastChannel = 0;
                    break;

                case MP3AudioOutputMode.Right:
                    firstChannel = lastChannel = 1;
                    break;

                case MP3AudioOutputMode.Both:
                default:
                    firstChannel = 0;
                    lastChannel = 1;
                    break;
            }
        }
        else
        {
            firstChannel = lastChannel = 0;
        }

        for (var ch = 0; ch < 2; ch++)
        {
            for (var j = 0; j < 576; j++)
            {
                prevblck[ch][j] = 0.0f;
            }
        }

        nonzero[0] = nonzero[1] = 576;

        bitReserve = new MP3BitReserve();
        sideInfo = new SideInfo();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the name of the log source.</summary>
    /// <value>The name of the log source.</value>
    public string LogSourceName => "MP3AudioLayerIIIDecoder";

    #endregion Public Properties

    #region Public Methods

    /// <summary>Decodes the audio frame.</summary>
    /// <param name="frame">The frame to decode.</param>
    /// <exception cref="NotImplementedException">MP3AudioFrameVersion {0} is not implemented.</exception>
    public void DecodeFrame(MP3AudioFrame frame)
    {
        var frameHeader = frame.Header;
        var nSlots = frameHeader.Slots;
        int flush_main;
        int gr, ch, ss, sb, sb18;
        int main_data_end;
        int bytes_to_discard;

        switch (frameHeader.Version)
        {
            case MP3AudioFrameVersion.Version1:
                GetSideInfoVer1(frame);
                break;

            case MP3AudioFrameVersion.Version2:
            case MP3AudioFrameVersion.Version25:
                GetSideInfoVer2(frame);
                break;

            default: throw new NotImplementedException(string.Format("MP3AudioFrameVersion {0} is not implemented!", frameHeader.Version));
        }

        frame.Bits.MoveBits(nSlots * 8, bitReserve);

        main_data_end = bitReserve.ReadPosition >> 3; // of previous frame

        if ((flush_main = bitReserve.ReadPosition & 7) != 0)
        {
            bitReserve.Skip(8 - flush_main);
            main_data_end++;
        }

        bytes_to_discard = frameStart - main_data_end - sideInfo.MainDataBegin;

        frameStart += nSlots;

        if (bytes_to_discard < 0)
        {
            return;
        }

        if (main_data_end > 4096)
        {
            frameStart -= 4096;
            bitReserve.Rewind(4096 * 8);
        }

        if (bytes_to_discard > 0)
        {
            bitReserve.Skip(bytes_to_discard * 8);
        }

        for (gr = 0; gr < maxGr; gr++)
        {
            for (ch = 0; ch < channels; ch++)
            {
                part2Start = bitReserve.ReadPosition;

                switch (frameHeader.Version)
                {
                    case MP3AudioFrameVersion.Version1:
                        GetScaleFactors(ch, gr);
                        break;

                    case MP3AudioFrameVersion.Version2:
                    case MP3AudioFrameVersion.Version25:
                        GetLsfScaleFactors(frameHeader, ch, gr);
                        break;

                    default: throw new NotImplementedException(string.Format("MP3AudioFrameVersion {0} is not implemented!", frameHeader.Version));
                }

                HuffmanDecode(ch, gr);

                // System.out.println("CheckSum HuffMan = " + CheckSumHuff);
                DequantizeSample(ro[ch], ch, gr);
            }

            Stereo(frameHeader, gr);

            if ((outputMode == MP3AudioOutputMode.DownMix) && (channels > 1))
            {
                DoDownmix();
            }

            for (ch = firstChannel; ch <= lastChannel; ch++)
            {
                Reorder(lr[ch], ch, gr);
                Antialias(ch, gr);
                Hybrid(ch, gr);

                for (sb18 = 18; sb18 < 576; sb18 += 36)
                {
                    // Frequency inversion
                    for (ss = 1; ss < SSLIMIT; ss += 2)
                    {
                        out1d[sb18 + ss] = -out1d[sb18 + ss];
                    }
                }

                if ((ch == 0) || (outputMode == MP3AudioOutputMode.Right))
                {
                    for (ss = 0; ss < SSLIMIT; ss++)
                    {
                        // Polyphase synthesis
                        sb = 0;
                        for (sb18 = 0; sb18 < 576; sb18 += 18)
                        {
                            sampleBuffer1[sb] = out1d[sb18 + ss];

                            // filter1.input_sample(out_1d[sb18+ss], sb);
                            sb++;
                        }

                        // buffer.appendSamples(0, samples1);
                        filter1!.AddSamples(sampleBuffer1);
                        filter1!.CalculateSamples(outputBuffer);
                    }
                }
                else
                {
                    for (ss = 0; ss < SSLIMIT; ss++)
                    {
                        // Polyphase synthesis
                        sb = 0;
                        for (sb18 = 0; sb18 < 576; sb18 += 18)
                        {
                            sampleBuffer2[sb] = out1d[sb18 + ss];

                            // filter2.input_sample(out_1d[sb18+ss], sb);
                            sb++;
                        }

                        // buffer.appendSamples(1, samples2); Console.WriteLine("Adding samples right into output buffer");
                        filter2!.AddSamples(sampleBuffer2);
                        filter2!.CalculateSamples(outputBuffer);
                    }
                }
            }

            // channels
        }

        // granule
        counter++;
    }

    /// <summary>Notify decoder that a seek is being made.</summary>
    public void Reset()
    {
        frameStart = 0;
        for (var ch = 0; ch < 2; ch++)
        {
            for (var j = 0; j < 576; j++)
            {
                prevblck[ch][j] = 0.0f;
            }
        }

        bitReserve = new MP3BitReserve();
        sideInfo = new SideInfo();
    }

    #endregion Public Methods
}
