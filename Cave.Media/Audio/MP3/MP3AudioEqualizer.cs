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
    /// <summary> The Equalizer class can be used to specify
    /// equalization settings for the MPEG audio decoder.
    /// The equalizer consists of 32 band-pass filters.
    /// Each band of the equalizer can take on a fractional value between -1.0 and +1.0.
    /// At -1.0, the input signal is attenuated by 6dB, at +1.0 the signal is amplified by 6dB.
    /// </summary>
    public sealed class MP3AudioEqualizer
    {
        const int bands = 32;

        /// <summary>
        /// Equalizer setting to denote that a given band will not be present in the output signal.
        /// </summary>
        public static readonly float BandNotPresent = float.NegativeInfinity;

        float[] values;

        float Limit(float eq)
        {
            if (eq == BandNotPresent)
            {
                return eq;
            }

            if (eq > 1.0f)
            {
                return 1.0f;
            }

            return eq < -1.0f ? -1.0f : eq;
        }

        /// <summary>Creates a new Equalizer instance.</summary>
        public MP3AudioEqualizer()
        {
            values = new float[bands];
        }

        /// <summary>Initializes a new instance of the <see cref="MP3AudioEqualizer"/> class.</summary>
        /// <param name="itemstings">The 32 band settings.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MP3AudioEqualizer(float[] itemstings)
            : this()
        {
            if (itemstings == null)
            {
                throw new ArgumentNullException("Settings");
            }

            Reset();
            if (itemstings.Length != bands)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < bands; i++)
            {
                values[i] = Limit(itemstings[i]);
            }
        }

        /// <summary> Sets all bands to 0.0.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < bands; i++)
            {
                values[i] = 0.0f;
            }
        }

        /// <summary>Gets or sets the amplification value (-1 .. 1) for the specified band.</summary>
        /// <value>The amplification.</value>
        /// <param name="band">The band.</param>
        /// <returns></returns>
        public float this[int band]
        {
            get
            {
                return values[band];
            }
            set
            {
                values[band] = Limit(value);
            }
        }

        /// <summary>
        /// Converts an equalizer band setting to a sample factor.
        /// The factor is determined by the function f = 2^n where
        /// n is the equalizer band setting in the range [-1.0,1.0].
        /// </summary>
        /// <param name="band">The band.</param>
        /// <returns></returns>
        public float GetFactor(int band)
        {
            float value = values[band];
            return value == BandNotPresent ? 0.0f : (float)Math.Pow(2.0, value);
        }

        /// <summary>
        /// Converts all equalizer band setting to sample factors.
        /// See <see cref="GetFactor(int)"/> for further details.
        /// </summary>
        /// <returns></returns>
        public float[] GetFactors()
        {
            float[] result = new float[bands];
            for (int i = 0; i < bands; i++)
            {
                result[i] = GetFactor(i);
            }
            return result;
        }
    }
}