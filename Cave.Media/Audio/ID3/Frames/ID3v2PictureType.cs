namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Provides avaiable ID3v2 pricture types.
    /// </summary>

    public enum ID3v2PictureType : byte
    {
        /// <summary>
        /// Unkown type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 32x32 32bit png picture
        /// </summary>
        IconPng32x32 = 1,

        /// <summary>
        /// icon
        /// </summary>
        Icon = 2,

        /// <summary>
        /// front cover
        /// </summary>
        CoverFront = 3,

        /// <summary>
        /// back cover
        /// </summary>
        CoverBack = 4,

        /// <summary>
        /// leaflet
        /// </summary>
        Leaflet = 5,

        /// <summary>
        /// picture of the media (e.g. CD)
        /// </summary>
        Media = 6,

        /// <summary>
        /// Lead artist/lead performer/soloist
        /// </summary>
        LeadArtist = 7,

        /// <summary>
        /// Artist/performer
        /// </summary>
        Artist = 8,

        /// <summary>
        /// Conductor
        /// </summary>
        Conductor = 9,

        /// <summary>
        /// Band/Orchestra
        /// </summary>
        Band = 0xA,

        /// <summary>
        /// Composer
        /// </summary>
        Composer = 0x0B,

        /// <summary>
        /// Lyricist/text writer
        /// </summary>
        Lyricist = 0x0C,

        /// <summary>
        /// Recording Location
        /// </summary>
        RecordingLocation = 0x0D,

        /// <summary>
        /// During recording
        /// </summary>
        DuringRecording = 0x0E,

        /// <summary>
        /// During performance
        /// </summary>
        DuringPerformance = 0x0F,

        /// <summary>
        /// Movie/video screen capture
        /// </summary>
        MovieCapture = 0x10,

        /// <summary>
        /// A bright coloured fish
        /// </summary>
        Fish = 0x11,

        /// <summary>
        /// Illustration
        /// </summary>
        Illustration = 0x12,

        /// <summary>
        /// Band/artist logotype
        /// </summary>
        BandLogo = 0x13,

        /// <summary>
        /// Publisher/Studio logotype
        /// </summary>
        PublisherLogo = 0x14,
    }
}
