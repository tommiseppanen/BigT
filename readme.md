# BigT
[![bigt MyGet Build Status](https://www.myget.org/BuildSource/Badge/bigt?identifier=ac72d4d4-3c7b-46d0-bca9-8ca02ea4e177)](https://www.myget.org/gallery/bigt)

BigT (Basic internalization/globalization Translations) is an easy to use localization library inpired by QT's tr() translation function.

Translations are stored in a CSV file.

NuGet package available in [MyGet](https://www.myget.org/gallery/bigt).

#Usage examples
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
