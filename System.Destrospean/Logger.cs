namespace System.Destrospean
{
    public static class Logger
    {
        public static void WriteError(Exception ex, bool fatal = false)
        {
            IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + (fatal ? "crash-" : "error-") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log", ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}
