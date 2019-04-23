using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    internal static class CoupledFilesExtensions{
        public static void ToConsole(this IEnumerable<CoupledFiles.Result> self){
            int counter = 1;
            foreach (var item in self)
            {
                Console.WriteLine($"\n----\n{counter++}.\n{item.FirstFile}\n{item.SecondFile}\n{item.Description}\n-----");
            }
        }
    }
}