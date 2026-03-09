namespace System.Destrospean
{
    public static class Logger
    {
        public static void WriteError(System.Exception ex)
        {
            System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "error-" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".log", ex.Message + "\n" + ex.StackTrace);
        }
    }
}
