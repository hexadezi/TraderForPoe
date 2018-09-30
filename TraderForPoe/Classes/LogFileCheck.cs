using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraderForPoe.Properties;

namespace TraderForPoe.Classes
{
    static class LogFileCheck
    {
        public static void CheckForClientTxt()
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Grinding Gear Games\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Grinding Gear Games\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Steam\steamapps\common\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Steam\steamapps\common\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Grinding Gear Games\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Grinding Gear Games\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }
        }
    }
}
