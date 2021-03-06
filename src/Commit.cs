using System;
using System.Collections.Generic;

namespace ChurnAnalyzers
{
    public class Commit
    {
        public Commit(string author, DateTime date, IEnumerable<FileChanged> changes)
        {
            Date = date;
            Author = author;
            foreach (var item in changes)
            {
                FileInfos.AddLast(item);
            }
        }
        public DateTime Date { get; }
        public string Author { get; }

        public LinkedList<FileChanged> FileInfos { get; } = new LinkedList<FileChanged>();
        public class FileChanged
        {
            public FileChanged(string name, int added, int removed)
            {
                FileName = name;
                Added = added;
                Removed = removed;
            }
            public string FileName { get; }
            public int Added { get; }
            public int Removed { get; }
        }
    }
}