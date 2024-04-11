using OCRmyPDF.Model;
using System.Diagnostics;

namespace OCRmyPDF
{
    public class OCR
    {
        public Response StartProcess(string inputFilePath, string outputFilePath, string language = "eng")
        {
            var response = new Response() { Success = true };

            try
            {
                if (string.IsNullOrEmpty(inputFilePath))
                {
                    response.Success = false;
                    response.Error = "Input File Path is required";
                }

                if (string.IsNullOrEmpty(outputFilePath))
                {
                    response.Success = false;
                    response.Error = "Output File Path is required";
                }

                if (response.Success == false)
                {
                    return response;
                }

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = Path.Combine(AppContext.BaseDirectory, "Embedded", "ocrmypdf.exe");
                    process.StartInfo.Arguments = $"\"{inputFilePath}\" \"{outputFilePath}\"";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    string errorOutput = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(errorOutput))
                    {
                        response.Success = false;
                        response.Error = errorOutput;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.GetBaseException().Message;
            }

            return response;
        }
    }
}