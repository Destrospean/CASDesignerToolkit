using System;
using System.Runtime.InteropServices;
using Cairo;
using GLib;

namespace Destrospean.DestrospeanCASPEditor
{
    public class CairoHelper
    {
        static readonly ICairoHelperInternal sSingleton = Common.Platform.IsWindows ? (ICairoHelperInternal)new CairoHelperInternalWindows() : (ICairoHelperInternal)new CairoHelperInternalOther();

        class CairoHelperInternalOther : ICairoHelperInternal
        {
            public Context Create(Gdk.Drawable drawable)
            {
                return Gdk.CairoHelper.Create(drawable);
            }

            public void Rectangle(Context cr, Gdk.Rectangle rectangle)
            {
                Gdk.CairoHelper.Rectangle(cr, rectangle);
            }

            public void Region(Context cr, Gdk.Region region)
            {
                Gdk.CairoHelper.Region(cr, region);
            }

            public void SetSourceColor(Context cr, Gdk.Color color)
            {
                Gdk.CairoHelper.SetSourceColor(cr, color);
            }

            public void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
            {
                Gdk.CairoHelper.SetSourcePixbuf(cr, pixbuf, pixbuf_x, pixbuf_y);
            }

            public void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
            {
                Gdk.CairoHelper.SetSourcePixmap(cr, pixmap, pixmap_x, pixmap_y);
            }
        }

        class CairoHelperInternalWindows : ICairoHelperInternal
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

            public Context Create(Gdk.Drawable drawable)
            {
#pragma warning disable 0612
                return new Context(gdk_cairo_create(drawable == null ? IntPtr.Zero : drawable.Handle));
#pragma warning restore 0612
            }

            public void Rectangle(Context cr, Gdk.Rectangle rectangle)
            {
                var ptr = Marshaller.StructureToPtrAlloc(rectangle);
                gdk_cairo_rectangle(cr == null ? IntPtr.Zero : cr.Handle, ptr);
                rectangle = Gdk.Rectangle.New(ptr);
                Marshal.FreeHGlobal(ptr);
            }

            public void Region(Context cr, Gdk.Region region)
            {
                gdk_cairo_region(cr == null ? IntPtr.Zero : cr.Handle, region == null ? IntPtr.Zero : region.Handle);
            }

            public void SetSourceColor(Context cr, Gdk.Color color)
            {
                var ptr = Marshaller.StructureToPtrAlloc(color);
                gdk_cairo_set_source_color(cr == null ? IntPtr.Zero : cr.Handle, ptr);
                color = Gdk.Color.New(ptr);
                Marshal.FreeHGlobal(ptr);
            }

            public void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
            {
                gdk_cairo_set_source_pixbuf(cr == null ? IntPtr.Zero : cr.Handle, pixbuf == null ? IntPtr.Zero : pixbuf.Handle, pixbuf_x, pixbuf_y);
            }

            public void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
            {
                gdk_cairo_set_source_pixmap(cr == null ? IntPtr.Zero : cr.Handle, pixmap == null ? IntPtr.Zero : pixmap.Handle, pixmap_x, pixmap_y);
            }
        }

        interface ICairoHelperInternal
        {
            Context Create(Gdk.Drawable drawable);

            void Rectangle(Context cr, Gdk.Rectangle rectangle);

            void Region(Context cr, Gdk.Region region);

            void SetSourceColor(Context cr, Gdk.Color color);

            void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y);

            void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y);
        }

        public static Context Create(Gdk.Drawable drawable)
        {
            return sSingleton.Create(drawable);
        }

        public static void Rectangle(Context cr, Gdk.Rectangle rectangle)
        {
            sSingleton.Rectangle(cr, rectangle);
        }

        public static void Region(Context cr, Gdk.Region region)
        {
            sSingleton.Region(cr, region);
        }

        public static void SetSourceColor(Context cr, Gdk.Color color)
        {
            sSingleton.SetSourceColor(cr, color);
        }

        public static void SetSourcePixbuf(Context cr, Gdk.Pixbuf pixbuf, double pixbuf_x, double pixbuf_y)
        {
            sSingleton.SetSourcePixbuf(cr, pixbuf, pixbuf_x, pixbuf_y);
        }

        public static void SetSourcePixmap(Context cr, Gdk.Pixmap pixmap, double pixmap_x, double pixmap_y)
        {
            sSingleton.SetSourcePixmap(cr, pixmap, pixmap_x, pixmap_y);
        }
    }
}
