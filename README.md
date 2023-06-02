# InfoSearch Project

## Solutions of tasks for subject "Methods of information research"

## InfoSearch Dictionary

Practical work #1:
Dictionary.

App creates set of unique terms out of provided PDF or EPUB documents.

Run example:

````sh
cd src\InfoSearch\Executable\InfoSearch.DictionaryApp\bin\Debug\net7.0
InfoSearch.DictionaryApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub -s json
````

### Supported arguments

 - -t/--type - parser type: PDF/EPUB;
 - -d/--directory - path to directory with documents to parse;
 - -s/--serializer - the type of serializer: text, json, binary.

## InfoSearch Indexes

Practical work #2:
Boolean Search. Incidence Matrix and Inverted List Indexes

Run example:

````sh
cd src\InfoSearch\Executable\InfoSearch.IndexApp\bin\Debug\net7.0
InfoSearch.IndexApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query format to provide:
````
sherlock OR dorian
sherlock OR dorian AND gray
sherlock OR dorian OR dracula
sherlock OR dorian OR dracula NOT whale
````

To exit enter one of these options: exit, quit, e, q
