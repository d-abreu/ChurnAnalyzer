using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace ChurnAnalyzers
{
    public class CoupledFiles : IAnalyzer<ICollection<CoupledFiles.Result>>
    {
        public Parameters Options { get; }

        public class Parameters
        {
            public List<Commit> Commits { get; set; } = new List<Commit>();
            public List<string> Exclusions { get; set; } = new List<string> { ".csproj", ".json" };
        }

        public class Result
        {
            public string FirstFile { get; set; }
            public string SecondFile { get; set; }
            public int ChangeCount { get; set; }
            public decimal Coupling { get; set; }
            public string Description { get; set; }
        }

        public CoupledFiles(Parameters options)
        {
            Options = options;
        }

        public ICollection<Result> Execute()
        {
            var res = new ConcurrentBag<Result>();

            var fileNames = Options.Commits
                .SelectMany(r => r.FileInfos)
                .Select(r => r.FileName)
                .Where(r => !Options.Exclusions.Any(t => r.Contains(t)))
                .Distinct()
                .ToArray();

            Parallel.ForEach(fileNames, item =>
            {
                var set = Options.Commits.Where(r => r.FileInfos.Any(t => t.FileName == item)).ToList();
                var changeCount = set.Count();

                var coupleCountDict = set
                                        .SelectMany(r => r.FileInfos)
                                        .GroupBy(r => r.FileName)
                                        .Where(r => r.Key != item)
                                        .ToDictionary(key => key.Key, value => value.Count());

                Parallel.ForEach(coupleCountDict, (r) =>
                {
                    var coupling = (decimal)r.Value / (decimal)changeCount;
                    res.Add(
                        new Result
                        {
                            FirstFile = item,
                            ChangeCount = changeCount,
                            SecondFile = r.Key,
                            Coupling = coupling,
                            Description = $"Coupling {coupling:p2} in {r.Value}/{changeCount}"
                        });
                });
            });

            return res.Where(r => r.ChangeCount > 30)
                .OrderByDescending(r => r.Coupling)
                .Take(20)
                .ToList();
        }
    }
}