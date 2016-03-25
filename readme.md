# BigT
BigT (Basic internalization/globalization Translations) is an easy to use localization library inpired by QT's tr() translation function.

Translations are stored in a CSV file.

#Usage examples
Basic string:
```
using BigT;

...

var text = Big.T("This is a string to be translated");
```

Shorter version with C# 6.0:
```
using static BigT.Big;

...

var text = T("This is a string to be translated!");
```

