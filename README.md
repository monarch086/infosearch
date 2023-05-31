# InfoSearch Project

## Solutions of tasks for subject "Methods of information research"

## InfoSearch Dictionary

App creates set of unique words out of provided PDF or EPUB documents.

Run example:

````sh
cd src\InfoSearch\InfoSearch.App\bin\Debug\net7.0
InfoSearch.DictionaryApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub -s json
````

### Supported arguments

 - -t/--type - parser type: PDF/EPUB;
 - -d/--directory - path to directory with documents to parse;
 - -s/--serializer - the type of serializer: text, json, binary.

## InfoSearch Indexes

Run example:

````sh
cd src\InfoSearch\InfoSearch.IndexApp\bin\Debug\net7.0
InfoSearch.IndexApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query format to provide:
````
sherlock OR dorian
sherlock OR dorian AND gray
sherlock OR dorian OR dracula
sherlock OR dorian OR dracula NOT whale
````
