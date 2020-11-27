using System.IO;

namespace PatentsHelperFileSystem
{
    public class DirectoryItem
    {
        public DirectoryItemType Type { get; set; }
        public string FullPath { get; set; }
        public string Name => Path.GetFileName(FullPath);
    }
}
