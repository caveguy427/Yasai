namespace Yasai.Debug.Logging
{
    public enum LogLevel
    {
        // debugging, will not be included in final product
        Debug,
        // general traceback messages
        Info,
        // non-fatal warnings
        Warning,
        // errors, typically exceptions
        Error,
    }
}