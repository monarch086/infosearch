namespace InfoSearch.Core
{
    public static class FileScanner
    {
        public static IEnumerable<string> Scan(string path, string searchPattern)
        {
            var directory = new DirectoryInfo(path);

            var files = directory.GetFiles(searchPattern, SearchOption.AllDirectories);
            var fileNames = new List<string>();

            foreach (var file in files)
            {
                fileNames.Add(file.FullName);
            }

            return fileNames;
        }
    }
}
