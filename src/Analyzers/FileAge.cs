using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers
{
    public class FileAge : IAnalyzer<IEnumerable<FileAge.Result>>
    {
        public class Result
        {
            public string FileName { get; set; }
            public int AgeInWeeks { get; set; }
        }
        public class Parameters
        {
            public int Take { get; set; } = 10;
            public List<string> Exclusions { get; set; } = new List<string> { ".csproj" };
            public List<Commit> Commits { get; set; } = new List<Commit>();
        }

        private Parameters Options { get; }
        public FileAge(Parameters p)
        {
            Options = p;
        }
        public IEnumerable<FileAge.Result> Execute()
        {
            return Options.Commits
                .SelectMany(r => {
                    return r.FileInfos.ToList().Select(i => (i.FileName, r.Date));
                })
                .GroupBy(r => r.FileName)
                .Where(r => !Options.Exclusions.Any(t => r.Key.Contains(t)))
                .Select(r => new FileAge.Result
                {
                    FileName = r.Key,
                    AgeInWeeks = DateTime.Now.Subtract(r.OrderByDescending(t => t.Date).First().Date).Days / 7
                })
                .OrderBy(r => r.AgeInWeeks)
                .Take(Options.Take)
                .AsEnumerable();
        }
    }
}