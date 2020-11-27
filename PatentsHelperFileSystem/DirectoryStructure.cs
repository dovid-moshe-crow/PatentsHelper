using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PatentsHelperFileSystem
{
    public static class DirectoryStructure
    {
        public static List<DirectoryItem> GetDirectoryItems(string fullPath)
        {
            var items = new List<DirectoryItem>();

            try
            {

                var dirInfo = new DirectoryInfo(fullPath);
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                {
                    items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder }));
                }
            }
            catch
            {

            }

            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                {
                    items.AddRange(fs.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File }));
                }
            }
            catch
            {

            }

            return items;
        }
    }
}
