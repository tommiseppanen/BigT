# BigT
[![bigt MyGet Build Status](https://www.myget.org/BuildSource/Badge/bigt?identifier=ac72d4d4-3c7b-46d0-bca9-8ca02ea4e177)](https://www.myget.org/gallery/bigt)

BigT (Basic internalization/globalization Translations) is an easy to use localization library inpired by QT's tr() translation function.

Translations are stored in a CSV file so translators can work, for example, with Excel or Google Sheets.

NuGet package available in [MyGet](https://www.myget.org/gallery/bigt).

#Installation in Visual Studio
Add the following URL to Package Sources:
```
https://www.myget.org/F/bigt/api/v3/index.json
```
Install BigT from Package Manager Console by running the following command:
```
Install-Package BigT
```

#Usage examples
In addition to the following examples, check also the source code of [BigT Localization Example](https://github.com/tommiseppanen/BigT-Localization-Example).

Basic string:
```
using BigT;

//...

var text = Big.T("This is a string to be translated");
```

Shorter version with C# 6.0:
```
using static BigT.Big;

//...

var text = T("This is a string to be translated!");
```

Use BigT.Parser.RunParsing() to update translation CSV with new strings. For example:
```
static void Main(string[] args)
{
#if DEBUG
	BigT.Parser.RunParsing("..\\..");
#endif

//...

}
```
