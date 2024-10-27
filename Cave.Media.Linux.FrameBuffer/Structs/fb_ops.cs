// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Structs;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct fb_ops
{
    /// <summary>open/release and usage marking</summary>
    /// <remarks>struct module* owner;</remarks>
    public UIntPtr owner;

    /// <remarks>int (*fb_open)(struct fb_info* info, int user);</remarks>
    public fb_open_delegate fb_open;

    /// <remarks>int (*fb_release)(struct fb_info* info, int user);</remarks>
    public fb_release_delegate fb_release;

    /// <summary>For framebuffers with strange non linear layouts or that do not work with normal memory mapped access</summary>
    /// <remarks>ssize_t(*fb_read)(struct fb_info* info, char __user* buf, size_t count, loff_t* ppos);</remarks>
    public fb_read_delegate fb_read;

    /// <remarks>ssize_t(*fb_write)(struct fb_info* info, const char __user* buf, size_t count, loff_t* ppos);</remarks>
    public fb_write_delegate fb_write;

    /// <summary>checks var and eventually tweaks it to something supported, DO NOT MODIFY PAR set the video mode according to info-&gt;var</summary>
    /// <remarks>int (*fb_check_var)(struct fb_var_screeninfo* var, struct fb_info* info);</remarks>
    public fb_check_var_delegate fb_check_var;

    /// <remarks>int (*fb_set_par)(struct fb_info* info);</remarks>
    public fb_set_par_delegate fb_set_par;

    /// <summary>set color register set color registers in batch</summary>
    /// <remarks>int (*fb_setcolreg)(unsigned regno, unsigned red, unsigned green, unsigned blue, unsigned transp, struct fb_info* info);</remarks>
    public fb_setcolreg_delegate fb_setcolreg;

    /// <remarks>int (*fb_setcmap)(struct fb_cmap* cmap, struct fb_info* info);</remarks>
    public fb_setcmap_delegate fb_setcmap;

    /// <summary>blank display pan display</summary>
    /// <remarks>int (*fb_blank)(int blank, struct fb_info* info);</remarks>
    public fb_blank_delegate fb_blank;

    /// <remarks>int (*fb_pan_display)(struct fb_var_screeninfo* var, struct fb_info* info);</remarks>
    public fb_pan_display_delegate fb_pan_display;

    /// <summary>Draws a rectangle Copy data from area to another</summary>
    /// <remarks>void (*fb_fillrect) (struct fb_info* info, const struct fb_fillrect* rect);</remarks>
    public fb_fillrect_delegate fb_fillrect;

    /// <remarks>void (*fb_copyarea) (struct fb_info* info, const struct fb_copyarea* region);</remarks>
    public fb_copyarea_delegate fb_copyarea;

    /// <summary>Draws a image to the display Draws cursor</summary>
    /// <remarks>void (*fb_imageblit) (struct fb_info* info, const struct fb_image* image);</remarks>
    public fb_imageblit_delegate fb_imageblit;

    /// <remarks>int (*fb_cursor) (struct fb_info* info, struct fb_cursor* cursor);</remarks>
    public fb_cursor_delegate fb_cursor;

    /// <summary>wait for blit idle, optional perform fb specific ioctl (optional)</summary>
    /// <remarks>int (*fb_sync)(struct fb_info* info);</remarks>
    public fb_sync_delegate fb_sync;

    /// <remarks>int (*fb_ioctl)(struct fb_info* info, unsigned int cmd, unsigned long arg);</remarks>
    public fb_ioctl_delegate fb_ioctl;

    /// <summary>Handle 32bit compat ioctl (optional) perform fb specific mmap</summary>
    /// <remarks>int (*fb_compat_ioctl)(struct fb_info* info, unsigned cmd, unsigned long arg);</remarks>
    public fb_compat_ioctl_delegate fb_compat_ioctl;

    /// <remarks>int (*fb_mmap)(struct fb_info* info, struct vm_area_struct* vma);</remarks>
    public fb_mmap_delegate fb_mmap;

    /// <summary>get capability given var teardown any resources to do with this framebuffer</summary>
    /// <remarks>void (*fb_get_caps)(struct fb_info* info, struct fb_blit_caps* caps, struct fb_var_screeninfo* var);</remarks>
    public fb_get_caps_delegate fb_get_caps;

    /// <remarks>void (*fb_destroy)(struct fb_info* info);</remarks>
    public fb_destroy_delegate fb_destroy;

    /// <summary>called at KDB enter and leave time to prepare the console</summary>
    /// <remarks>int (*fb_debug_enter)(struct fb_info* info);</remarks>
    public fb_debug_enter_delegate fb_debug_enter;

    /// <remarks>int (*fb_debug_leave)(struct fb_info* info);</remarks>
    public fb_debug_leave_delegate fb_debug_leave;
};

/// <summary>blank display pan display</summary>
/// <remarks>int (*fb_blank)(int blank, struct fb_info* info);</remarks>
public unsafe delegate nint fb_blank_delegate(int blank, fb_info* info);

/// <summary>checks var and eventually tweaks it to something supported, DO NOT MODIFY PAR set the video mode according to info-&gt;var</summary>
/// <remarks>int (*fb_check_var)(struct fb_var_screeninfo* var, struct fb_info* info);</remarks>
public unsafe delegate nint fb_check_var_delegate(fb_var_screeninfo* var, fb_info* info);

/// <summary>Handle 32bit compat ioctl (optional) perform fb specific mmap</summary>
/// <remarks>int (*fb_compat_ioctl)(struct fb_info* info, unsigned cmd, unsigned long arg);</remarks>
public unsafe delegate nint fb_compat_ioctl_delegate(fb_info* info, uint cmd, nuint arg);

/// <summary></summary>
/// <remarks>void (*fb_copyarea) (struct fb_info* info, const struct fb_copyarea* region);</remarks>
public unsafe delegate void fb_copyarea_delegate(fb_info* info, fb_copyarea* region);

/// <summary></summary>
/// <remarks>int (*fb_cursor) (struct fb_info* info, struct fb_cursor* cursor);</remarks>
public unsafe delegate nint fb_cursor_delegate(fb_info* info, fb_cursor* cursor);

/// <summary>called at KDB enter and leave time to prepare the console</summary>
/// <remarks>int (*fb_debug_enter)(struct fb_info* info);</remarks>
public unsafe delegate nint fb_debug_enter_delegate(fb_info* info);

/// <summary></summary>
/// <remarks>void (*fb_destroy)(struct fb_info* info);</remarks>
public unsafe delegate void fb_destroy_delegate(fb_info* info);

/// <summary>Draws a rectangle Copy data from area to another</summary>
/// <remarks>void (*fb_fillrect) (struct fb_info* info, const struct fb_fillrect* rect);</remarks>
public unsafe delegate void fb_fillrect_delegate(fb_info* info, fb_fillrect* rect);

/// <summary>get capability given var teardown any resources to do with this framebuffer</summary>
/// <remarks>void (*fb_get_caps)(struct fb_info* info, struct fb_blit_caps* caps, struct fb_var_screeninfo* var);</remarks>
public unsafe delegate void fb_get_caps_delegate(fb_info* info, fb_blit_caps* caps, fb_var_screeninfo* var);

/// <summary>Draws a image to the display Draws cursor</summary>
/// <remarks>void (*fb_imageblit) (struct fb_info* info, const struct fb_image* image);</remarks>
public unsafe delegate void fb_imageblit_delegate(fb_info* info, fb_image* image);

/// <summary></summary>
/// <remarks>int (*fb_ioctl)(struct fb_info* info, unsigned int cmd, unsigned long arg);</remarks>
public unsafe delegate nint fb_ioctl_delegate(fb_info* info, uint cmd, nuint arg);

/// <summary></summary>
/// <remarks>int (*fb_mmap)(struct fb_info* info, struct vm_area_struct* vma);</remarks>
public unsafe delegate nint fb_mmap_delegate(fb_info* info, nint vma);

/// <summary></summary>
/// <remarks>int (*fb_open)(struct fb_info* info, int user);</remarks>
public unsafe delegate nint fb_open_delegate(fb_info* info, int user);

/// <summary></summary>
/// <remarks>int (*fb_pan_display)(struct fb_var_screeninfo* var, struct fb_info* info);</remarks>
public unsafe delegate nint fb_pan_display_delegate(fb_var_screeninfo* var, fb_info* info);

/// <summary>For framebuffers with strange non linear layouts or that do not work with normal memory mapped access</summary>
/// <remarks>ssize_t(*fb_read)(struct fb_info* info, char __user* buf, size_t count, loff_t* ppos);</remarks>
public unsafe delegate nint fb_read_delegate(fb_info* info, byte* buf, nuint count, nuint ppos);

/// <summary></summary>
/// <remarks>int (*fb_release)(struct fb_info* info, int user);</remarks>
public unsafe delegate nint fb_release_delegate(fb_info* info, int user);

/// <summary></summary>
/// <remarks>int (*fb_set_par)(struct fb_info* info);</remarks>
public unsafe delegate nint fb_set_par_delegate(fb_info* info);

/// <summary></summary>
/// <remarks>int (*fb_setcmap)(struct fb_cmap* cmap, struct fb_info* info);</remarks>
public unsafe delegate nint fb_setcmap_delegate(fb_cmap* cmap, fb_info* info);

/// <summary>set color register set color registers in batch</summary>
/// <remarks>int (*fb_setcolreg)(unsigned regno, unsigned red, unsigned green, unsigned blue, unsigned transp, struct fb_info* info);</remarks>
public unsafe delegate nint fb_setcolreg_delegate(uint regno, uint red, uint green, uint blue, uint transp, fb_info* info);

/// <summary>wait for blit idle, optional perform fb specific ioctl (optional)</summary>
/// <remarks>int (*fb_sync)(struct fb_info* info);</remarks>
public unsafe delegate nint fb_sync_delegate(fb_info* info);

/// <summary></summary>
/// <remarks>ssize_t(*fb_write)(struct fb_info* info, const char __user* buf, size_t count, loff_t* ppos);</remarks>
public unsafe delegate nint fb_write_delegate(fb_info* info, byte* buf, nuint count, nuint ppos);

/// <summary></summary>
/// <remarks>int (*fb_debug_leave)(struct fb_info* info);</remarks>
public unsafe delegate nint fb_debug_leave_delegate(fb_info* info);
