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

## InfoSearch Two Word Index

Run example:

````sh
cd src\InfoSearch\Executable\InfoSearch.TwoWordIndexApp\bin\Debug\net7.0
InfoSearch.TwoWordIndexApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query format to provide:

````sh
# The Iliad by Homer.epub
The son of Pelagon
my choked streams no more their course can keep

# Middlemarch by George Eliot.epub
“Yes,” said Will, sardonically.
The two men were pitying each other

# Dracula by Bram Stoker.epub
Despair seized me
Then there came the sound of many feet tramping and dying away in some
````
