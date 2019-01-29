namespace Cave.Media.OpenGL
{
    using System;
    using System.Runtime.InteropServices;
	using System.Security;

	partial class gl2
	{
        internal static partial class Imports
        {
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNewList", ExactSpelling = true)]
            internal extern static void NewList(UInt32 list, int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEndList", ExactSpelling = true)]
            internal extern static void EndList();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCallList", ExactSpelling = true)]
            internal extern static void CallList(UInt32 list);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCallLists", ExactSpelling = true)]
            internal extern static void CallLists(Int32 n, int type, IntPtr lists);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteLists", ExactSpelling = true)]
            internal extern static void DeleteLists(UInt32 list, Int32 range);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGenLists", ExactSpelling = true)]
            internal extern static Int32 GenLists(Int32 range);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glListBase", ExactSpelling = true)]
            internal extern static void ListBase(UInt32 @base);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBegin", ExactSpelling = true)]
            internal extern static void Begin(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBitmap", ExactSpelling = true)]
            internal extern static unsafe void Bitmap(Int32 width, Int32 height, Single xorig, Single yorig, Single xmove, Single ymove, Byte* bitmap);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3b", ExactSpelling = true)]
            internal extern static void Color3b(SByte red, SByte green, SByte blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3bv", ExactSpelling = true)]
            internal extern static unsafe void Color3bv(SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3d", ExactSpelling = true)]
            internal extern static void Color3d(Double red, Double green, Double blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3dv", ExactSpelling = true)]
            internal extern static unsafe void Color3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3f", ExactSpelling = true)]
            internal extern static void Color3f(Single red, Single green, Single blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3fv", ExactSpelling = true)]
            internal extern static unsafe void Color3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3i", ExactSpelling = true)]
            internal extern static void Color3i(Int32 red, Int32 green, Int32 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3iv", ExactSpelling = true)]
            internal extern static unsafe void Color3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3s", ExactSpelling = true)]
            internal extern static void Color3s(Int16 red, Int16 green, Int16 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3sv", ExactSpelling = true)]
            internal extern static unsafe void Color3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3ub", ExactSpelling = true)]
            internal extern static void Color3ub(Byte red, Byte green, Byte blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3ubv", ExactSpelling = true)]
            internal extern static unsafe void Color3ubv(Byte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3ui", ExactSpelling = true)]
            internal extern static void Color3ui(UInt32 red, UInt32 green, UInt32 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3uiv", ExactSpelling = true)]
            internal extern static unsafe void Color3uiv(UInt32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3us", ExactSpelling = true)]
            internal extern static void Color3us(UInt16 red, UInt16 green, UInt16 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor3usv", ExactSpelling = true)]
            internal extern static unsafe void Color3usv(UInt16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4b", ExactSpelling = true)]
            internal extern static void Color4b(SByte red, SByte green, SByte blue, SByte alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4bv", ExactSpelling = true)]
            internal extern static unsafe void Color4bv(SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4d", ExactSpelling = true)]
            internal extern static void Color4d(Double red, Double green, Double blue, Double alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4dv", ExactSpelling = true)]
            internal extern static unsafe void Color4dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4f", ExactSpelling = true)]
            internal extern static void Color4f(Single red, Single green, Single blue, Single alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4fv", ExactSpelling = true)]
            internal extern static unsafe void Color4fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4i", ExactSpelling = true)]
            internal extern static void Color4i(Int32 red, Int32 green, Int32 blue, Int32 alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4iv", ExactSpelling = true)]
            internal extern static unsafe void Color4iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4s", ExactSpelling = true)]
            internal extern static void Color4s(Int16 red, Int16 green, Int16 blue, Int16 alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4sv", ExactSpelling = true)]
            internal extern static unsafe void Color4sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4ub", ExactSpelling = true)]
            internal extern static void Color4ub(Byte red, Byte green, Byte blue, Byte alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4ubv", ExactSpelling = true)]
            internal extern static unsafe void Color4ubv(Byte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4ui", ExactSpelling = true)]
            internal extern static void Color4ui(UInt32 red, UInt32 green, UInt32 blue, UInt32 alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4uiv", ExactSpelling = true)]
            internal extern static unsafe void Color4uiv(UInt32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4us", ExactSpelling = true)]
            internal extern static void Color4us(UInt16 red, UInt16 green, UInt16 blue, UInt16 alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColor4usv", ExactSpelling = true)]
            internal extern static unsafe void Color4usv(UInt16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEdgeFlag", ExactSpelling = true)]
            internal extern static void EdgeFlag(Int32 flag);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEdgeFlagv", ExactSpelling = true)]
            internal extern static unsafe void EdgeFlagv(Int32* flag);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEnd", ExactSpelling = true)]
            internal extern static void End();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexd", ExactSpelling = true)]
            internal extern static void Indexd(Double c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexdv", ExactSpelling = true)]
            internal extern static unsafe void Indexdv(Double* c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexf", ExactSpelling = true)]
            internal extern static void Indexf(Single c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexfv", ExactSpelling = true)]
            internal extern static unsafe void Indexfv(Single* c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexi", ExactSpelling = true)]
            internal extern static void Indexi(Int32 c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexiv", ExactSpelling = true)]
            internal extern static unsafe void Indexiv(Int32* c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexs", ExactSpelling = true)]
            internal extern static void Indexs(Int16 c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexsv", ExactSpelling = true)]
            internal extern static unsafe void Indexsv(Int16* c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3b", ExactSpelling = true)]
            internal extern static void Normal3b(SByte nx, SByte ny, SByte nz);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3bv", ExactSpelling = true)]
            internal extern static unsafe void Normal3bv(SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3d", ExactSpelling = true)]
            internal extern static void Normal3d(Double nx, Double ny, Double nz);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3dv", ExactSpelling = true)]
            internal extern static unsafe void Normal3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3f", ExactSpelling = true)]
            internal extern static void Normal3f(Single nx, Single ny, Single nz);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3fv", ExactSpelling = true)]
            internal extern static unsafe void Normal3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3i", ExactSpelling = true)]
            internal extern static void Normal3i(Int32 nx, Int32 ny, Int32 nz);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3iv", ExactSpelling = true)]
            internal extern static unsafe void Normal3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3s", ExactSpelling = true)]
            internal extern static void Normal3s(Int16 nx, Int16 ny, Int16 nz);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormal3sv", ExactSpelling = true)]
            internal extern static unsafe void Normal3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2d", ExactSpelling = true)]
            internal extern static void RasterPos2d(Double x, Double y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2dv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos2dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2f", ExactSpelling = true)]
            internal extern static void RasterPos2f(Single x, Single y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2fv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos2fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2i", ExactSpelling = true)]
            internal extern static void RasterPos2i(Int32 x, Int32 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2iv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos2iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2s", ExactSpelling = true)]
            internal extern static void RasterPos2s(Int16 x, Int16 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos2sv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos2sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3d", ExactSpelling = true)]
            internal extern static void RasterPos3d(Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3dv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3f", ExactSpelling = true)]
            internal extern static void RasterPos3f(Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3fv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3i", ExactSpelling = true)]
            internal extern static void RasterPos3i(Int32 x, Int32 y, Int32 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3iv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3s", ExactSpelling = true)]
            internal extern static void RasterPos3s(Int16 x, Int16 y, Int16 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos3sv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4d", ExactSpelling = true)]
            internal extern static void RasterPos4d(Double x, Double y, Double z, Double w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4dv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos4dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4f", ExactSpelling = true)]
            internal extern static void RasterPos4f(Single x, Single y, Single z, Single w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4fv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos4fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4i", ExactSpelling = true)]
            internal extern static void RasterPos4i(Int32 x, Int32 y, Int32 z, Int32 w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4iv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos4iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4s", ExactSpelling = true)]
            internal extern static void RasterPos4s(Int16 x, Int16 y, Int16 z, Int16 w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRasterPos4sv", ExactSpelling = true)]
            internal extern static unsafe void RasterPos4sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectd", ExactSpelling = true)]
            internal extern static void Rectd(Double x1, Double y1, Double x2, Double y2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectdv", ExactSpelling = true)]
            internal extern static unsafe void Rectdv(Double* v1, Double* v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectf", ExactSpelling = true)]
            internal extern static void Rectf(Single x1, Single y1, Single x2, Single y2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectfv", ExactSpelling = true)]
            internal extern static unsafe void Rectfv(Single* v1, Single* v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRecti", ExactSpelling = true)]
            internal extern static void Recti(Int32 x1, Int32 y1, Int32 x2, Int32 y2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectiv", ExactSpelling = true)]
            internal extern static unsafe void Rectiv(Int32* v1, Int32* v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRects", ExactSpelling = true)]
            internal extern static void Rects(Int16 x1, Int16 y1, Int16 x2, Int16 y2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRectsv", ExactSpelling = true)]
            internal extern static unsafe void Rectsv(Int16* v1, Int16* v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1d", ExactSpelling = true)]
            internal extern static void TexCoord1d(Double s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1dv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord1dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1f", ExactSpelling = true)]
            internal extern static void TexCoord1f(Single s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1fv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord1fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1i", ExactSpelling = true)]
            internal extern static void TexCoord1i(Int32 s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1iv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord1iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1s", ExactSpelling = true)]
            internal extern static void TexCoord1s(Int16 s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord1sv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord1sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2d", ExactSpelling = true)]
            internal extern static void TexCoord2d(Double s, Double t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2dv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord2dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2f", ExactSpelling = true)]
            internal extern static void TexCoord2f(Single s, Single t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2fv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord2fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2i", ExactSpelling = true)]
            internal extern static void TexCoord2i(Int32 s, Int32 t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2iv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord2iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2s", ExactSpelling = true)]
            internal extern static void TexCoord2s(Int16 s, Int16 t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord2sv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord2sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3d", ExactSpelling = true)]
            internal extern static void TexCoord3d(Double s, Double t, Double r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3dv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3f", ExactSpelling = true)]
            internal extern static void TexCoord3f(Single s, Single t, Single r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3fv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3i", ExactSpelling = true)]
            internal extern static void TexCoord3i(Int32 s, Int32 t, Int32 r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3iv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3s", ExactSpelling = true)]
            internal extern static void TexCoord3s(Int16 s, Int16 t, Int16 r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord3sv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4d", ExactSpelling = true)]
            internal extern static void TexCoord4d(Double s, Double t, Double r, Double q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4dv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord4dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4f", ExactSpelling = true)]
            internal extern static void TexCoord4f(Single s, Single t, Single r, Single q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4fv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord4fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4i", ExactSpelling = true)]
            internal extern static void TexCoord4i(Int32 s, Int32 t, Int32 r, Int32 q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4iv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord4iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4s", ExactSpelling = true)]
            internal extern static void TexCoord4s(Int16 s, Int16 t, Int16 r, Int16 q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoord4sv", ExactSpelling = true)]
            internal extern static unsafe void TexCoord4sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2d", ExactSpelling = true)]
            internal extern static void Vertex2d(Double x, Double y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2dv", ExactSpelling = true)]
            internal extern static unsafe void Vertex2dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2f", ExactSpelling = true)]
            internal extern static void Vertex2f(Single x, Single y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2fv", ExactSpelling = true)]
            internal extern static unsafe void Vertex2fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2i", ExactSpelling = true)]
            internal extern static void Vertex2i(Int32 x, Int32 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2iv", ExactSpelling = true)]
            internal extern static unsafe void Vertex2iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2s", ExactSpelling = true)]
            internal extern static void Vertex2s(Int16 x, Int16 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex2sv", ExactSpelling = true)]
            internal extern static unsafe void Vertex2sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3d", ExactSpelling = true)]
            internal extern static void Vertex3d(Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3dv", ExactSpelling = true)]
            internal extern static unsafe void Vertex3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3f", ExactSpelling = true)]
            internal extern static void Vertex3f(Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3fv", ExactSpelling = true)]
            internal extern static unsafe void Vertex3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3i", ExactSpelling = true)]
            internal extern static void Vertex3i(Int32 x, Int32 y, Int32 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3iv", ExactSpelling = true)]
            internal extern static unsafe void Vertex3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3s", ExactSpelling = true)]
            internal extern static void Vertex3s(Int16 x, Int16 y, Int16 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex3sv", ExactSpelling = true)]
            internal extern static unsafe void Vertex3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4d", ExactSpelling = true)]
            internal extern static void Vertex4d(Double x, Double y, Double z, Double w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4dv", ExactSpelling = true)]
            internal extern static unsafe void Vertex4dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4f", ExactSpelling = true)]
            internal extern static void Vertex4f(Single x, Single y, Single z, Single w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4fv", ExactSpelling = true)]
            internal extern static unsafe void Vertex4fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4i", ExactSpelling = true)]
            internal extern static void Vertex4i(Int32 x, Int32 y, Int32 z, Int32 w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4iv", ExactSpelling = true)]
            internal extern static unsafe void Vertex4iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4s", ExactSpelling = true)]
            internal extern static void Vertex4s(Int16 x, Int16 y, Int16 z, Int16 w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertex4sv", ExactSpelling = true)]
            internal extern static unsafe void Vertex4sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClipPlane", ExactSpelling = true)]
            internal extern static unsafe void ClipPlane(int plane, Double* equation);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorMaterial", ExactSpelling = true)]
            internal extern static void ColorMaterial(int face, int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCullFace", ExactSpelling = true)]
            internal extern static void CullFace(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogf", ExactSpelling = true)]
            internal extern static void Fogf(int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogfv", ExactSpelling = true)]
            internal extern static unsafe void Fogfv(int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogi", ExactSpelling = true)]
            internal extern static void Fogi(int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogiv", ExactSpelling = true)]
            internal extern static unsafe void Fogiv(int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFrontFace", ExactSpelling = true)]
            internal extern static void FrontFace(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glHint", ExactSpelling = true)]
            internal extern static void Hint(int target, int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightf", ExactSpelling = true)]
            internal extern static void Lightf(int light, int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightfv", ExactSpelling = true)]
            internal extern static unsafe void Lightfv(int light, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLighti", ExactSpelling = true)]
            internal extern static void Lighti(int light, int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightiv", ExactSpelling = true)]
            internal extern static unsafe void Lightiv(int light, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightModelf", ExactSpelling = true)]
            internal extern static void LightModelf(int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightModelfv", ExactSpelling = true)]
            internal extern static unsafe void LightModelfv(int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightModeli", ExactSpelling = true)]
            internal extern static void LightModeli(int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLightModeliv", ExactSpelling = true)]
            internal extern static unsafe void LightModeliv(int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLineStipple", ExactSpelling = true)]
            internal extern static void LineStipple(Int32 factor, UInt16 pattern);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLineWidth", ExactSpelling = true)]
            internal extern static void LineWidth(Single width);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMaterialf", ExactSpelling = true)]
            internal extern static void Materialf(int face, int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMaterialfv", ExactSpelling = true)]
            internal extern static unsafe void Materialfv(int face, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMateriali", ExactSpelling = true)]
            internal extern static void Materiali(int face, int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMaterialiv", ExactSpelling = true)]
            internal extern static unsafe void Materialiv(int face, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPointSize", ExactSpelling = true)]
            internal extern static void PointSize(Single size);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPolygonMode", ExactSpelling = true)]
            internal extern static void PolygonMode(int face, int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPolygonStipple", ExactSpelling = true)]
            internal extern static unsafe void PolygonStipple(Byte* mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glScissor", ExactSpelling = true)]
            internal extern static void Scissor(Int32 x, Int32 y, Int32 width, Int32 height);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glShadeModel", ExactSpelling = true)]
            internal extern static void ShadeModel(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexParameterf", ExactSpelling = true)]
            internal extern static void TexParameterf(int target, int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexParameterfv", ExactSpelling = true)]
            internal extern static unsafe void TexParameterfv(int target, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexParameteri", ExactSpelling = true)]
            internal extern static void TexParameteri(int target, int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexParameteriv", ExactSpelling = true)]
            internal extern static unsafe void TexParameteriv(int target, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexImage1D", ExactSpelling = true)]
            internal extern static void TexImage1D(int target, Int32 level, int internalformat, Int32 width, Int32 border, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexImage2D", ExactSpelling = true)]
            internal extern static void TexImage2D(int target, Int32 level, int internalformat, Int32 width, Int32 height, Int32 border, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexEnvf", ExactSpelling = true)]
            internal extern static void TexEnvf(int target, int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexEnvfv", ExactSpelling = true)]
            internal extern static unsafe void TexEnvfv(int target, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexEnvi", ExactSpelling = true)]
            internal extern static void TexEnvi(int target, int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexEnviv", ExactSpelling = true)]
            internal extern static unsafe void TexEnviv(int target, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGend", ExactSpelling = true)]
            internal extern static void TexGend(int coord, int pname, Double param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGendv", ExactSpelling = true)]
            internal extern static unsafe void TexGendv(int coord, int pname, Double* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGenf", ExactSpelling = true)]
            internal extern static void TexGenf(int coord, int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGenfv", ExactSpelling = true)]
            internal extern static unsafe void TexGenfv(int coord, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGeni", ExactSpelling = true)]
            internal extern static void TexGeni(int coord, int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexGeniv", ExactSpelling = true)]
            internal extern static unsafe void TexGeniv(int coord, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFeedbackBuffer", ExactSpelling = true)]
            internal extern static unsafe void FeedbackBuffer(Int32 size, int type, [Out] Single* buffer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSelectBuffer", ExactSpelling = true)]
            internal extern static unsafe void SelectBuffer(Int32 size, [Out] UInt32* buffer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRenderMode", ExactSpelling = true)]
            internal extern static Int32 RenderMode(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glInitNames", ExactSpelling = true)]
            internal extern static void InitNames();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadName", ExactSpelling = true)]
            internal extern static void LoadName(UInt32 name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPassThrough", ExactSpelling = true)]
            internal extern static void PassThrough(Single token);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPopName", ExactSpelling = true)]
            internal extern static void PopName();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPushName", ExactSpelling = true)]
            internal extern static void PushName(UInt32 name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawBuffer", ExactSpelling = true)]
            internal extern static void DrawBuffer(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClear", ExactSpelling = true)]
            internal extern static void Clear(int mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClearAccum", ExactSpelling = true)]
            internal extern static void ClearAccum(Single red, Single green, Single blue, Single alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClearIndex", ExactSpelling = true)]
            internal extern static void ClearIndex(Single c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClearColor", ExactSpelling = true)]
            internal extern static void ClearColor(Single red, Single green, Single blue, Single alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClearStencil", ExactSpelling = true)]
            internal extern static void ClearStencil(Int32 s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClearDepth", ExactSpelling = true)]
            internal extern static void ClearDepth(Double depth);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilMask", ExactSpelling = true)]
            internal extern static void StencilMask(UInt32 mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorMask", ExactSpelling = true)]
            internal extern static void ColorMask(Int32 red, Int32 green, Int32 blue, Int32 alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDepthMask", ExactSpelling = true)]
            internal extern static void DepthMask(Int32 flag);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexMask", ExactSpelling = true)]
            internal extern static void IndexMask(UInt32 mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glAccum", ExactSpelling = true)]
            internal extern static void Accum(int op, Single value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDisable", ExactSpelling = true)]
            internal extern static void Disable(int cap);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEnable", ExactSpelling = true)]
            internal extern static void Enable(int cap);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFinish", ExactSpelling = true)]
            internal extern static void Finish();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFlush", ExactSpelling = true)]
            internal extern static void Flush();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPopAttrib", ExactSpelling = true)]
            internal extern static void PopAttrib();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPushAttrib", ExactSpelling = true)]
            internal extern static void PushAttrib(int mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMap1d", ExactSpelling = true)]
            internal extern static unsafe void Map1d(int target, Double u1, Double u2, Int32 stride, Int32 order, Double* points);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMap1f", ExactSpelling = true)]
            internal extern static unsafe void Map1f(int target, Single u1, Single u2, Int32 stride, Int32 order, Single* points);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMap2d", ExactSpelling = true)]
            internal extern static unsafe void Map2d(int target, Double u1, Double u2, Int32 ustride, Int32 uorder, Double v1, Double v2, Int32 vstride, Int32 vorder, Double* points);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMap2f", ExactSpelling = true)]
            internal extern static unsafe void Map2f(int target, Single u1, Single u2, Int32 ustride, Int32 uorder, Single v1, Single v2, Int32 vstride, Int32 vorder, Single* points);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMapGrid1d", ExactSpelling = true)]
            internal extern static void MapGrid1d(Int32 un, Double u1, Double u2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMapGrid1f", ExactSpelling = true)]
            internal extern static void MapGrid1f(Int32 un, Single u1, Single u2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMapGrid2d", ExactSpelling = true)]
            internal extern static void MapGrid2d(Int32 un, Double u1, Double u2, Int32 vn, Double v1, Double v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMapGrid2f", ExactSpelling = true)]
            internal extern static void MapGrid2f(Int32 un, Single u1, Single u2, Int32 vn, Single v1, Single v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord1d", ExactSpelling = true)]
            internal extern static void EvalCoord1d(Double u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord1dv", ExactSpelling = true)]
            internal extern static unsafe void EvalCoord1dv(Double* u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord1f", ExactSpelling = true)]
            internal extern static void EvalCoord1f(Single u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord1fv", ExactSpelling = true)]
            internal extern static unsafe void EvalCoord1fv(Single* u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord2d", ExactSpelling = true)]
            internal extern static void EvalCoord2d(Double u, Double v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord2dv", ExactSpelling = true)]
            internal extern static unsafe void EvalCoord2dv(Double* u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord2f", ExactSpelling = true)]
            internal extern static void EvalCoord2f(Single u, Single v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalCoord2fv", ExactSpelling = true)]
            internal extern static unsafe void EvalCoord2fv(Single* u);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalMesh1", ExactSpelling = true)]
            internal extern static void EvalMesh1(int mode, Int32 i1, Int32 i2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalPoint1", ExactSpelling = true)]
            internal extern static void EvalPoint1(Int32 i);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalMesh2", ExactSpelling = true)]
            internal extern static void EvalMesh2(int mode, Int32 i1, Int32 i2, Int32 j1, Int32 j2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEvalPoint2", ExactSpelling = true)]
            internal extern static void EvalPoint2(Int32 i, Int32 j);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glAlphaFunc", ExactSpelling = true)]
            internal extern static void AlphaFunc(int func, Single @ref);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBlendFunc", ExactSpelling = true)]
            internal extern static void BlendFunc(int sfactor, int dfactor);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLogicOp", ExactSpelling = true)]
            internal extern static void LogicOp(int opcode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilFunc", ExactSpelling = true)]
            internal extern static void StencilFunc(int func, Int32 @ref, UInt32 mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilOp", ExactSpelling = true)]
            internal extern static void StencilOp(int fail, int zfail, int zpass);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDepthFunc", ExactSpelling = true)]
            internal extern static void DepthFunc(int func);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelZoom", ExactSpelling = true)]
            internal extern static void PixelZoom(Single xfactor, Single yfactor);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelTransferf", ExactSpelling = true)]
            internal extern static void PixelTransferf(int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelTransferi", ExactSpelling = true)]
            internal extern static void PixelTransferi(int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelStoref", ExactSpelling = true)]
            internal extern static void PixelStoref(int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelStorei", ExactSpelling = true)]
            internal extern static void PixelStorei(int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelMapfv", ExactSpelling = true)]
            internal extern static unsafe void PixelMapfv(int map, Int32 mapsize, Single* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelMapuiv", ExactSpelling = true)]
            internal extern static unsafe void PixelMapuiv(int map, Int32 mapsize, UInt32* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPixelMapusv", ExactSpelling = true)]
            internal extern static unsafe void PixelMapusv(int map, Int32 mapsize, UInt16* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glReadBuffer", ExactSpelling = true)]
            internal extern static void ReadBuffer(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyPixels", ExactSpelling = true)]
            internal extern static void CopyPixels(Int32 x, Int32 y, Int32 width, Int32 height, int type);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glReadPixels", ExactSpelling = true)]
            internal extern static void ReadPixels(Int32 x, Int32 y, Int32 width, Int32 height, int format, int type, [Out] IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawPixels", ExactSpelling = true)]
            internal extern static void DrawPixels(Int32 width, Int32 height, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetBooleanv", ExactSpelling = true)]
            internal extern static unsafe void GetBooleanv(int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetClipPlane", ExactSpelling = true)]
            internal extern static unsafe void GetClipPlane(int plane, [Out] Double* equation);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetDoublev", ExactSpelling = true)]
            internal extern static unsafe void GetDoublev(int pname, [Out] Double* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetError", ExactSpelling = true)]
            internal extern static int GetError();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetFloatv", ExactSpelling = true)]
            internal extern static unsafe void GetFloatv(int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetIntegerv", ExactSpelling = true)]
            internal extern static unsafe void GetIntegerv(int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetLightfv", ExactSpelling = true)]
            internal extern static unsafe void GetLightfv(int light, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetLightiv", ExactSpelling = true)]
            internal extern static unsafe void GetLightiv(int light, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMapdv", ExactSpelling = true)]
            internal extern static unsafe void GetMapdv(int target, int query, [Out] Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMapfv", ExactSpelling = true)]
            internal extern static unsafe void GetMapfv(int target, int query, [Out] Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMapiv", ExactSpelling = true)]
            internal extern static unsafe void GetMapiv(int target, int query, [Out] Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMaterialfv", ExactSpelling = true)]
            internal extern static unsafe void GetMaterialfv(int face, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMaterialiv", ExactSpelling = true)]
            internal extern static unsafe void GetMaterialiv(int face, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetPixelMapfv", ExactSpelling = true)]
            internal extern static unsafe void GetPixelMapfv(int map, [Out] Single* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetPixelMapuiv", ExactSpelling = true)]
            internal extern static unsafe void GetPixelMapuiv(int map, [Out] UInt32* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetPixelMapusv", ExactSpelling = true)]
            internal extern static unsafe void GetPixelMapusv(int map, [Out] UInt16* values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetPolygonStipple", ExactSpelling = true)]
            internal extern static unsafe void GetPolygonStipple([Out] Byte* mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetString", ExactSpelling = true)]
            internal extern static IntPtr GetString(int name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexEnvfv", ExactSpelling = true)]
            internal extern static unsafe void GetTexEnvfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexEnviv", ExactSpelling = true)]
            internal extern static unsafe void GetTexEnviv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexGendv", ExactSpelling = true)]
            internal extern static unsafe void GetTexGendv(int coord, int pname, [Out] Double* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexGenfv", ExactSpelling = true)]
            internal extern static unsafe void GetTexGenfv(int coord, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexGeniv", ExactSpelling = true)]
            internal extern static unsafe void GetTexGeniv(int coord, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexImage", ExactSpelling = true)]
            internal extern static void GetTexImage(int target, Int32 level, int format, int type, [Out] IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetTexParameterfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetTexParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexLevelParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetTexLevelParameterfv(int target, Int32 level, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetTexLevelParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetTexLevelParameteriv(int target, Int32 level, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsEnabled", ExactSpelling = true)]
            internal extern static Int32 IsEnabled(int cap);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsList", ExactSpelling = true)]
            internal extern static Int32 IsList(UInt32 list);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDepthRange", ExactSpelling = true)]
            internal extern static void DepthRange(Double near, Double far);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFrustum", ExactSpelling = true)]
            internal extern static void Frustum(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadIdentity", ExactSpelling = true)]
            internal extern static void LoadIdentity();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadMatrixf", ExactSpelling = true)]
            internal extern static unsafe void LoadMatrixf(Single* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadMatrixd", ExactSpelling = true)]
            internal extern static unsafe void LoadMatrixd(Double* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMatrixMode", ExactSpelling = true)]
            internal extern static void MatrixMode(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultMatrixf", ExactSpelling = true)]
            internal extern static unsafe void MultMatrixf(Single* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultMatrixd", ExactSpelling = true)]
            internal extern static unsafe void MultMatrixd(Double* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glOrtho", ExactSpelling = true)]
            internal extern static void Ortho(Double left, Double right, Double bottom, Double top, Double zNear, Double zFar);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPopMatrix", ExactSpelling = true)]
            internal extern static void PopMatrix();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPushMatrix", ExactSpelling = true)]
            internal extern static void PushMatrix();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRotated", ExactSpelling = true)]
            internal extern static void Rotated(Double angle, Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glRotatef", ExactSpelling = true)]
            internal extern static void Rotatef(Single angle, Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glScaled", ExactSpelling = true)]
            internal extern static void Scaled(Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glScalef", ExactSpelling = true)]
            internal extern static void Scalef(Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTranslated", ExactSpelling = true)]
            internal extern static void Translated(Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTranslatef", ExactSpelling = true)]
            internal extern static void Translatef(Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glViewport", ExactSpelling = true)]
            internal extern static void Viewport(Int32 x, Int32 y, Int32 width, Int32 height);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glArrayElement", ExactSpelling = true)]
            internal extern static void ArrayElement(Int32 i);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorPointer", ExactSpelling = true)]
            internal extern static void ColorPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDisableClientState", ExactSpelling = true)]
            internal extern static void DisableClientState(int array);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawArrays", ExactSpelling = true)]
            internal extern static void DrawArrays(int mode, Int32 first, Int32 count);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawElements", ExactSpelling = true)]
            internal extern static void DrawElements(int mode, Int32 count, int type, IntPtr indices);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEdgeFlagPointer", ExactSpelling = true)]
            internal extern static void EdgeFlagPointer(Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEnableClientState", ExactSpelling = true)]
            internal extern static void EnableClientState(int array);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetPointerv", ExactSpelling = true)]
            internal extern static void GetPointerv(int pname, [Out] IntPtr @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexPointer", ExactSpelling = true)]
            internal extern static void IndexPointer(int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glInterleavedArrays", ExactSpelling = true)]
            internal extern static void InterleavedArrays(int format, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glNormalPointer", ExactSpelling = true)]
            internal extern static void NormalPointer(int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexCoordPointer", ExactSpelling = true)]
            internal extern static void TexCoordPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexPointer", ExactSpelling = true)]
            internal extern static void VertexPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPolygonOffset", ExactSpelling = true)]
            internal extern static void PolygonOffset(Single factor, Single units);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyTexImage1D", ExactSpelling = true)]
            internal extern static void CopyTexImage1D(int target, Int32 level, int internalformat, Int32 x, Int32 y, Int32 width, Int32 border);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyTexImage2D", ExactSpelling = true)]
            internal extern static void CopyTexImage2D(int target, Int32 level, int internalformat, Int32 x, Int32 y, Int32 width, Int32 height, Int32 border);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyTexSubImage1D", ExactSpelling = true)]
            internal extern static void CopyTexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 x, Int32 y, Int32 width);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyTexSubImage2D", ExactSpelling = true)]
            internal extern static void CopyTexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 x, Int32 y, Int32 width, Int32 height);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexSubImage1D", ExactSpelling = true)]
            internal extern static void TexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 width, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexSubImage2D", ExactSpelling = true)]
            internal extern static void TexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glAreTexturesResident", ExactSpelling = true)]
            internal extern static unsafe Int32 AreTexturesResident(Int32 n, UInt32* textures, [Out] Int32* residences);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBindTexture", ExactSpelling = true)]
            internal extern static void BindTexture(int target, UInt32 texture);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteTextures", ExactSpelling = true)]
            internal extern static unsafe void DeleteTextures(Int32 n, UInt32* textures);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGenTextures", ExactSpelling = true)]
            internal extern static unsafe void GenTextures(Int32 n, [Out] UInt32* textures);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsTexture", ExactSpelling = true)]
            internal extern static Int32 IsTexture(UInt32 texture);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPrioritizeTextures", ExactSpelling = true)]
            internal extern static unsafe void PrioritizeTextures(Int32 n, UInt32* textures, Single* priorities);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexub", ExactSpelling = true)]
            internal extern static void Indexub(Byte c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIndexubv", ExactSpelling = true)]
            internal extern static unsafe void Indexubv(Byte* c);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPopClientAttrib", ExactSpelling = true)]
            internal extern static void PopClientAttrib();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPushClientAttrib", ExactSpelling = true)]
            internal extern static void PushClientAttrib(int mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBlendColor", ExactSpelling = true)]
            internal extern static void BlendColor(Single red, Single green, Single blue, Single alpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBlendEquation", ExactSpelling = true)]
            internal extern static void BlendEquation(int mode);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawRangeElements", ExactSpelling = true)]
            internal extern static void DrawRangeElements(int mode, UInt32 start, UInt32 end, Int32 count, int type, IntPtr indices);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorTable", ExactSpelling = true)]
            internal extern static void ColorTable(int target, int internalformat, Int32 width, int format, int type, IntPtr table);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorTableParameterfv", ExactSpelling = true)]
            internal extern static unsafe void ColorTableParameterfv(int target, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorTableParameteriv", ExactSpelling = true)]
            internal extern static unsafe void ColorTableParameteriv(int target, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyColorTable", ExactSpelling = true)]
            internal extern static void CopyColorTable(int target, int internalformat, Int32 x, Int32 y, Int32 width);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetColorTable", ExactSpelling = true)]
            internal extern static void GetColorTable(int target, int format, int type, [Out] IntPtr table);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetColorTableParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetColorTableParameterfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetColorTableParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetColorTableParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glColorSubTable", ExactSpelling = true)]
            internal extern static void ColorSubTable(int target, Int32 start, Int32 count, int format, int type, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyColorSubTable", ExactSpelling = true)]
            internal extern static void CopyColorSubTable(int target, Int32 start, Int32 x, Int32 y, Int32 width);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionFilter1D", ExactSpelling = true)]
            internal extern static void ConvolutionFilter1D(int target, int internalformat, Int32 width, int format, int type, IntPtr image);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionFilter2D", ExactSpelling = true)]
            internal extern static void ConvolutionFilter2D(int target, int internalformat, Int32 width, Int32 height, int format, int type, IntPtr image);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionParameterf", ExactSpelling = true)]
            internal extern static void ConvolutionParameterf(int target, int pname, Single @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionParameterfv", ExactSpelling = true)]
            internal extern static unsafe void ConvolutionParameterfv(int target, int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionParameteri", ExactSpelling = true)]
            internal extern static void ConvolutionParameteri(int target, int pname, Int32 @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glConvolutionParameteriv", ExactSpelling = true)]
            internal extern static unsafe void ConvolutionParameteriv(int target, int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyConvolutionFilter1D", ExactSpelling = true)]
            internal extern static void CopyConvolutionFilter1D(int target, int internalformat, Int32 x, Int32 y, Int32 width);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyConvolutionFilter2D", ExactSpelling = true)]
            internal extern static void CopyConvolutionFilter2D(int target, int internalformat, Int32 x, Int32 y, Int32 width, Int32 height);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetConvolutionFilter", ExactSpelling = true)]
            internal extern static void GetConvolutionFilter(int target, int format, int type, [Out] IntPtr image);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetConvolutionParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetConvolutionParameterfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetConvolutionParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetConvolutionParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetSeparableFilter", ExactSpelling = true)]
            internal extern static void GetSeparableFilter(int target, int format, int type, [Out] IntPtr row, [Out] IntPtr column, [Out] IntPtr span);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSeparableFilter2D", ExactSpelling = true)]
            internal extern static void SeparableFilter2D(int target, int internalformat, Int32 width, Int32 height, int format, int type, IntPtr row, IntPtr column);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetHistogram", ExactSpelling = true)]
            internal extern static void GetHistogram(int target, Int32 reset, int format, int type, [Out] IntPtr values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetHistogramParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetHistogramParameterfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetHistogramParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetHistogramParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMinmax", ExactSpelling = true)]
            internal extern static void GetMinmax(int target, Int32 reset, int format, int type, [Out] IntPtr values);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMinmaxParameterfv", ExactSpelling = true)]
            internal extern static unsafe void GetMinmaxParameterfv(int target, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetMinmaxParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetMinmaxParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glHistogram", ExactSpelling = true)]
            internal extern static void Histogram(int target, Int32 width, int internalformat, Int32 sink);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMinmax", ExactSpelling = true)]
            internal extern static void Minmax(int target, int internalformat, Int32 sink);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glResetHistogram", ExactSpelling = true)]
            internal extern static void ResetHistogram(int target);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glResetMinmax", ExactSpelling = true)]
            internal extern static void ResetMinmax(int target);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexImage3D", ExactSpelling = true)]
            internal extern static void TexImage3D(int target, Int32 level, int internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glTexSubImage3D", ExactSpelling = true)]
            internal extern static void TexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, int format, int type, IntPtr pixels);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCopyTexSubImage3D", ExactSpelling = true)]
            internal extern static void CopyTexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 x, Int32 y, Int32 width, Int32 height);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glActiveTexture", ExactSpelling = true)]
            internal extern static void ActiveTexture(int texture);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glClientActiveTexture", ExactSpelling = true)]
            internal extern static void ClientActiveTexture(int texture);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1d", ExactSpelling = true)]
            internal extern static void MultiTexCoord1d(int target, Double s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1dv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord1dv(int target, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1f", ExactSpelling = true)]
            internal extern static void MultiTexCoord1f(int target, Single s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1fv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord1fv(int target, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1i", ExactSpelling = true)]
            internal extern static void MultiTexCoord1i(int target, Int32 s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1iv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord1iv(int target, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1s", ExactSpelling = true)]
            internal extern static void MultiTexCoord1s(int target, Int16 s);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord1sv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord1sv(int target, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2d", ExactSpelling = true)]
            internal extern static void MultiTexCoord2d(int target, Double s, Double t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2dv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord2dv(int target, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2f", ExactSpelling = true)]
            internal extern static void MultiTexCoord2f(int target, Single s, Single t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2fv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord2fv(int target, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2i", ExactSpelling = true)]
            internal extern static void MultiTexCoord2i(int target, Int32 s, Int32 t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2iv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord2iv(int target, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2s", ExactSpelling = true)]
            internal extern static void MultiTexCoord2s(int target, Int16 s, Int16 t);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord2sv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord2sv(int target, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3d", ExactSpelling = true)]
            internal extern static void MultiTexCoord3d(int target, Double s, Double t, Double r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3dv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord3dv(int target, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3f", ExactSpelling = true)]
            internal extern static void MultiTexCoord3f(int target, Single s, Single t, Single r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3fv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord3fv(int target, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3i", ExactSpelling = true)]
            internal extern static void MultiTexCoord3i(int target, Int32 s, Int32 t, Int32 r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3iv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord3iv(int target, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3s", ExactSpelling = true)]
            internal extern static void MultiTexCoord3s(int target, Int16 s, Int16 t, Int16 r);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord3sv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord3sv(int target, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4d", ExactSpelling = true)]
            internal extern static void MultiTexCoord4d(int target, Double s, Double t, Double r, Double q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4dv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord4dv(int target, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4f", ExactSpelling = true)]
            internal extern static void MultiTexCoord4f(int target, Single s, Single t, Single r, Single q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4fv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord4fv(int target, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4i", ExactSpelling = true)]
            internal extern static void MultiTexCoord4i(int target, Int32 s, Int32 t, Int32 r, Int32 q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4iv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord4iv(int target, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4s", ExactSpelling = true)]
            internal extern static void MultiTexCoord4s(int target, Int16 s, Int16 t, Int16 r, Int16 q);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiTexCoord4sv", ExactSpelling = true)]
            internal extern static unsafe void MultiTexCoord4sv(int target, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadTransposeMatrixf", ExactSpelling = true)]
            internal extern static unsafe void LoadTransposeMatrixf(Single* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLoadTransposeMatrixd", ExactSpelling = true)]
            internal extern static unsafe void LoadTransposeMatrixd(Double* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultTransposeMatrixf", ExactSpelling = true)]
            internal extern static unsafe void MultTransposeMatrixf(Single* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultTransposeMatrixd", ExactSpelling = true)]
            internal extern static unsafe void MultTransposeMatrixd(Double* m);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSampleCoverage", ExactSpelling = true)]
            internal extern static void SampleCoverage(Single value, Int32 invert);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexImage3D", ExactSpelling = true)]
            internal extern static void CompressedTexImage3D(int target, Int32 level, int internalformat, Int32 width, Int32 height, Int32 depth, Int32 border, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexImage2D", ExactSpelling = true)]
            internal extern static void CompressedTexImage2D(int target, Int32 level, int internalformat, Int32 width, Int32 height, Int32 border, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexImage1D", ExactSpelling = true)]
            internal extern static void CompressedTexImage1D(int target, Int32 level, int internalformat, Int32 width, Int32 border, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexSubImage3D", ExactSpelling = true)]
            internal extern static void CompressedTexSubImage3D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 zoffset, Int32 width, Int32 height, Int32 depth, int format, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexSubImage2D", ExactSpelling = true)]
            internal extern static void CompressedTexSubImage2D(int target, Int32 level, Int32 xoffset, Int32 yoffset, Int32 width, Int32 height, int format, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompressedTexSubImage1D", ExactSpelling = true)]
            internal extern static void CompressedTexSubImage1D(int target, Int32 level, Int32 xoffset, Int32 width, int format, Int32 imageSize, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetCompressedTexImage", ExactSpelling = true)]
            internal extern static void GetCompressedTexImage(int target, Int32 level, [Out] IntPtr img);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBlendFuncSeparate", ExactSpelling = true)]
            internal extern static void BlendFuncSeparate(int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogCoordf", ExactSpelling = true)]
            internal extern static void FogCoordf(Single coord);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogCoordfv", ExactSpelling = true)]
            internal extern static unsafe void FogCoordfv(Single* coord);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogCoordd", ExactSpelling = true)]
            internal extern static void FogCoordd(Double coord);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogCoorddv", ExactSpelling = true)]
            internal extern static unsafe void FogCoorddv(Double* coord);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glFogCoordPointer", ExactSpelling = true)]
            internal extern static void FogCoordPointer(int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiDrawArrays", ExactSpelling = true)]
            internal extern static unsafe void MultiDrawArrays(int mode, [Out] Int32* first, [Out] Int32* count, Int32 primcount);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMultiDrawElements", ExactSpelling = true)]
            internal extern static unsafe void MultiDrawElements(int mode, Int32* count, int type, IntPtr indices, Int32 primcount);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPointParameterf", ExactSpelling = true)]
            internal extern static void PointParameterf(int pname, Single param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPointParameterfv", ExactSpelling = true)]
            internal extern static unsafe void PointParameterfv(int pname, Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPointParameteri", ExactSpelling = true)]
            internal extern static void PointParameteri(int pname, Int32 param);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glPointParameteriv", ExactSpelling = true)]
            internal extern static unsafe void PointParameteriv(int pname, Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3b", ExactSpelling = true)]
            internal extern static void SecondaryColor3b(SByte red, SByte green, SByte blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3bv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3bv(SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3d", ExactSpelling = true)]
            internal extern static void SecondaryColor3d(Double red, Double green, Double blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3dv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3f", ExactSpelling = true)]
            internal extern static void SecondaryColor3f(Single red, Single green, Single blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3fv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3i", ExactSpelling = true)]
            internal extern static void SecondaryColor3i(Int32 red, Int32 green, Int32 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3iv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3s", ExactSpelling = true)]
            internal extern static void SecondaryColor3s(Int16 red, Int16 green, Int16 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3sv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3ub", ExactSpelling = true)]
            internal extern static void SecondaryColor3ub(Byte red, Byte green, Byte blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3ubv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3ubv(Byte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3ui", ExactSpelling = true)]
            internal extern static void SecondaryColor3ui(UInt32 red, UInt32 green, UInt32 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3uiv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3uiv(UInt32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3us", ExactSpelling = true)]
            internal extern static void SecondaryColor3us(UInt16 red, UInt16 green, UInt16 blue);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColor3usv", ExactSpelling = true)]
            internal extern static unsafe void SecondaryColor3usv(UInt16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glSecondaryColorPointer", ExactSpelling = true)]
            internal extern static void SecondaryColorPointer(Int32 size, int type, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2d", ExactSpelling = true)]
            internal extern static void WindowPos2d(Double x, Double y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2dv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos2dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2f", ExactSpelling = true)]
            internal extern static void WindowPos2f(Single x, Single y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2fv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos2fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2i", ExactSpelling = true)]
            internal extern static void WindowPos2i(Int32 x, Int32 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2iv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos2iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2s", ExactSpelling = true)]
            internal extern static void WindowPos2s(Int16 x, Int16 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos2sv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos2sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3d", ExactSpelling = true)]
            internal extern static void WindowPos3d(Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3dv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos3dv(Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3f", ExactSpelling = true)]
            internal extern static void WindowPos3f(Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3fv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos3fv(Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3i", ExactSpelling = true)]
            internal extern static void WindowPos3i(Int32 x, Int32 y, Int32 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3iv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos3iv(Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3s", ExactSpelling = true)]
            internal extern static void WindowPos3s(Int16 x, Int16 y, Int16 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glWindowPos3sv", ExactSpelling = true)]
            internal extern static unsafe void WindowPos3sv(Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGenQueries", ExactSpelling = true)]
            internal extern static unsafe void GenQueries(Int32 n, [Out] UInt32* ids);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteQueries", ExactSpelling = true)]
            internal extern static unsafe void DeleteQueries(Int32 n, UInt32* ids);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsQuery", ExactSpelling = true)]
            internal extern static Int32 IsQuery(UInt32 id);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBeginQuery", ExactSpelling = true)]
            internal extern static void BeginQuery(int target, UInt32 id);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEndQuery", ExactSpelling = true)]
            internal extern static void EndQuery(int target);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetQueryiv", ExactSpelling = true)]
            internal extern static unsafe void GetQueryiv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetQueryObjectiv", ExactSpelling = true)]
            internal extern static unsafe void GetQueryObjectiv(UInt32 id, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetQueryObjectuiv", ExactSpelling = true)]
            internal extern static unsafe void GetQueryObjectuiv(UInt32 id, int pname, [Out] UInt32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBindBuffer", ExactSpelling = true)]
            internal extern static void BindBuffer(int target, UInt32 buffer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteBuffers", ExactSpelling = true)]
            internal extern static unsafe void DeleteBuffers(Int32 n, UInt32* buffers);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGenBuffers", ExactSpelling = true)]
            internal extern static unsafe void GenBuffers(Int32 n, [Out] UInt32* buffers);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsBuffer", ExactSpelling = true)]
            internal extern static Int32 IsBuffer(UInt32 buffer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBufferData", ExactSpelling = true)]
            internal extern static void BufferData(int target, IntPtr size, IntPtr data, int usage);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBufferSubData", ExactSpelling = true)]
            internal extern static void BufferSubData(int target, IntPtr offset, IntPtr size, IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetBufferSubData", ExactSpelling = true)]
            internal extern static void GetBufferSubData(int target, IntPtr offset, IntPtr size, [Out] IntPtr data);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glMapBuffer", ExactSpelling = true)]
            internal extern static unsafe IntPtr MapBuffer(int target, int access);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUnmapBuffer", ExactSpelling = true)]
            internal extern static Int32 UnmapBuffer(int target);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetBufferParameteriv", ExactSpelling = true)]
            internal extern static unsafe void GetBufferParameteriv(int target, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetBufferPointerv", ExactSpelling = true)]
            internal extern static void GetBufferPointerv(int target, int pname, [Out] IntPtr @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBlendEquationSeparate", ExactSpelling = true)]
            internal extern static void BlendEquationSeparate(int modeRGB, int modeAlpha);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDrawBuffers", ExactSpelling = true)]
            internal extern static unsafe void DrawBuffers(Int32 n, int* bufs);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilOpSeparate", ExactSpelling = true)]
            internal extern static void StencilOpSeparate(int face, int sfail, int dpfail, int dppass);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilFuncSeparate", ExactSpelling = true)]
            internal extern static void StencilFuncSeparate(int frontfunc, int backfunc, Int32 @ref, UInt32 mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glStencilMaskSeparate", ExactSpelling = true)]
            internal extern static void StencilMaskSeparate(int face, UInt32 mask);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glAttachShader", ExactSpelling = true)]
            internal extern static void AttachShader(UInt32 program, UInt32 shader);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glBindAttribLocation", ExactSpelling = true)]
            internal extern static void BindAttribLocation(UInt32 program, UInt32 index, System.String name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCompileShader", ExactSpelling = true)]
            internal extern static void CompileShader(UInt32 shader);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCreateProgram", ExactSpelling = true)]
            internal extern static Int32 CreateProgram();
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glCreateShader", ExactSpelling = true)]
            internal extern static Int32 CreateShader(int type);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteProgram", ExactSpelling = true)]
            internal extern static void DeleteProgram(UInt32 program);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDeleteShader", ExactSpelling = true)]
            internal extern static void DeleteShader(UInt32 shader);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDetachShader", ExactSpelling = true)]
            internal extern static void DetachShader(UInt32 program, UInt32 shader);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glDisableVertexAttribArray", ExactSpelling = true)]
            internal extern static void DisableVertexAttribArray(UInt32 index);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glEnableVertexAttribArray", ExactSpelling = true)]
            internal extern static void EnableVertexAttribArray(UInt32 index);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetActiveAttrib", ExactSpelling = true)]
            internal extern static unsafe void GetActiveAttrib(UInt32 program, UInt32 index, Int32 bufSize, [Out] Int32* length, [Out] Int32* size, [Out] int* type, [Out] System.Text.StringBuilder name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetActiveUniform", ExactSpelling = true)]
            internal extern static unsafe void GetActiveUniform(UInt32 program, UInt32 index, Int32 bufSize, [Out] Int32* length, [Out] Int32* size, [Out] int* type, [Out] System.Text.StringBuilder name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetAttachedShaders", ExactSpelling = true)]
            internal extern static unsafe void GetAttachedShaders(UInt32 program, Int32 maxCount, [Out] Int32* count, [Out] UInt32* obj);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetAttribLocation", ExactSpelling = true)]
            internal extern static Int32 GetAttribLocation(UInt32 program, System.String name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetProgramiv", ExactSpelling = true)]
            internal extern static unsafe void GetProgramiv(UInt32 program, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetProgramInfoLog", ExactSpelling = true)]
            internal extern static unsafe void GetProgramInfoLog(UInt32 program, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder infoLog);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetShaderiv", ExactSpelling = true)]
            internal extern static unsafe void GetShaderiv(UInt32 shader, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetShaderInfoLog", ExactSpelling = true)]
            internal extern static unsafe void GetShaderInfoLog(UInt32 shader, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder infoLog);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetShaderSource", ExactSpelling = true)]
            internal extern static unsafe void GetShaderSource(UInt32 shader, Int32 bufSize, [Out] Int32* length, [Out] System.Text.StringBuilder[] source);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetUniformLocation", ExactSpelling = true)]
            internal extern static Int32 GetUniformLocation(UInt32 program, System.String name);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetUniformfv", ExactSpelling = true)]
            internal extern static unsafe void GetUniformfv(UInt32 program, Int32 location, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetUniformiv", ExactSpelling = true)]
            internal extern static unsafe void GetUniformiv(UInt32 program, Int32 location, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetVertexAttribdv", ExactSpelling = true)]
            internal extern static unsafe void GetVertexAttribdv(UInt32 index, int pname, [Out] Double* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetVertexAttribfv", ExactSpelling = true)]
            internal extern static unsafe void GetVertexAttribfv(UInt32 index, int pname, [Out] Single* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetVertexAttribiv", ExactSpelling = true)]
            internal extern static unsafe void GetVertexAttribiv(UInt32 index, int pname, [Out] Int32* @params);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glGetVertexAttribPointerv", ExactSpelling = true)]
            internal extern static void GetVertexAttribPointerv(UInt32 index, int pname, [Out] IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsProgram", ExactSpelling = true)]
            internal extern static Int32 IsProgram(UInt32 program);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glIsShader", ExactSpelling = true)]
            internal extern static Int32 IsShader(UInt32 shader);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glLinkProgram", ExactSpelling = true)]
            internal extern static void LinkProgram(UInt32 program);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glShaderSource", ExactSpelling = true)]
            internal extern static unsafe void ShaderSource(UInt32 shader, Int32 count, System.String[] @string, Int32* length);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUseProgram", ExactSpelling = true)]
            internal extern static void UseProgram(UInt32 program);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform1f", ExactSpelling = true)]
            internal extern static void Uniform1f(Int32 location, Single v0);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform2f", ExactSpelling = true)]
            internal extern static void Uniform2f(Int32 location, Single v0, Single v1);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform3f", ExactSpelling = true)]
            internal extern static void Uniform3f(Int32 location, Single v0, Single v1, Single v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform4f", ExactSpelling = true)]
            internal extern static void Uniform4f(Int32 location, Single v0, Single v1, Single v2, Single v3);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform1i", ExactSpelling = true)]
            internal extern static void Uniform1i(Int32 location, Int32 v0);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform2i", ExactSpelling = true)]
            internal extern static void Uniform2i(Int32 location, Int32 v0, Int32 v1);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform3i", ExactSpelling = true)]
            internal extern static void Uniform3i(Int32 location, Int32 v0, Int32 v1, Int32 v2);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform4i", ExactSpelling = true)]
            internal extern static void Uniform4i(Int32 location, Int32 v0, Int32 v1, Int32 v2, Int32 v3);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform1fv", ExactSpelling = true)]
            internal extern static unsafe void Uniform1fv(Int32 location, Int32 count, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform2fv", ExactSpelling = true)]
            internal extern static unsafe void Uniform2fv(Int32 location, Int32 count, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform3fv", ExactSpelling = true)]
            internal extern static unsafe void Uniform3fv(Int32 location, Int32 count, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform4fv", ExactSpelling = true)]
            internal extern static unsafe void Uniform4fv(Int32 location, Int32 count, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform1iv", ExactSpelling = true)]
            internal extern static unsafe void Uniform1iv(Int32 location, Int32 count, Int32* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform2iv", ExactSpelling = true)]
            internal extern static unsafe void Uniform2iv(Int32 location, Int32 count, Int32* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform3iv", ExactSpelling = true)]
            internal extern static unsafe void Uniform3iv(Int32 location, Int32 count, Int32* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniform4iv", ExactSpelling = true)]
            internal extern static unsafe void Uniform4iv(Int32 location, Int32 count, Int32* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix2fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix3fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix4fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glValidateProgram", ExactSpelling = true)]
            internal extern static void ValidateProgram(UInt32 program);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1d", ExactSpelling = true)]
            internal extern static void VertexAttrib1d(UInt32 index, Double x);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1dv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib1dv(UInt32 index, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1f", ExactSpelling = true)]
            internal extern static void VertexAttrib1f(UInt32 index, Single x);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1fv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib1fv(UInt32 index, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1s", ExactSpelling = true)]
            internal extern static void VertexAttrib1s(UInt32 index, Int16 x);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib1sv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib1sv(UInt32 index, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2d", ExactSpelling = true)]
            internal extern static void VertexAttrib2d(UInt32 index, Double x, Double y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2dv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib2dv(UInt32 index, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2f", ExactSpelling = true)]
            internal extern static void VertexAttrib2f(UInt32 index, Single x, Single y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2fv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib2fv(UInt32 index, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2s", ExactSpelling = true)]
            internal extern static void VertexAttrib2s(UInt32 index, Int16 x, Int16 y);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib2sv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib2sv(UInt32 index, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3d", ExactSpelling = true)]
            internal extern static void VertexAttrib3d(UInt32 index, Double x, Double y, Double z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3dv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib3dv(UInt32 index, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3f", ExactSpelling = true)]
            internal extern static void VertexAttrib3f(UInt32 index, Single x, Single y, Single z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3fv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib3fv(UInt32 index, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3s", ExactSpelling = true)]
            internal extern static void VertexAttrib3s(UInt32 index, Int16 x, Int16 y, Int16 z);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib3sv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib3sv(UInt32 index, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nbv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Nbv(UInt32 index, SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Niv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Niv(UInt32 index, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nsv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Nsv(UInt32 index, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nub", ExactSpelling = true)]
            internal extern static void VertexAttrib4Nub(UInt32 index, Byte x, Byte y, Byte z, Byte w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nubv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Nubv(UInt32 index, Byte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nuiv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Nuiv(UInt32 index, UInt32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4Nusv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4Nusv(UInt32 index, UInt16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4bv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4bv(UInt32 index, SByte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4d", ExactSpelling = true)]
            internal extern static void VertexAttrib4d(UInt32 index, Double x, Double y, Double z, Double w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4dv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4dv(UInt32 index, Double* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4f", ExactSpelling = true)]
            internal extern static void VertexAttrib4f(UInt32 index, Single x, Single y, Single z, Single w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4fv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4fv(UInt32 index, Single* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4iv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4iv(UInt32 index, Int32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4s", ExactSpelling = true)]
            internal extern static void VertexAttrib4s(UInt32 index, Int16 x, Int16 y, Int16 z, Int16 w);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4sv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4sv(UInt32 index, Int16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4ubv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4ubv(UInt32 index, Byte* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4uiv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4uiv(UInt32 index, UInt32* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttrib4usv", ExactSpelling = true)]
            internal extern static unsafe void VertexAttrib4usv(UInt32 index, UInt16* v);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glVertexAttribPointer", ExactSpelling = true)]
            internal extern static void VertexAttribPointer(UInt32 index, Int32 size, int type, Int32 normalized, Int32 stride, IntPtr pointer);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix2x3fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix2x3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix3x2fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix3x2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix2x4fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix2x4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix4x2fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix4x2fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix3x4fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix3x4fv(Int32 location, Int32 count, Int32 transpose, Single* value);
            [SuppressUnmanagedCodeSecurity()]
            [DllImport(NATIVE_LIB, EntryPoint = "glUniformMatrix4x3fv", ExactSpelling = true)]
            internal extern static unsafe void UniformMatrix4x3fv(Int32 location, Int32 count, Int32 transpose, Single* value);
        }
    }
}