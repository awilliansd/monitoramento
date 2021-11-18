namespace Monitoramento.ErrorHandling
{
    public class CustomException
    {
        public string App { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public CustomException InnerException { get; set; }
    }
}