namespace OCRmyPDF.Model
{
    public class Request
    {
        public string PythonVersion { get; set; } = string.Empty;
        public string PythonHome { get; set; } = string.Empty;
        public string PythonDLL { get; set; } = string.Empty;
        public string InputFile { get; set; } = string.Empty;
        public string OutputFile { get; set; } = string.Empty;
        public string Language { get; set; } = "eng";
        public bool RedoOCR { get; set; } = false;
    }
}