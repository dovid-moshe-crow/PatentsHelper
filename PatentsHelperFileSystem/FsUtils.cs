using System.IO;

namespace PatentsHelperFileSystem
{
    public static class FsUtils
    {
        public static string PickFolder(string startingPath = null)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();


            if (!string.IsNullOrEmpty(startingPath))
            {
                dialog.SelectedPath = startingPath;
            }
            bool? result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                return dialog.SelectedPath;
            }
            else
            {
                return null;
            }

        }

        public static bool IsFilePathValid(string path)
        {

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }


            try
            {
                if (!Path.IsPathRooted(path))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }



            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }
            return true;
        }
    }
}
