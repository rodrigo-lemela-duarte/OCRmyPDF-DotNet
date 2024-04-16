namespace OCRmyPDF.Model
{
    public class Response
    {
        public bool Success { get; set; }
        public string ErrorType { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}