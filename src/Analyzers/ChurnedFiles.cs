using System.Collections.Generic;
using System.Linq;
using Math = System.Math;

namespace ChurnAnalyzers
{
    public class ChurnedFiles : IAnalyzer<IEnumerable<ChurnedFiles.Result>>{
        
        public class Result{
            public string FileName {get; set;}
            public int ChurnedLOC {get;set;}
            public int TotalLOC {get;set;}
            public int DeletedLOC {get;set;}
            public decimal M1 {get;set;}
            public decimal M2 {get;set;}
            public decimal M7 {get;set;}
        }
        public Parameters Options { get; }

        public class Parameters{
            public List<Commit> Commits {get;set;} = new List<Commit>();

            public List<string> Inclusions {get;set;} = new List<string>{".cs",".xaml"};
            public List<string> Exclusions {get;set;} = new List<string>{
                ".csproj",
                ".html",
                ".js",
                ".pb",
                "Reference.cs",
                "Designer.cs",
                ".css",
                ".csv",
                "Connected Services",
                "ODataClient"};
        }
        public ChurnedFiles(Parameters p)
        {
            Options = p;
        }

        public IEnumerable<ChurnedFiles.Result> Execute()
        {
            return Options.Commits
                .SelectMany(r => r.FileInfos)
                .GroupBy(r => r.FileName)
                .Where(r => {
                    foreach (var item in Options.Exclusions)
                    {
                        if(r.Key.Contains(item))
                            return false;
                    }
                    foreach (var item in Options.Inclusions)
                    {
                        if(r.Key.Contains(item))
                            return true;
                    }
                    return false;
                })
                .Select(r => 
                {
                    var commits = r.ToList();
                    var fileName = r.Key;
                    var totalLOC = commits.First().Added;
                    var deletedLOC = 0;
                    var churnedLOC = 0;

                    foreach (var item in commits.Skip(1))
                    {
                        var diff = item.Added - item.Removed;
                        if(diff < 0)
                            churnedLOC += 0;
                        else{
                            churnedLOC += Math.Abs(diff) + (item.Added - Math.Abs(diff));
                        }
                    
                        deletedLOC += item.Removed;
                        totalLOC = totalLOC + item.Added - item.Removed;
                    }

                    return new Result{
                        FileName = fileName,
                        ChurnedLOC = churnedLOC,
                        DeletedLOC = deletedLOC,
                        TotalLOC = totalLOC
                    };
                })
                .Select(r => new Result{
                    FileName = r.FileName,
                    ChurnedLOC = r.ChurnedLOC,
                    TotalLOC = r.TotalLOC,
                    DeletedLOC = r.DeletedLOC,
                    M1 = (decimal)r.ChurnedLOC / Math.Max((decimal)r.TotalLOC,1),
                    M2 = (decimal)r.DeletedLOC / Math.Max((decimal)r.TotalLOC,1),
                    M7 = (decimal)r.ChurnedLOC / Math.Max((decimal)r.DeletedLOC,1)
                })
                .OrderByDescending(r => r.TotalLOC)
                .AsEnumerable();
        }
    }
}