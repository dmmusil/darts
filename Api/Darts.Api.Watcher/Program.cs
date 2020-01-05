using System;
using System.Diagnostics;
using System.IO;

namespace Darts.Api.Watcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory().Replace(".Watcher", "");
            var info = new ProcessStartInfo
            {
                WorkingDirectory = dir,
                FileName = "cmd.exe",
                Arguments =
                    "/K func host start --cors http://localhost:5000"
            };
            Process.Start(info).WaitForExit();
        }
    }
}
