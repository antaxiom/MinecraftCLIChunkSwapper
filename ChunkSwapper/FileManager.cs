using System;

namespace Testing
{
    public class FileManager
    {
        public static (string filename1, string filename2) Dirfind(int rx1, int rz1, int rx2, int rz2)
        {
            var filename1 = AppDomain.CurrentDomain.BaseDirectory + "/r." + rx1 + "." + rz1 + ".mca";
            var filename2 = AppDomain.CurrentDomain.BaseDirectory + "/r." + rx2 + "." + rz2 + ".mca";
            return (filename1, filename2);
        }
    }
}