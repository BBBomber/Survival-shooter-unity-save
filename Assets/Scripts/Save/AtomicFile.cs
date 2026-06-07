using System;
using System.IO;

namespace SurvivalShooter.SaveSystem
{
    // used by save and config to manage interrupted writes and safely write.
    public static class AtomicFile
    {
        public static void WriteText(string path, string contents)
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string tmp = path + ".tmp";
            string bak = path + ".bak";

            File.WriteAllText(tmp, contents);

            if (File.Exists(path))
            {
                try
                {
                    File.Replace(tmp, path, bak);   
                }
                catch (Exception)
                {
                    
                    try { if (File.Exists(bak)) File.Delete(bak); } catch { }
                    try { File.Copy(path, bak, true); } catch { }
                    File.Delete(path);
                    File.Move(tmp, path);
                }
            }
            else
            {
                File.Move(tmp, path);
            }
        }
    }
}