# InfoSearch Project

## Solutions of tasks for subject "Methods of information research"

## InfoSearch Dictionary

App creates set of unique words out of provided PDF or EPUB documents.

Run example:

````sh
cd src\InfoSearch\InfoSearch.App\bin\Debug\net7.0
InfoSearch.App -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub -s json
````

### Supported arguments

 - -t/--type - parser type: PDF/EPUB;
 - -d/--directory - path to directory with documents to parse;
 - -s/--serializer - the type of serializer: text, json, binary.
