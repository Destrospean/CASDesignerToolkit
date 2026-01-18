using System;
using System.Runtime.InteropServices;
using Cairo;
using GLib;

namespace Destrospean.DestrospeanCASPEditor
{
    public class CairoHelper
    {
        class CairoHelperInternal
        {
            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr gdk_cairo_create(IntPtr drawable);

            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern void gdk_cairo_rectangle(IntPtr cr, IntPtr rectangle);

            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern void gdk_cairo_region(IntPtr cr, IntPtr region);

            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern void gdk_cairo_set_source_color(IntPtr cr, IntPtr color);

            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern void gdk_cairo_set_source_pixbuf(IntPtr cr, IntPtr pixbuf, double pixbuf_x, double pixbuf_y);

            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern void gdk_cairo_set_source_pixmap(IntPtr cr, IntPtr pixmap, double pixmap_x, double pixmap_y);

            public static Context Create(Gdk.Drawable drawable)
            {
                var state = gdk_cairo_create(drawable == null ? IntPtr.Zero : drawable.Handle);
#pragma warning disable 0612
                return new Context(state);
#pragma warning restore 0612
            }

            public static void Rectangle(Context cr, Gdk.Rectangle rectangle)
            {
                var ptr = Marshaller.StructureToPtrAlloc(rectangle);
                gdk_cairo_rectangle(cr == null ? IntPtr.Zero : cr.Handle, ptr);
                rectangle = Gdk.Rectangle.New(ptr);
                Marshal.FreeHGlobal(ptr);
            }

            public static void Region(Context cr, Gdk.Region region)
            {
                gdk_cairo_region(cr == null ? IntPtr.Zero : cr.Handle, region == null ? IntPtr.Zero : region.Handle);
            }

            public static void SetSourceColor(Context cr, Gdk.Color color)
            {
                var ptr = Marshaller.StructureToPtrAlloc(color);
                gdk_cairo_set_source_color(cr == null ? IntPtr.Zero : cr.Handle, ptr);
                color = Gdk.Color.New(ptr);
                Marshal.FreeHGlobal(ptr);
            }

            public static void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
            {
                gdk_cairo_set_source_pixbuf(cr == null ? IntPtr.Zero : cr.Handle, pixbuf == null ? IntPtr.Zero : pixbuf.Handle, pixbuf_x, pixbuf_y);
            }

            public static void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
            {
                gdk_cairo_set_source_pixmap(cr == null ? IntPtr.Zero : cr.Handle, pixmap == null ? IntPtr.Zero : pixmap.Handle, pixmap_x, pixmap_y);
            }
        }

        static Context CreateExternal(Gdk.Drawable drawable)
        {
            return Gdk.CairoHelper.Create(drawable);
        }

        static void RectangleExternal(Context cr, Gdk.Rectangle rectangle)
        {
            Gdk.CairoHelper.Rectangle(cr, rectangle);
        }

        static void RegionExternal(Context cr, Gdk.Region region)
        {
            Gdk.CairoHelper.Region(cr, region);
        }

        static void SetSourceColorExternal(Context cr, Gdk.Color color)
        {
            Gdk.CairoHelper.SetSourceColor(cr, color);
        }

        static void SetSourcePixbufExternal(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
        {
            Gdk.CairoHelper.SetSourcePixbuf(cr, pixbuf, pixbuf_x, pixbuf_y);
        }

        static void SetSourcePixmapExternal(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
        {
            Gdk.CairoHelper.SetSourcePixmap(cr, pixmap, pixmap_x, pixmap_y);
        }

        public static Context Create(Gdk.Drawable drawable)
        {
            return Common.Platform.IsWindows ? CairoHelperInternal.Create(drawable) : CreateExternal(drawable);
        }

        public static void Rectangle(Context cr, Gdk.Rectangle rectangle)
        {
            if (Common.Platform.IsWindows)
            {
                CairoHelperInternal.Rectangle(cr, rectangle);
                return;
            }
            RectangleExternal(cr, rectangle);
        }

        public static void Region(Context cr, Gdk.Region region)
        {
            if (Common.Platform.IsWindows)
            {
                CairoHelperInternal.Region(cr, region);
                return;
            }
            RegionExternal(cr, region);
        }

        public static void SetSourceColor(Context cr, Gdk.Color color)
        {
            if (Common.Platform.IsWindows)
            {
                CairoHelperInternal.SetSourceColor(cr, color);
                return;
            }
            SetSourceColorExternal(cr, color);
        }

        public static void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
        {
            if (Common.Platform.IsWindows)
            {
                CairoHelperInternal.SetSourcePixbuf(cr, pixbuf, pixbuf_x, pixbuf_y);
                return;
            }
            SetSourcePixbufExternal(cr, pixbuf, pixbuf_x, pixbuf_y);
        }

        public static void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
        {
            if (Common.Platform.IsWindows)
            {
                CairoHelperInternal.SetSourcePixmap(cr, pixmap, pixmap_x, pixmap_y);
                return;
            }
            SetSourcePixmapExternal(cr, pixmap, pixmap_x, pixmap_y);
        }
    }
}

