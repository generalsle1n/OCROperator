# OCROperator

This tool provides functionality to automatically read pdf's that were previously created without text recognition, extract the text and pass this data to the actions that will process the data that.

[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://GitHub.com/Naereen/StrapDown.js/graphs/commit-activity)

 


## Tech Stack

**Framework:** ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) 
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)


**Server:** ![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)





## Demo

![App Screenshot](https://raw.githubusercontent.com/generalsle1n/OCROperator/master/blob/OCROperatorDemo.gif)


## Run Locally to Develop

Clone the project

```bash
  git clone https://github.com/generalsle1n/OCROperator
```

Go to the project directory

```bash
  cd OCROperator\OCROperator
```

Install dependencies

```bash
  dotnet restore
```

Start the server

```bash
  dotnet run
```


## Installation for Production Usage

Publish the project with the following settings
- Config:               ```Release```
- TargetFramework:      ```.Net 6```
- Deployment:           ```SelfContained```
- Singlefile:           ```false```
- ReadyToRun:           ```false```
- Remove not used Code: ```false```

```bash
  sc.exe create "OCROperator" binpath="C:\Path\To\OCROperator.exe"
```
    
## Roadmap

- Add an Microsoft Busines Central Action
- Add more watchers (Database, Mailbox, eg.)

## Lessons Learned

Used mostly Interfaces to allowes multiple classes that can define actions over an user controlled config file
## Feedback

If you have any feedback, please open an issue or an pull request 😀


## Features
This tool searchs asynchron for pdf and metadata that are specifid in an watcher config.
If the tool finds some the tools process the pdf with the ocr enginge tesseract to get the text and then the file with the text is transferd to an action which can do any stuff with the pdf (Upload to an ticketsystem, erp, crm or so on)
Currently it only works in connection with papercut mf, because papercut generate an metadata json which is processed

### Watchers
Currently there is only one Watcher implemented:

#### FileSystem
This watchers looks in an folder for the pdf

### Actions

#### FileToZammad
This action trys to extract an ticketnumber and then upload the pdf to this ticket.
If no ticket is found then it create an empty ticket

The setting is splitted by an ;
- Zammad URL
- API Token
- UserID (In the example the 1)

So the string must look like: https://zammadserver.com;SECRET;1


#### FileToUser

This actions send the extracted text to the user mail that is specifid in the metadata json from papercut.
The mail settings are specifid via the mailfactory solution
## FAQ

#### Can i add by my own an custom action?

Yeah sure --> If you think its good and create an pull request to merge it

#### Will there be an implementation without papercut?

Currently its not planned, but if you want to implement it just open an pull request too



## Configuration
To run this project, you will need to add the following config variables to your appsettings.json file.
All important settings are in "Watchers"
There is an example config in the repo
### Connectors


`Destination`: The Path where the watcher should look to get the pdf **string**

`SuffixMetadata`: The suffix pattern to look for the metadata **string**

`ActionType`: The binary Type for the action, possible values: **string**

- OCROperator.Models.Interface.Action.FileToFixedEmail
- OCROperator.Models.Interface.Action.FileToUserEmail
- OCROperator.Models.Interface.Action.FileToZammad

`ActionSettings`: Enter the custom settings for the action **int**

`Type`: The binary Type for the watcher , possible values: **string**

- OCROperator.Models.Interface.FileSystem

`Language`: Enter the OCR Langaue, possible values: **string**

- deu (German)
- eng (English)
- spa (spanish)

`HoldPDF`: Decide if the pdf after the process is fisnihed should be hold and not deleted **bool**

### MailFactory

`SMTPServer`: Enter the name of the smtp server which should be used

`Port`: Enter the port for the smtp server

`GenerateFrom`: The from mail, which should be used

## Authors

- [@Niels Schuler](https://github.com/generalsle1n/)


## Acknowledgements

 - [Main PDF Processing Framework: iText7](https://github.com/itext/itext7-dotnet)
 - [Main PDF Processing Framework: iText7 PDF2Image](https://github.com/thombrink/itext7.pdfimage)
 - [Logging Framework: Serilog](https://github.com/serilog/serilog)
 - [Logging Framework: Serilog Hosting Extension](https://github.com/serilog/serilog-extensions-hosting)
 - [Logging Framework: Serilog Console Extension](https://github.com/serilog/serilog-sinks-console)
 - [Logging Framework: Serilog File Extension](https://github.com/serilog/serilog-sinks-file)
 - [OCR Engine: Tesseract](https://github.com/tesseract-ocr/tesseract)
 - [OCR Engine: Tesseract Wrapper](https://github.com/serilog/serilog-sinks-file)
 - [Zammad API Wrapper](https://github.com/Asesjix/Zammad.Client)

