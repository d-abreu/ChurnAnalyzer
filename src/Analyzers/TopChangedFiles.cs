using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers
{
    public class TopChangedFiles : IAnalyzer<IEnumerable<TopChangedFiles.Result>>
    {
        public class Result
        {
            public string FileName { get; set; }
            public int TotalChanges { get; set; }
        }
        public class Parameters
        {
            public int Take { get; set; } = 10;
            public List<string> Exclusions { get; set; } = new List<string> { ".csproj" };
            public List<Commit> Commits { get; set; } = new List<Commit>();
        }

        private Parameters Options { get; }
        public TopChangedFiles(Parameters p)
        {
            Options = p;
        }
        public IEnumerable<TopChangedFiles.Result> Execute()
        {
            return Options.Commits
                .SelectMany(r => r.FileInfos)
                .GroupBy(r => r.FileName)
                .Where(r => !Options.Exclusions.Any(t => r.Key.Contains(t)))
                .Select(r => new Result
                {
                    FileName = r.Key,
                    TotalChanges = r.Count()
                })
                .OrderByDescending(r => r.TotalChanges)
                .Take(Options.Take)
                .AsEnumerable();
        }
    }
}