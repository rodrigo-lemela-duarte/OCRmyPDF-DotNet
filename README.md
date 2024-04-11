# OCRmyPDF-DotNet
Este é um pequeno wrapper, feito em .NET 8/C#, para o projeto OCRmyPDF.

## Requerimentos
Fazendo o clone deste, ou utilizando o nuget, não haverá nenhum requerimento para a execução deste, caso queira refazer o executável, será necessário ir até o projeto [OCRmyPDF](https://github.com/ocrmypdf/OCRmyPDF) e seguir as [instruções de instalação](https://ocrmypdf.readthedocs.io/en/latest/installation.html).

## Uso/Exemplos
```dotnet
using OCRMyPDF;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new OCR().StartProcess("arquivo.pdf", "arquivo-processado.pdf");

            if (response.Success == false)
            {
                Console.WriteLine("erro!");
                Console.WriteLine(response.Error);
            }
            else
            {
                Console.WriteLine("sucesso!");
            }
        }
    }
}
```

## Licença
[Mozilla Public License 2.0](https://github.com/rodrigo-lemela-duarte/OCRmyPDF-DotNet/blob/main/LICENSE)

## Autores
- [Rodrigo Lemela Duarte](https://github.com/rodrigo-lemela-duarte)

## English
- [ReadME EN](https://github.com/rodrigo-lemela-duarte/OCRmyPDF-DotNet/blob/main/README-en.md)
