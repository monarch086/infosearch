using InfoSearch.Core;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace InfoSearch.Serializing;

public class BinarySerializer : ISerializer
{
    public void Serialize(ISet<string> set, string filename)
    {
        var ms = File.OpenWrite(filename);
        //Format the object as Binary  

        var formatter = new BinaryFormatter();

        //formatter.Serialize(ms, set);
        ms.Flush();
        ms.Close();
        ms.Dispose();
    }
}

