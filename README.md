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

## InfoSearch Coordinate Index

Run example:

````sh
cd src\InfoSearch\Executable\InfoSearch.CoordinateIndexApp\bin\Debug\net7.0
InfoSearch.CoordinateIndexApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query format to provide:

````sh
# The Iliad by Homer
son /d=5 of Pelagon    # Stack overflow with recursive algorithm

# Middlemarch by George Eliot
she /d=100 read /d=100 this /d=100 letter
she /d=1 read /d=1 this /d=1 letter

accurate /d=1 statement
accurate /d=100 statement

cannot /d=1 but /d=1 feel /d=1 that /d=1 resignation    # Stack overflow with recursive algorithm
cannot /d=100 but /d=100 feel /d=100 that /d=100 resignation

find /d=10 records
find /d=1 records

# Dracula by Bram Stoker
He /d=10 is /d=10 waking!    # Stack overflow with recursive algorithm
willing, /d=10 Wilhelmina
````

## InfoSearch PostreSQL Extensions

Run example:

````sh
cd src\InfoSearch\PostgreSQL\InfoSearch.PgSearch.App\bin\Debug\net7.0
InfoSearch.PgSearch.App -t byauthor

InfoSearch.PgSearch.App -t bytitle
````

Params:

- -t - search type: ByAuthor or ByTitle;

Query examples to provide by author:
`wels`, `sidni`, `doil`, `brem`, `artur`

Query examples to provide by title:

`pictur of dorian`

`rum with a few`

`ventures of serloc`

`mobi dick`

## InfoSearch Trigram Index

````sh
cd src\InfoSearch\Executable\InfoSearch.TrigramIndexApp\bin\Debug\net7.0
InfoSearch.TrigramIndexApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query examples: `*check`, `check*`, `ch*ck`

## InfoSearch Trie Search

````sh
cd src\InfoSearch\Executable\InfoSearch.TrieApp\bin\Debug\net7.0
InfoSearch.TrieApp -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query examples: `*check`, `check*`, `ch*ck`

## InfoSearch Lucene.NET full-text search

````sh
cd src\InfoSearch\Lucene\Lucene.App
dotnet run -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub
````

Query examples:

`pictur of dorian`

`pictur of dorian AND gray`

`ventures of serloc`

`mobi dick`

`mobi dick NOT "trimmed lamp"`

`mobi dick OR dracula`

`Szekelys have a right`

`"Szekelys have a right"`

## InfoSearch SPIMI big index

````sh
cd src\InfoSearch\Executable\InfoSearch.SpimiIndexApp

# To build new index
dotnet run -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub -l buildindex

dotnet run -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\book_collection\epub -l buildindex

dotnet run -t pdf -d C:\Users\Oleksandr_Barsuk\Downloads\book_collection\pdf -l buildindex

# To load pre-built index
dotnet run -t epub -d C:\Users\Oleksandr_Barsuk\Downloads\epub -l loadindex
````
