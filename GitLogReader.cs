using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChurnAnalyzers
{

    class GitLogReader
    {
        private string FileName {get;}
        public GitLogReader(string fileName)
        {
           FileName = fileName;
        }

        private DateTime NextCommitDate {get;set;} = DateTime.MaxValue;
        public IEnumerable<Commit> Read()
        {
            using (var fs = new FileStream(FileName,FileMode.Open))
            {
                using (TextReader ts = new StreamReader(fs)) 
                {
                    bool shouldContinue = true;
                    while(shouldContinue)
                    {
                        string date = null;
                        string token;
                        
                        while(!string.IsNullOrWhiteSpace(token = ts.ReadLine()))
                        {
                            date = token;
                        }
                        if(date == null && NextCommitDate != DateTime.MaxValue)
                            date = NextCommitDate.ToShortDateString();
                        var commitDate = DateTime.Parse(date);
                        
                        LinkedList<(string,int,int)> modificationTokens = new LinkedList<(string, int, int)>();
                        DateTime tmp;
                        while (!DateTime.TryParse(token = ts.ReadLine(), out tmp))
                        {
                            if(token == null)
                                break;
                            var changedFileToken = token.Split('\t',StringSplitOptions.RemoveEmptyEntries).ToList();
                            var name = changedFileToken.Last();
                            var added = changedFileToken[0] == "-" ? 0 : int.Parse(changedFileToken[0]);
                            var removed = changedFileToken[1] == "-" ? 0 : int.Parse(changedFileToken[1]);

                            modificationTokens.AddLast((name,added,removed));
                        }
                        NextCommitDate = tmp;
                        if(!modificationTokens.Any())
                            shouldContinue = false;
                        else    
                            yield return new Commit(commitDate, modificationTokens.Select(r => new Commit.FileChanged(r.Item1,r.Item2,r.Item3)));
                    }

                }
            }
        }

      
    }
}