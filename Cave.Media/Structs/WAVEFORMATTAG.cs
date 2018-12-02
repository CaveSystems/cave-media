
namespace Cave.Media.Structs
{
    /// <summary>
    /// Provides the wave data format
    /// </summary>
    public enum WAVEFORMATTAG : ushort
    {
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        UNKNOWN = 0x0000,
        /// <summary>
        /// Default PCM
        /// </summary>
        PCM = 0x0001,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        ADPCM = 0x0002,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        IEEE_FLOAT = 0x0003,
        /// <summary>
        /// Compaq Computer Corp.
        /// </summary>
        VSELP = 0x0004,
        /// <summary>
        /// IBM Corporation
        /// </summary>
        IBM_CVSD = 0x0005,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        ALAW = 0x0006,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MULAW = 0x0007,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        DTS = 0x0008,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        DRM = 0x0009,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMAVOICE9 = 0x000A,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMAVOICE10 = 0x000B,
        /// <summary>
        /// OKI
        /// </summary>
        OKI_ADPCM = 0x0010,
        /// <summary>
        /// Intel Corporation
        /// </summary>
        DVI_ADPCM = 0x0011,
        /// <summary>
        /// Intel Corporation
        /// </summary>
        IMA_ADPCM = (DVI_ADPCM),
        /// <summary>
        /// Videologic
        /// </summary>
        MEDIASPACE_ADPCM = 0x0012,
        /// <summary>
        /// Sierra Semiconductor Corp
        /// </summary>
        SIERRA_ADPCM = 0x0013,
        /// <summary>
        /// Antex Electronics Corporation
        /// </summary>
        G723_ADPCM = 0x0014,
        /// <summary>
        /// DSP Solutions, Inc.
        /// </summary>
        DIGISTD = 0x0015,
        /// <summary>
        /// DSP Solutions, Inc.
        /// </summary>
        DIGIFIX = 0x0016,
        /// <summary>
        /// Dialogic Corporation
        /// </summary>
        DIALOGIC_OKI_ADPCM = 0x0017,
        /// <summary>
        /// Media Vision, Inc.
        /// </summary>
        MEDIAVISION_ADPCM = 0x0018,
        /// <summary>
        /// Hewlett-Packard Company
        /// </summary>
        CU_CODEC = 0x0019,
        /// <summary>
        /// Yamaha Corporation of America
        /// </summary>
        YAMAHA_ADPCM = 0x0020,
        /// <summary>
        /// Speech Compression
        /// </summary>
        SONARC = 0x0021,
        /// <summary>
        /// DSP Group, Inc
        /// </summary>
        DSPGROUP_TRUESPEECH = 0x0022,
        /// <summary>
        /// Echo Speech Corporation
        /// </summary>
        ECHOSC1 = 0x0023,
        /// <summary>
        /// Virtual Music, Inc.
        /// </summary>
        AUDIOFILE_AF36 = 0x0024,
        /// <summary>
        /// Audio Processing Technology
        /// </summary>
        APTX = 0x0025,
        /// <summary>
        /// Virtual Music, Inc.
        /// </summary>
        AUDIOFILE_AF10 = 0x0026,
        /// <summary>
        /// Aculab plc
        /// </summary>
        PROSODY_1612 = 0x0027,
        /// <summary>
        /// Merging Technologies S.A.
        /// </summary>
        LRC = 0x0028,
        /// <summary>
        /// Dolby Laboratories
        /// </summary>
        DOLBY_AC2 = 0x0030,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        GSM610 = 0x0031,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MSNAUDIO = 0x0032,
        /// <summary>
        /// Antex Electronics Corporation
        /// </summary>
        ANTEX_ADPCME = 0x0033,
        /// <summary>
        /// Control Resources Limited
        /// </summary>
        CONTROL_RES_VQLPC = 0x0034,
        /// <summary>
        /// DSP Solutions, Inc.
        /// </summary>
        DIGIREAL = 0x0035,
        /// <summary>
        /// DSP Solutions, Inc.
        /// </summary>
        DIGIADPCM = 0x0036,
        /// <summary>
        /// Control Resources Limited
        /// </summary>
        CONTROL_RES_CR10 = 0x0037,
        /// <summary>
        /// Natural MicroSystems
        /// </summary>
        NMS_VBXADPCM = 0x0038,
        /// <summary>
        /// Crystal Semiconductor IMA ADPCM
        /// </summary>
        CS_IMAADPCM = 0x0039,
        /// <summary>
        /// Echo Speech Corporation
        /// </summary>
        ECHOSC3 = 0x003A,
        /// <summary>
        /// Rockwell International
        /// </summary>
        ROCKWELaDPCM = 0x003B,
        /// <summary>
        /// Rockwell International
        /// </summary>
        ROCKWELL_DIGITALK = 0x003C,
        /// <summary>
        /// Xebec Multimedia Solutions Limited
        /// </summary>
        XEBEC = 0x003D,
        /// <summary>
        /// Antex Electronics Corporation
        /// </summary>
        G721_ADPCM = 0x0040,
        /// <summary>
        /// Antex Electronics Corporation
        /// </summary>
        G728_CELP = 0x0041,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MSG723 = 0x0042,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MPEG = 0x0050,
        /// <summary>
        /// InSoft, Inc.
        /// </summary>
        RT24 = 0x0052,
        /// <summary>
        /// InSoft, Inc.
        /// </summary>
        PAC = 0x0053,
        /// <summary>
        /// ISO/MPEG Layer3 Format Tag
        /// </summary>
        MPEGLAYER3 = 0x0055,
        /// <summary>
        /// Lucent Technologies
        /// </summary>
        LUCENT_G723 = 0x0059,
        /// <summary>
        /// Cirrus Logic
        /// </summary>
        CIRRUS = 0x0060,
        /// <summary>
        /// ESS Technology
        /// </summary>
        ESPCM = 0x0061,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE = 0x0062,
        /// <summary>
        /// Canopus, co., Ltd.
        /// </summary>
        CANOPUS_ATRAC = 0x0063,
        /// <summary>
        /// APICOM
        /// </summary>
        G726_ADPCM = 0x0064,
        /// <summary>
        /// APICOM
        /// </summary>
        G722_ADPCM = 0x0065,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        DSAT_DISPLAY = 0x0067,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_BYTE_ALIGNED = 0x0069,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_AC8 = 0x0070,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_AC10 = 0x0071,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_AC16 = 0x0072,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_AC20 = 0x0073,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_RT24 = 0x0074,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_RT29 = 0x0075,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_RT29HW = 0x0076,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_VR12 = 0x0077,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_VR18 = 0x0078,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_TQ40 = 0x0079,
        /// <summary>
        /// Softsound, Ltd.
        /// </summary>
        SOFTSOUND = 0x0080,
        /// <summary>
        /// Voxware Inc
        /// </summary>
        VOXWARE_TQ60 = 0x0081,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MSRT24 = 0x0082,
        /// <summary>
        /// AT<![CDATA[&]]>T Labs, Inc.
        /// </summary>
        G729A = 0x0083,
        /// <summary>
        /// Motion Pixels
        /// </summary>
        MVI_MVI2 = 0x0084,
        /// <summary>
        /// DataFusion Systems (Pty) (Ltd)
        /// </summary>
        DF_G726 = 0x0085,
        /// <summary>
        /// DataFusion Systems (Pty) (Ltd)
        /// </summary>
        DF_GSM610 = 0x0086,
        /// <summary>
        /// Iterated Systems, Inc.
        /// </summary>
        ISIAUDIO = 0x0088,
        /// <summary>
        /// OnLive! Technologies, Inc.
        /// </summary>
        ONLIVE = 0x0089,
        /// <summary>
        /// Siemens Business Communications Sys
        /// </summary>
        SBC24 = 0x0091,
        /// <summary>
        /// Sonic Foundry
        /// </summary>
        DOLBY_AC3_SPDIF = 0x0092,
        /// <summary>
        /// MediaSonic
        /// </summary>
        MEDIASONIC_G723 = 0x0093,
        /// <summary>
        /// Aculab plc
        /// </summary>
        PROSODY_8KBPS = 0x0094,
        /// <summary>
        /// ZyXEL Communications, Inc.
        /// </summary>
        ZYXEaDPCM = 0x0097,
        /// <summary>
        /// Philips Speech Processing
        /// </summary>
        PHILIPS_LPCBB = 0x0098,
        /// <summary>
        /// Studer Professional Audio AG
        /// </summary>
        PACKED = 0x0099,
        /// <summary>
        /// Malden Electronics Ltd.
        /// </summary>
        MALDEN_PHONYTALK = 0x00A0,
        /// <summary>
        /// For Raw AAC, with format block AudioSpecificConfig() (as defined by MPEG-4), that follows WAVEFORMATEX
        /// </summary>
        RAW_AAC1 = 0x00FF,
        /// <summary>
        /// Rhetorex Inc.
        /// </summary>
        RHETOREX_ADPCM = 0x0100,
        /// <summary>
        /// BeCubed Software Inc.
        /// </summary>
        IRAT = 0x0101,
        /// <summary>
        /// Vivo Software
        /// </summary>
        VIVO_G723 = 0x0111,
        /// <summary>
        /// Vivo Software
        /// </summary>
        VIVO_SIREN = 0x0112,
        /// <summary>
        /// Digital Equipment Corporation
        /// </summary>
        DIGITAL_G723 = 0x0123,
        /// <summary>
        /// Sanyo Electric Co., Ltd.
        /// </summary>
        SANYO_LD_ADPCM = 0x0125,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_ACEPLNET = 0x0130,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_ACELP4800 = 0x0131,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_ACELP8V3 = 0x0132,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_G729 = 0x0133,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_G729A = 0x0134,
        /// <summary>
        /// Sipro Lab Telecom Inc.
        /// </summary>
        SIPROLAB_KELVIN = 0x0135,
        /// <summary>
        /// Dictaphone Corporation
        /// </summary>
        G726ADPCM = 0x0140,
        /// <summary>
        /// Qualcomm, Inc.
        /// </summary>
        QUALCOMM_PUREVOICE = 0x0150,
        /// <summary>
        /// Qualcomm, Inc.
        /// </summary>
        QUALCOMM_HALFRATE = 0x0151,
        /// <summary>
        /// Ring Zero Systems, Inc.
        /// </summary>
        TUBGSM = 0x0155,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MSAUDIO1 = 0x0160,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMAUDIO2 = 0x0161,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMAUDIO3 = 0x0162,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMAUDIO_LOSSLESS = 0x0163,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        WMASPDIF = 0x0164,
        /// <summary>
        /// Unisys Corp.
        /// </summary>
        UNISYS_NAP_ADPCM = 0x0170,
        /// <summary>
        /// Unisys Corp.
        /// </summary>
        UNISYS_NAP_ULAW = 0x0171,
        /// <summary>
        /// Unisys Corp.
        /// </summary>
        UNISYS_NAP_ALAW = 0x0172,
        /// <summary>
        /// Unisys Corp.
        /// </summary>
        UNISYS_NAP_16K = 0x0173,
        /// <summary>
        /// Creative Labs, Inc
        /// </summary>
        CREATIVE_ADPCM = 0x0200,
        /// <summary>
        /// Creative Labs, Inc
        /// </summary>
        CREATIVE_FASTSPEECH8 = 0x0202,
        /// <summary>
        /// Creative Labs, Inc
        /// </summary>
        CREATIVE_FASTSPEECH10 = 0x0203,
        /// <summary>
        /// UHER informatic GmbH
        /// </summary>
        UHER_ADPCM = 0x0210,
        /// <summary>
        /// Quarterdeck Corporation
        /// </summary>
        QUARTERDECK = 0x0220,
        /// <summary>
        /// I-link Worldwide
        /// </summary>
        ILINK_VC = 0x0230,
        /// <summary>
        /// Aureal Semiconductor
        /// </summary>
        RAW_SPORT = 0x0240,
        /// <summary>
        /// ESS Technology, Inc.
        /// </summary>
        ESST_AC3 = 0x0241,
        /// <summary>
        /// generic pass thru
        /// </summary>
        GENERIC_PASSTHRU = 0x0249,
        /// <summary>
        /// Interactive Products, Inc.
        /// </summary>
        IPI_HSX = 0x0250,
        /// <summary>
        /// Interactive Products, Inc.
        /// </summary>
        IPI_RPELP = 0x0251,
        /// <summary>
        /// Consistent Software
        /// </summary>
        CS2 = 0x0260,
        /// <summary>
        /// Sony Corp.
        /// </summary>
        SONY_SCX = 0x0270,
        /// <summary>
        /// Fujitsu Corp.
        /// </summary>
        FM_TOWNS_SND = 0x0300,
        /// <summary>
        /// Brooktree Corporation
        /// </summary>
        BTV_DIGITAL = 0x0400,
        /// <summary>
        /// QDesign Corporation
        /// </summary>
        QDESIGN_MUSIC = 0x0450,
        /// <summary>
        /// AT<![CDATA[&]]>T Labs, Inc.
        /// </summary>
        VME_VMPCM = 0x0680,
        /// <summary>
        /// AT<![CDATA[&]]>T Labs, Inc.
        /// </summary>
        TPC = 0x0681,
        /// <summary>
        /// Ing C. Olivetti <![CDATA[&]]> C., S.p.A.
        /// </summary>
        OLIGSM = 0x1000,
        /// <summary>
        /// Ing C. Olivetti <![CDATA[&]]> C., S.p.A.
        /// </summary>
        OLIADPCM = 0x1001,
        /// <summary>
        /// Ing C. Olivetti <![CDATA[&]]> C., S.p.A.
        /// </summary>
        OLICELP = 0x1002,
        /// <summary>
        /// Ing C. Olivetti <![CDATA[&]]> C., S.p.A.
        /// </summary>
        OLISBC = 0x1003,
        /// <summary>
        /// Ing C. Olivetti <![CDATA[&]]> C., S.p.A.
        /// </summary>
        OLIOPR = 0x1004,
        /// <summary>
        /// Lernout <![CDATA[&]]> Hauspie
        /// </summary>
        LH_CODEC = 0x1100,
        /// <summary>
        /// Norris Communications, Inc.
        /// </summary>
        NORRIS = 0x1400,
        /// <summary>
        /// AT<![CDATA[&]]>T Labs, Inc.
        /// </summary>
        SOUNDSPACE_MUSICOMPRESS = 0x1500,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MPEG_ADTS_AAC = 0x1600,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MPEG_RAW_AAC = 0x1601,
        /// <summary>
        /// Microsoft Corporation (MPEG-4 Audio Transport Streams (LOAS/LATM)
        /// </summary>
        MPEG_LOAS = 0x1602,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        NOKIA_MPEG_ADTS_AAC = 0x1608,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        NOKIA_MPEG_RAW_AAC = 0x1609,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        VODAFONE_MPEG_ADTS_AAC = 0x160A,
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        VODAFONE_MPEG_RAW_AAC = 0x160B,
        /// <summary>
        /// Microsoft Corporation (MPEG-2 AAC or MPEG-4 HE-AAC v1/v2 streams with any payload (ADTS, ADIF, LOAS/LATM, RAW). Format block includes MP4 AudioSpecificConfig() -- see HEAACWAVEFORMAT below
        /// </summary>
        MPEG_HEAAC = 0x1610,
        /// <summary>
        /// FAST Multimedia AG
        /// </summary>
        DVM = 0x2000,
        /// <summary>
        /// DTS 2
        /// </summary>
        DTS2 = 0x2001,

        /// <summary>
        /// Microsoft
        /// </summary>
        EXTENSIBLE = 0xFFFE,
        /// <summary>
        /// New wave format development should be based on the
        /// WAVEFORMATEXTENSIBLE structure. WAVEFORMATEXTENSIBLE allows you to
        /// avoid having to register a new format tag with Microsoft. However, if
        /// you must still define a new format tag, the WAVE_FORMAT_DEVELOPMENT
        /// format tag can be used during the development phase of a new wave
        /// format.  Before shipping, you MUST acquire an official format tag from
        /// Microsoft.
        /// </summary>
        DEVELOPMENT = (0xFFFF),
    }
}
