# OCRmyPDF-DotNet
This is a small wrapper, made in .NET 8/C#, for the OCRmyPDF project.

## Requirements
By cloning this, or using [NuGet](https://www.nuget.org/packages/OCRmyPDF-DotNet/1.0.0), there will be no requirements for executing this. However, if you want to rebuild the executable, it is necessary to go to the [OCRmyPDF](https://github.com/ocrmypdf/OCRmyPDF) project and follow the [installation instructions](https://ocrmypdf.readthedocs.io/en/latest/installation.html).

## Usage/Examples
```dotnet
using OCRMyPDF;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new OCR().StartProcess("file.pdf", "output-file.pdf");

            if (response.Success == false)
            {
                Console.WriteLine("error!");
                Console.WriteLine(response.Error);
            }
            else
            {
                Console.WriteLine("success!");
            }
        }
    }
}
```

## License
[Mozilla Public License 2.0](https://github.com/rodrigo-lemela-duarte/OCRmyPDF-DotNet/blob/main/LICENSE)

## Authors
- [Rodrigo Lemela Duarte](https://github.com/rodrigo-lemela-duarte)
