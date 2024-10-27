// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_ACCEL
{
    /// <summary>no hardware accelerator</summary>
    NONE = 0,

    /// <summary>Atari Blitter</summary>
    ATARIBLITT = 1,

    /// <summary>Amiga Blitter</summary>
    AMIGABLITT = 2,

    /// <summary>Cybervision64 (S3 Trio64)</summary>
    S3_TRIO64 = 3,

    /// <summary>RetinaZ3 (NCR 77C32BLT)</summary>
    NCR_77C32BLT = 4,

    /// <summary>Cybervision64/3D (S3 ViRGE)</summary>
    S3_VIRGE = 5,

    /// <summary>ATI Mach 64GX family</summary>
    ATI_MACH64GX = 6,

    /// <summary>DEC 21030 TGA</summary>
    DEC_TGA = 7,

    /// <summary>ATI Mach 64CT family</summary>
    ATI_MACH64CT = 8,

    /// <summary>ATI Mach 64CT family VT class</summary>
    ATI_MACH64VT = 9,

    /// <summary>ATI Mach 64CT family GT class</summary>
    ATI_MACH64GT = 10,

    /// <summary>Sun Creator/Creator3D</summary>
    SUN_CREATOR = 11,

    /// <summary>Sun cg6</summary>
    SUN_CGSIX = 12,

    /// <summary>Sun leo/zx</summary>
    SUN_LEO = 13,

    /// <summary>IMS Twin Turbo</summary>
    IMS_TWINTURBO = 14,

    /// <summary>3Dlabs Permedia 2</summary>
    _3DLABS_PERMEDIA2 = 15,

    /// <summary>Matrox MGA2064W (Millenium)</summary>
    MATROX_MGA2064W = 16,

    /// <summary>Matrox MGA1064SG (Mystique)</summary>
    MATROX_MGA1064SG = 17,

    /// <summary>Matrox MGA2164W (Millenium II)</summary>
    MATROX_MGA2164W = 18,

    /// <summary>Matrox MGA2164W (Millenium II)</summary>
    MATROX_MGA2164W_AGP = 19,

    /// <summary>Matrox G100 (Productiva G100)</summary>
    MATROX_MGAG100 = 20,

    /// <summary>Matrox G200 (Myst, Mill, ...)</summary>
    MATROX_MGAG200 = 21,

    /// <summary>Sun cgfourteen</summary>
    SUN_CG14 = 22,

    /// <summary>Sun bwtwo</summary>
    SUN_BWTWO = 23,

    /// <summary>Sun cgthree</summary>
    SUN_CGTHREE = 24,

    /// <summary>Sun tcx</summary>
    SUN_TCX = 25,

    /// <summary>Matrox G400</summary>
    MATROX_MGAG400 = 26,

    /// <summary>nVidia RIVA 128</summary>
    NV3 = 27,

    /// <summary>nVidia RIVA TNT</summary>
    NV4 = 28,

    /// <summary>nVidia RIVA TNT2</summary>
    NV5 = 29,

    /// <summary>C&amp;T 6555x</summary>
    CT_6555x = 30,

    /// <summary>3Dfx Banshee</summary>
    _3DFX_BANSHEE = 31,

    /// <summary>ATI Rage128 family</summary>
    ATI_RAGE128 = 32,

    /// <summary>CyberPro 2000</summary>
    IGS_CYBER2000 = 33,

    /// <summary>CyberPro 2010</summary>
    IGS_CYBER2010 = 34,

    /// <summary>CyberPro 5000</summary>
    IGS_CYBER5000 = 35,

    /// <summary>SiS 300/630/540</summary>
    SIS_GLAMOUR = 36,

    /// <summary>3Dlabs Permedia 3</summary>
    _3DLABS_PERMEDIA3 = 37,

    /// <summary>ATI Radeon family</summary>
    ATI_RADEON = 38,

    /// <summary>Intel 810/815</summary>
    I810 = 39,

    /// <summary>SiS 315, 650, 740</summary>
    SIS_GLAMOUR_2 = 40,

    /// <summary>SiS 330 ("Xabre")</summary>
    SIS_XABRE = 41,

    /// <summary>Intel 830M/845G/85x/865G</summary>
    I830 = 42,

    /// <summary>nVidia Arch 10</summary>
    NV_10 = 43,

    /// <summary>nVidia Arch 20</summary>
    NV_20 = 44,

    /// <summary>nVidia Arch 30</summary>
    NV_30 = 45,

    /// <summary>nVidia Arch 40</summary>
    NV_40 = 46,

    /// <summary>XGI Volari V3XT, V5, V8</summary>
    XGI_VOLARI_V = 47,

    /// <summary>XGI Volari Z7</summary>
    XGI_VOLARI_Z = 48,

    /// <summary>TI OMAP16xx</summary>
    OMAP1610 = 49,

    /// <summary>Trident TGUI</summary>
    TRIDENT_TGUI = 50,

    /// <summary>Trident 3DImage</summary>
    TRIDENT_3DIMAGE = 51,

    /// <summary>Trident Blade3D</summary>
    TRIDENT_BLADE3D = 52,

    /// <summary>Trident BladeXP</summary>
    TRIDENT_BLADEXP = 53,

    /// <summary>Cirrus Logic 543x/544x/5480</summary>
    CIRRUS_ALPINE = 53,

    /// <summary>NeoMagic NM2070</summary>
    NEOMAGIC_NM2070 = 90,

    /// <summary>NeoMagic NM2090</summary>
    NEOMAGIC_NM2090 = 91,

    /// <summary>NeoMagic NM2093</summary>
    NEOMAGIC_NM2093 = 92,

    /// <summary>NeoMagic NM2097</summary>
    NEOMAGIC_NM2097 = 93,

    /// <summary>NeoMagic NM2160</summary>
    NEOMAGIC_NM2160 = 94,

    /// <summary>NeoMagic NM2200</summary>
    NEOMAGIC_NM2200 = 95,

    /// <summary>NeoMagic NM2230</summary>
    NEOMAGIC_NM2230 = 96,

    /// <summary>NeoMagic NM2360</summary>
    NEOMAGIC_NM2360 = 97,

    /// <summary>NeoMagic NM2380</summary>
    NEOMAGIC_NM2380 = 98,

    /// <summary>PXA3xx</summary>
    PXA3XX = 99,

    /// <summary>S3 Savage4</summary>
    SAVAGE4 = 0x80,

    /// <summary>S3 Savage3D</summary>
    SAVAGE3D = 0x81,

    /// <summary>S3 Savage3D-MV</summary>
    SAVAGE3D_MV = 0x82,

    /// <summary>S3 Savage2000</summary>
    SAVAGE2000 = 0x83,

    /// <summary>S3 Savage/MX-MV</summary>
    SAVAGE_MX_MV = 0x84,

    /// <summary>S3 Savage/MX</summary>
    SAVAGE_MX = 0x85,

    /// <summary>S3 Savage/IX-MV</summary>
    SAVAGE_IX_MV = 0x86,

    /// <summary>S3 Savage/IX</summary>
    SAVAGE_IX = 0x87,

    /// <summary>S3 ProSavage PM133</summary>
    PROSAVAGE_PM = 0x88,

    /// <summary>S3 ProSavage KM133</summary>
    PROSAVAGE_KM = 0x89,

    /// <summary>S3 Twister</summary>
    S3TWISTER_P = 0x8a,

    /// <summary>S3 TwisterK</summary>
    S3TWISTER_K = 0x8b,

    /// <summary>S3 Supersavage</summary>
    SUPERSAVAGE = 0x8c,

    /// <summary>S3 ProSavage DDR</summary>
    PROSAVAGE_DDR = 0x8d,

    /// <summary>S3 ProSavage DDR-K</summary>
    PROSAVAGE_DDRK = 0x8e,

    /// <summary>PKUnity-v3 Unigfx</summary>
    PUV3_UNIGFX = 0xa0,
}
