using System;

namespace ChurnAnalyzers
{
    internal static class GitStatisticsExtensions
    {
        public static void ToConsole(this GitStatistics.Result self)
        {
            Console.WriteLine($"Total changes: {self.CommitCount} #Days with checkins: {self.DaysWithCommits} Avg.:{self.AverageCommitsPerDay:n2}/day");
            foreach (var item in self.CommitsPerAuthor)
            {
                Console.WriteLine($"Author:{item.Name} Commit count:{item.Count}");
            }
        }
    }
}