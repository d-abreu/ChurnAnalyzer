using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    internal static class ChurnedFilesExtensions
    {
        public static void ToConsole(this IEnumerable<ChurnedFiles.Result> self)
        {
            int counter = 1;
            foreach (var item in self)
            {
                Console.WriteLine($"{counter++}. {item.FileName} ChurnedLOC:{item.ChurnedLOC} TotalLOC:{item.TotalLOC} DeletedLOC:{item.DeletedLOC} M1:{item.M1:p2} M2:{item.M2:p2} M7:{item.M7:p2}");
            }
        }
    }
}