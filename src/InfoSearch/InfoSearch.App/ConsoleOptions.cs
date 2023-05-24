﻿using InfoSearch.Core;
using System.Reflection;

namespace InfoSearch.App;

internal class ConsoleOptions
{
    public ParserType Type { get; set; } = ParserType.EPUB;

    public string WorkingDirectory { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "\\";

    public SerializerType SerializerType { get; set; } = SerializerType.Text;
}
