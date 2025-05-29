// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Structs;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
/// <summary>set tile characteristics</summary>
struct fb_tile_ops
{
    /// <summary>all dimensions from hereon are in terms of tiles move a rectangular region of tiles from one area to another</summary>
    /// <remarks>void (*fb_settile)(struct fb_info* info, struct fb_tilemap* map);</remarks>
    public fb_settile_delegate fb_settile;

    /// <summary>fill a rectangular region with a tile</summary>
    /// <remarks>void (*fb_tilecopy)(struct fb_info* info, struct fb_tilearea* area);</remarks>
    public fb_tilecopy_delegate fb_tilecopy;

    /// <summary>copy an array of tiles</summary>
    /// <remarks>void (*fb_tilefill)(struct fb_info* info, struct fb_tilerect* rect);</remarks>
    public fb_tilefill_delegate fb_tilefill;

    /// <summary>cursor</summary>
    /// <remarks>void (*fb_tileblit)(struct fb_info* info, struct fb_tileblit* blit);</remarks>
    public fb_tileblit_delegate fb_tileblit;

    /// <summary>get maximum length of the tile map</summary>
    /// <remarks>void (*fb_tilecursor)(struct fb_info* info, struct fb_tilecursor* cursor);</remarks>
    public fb_tilecursor_delegate fb_tilecursor;

    /// <remarks>int (*fb_get_tilemax)(struct fb_info* info);</remarks>
    public fb_get_tilemax_delegate fb_get_tilemax;
};

/// <summary>all dimensions from hereon are in terms of tiles move a rectangular region of tiles from one area to another</summary>
/// <remarks>void (*fb_settile)(struct fb_info* info, struct fb_tilemap* map);</remarks>
public unsafe delegate void fb_settile_delegate(fb_info* info, fb_tilemap* map);

/// <summary>cursor</summary>
/// <remarks>void (*fb_tileblit)(struct fb_info* info, struct fb_tileblit* blit);</remarks>
public unsafe delegate void fb_tileblit_delegate(fb_info* info, fb_tileblit* blit);

/// <summary>fill a rectangular region with a tile</summary>
/// <remarks>void (*fb_tilecopy)(struct fb_info* info, struct fb_tilearea* area);</remarks>
public unsafe delegate void fb_tilecopy_delegate(fb_info* info, fb_tilearea* area);

/// <summary>get maximum length of the tile map</summary>
/// <remarks>void (*fb_tilecursor)(struct fb_info* info, struct fb_tilecursor* cursor);</remarks>
public unsafe delegate void fb_tilecursor_delegate(fb_info* info, fb_tilecursor* cursor);

/// <summary>copy an array of tiles</summary>
/// <remarks>void (*fb_tilefill)(struct fb_info* info, struct fb_tilerect* rect);</remarks>
public unsafe delegate void fb_tilefill_delegate(fb_info* info, fb_tilerect* rect);

/// <summary></summary>
/// <remarks>int (*fb_get_tilemax)(struct fb_info* info);</remarks>
public unsafe delegate int fb_get_tilemax_delegate(fb_info* info);
