namespace TimeTracker.Helper
{
    public static class LogWriter
    {
        public static void LogWrite(Exception ex)
        {
            try
            {
                var description = ex.Message; //Description
                var stackTrace = ex.StackTrace; //StackTrace
                int lineNumber = GetLineNumber(ex); //LineNumber

                //TODO: Save in the Database.
            }
            catch { }
        }

        public static int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex?.StackTrace?.LastIndexOf(lineSearch) ?? 0;
            if (index != -1)
            {
                var lineNumberText = ex?.StackTrace?.Substring(index + lineSearch.Length) ?? "";
                lineNumber = int.Parse(lineNumberText);
            }
            return lineNumber;
        }
    }
}
