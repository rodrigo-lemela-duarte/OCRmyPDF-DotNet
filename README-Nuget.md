# OCRmyPDF-DotNet
Este é um pequeno wrapper, feito em .NET 8/C#, para o projeto OCRmyPDF.

## Instalação

Após a instalação é necessário fazer a configuração do Embedded.

### Configuração no Projeto

Procure a pasta ```Embedded```, clique com o botão direito no ```exe``` e clique em ***propriedades***.

![Embedded e exe](https://i.imgur.com/R8EszP9_d.webp?maxwidth=760&fidelity=grand)

No "*Copiar para Diretório de Saída*", selecione o "***Copiar se for mais novo***", conforme imagem abaixo:

![Propriedades](https://i.imgur.com/fUi7GEa_d.webp?maxwidth=760&fidelity=grand)

### Uso/Exemplos

O método principal, ```StartProcess```, espera quatro argumentos:

```dotnet
StartProcess(string inputFilePath, string outputFilePath, string language = "eng", bool redoOCR = false)
```

- **inputFilePath** (String - Arquivo que será processado)
- **outputFilePath** (String - Arquivo final, após processamento)
- **language** (String - Linguagem do arquivo, caso não seja inglês, é importante enviar)
- **redoOCR** (Booleano - Caso o arquivo encontre erro ao processar o arquivo, envie como ```true``` e a aplicação irá reprocessa-lo)

Aqui, abaixo, uma chamada exemplo:

```dotnet
using OCRMyPDF;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var response = new OCR().StartProcess("arquivo.pdf", "arquivo-processado.pdf", "eng", true);

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