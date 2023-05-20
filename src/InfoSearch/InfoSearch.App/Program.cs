using InfoSearch.Core;
using InfoSearch.Parsing;
using InfoSearch.Serializing;
using System.Reflection;

Console.WriteLine("InfoSearch Dictionary");

// var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "\\";
var downloadDirectory = "C:\\Users\\Oleksandr_Barsuk\\Downloads";

var searchPattern = "*.epub"; // "*.pdf";

var documents = FileScanner.Scan(downloadDirectory, searchPattern);
Console.WriteLine($"Documents: \n - {string.Join("\n - ", documents)}.");

var dictionary = new Dictionary();
var parser = new EpubParser();//PdfParser();

foreach (var document in documents)
{
    var doc = parser.Parse(document);
    var terms = doc.SplitTerms();
    dictionary.AddRange(terms);
}

Console.WriteLine($"Result set: {dictionary}");

Console.WriteLine($"Dictionary length: {dictionary.Set.Count()}");

var currentTime = DateTime.Now;
var filename = $"{downloadDirectory}\\{currentTime.ToString("yyyy-MM-dd-HH-mm-ss")}_dictionary.txt";

dictionary.Save(new TextSerializer(), filename);

Console.WriteLine("Done...");

Console.ReadLine();
