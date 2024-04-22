using OCRmyPDF.Model;
using Python.Runtime;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OCRmyPDF
{
    public class OCR
    {
        private string ExecuteCommand(string command, bool log = false)
        {
            try
            {
                var fileName = "";
                var arguments = "";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    fileName = "cmd.exe";
                    arguments = $"/c {command}";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    fileName = "/bin/bash";
                    arguments = $"-c \"{command}\"";
                }

                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (log)
                    {
                        Console.WriteLine("=====================================================");
                        Console.WriteLine($"ARGUMENTS: {arguments}");
                        Console.WriteLine($"OUTPUT: {result}");
                        Console.WriteLine($"EXIT CODE: {process.ExitCode}");
                        Console.WriteLine("=====================================================");
                    }

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        private bool IsPythonInstalled(string version)
        {
            try
            {
                var outputPythonVersion = ExecuteCommand("python --version");
                var outputPython3Version = ExecuteCommand("python3 --version");

                if ((string.IsNullOrWhiteSpace(outputPythonVersion) || !outputPythonVersion.ToLower().Contains($"python {version}")) &&
                    (string.IsNullOrWhiteSpace(outputPython3Version) || !outputPython3Version.ToLower().Contains($"python {version}")))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        private bool IsTesseractInstalled()
        {
            try
            {
                var regex = new Regex(@"\b(tesseract)\s+(\d+\.\d+\.\d+)\b", RegexOptions.IgnoreCase);

                var output = ExecuteCommand("tesseract --version");

                if (string.IsNullOrWhiteSpace(output) || !regex.IsMatch(output.ToLower()))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        private Response ValidateRequest(Request request)
        {
            try
            {
                var response = new Response() { Success = true };

                if (string.IsNullOrWhiteSpace(request.PythonVersion))
                {
                    response.Success = false;
                    response.ErrorType = "PYTHONVERSIONMISSING";
                    response.Error = "PYTHON VERSION IS REQUIRED";
                }

                if (!IsPythonInstalled(request.PythonVersion))
                {
                    response.Success = false;
                    response.ErrorType = "PYTHONMISSING";
                    response.Error = "PYTHON IS NOT INSTALLED";
                }

                if (!IsTesseractInstalled())
                {
                    response.Success = false;
                    response.ErrorType = "TESSERACTMISSING";
                    response.Error = "TESSERACT-OCR IS NOT INSTALLED";
                }

                if (string.IsNullOrEmpty(request.InputFile))
                {
                    response.Success = false;
                    response.Error = "Input File Path is required";
                }

                if (string.IsNullOrEmpty(request.OutputFile))
                {
                    response.Success = false;
                    response.Error = "Output File Path is required";
                }

                return response;
            }
            catch
            {
                throw;
            }
        }

        private Response ValidatePythonHome(Request request)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (string.IsNullOrWhiteSpace(request.PythonHome))
                {
                    request.PythonHome = @$"C:\Program Files\Python\Python{request.PythonVersion}";
                }

                if (!Directory.Exists(request.PythonHome))
                {
                    return new Response()
                    {
                        Success = false,
                        ErrorType = "PYTHONHOMEMISSING",
                        Error = "PYTHONHOME IS MISSING"
                    };
                }
            }

            return new Response() { Success = true };
        }

        private Response ValidatePythonDLL(Request request)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (string.IsNullOrWhiteSpace(request.PythonDLL))
                {
                    request.PythonDLL = Path.Combine(request.PythonHome, $"python{request.PythonVersion.Replace(".", "")}.dll");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (string.IsNullOrWhiteSpace(request.PythonDLL))
                {
                    request.PythonDLL = $"/usr/lib/x86_64-linux-gnu/libpython{request.PythonVersion}.so";
                }
            }

            if (!File.Exists(request.PythonDLL))
            {
                return new Response()
                {
                    Success = false,
                    ErrorType = "PYTHONDLLMISSING",
                    Error = "PYTHONDLL IS MISSING"
                };
            }

            return new Response() { Success = true };
        }

        public Response StartProcess(Request request)
        {
            var response = new Response() { Success = true };

            try
            {
                response = ValidateRequest(request);

                if (response == null)
                {
                    return new Response()
                    {
                        Success = false,
                        ErrorType = "UNKNOWN",
                        Error = string.Empty
                    };
                }
                else if (response.Success == false)
                {
                    return response;
                }

                var responsePythonHome = ValidatePythonHome(request);

                if (responsePythonHome.Success == false)
                {
                    return responsePythonHome;
                }

                var responsePythonDLL = ValidatePythonDLL(request);

                if (responsePythonDLL.Success == false)
                {
                    return responsePythonDLL;
                }

                Runtime.PythonDLL = request.PythonDLL;
                PythonEngine.ProgramName = "OCRmyPDF-DotNet";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    PythonEngine.PythonHome = request.PythonHome;
                }

                PythonEngine.Initialize();

                var arguments = $"\"{request.InputFile}\", \"{request.OutputFile}\", quiet = True";

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    arguments += ", lang = \"eng\"";
                }

                if (request.RedoOCR)
                {
                    arguments += ", redo_ocr = True";
                }

                using (Py.GIL())
                {
                    PythonEngine.Exec($"import ocrmypdf; ocrmypdf.ocr({arguments})");
                }

                PythonEngine.Shutdown();

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorType = ex.GetType().Name;
                response.Error = ex.GetBaseException().Message;
            }

            return response;
        }
    }
}