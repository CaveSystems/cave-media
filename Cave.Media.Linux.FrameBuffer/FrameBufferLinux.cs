// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Cave.IO;
using Cave.Media.Linux.FrameBuffer.Enums;
using Cave.Media.Linux.FrameBuffer.Structs;
using Cave.Media.Lyrics;

namespace Cave.Media.Linux;

public unsafe class FrameBufferLinux : FrameBufferBase
{
    #region Private Constructors

    FrameBufferLinux(string device, int handle, fb_fix_screeninfo fsi, fb_var_screeninfo vsi, IntPtr memory, FrameBufferFormat format)
    {
        Device = device;
        Handle = handle;
        FixScreenInfo = fsi;
        VarScreenInfo = vsi;
        Memory = memory;
        BackBuffer = Bitmap32.Loader.Create((int)vsi.xres, (int)vsi.yres);
    }

    #endregion Private Constructors

    #region Private Destructors

    ~FrameBufferLinux()
    {
        Dispose(false);
    }

    #endregion Private Destructors

    #region Private Methods

    void Draw16(ARGBImageData data, int x, int y)
    {
        var p = (byte*)data.Pixels1;
        var lineLength = FixScreenInfo.line_length / 2;
        var screen = (ushort*)Memory;
        screen += lineLength * y;
        for (var ry = 0; ry < data.Height; ry++)
        {
            var i = data.Stride * ry;
            for (var rx = 0; rx < data.Width; rx++)
            {
                /*ignore alpha byte*/
                var b = (p[i++] >> 3) & 0x1F;
                var g = (p[i++] << (5 - 2)) & 0x7E0;
                var r = (p[i++] << (11 - 3)) & 0xF800;
                i++;
                screen[rx + x] = (ushort)(r | g | b);
            }
            screen += lineLength;
        }
    }

    unsafe void Present16(ARGBImageData data)
    {
        var p = (byte*)data.Pixels1;
        var lineLength = FixScreenInfo.line_length / 2;
        var screen = (ushort*)Memory.ToPointer();
        for (var y = 0; y < data.Height; y++)
        {
            var i = data.Stride * y;
            for (var x = 0; x < data.Width; x++)
            {
                /*ignore alpha byte*/
                var b = (p[i++] >> 3) & 0x1F;
                var g = (p[i++] << (5 - 2)) & 0x7E0;
                var r = (p[i++] << (11 - 3)) & 0xF800;
                i++;
                screen[x] = (ushort)(r | g | b);
            }
            screen += lineLength;
        }
    }

    #endregion Private Methods

    #region Protected Fields

    protected readonly fb_fix_screeninfo FixScreenInfo;
    protected readonly int Handle;
    protected readonly IntPtr Memory;
    protected readonly fb_var_screeninfo VarScreenInfo;

    #endregion Protected Fields

    #region Public Properties

    public IBitmap32 BackBuffer { get; }

    /// <summary>Gets the bytes per pixel.</summary>
    /// <value>The bytes per pixel.</value>
    public int BytesPerPixel { get { return (int)VarScreenInfo.bits_per_pixel / 8; } }

    public string Device { get; }

    public int Height => (int)VarScreenInfo.yres;

    public int Width => (int)VarScreenInfo.xres;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Initializes a new instance of the <see cref="FrameBufferLinux"/> class.</summary>
    /// <exception cref="Exception">Could not open FrameBuffer Device! or Error sending ioctl to device! or Cannot access framebuffer mapped memory!</exception>
    public static FrameBufferLinux Create(string device)
    {
        Trace.TraceInformation("Initializing FrameBuffer...");
        var handle = libc.SafeNativeMethods.open(device, 2);
        if (handle == 0) throw new Exception("Could not open FrameBuffer Device!");

        fb_fix_screeninfo fixScreenInfo;

        #region get fb_fix_screeninfo

        {
            var size = Marshal.SizeOf(typeof(fb_fix_screeninfo));
            var ptr = Marshal.AllocHGlobal(size);
            var result = libc.SafeNativeMethods.ioctl(handle, (int)FBIO.GET_FSCREENINFO, ptr);
            if (result != 0) throw new Exception("Error sending ioctl to device!");
            fixScreenInfo = (fb_fix_screeninfo)Marshal.PtrToStructure(ptr, typeof(fb_fix_screeninfo));
            Marshal.FreeHGlobal(ptr);
        }

        #endregion get fb_fix_screeninfo

        fb_var_screeninfo varScreenInfo;

        #region get fb_var_screeninfo

        {
            var size = Marshal.SizeOf(typeof(fb_var_screeninfo));
            var ptr = Marshal.AllocHGlobal(size);
            var result = libc.SafeNativeMethods.ioctl(handle, (int)FBIO.GET_VSCREENINFO, ptr);
            if (result != 0) throw new Exception("Error sending ioctl to device!");
            varScreenInfo = (fb_var_screeninfo)Marshal.PtrToStructure(ptr, typeof(fb_var_screeninfo));
            Trace.TraceInformation($"Framebuffer {varScreenInfo.xres}x{varScreenInfo.yres} {varScreenInfo.bits_per_pixel}");
        }

        #endregion get fb_var_screeninfo

        IntPtr memory;

        #region set mmap

        {
            var memorySize = varScreenInfo.yres * fixScreenInfo.line_length;
            memory = libc.SafeNativeMethods.mmap(IntPtr.Zero, new UIntPtr(memorySize), libc.PROT.READ | libc.PROT.WRITE, libc.MAP.SHARED, handle, IntPtr.Zero);
            if (memory == IntPtr.Zero) throw new Exception("Cannot access framebuffer mapped memory!");
            Trace.TraceInformation($"Framebuffer Memory at {memory} #{memorySize:X}");
        }

        #endregion set mmap

        FrameBufferFormat format;

        #region get FrameBufferFormat

        switch (varScreenInfo.bits_per_pixel)
        {
            case 8: format = FrameBufferFormat.Rgb8; break;
            case 16: format = FrameBufferFormat.Rgb16; break;
            case 24: format = FrameBufferFormat.Rgb24; break;
            case 32: format = FrameBufferFormat.Rgb32; break;
            default: throw new NotSupportedException(string.Format("FrameBufferFormat {0} bytes not supported!", varScreenInfo.bits_per_pixel));
        }

        #endregion get FrameBufferFormat

        return new FrameBufferLinux(device, handle, fixScreenInfo, varScreenInfo, memory, format);
    }

    /// <summary>Draws the specified bitmap at the given position.</summary>
    /// <param name="img">The bitmap.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public unsafe void DrawDirect(Bitmap32 img, int x, int y)
    {
        var sourceData = img.GetImageData();
        if (sourceData.Stride < 0) throw new NotSupportedException("Negative stride is not supported!");
        if (x < -Width) return;
        if (y < -Height) return;
        if (x > Width) return;
        if (y > Height) return;

        if (VarScreenInfo.bits_per_pixel == 16)
        {
            Draw16(sourceData, x, y);
            return;
        }

        var targetStride = (int)FixScreenInfo.line_length;
        var copyStride = sourceData.Stride;
        var source = sourceData.Pixels1;
        var target = (int*)Memory;
        var height = sourceData.Height;
        target += ((targetStride * y) + x);
        if (x < 0)
        {
            source += x;
            copyStride -= x;
        }
        else if (x + sourceData.Width > Width)
        {
            var xdiff = x + sourceData.Width - Width;
            copyStride -= xdiff;
        }
        if (y < 0)
        {
            source += -y * sourceData.Stride;
        }
        else if (y + sourceData.Height > Height)
        {
            var ydiff = y + sourceData.Height - Height;
            height -= ydiff;
        }
        //copy scan lines
        for (var currentLine = 0; currentLine < height; currentLine++)
        {
            libc.SafeNativeMethods.memcpy(target, source, copyStride);
            target += targetStride;
            source += sourceData.Stride;
        }
    }

    /// <inheritdoc/>
    public override void Present()
    {
        Trace.TraceInformation("Present");

        var data = BackBuffer.GetImageData();
        //16 bit conversion
        if (VarScreenInfo.bits_per_pixel == 16)
        {
            Present16(data);
        }
        //slow stride mismatch copy
        else if (data.Stride != FixScreenInfo.line_length)
        {
            if (data.Stride < 0) throw new NotSupportedException("Negative stride is not yet supported!");
            Trace.TraceInformation("SimpleFB.Present", "Stride does not match, slow copy!");
            var block = Math.Min(data.Stride, (int)FixScreenInfo.line_length);
            var source = data.Pixels1;
            var target = (int*)Memory;
            for (var y = 0; y < VarScreenInfo.yres; y++)
            {
                source += data.Stride;
                target += FixScreenInfo.line_length;
                libc.SafeNativeMethods.memcpy(target, source, block);
            }
        }
        //default fast copy
        else
        {
            var screensize = (int)(VarScreenInfo.yres * FixScreenInfo.line_length);
            libc.SafeNativeMethods.memcpy((int*)Memory, data.Pixels1, screensize);
        }
    }

    #endregion Public Methods
}
