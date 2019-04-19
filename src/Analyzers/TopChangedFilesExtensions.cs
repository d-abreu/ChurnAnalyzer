using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    internal static class TopChangedFilesExtensions{
        public static void ToConsole(this IEnumerable<TopChangedFiles.Result> self){
            int counter = 1;
            foreach (var item in self)
            {
                Console.WriteLine($"{counter++}. {item.FileName} Changes:{item.TotalChanges}");
            }
        }
    }
}