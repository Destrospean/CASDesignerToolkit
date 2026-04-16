namespace System.Destrospean
{
    public static class Logger
    {
        public static void WriteError(System.Exception ex, bool fatal = false)
        {
            System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + (fatal ? "crash-" : "error-") + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".log", ex.Message + System.Environment.NewLine + ex.StackTrace);
        }
    }
}
