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

            new GitStatistics(commits).Execute().ToConsole();

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