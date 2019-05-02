using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChurnAnalyzers
{
    class GitLogReader
    {
        private string FileName { get; }
        public GitLogReader(string fileName)
        {
            FileName = fileName;
        }
        public IEnumerable<Commit> Read()
        {
            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                using (TextReader ts = new StreamReader(fs))
                {
                    bool shouldContinue = true;
                    var isEmptyCommit = false;
                    string token = null;

                    while (shouldContinue)
                    {
                        string author = isEmptyCommit ? token : ts.ReadLine();
                        isEmptyCommit = false;

                        if (author == null)
                            break;

                        var date = ts.ReadLine();
                        var commitDate = DateTime.Parse(date);
                        var modificationTokens = new LinkedList<(string, int, int)>();

                        while (!string.IsNullOrWhiteSpace(token = ts.ReadLine()))
                        {
                            if (token.Substring(0, 2) == "--")
                            {
                                isEmptyCommit = true;
                                break;
                            }

                            var changedFileToken = token.Split('\t', StringSplitOptions.RemoveEmptyEntries).ToList();
                            var fileName = changedFileToken.Last();
                            var added = changedFileToken[0] == "-" ? 0 : int.Parse(changedFileToken[0]);
                            var removed = changedFileToken[1] == "-" ? 0 : int.Parse(changedFileToken[1]);

                            modificationTokens.AddLast((fileName, added, removed));
                        }

                        if (modificationTokens.Any())
                            yield return new Commit(commitDate, modificationTokens.Select(r => new Commit.FileChanged(r.Item1, r.Item2, r.Item3)));
                    }

                }
            }
        }


    }
}