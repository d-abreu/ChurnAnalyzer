using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    internal static class FileAgeFilesExtensions
    {
        public static void ToConsole(this IEnumerable<FileAge.Result> self)
        {
            int counter = 1;
            foreach (var item in self)
            {
                Console.WriteLine($"{counter++}. {item.FileName} weeks old:{item.AgeInWeeks}");
            }
        }
    }
}