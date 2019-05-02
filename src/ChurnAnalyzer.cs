using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GitLogReader glr = new GitLogReader(@"gitLogs\git.log");
            var commits = glr.Read().ToList();

            var daysWithCommits = commits.Select(r => r.Date).Distinct().Count();
            double avg = ((double)commits.Count / (double)daysWithCommits);

            Console.WriteLine($"Total changes: {commits.Count} #Days with checkins: {daysWithCommits} Avg:{avg}");

            new TopChangedFiles(new TopChangedFiles.Parameters
            {
                Commits = commits,
                Take = 10
            }).Execute().ToConsole();

            Console.WriteLine("----");

            new ChurnedFiles(new ChurnedFiles.Parameters
            {
                Commits = commits,
            }).Execute().Take(10).ToConsole();

            new CoupledFiles(new CoupledFiles.Parameters { Commits = commits }).Execute().ToConsole();
        }
    }
}