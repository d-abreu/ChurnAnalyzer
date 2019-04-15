using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurnAnalyzers{
    public class TopChangesFiles{

        public class Parameters{
            public int Take {get;set;} = 10;
            public List<string> Exclusions {get;set;} = new List<string>{".csproj"};
            public List<Commit> Commits {get;set;} = new List<Commit>();
        }

        private Parameters Options {get;}
        public TopChangesFiles(Parameters p)
        {
            Options = p;
        }
        public void Execute()
        {
            var counter = 1;

            Options.Commits
                .SelectMany(r => r.FileInfos)
                .GroupBy(r => r.FileName)
                .Where(r => {
                    foreach (var item in Options.Exclusions)
                    {
                        if(r.Key.Contains(item))
                            return false;
                    }
                    return true;
                })
                .Select(r => new { 
                    FileName = r.Key,
                    TotalChanges = r.Count()
                })
                .OrderByDescending(r => r.TotalChanges)
                .Take(Options.Take)
                .ToList()
                .ForEach(r => Console.WriteLine($"{counter++}. {r.FileName} Changes:{r.TotalChanges}"));
        }
    }
}