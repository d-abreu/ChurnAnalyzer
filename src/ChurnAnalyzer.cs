using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GitLogReader glr = new GitLogReader(@"gitLogs\gitfull.log");
            var commits = glr.Read().ToList();

            var daysWithCommits = commits.Select(r => r.Date).Distinct().Count();
            double avg = ((double)commits.Count/(double)daysWithCommits);
            
            Console.WriteLine($"Total changes: {commits.Count} #Days with checkins: {daysWithCommits} Avg:{avg}");

            new TopChangesFiles(new TopChangesFiles.Parameters{
                Commits = commits,
                Exclusions = new List<string>{"csproj"},
                Take = 10
            }).Execute();
        }
    }
}