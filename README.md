# OCRmyPDF-DotNet
Este é um pequeno wrapper, feito em .NET 8/C#, para o projeto OCRmyPDF.

## Instalação
Após a instalação, é necessário fazer algunas validações:

### Ambiente Windows
- Siga a instalação conforme [este link](https://ocrmypdf.readthedocs.io/en/latest/installation.html#installing-on-windows).

### Ambiente Linux
- Siga a instalação conforme [este link](https://ocrmypdf.readthedocs.io/en/latest/installation.html#installing-on-linux).

### Imagem Docker
Caso deseje criar uma imagem Docker, será necessário adicionar o seguinte código ao seu Dockerfile:

```dockerfile
RUN apt-get update && \
    apt-get install -y python3.11 python3-pip

RUN apt install -y ocrmypdf

RUN apt-get install -y tesseract-ocr-eng tesseract-ocr-por tesseract-ocr-spa tesseract-ocr-fra

ENV PATH="/usr/bin:${PATH}"

RUN ln -sf /usr/bin/python3.11 /usr/bin/python && \
    ln -sf /usr/bin/python3.11 /usr/bin/python3
```

### Uso/Exemplos

O método principal, ```StartProcess```, espera a seguinte entidade:

```dotnet  
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
```

- A propriedade ```PythonVersion``` é a versão do Python que será utilizada. Caso não seja informada, será retornado um erro de versão não encontrada;
- A propriedade ```PythonHome``` é a instalação do Python que será utilizada, **apenas para ambiente Windows**. Caso não seja informada, será utilizada o valor padrão ```C:\Program Files\Python\Python``` e a versão enviada no ```PythonVersion```;
- A propriedade ```PythonDLL``` é a DLL/SO que será utilizada. Caso não seja informada será utilizada a DLL/SO padrão do Python, para ambiente Windows é o ```python3.11.dll```, exemplo, e para ambiente Linux é o ```/usr/lib/x86_64-linux-gnu/libpython3.11.so```, exemplo;
- A propriedade ```InputFile``` será o arquivo a ser processado;
- A propriedade ```OutputFile``` será o arquivo processado e retornado;
- A propriedade ```Language``` é a linguagem do arquivo (pt/eng/etc...);
- A propriedade ```RedoOCR``` será utilizada quando o arquivo já é *selecionável*, mas ainda assim é necessário ser refeito o OCR;

## Licença
- [Mozilla Public License 2.0](https://github.com/rodrigo-lemela-duarte/OCRmyPDF-DotNet/blob/main/LICENSE)

## Autores
- [Rodrigo Lemela Duarte](https://github.com/rodrigo-lemela-duarte)