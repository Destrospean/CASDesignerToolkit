namespace Destrospean.DestrospeanCASPEditor
{
    [System.Flags]
    public enum NextStateOptions : byte
    {
        NoUnsavedChanges,
        UnsavedChanges,
        UpdateModels,
        UnsavedChangesAndUpdateModels
    }

    public abstract class MainWindowBase : Gtk.Window
    {
        public bool HasUnsavedChanges
        {
            get;
            protected set;
        }

        public abstract NextStateOptions NextState
        {
            set;
        }

        public abstract string OriginalWindowTitle
        {
            get;
        }

        public static MainWindowBase Singleton
        {
            get;
            protected set;
        }

        public MainWindowBase(Gtk.WindowType windowType) : base(windowType)
        {
            Singleton = this;
            Common.Abstractions.Complate.GetTextureCallback = ImageUtils.GetTexture;
            Common.Abstractions.Complate.MarkUnsavedChangesCallback = () => NextState = NextStateOptions.UnsavedChanges;
            CmarNYCBorrowed.TextureUtils.PreloadedGameImages = ImageUtils.PreloadedGameImages;
            CmarNYCBorrowed.TextureUtils.PreloadedImages = ImageUtils.PreloadedImages;
            HasUnsavedChanges = false;
            Common.ApplicationSettings.LoadSettings();
        }

        public void AddFilePathToWindowTitle(string path)
        {
            var directoryPath = System.IO.Path.GetDirectoryName(path);
            Title = OriginalWindowTitle + " \u2013 " + (directoryPath.Length > 40 ? "..." + directoryPath.Substring(directoryPath.Length - 40) : directoryPath) + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(path);
        }

        public abstract void DrawImage();

        public Gtk.ResponseType GetUnsavedChangesDialogResponseType()
        {
            var unsavedChangesDialog = new UnsavedChangesDialog(this);
            var responseType = (Gtk.ResponseType)unsavedChangesDialog.Run();
            unsavedChangesDialog.Destroy();
            return responseType;
        }

        public abstract void RescaleAndReposition(bool skipRescale = false);
    }
}
