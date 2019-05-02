using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers
{
    public class GitStatistics : IAnalyzer<GitStatistics.Result>
    {
        public ICollection<Commit> Commits { get; }

        public GitStatistics(ICollection<Commit> commits)
        {
            this.Commits = commits;
        }
        public class Result
        {
            public int CommitCount { get; set; }
            public int DaysWithCommits { get; set; }
            public double AverageCommitsPerDay { get; set; }
            public ICollection<(string Name, int Count)> CommitsPerAuthor { get; set; }
        }
        public Result Execute()
        {
            var commitCount = Commits.Count;
            var daysWithCommits = Commits.Select(r => r.Date).Distinct().Count();
            double avg = ((double)Commits.Count / (double)daysWithCommits);
            var commitsPerAuthor = Commits
                .Select(r => (r.Author, r.Date))
                .GroupBy(r => r.Author)
                .Select(r => (r.Key, r.Count()))
                .OrderByDescending(r => r.Item2)
                .ToList();

            return new Result()
            {
                CommitCount = commitCount,
                AverageCommitsPerDay = avg,
                DaysWithCommits = daysWithCommits,
                CommitsPerAuthor = commitsPerAuthor
            };


        }
    }
}